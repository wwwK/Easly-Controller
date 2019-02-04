﻿namespace EaslyController.Focus
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Controller for a node tree.
    /// This controller supports:
    /// * Operations to modify the tree.
    /// * Organizing nodes and their content in cells, assigning line and column numbers.
    /// </summary>
    public interface IFocusController : IFrameController
    {
        /// <summary>
        /// Index of the root node.
        /// </summary>
        new IFocusRootNodeIndex RootIndex { get; }

        /// <summary>
        /// State of the root node.
        /// </summary>
        new IFocusPlaceholderNodeState RootState { get; }

        /// <summary>
        /// State table.
        /// </summary>
        new IFocusIndexNodeStateReadOnlyDictionary StateTable { get; }

        /// <summary>
        /// List of operations that have been performed, and can be undone or redone.
        /// </summary>
        new IFocusOperationGroupReadOnlyList OperationStack { get; }

        /// <summary>
        /// Adds a new node to the list of nodes that can replace the current one. Does nothing if all types of nodes have been added.
        /// Applies only to bodies and features.
        /// </summary>
        /// <param name="state">Node that can be replaced.</param>
        void AddNodeToCycle(IFocusNodeState state);

        /// <summary>
        /// Replace an existing node with a new one, keeping its cycle.
        /// </summary>
        /// <param name="inner">The inner where the node is replaced.</param>
        /// <param name="cycleIndexList">Cycle of nodes that can replace the current node.</param>
        /// <param name="cyclePosition">New position in the cycle.</param>
        /// <param name="nodeIndex">Index of the replacing node upon return.</param>
        void Replace(IFocusInner inner, IFocusInsertionChildNodeIndexList cycleIndexList, int cyclePosition, out IFocusBrowsingChildIndex nodeIndex);
    }

    /// <summary>
    /// Controller for a node tree.
    /// This controller supports:
    /// * Operations to modify the tree.
    /// * Organizing nodes and their content in cells, assigning line and column numbers.
    /// * Keeping the focus in a cell.
    /// </summary>
    public class FocusController : FrameController, IFocusController
    {
        #region Init
        /// <summary>
        /// Creates and initializes a new instance of a <see cref="FocusController"/> object.
        /// </summary>
        /// <param name="nodeIndex">Index of the root of the node tree.</param>
        public static IFocusController Create(IFocusRootNodeIndex nodeIndex)
        {
            FocusController Controller = new FocusController();
            Controller.SetRoot(nodeIndex);
            Controller.SetInitialized();

            return Controller;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusController"/> class.
        /// </summary>
        private protected FocusController()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Index of the root node.
        /// </summary>
        public new IFocusRootNodeIndex RootIndex { get { return (IFocusRootNodeIndex)base.RootIndex; } }

        /// <summary>
        /// State of the root node.
        /// </summary>
        public new IFocusPlaceholderNodeState RootState { get { return (IFocusPlaceholderNodeState)base.RootState; } }

        /// <summary>
        /// State table.
        /// </summary>
        public new IFocusIndexNodeStateReadOnlyDictionary StateTable { get { return (IFocusIndexNodeStateReadOnlyDictionary)base.StateTable; } }

        /// <summary>
        /// List of operations that have been performed, and can be undone or redone.
        /// </summary>
        public new IFocusOperationGroupReadOnlyList OperationStack { get { return (IFocusOperationGroupReadOnlyList)base.OperationStack; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Adds a new node to the list of nodes that can replace the current one. Does nothing if all types of nodes have been added.
        /// Applies only to bodies and features.
        /// </summary>
        /// <param name="state">Node that can be replaced.</param>
        public virtual void AddNodeToCycle(IFocusNodeState state)
        {
            Debug.Assert(state != null);

            state.AddNodeToCycle();
        }

        /// <summary>
        /// Replace an existing node with a new one, keeping its cycle.
        /// </summary>
        /// <param name="inner">The inner where the node is replaced.</param>
        /// <param name="cycleIndexList">Cycle of nodes that can replace the current node.</param>
        /// <param name="cyclePosition">New position in the cycle.</param>
        /// <param name="nodeIndex">Index of the replacing node upon return.</param>
        public virtual void Replace(IFocusInner inner, IFocusInsertionChildNodeIndexList cycleIndexList, int cyclePosition, out IFocusBrowsingChildIndex nodeIndex)
        {
            Debug.Assert(cycleIndexList != null);
            Debug.Assert(cycleIndexList.Count >= 2);
            Debug.Assert(cyclePosition >= 0 && cyclePosition < cycleIndexList.Count);

            IFocusInsertionChildNodeIndex ReplacementIndex = cycleIndexList[cyclePosition];

            int BlockIndex = -1;
            int Index = -1;
            bool IsHandled = false;

            switch (ReplacementIndex)
            {
                case IWriteableInsertionPlaceholderNodeIndex AsPlaceholderNodeIndex:
                    IsHandled = true;
                    break;

                case IWriteableInsertionOptionalNodeIndex AsOptionalNodeIndex: // ClearIndex not acceptable for a cyclic replacement
                    IsHandled = true;
                    break;

                case IWriteableInsertionListNodeIndex AsListNodeIndex:
                    Index = AsListNodeIndex.Index;
                    IsHandled = true;
                    break;

                case IWriteableInsertionExistingBlockNodeIndex AsExistingBlockNodeIndex:
                    BlockIndex = AsExistingBlockNodeIndex.BlockIndex;
                    Index = AsExistingBlockNodeIndex.Index;
                    IsHandled = true;
                    break;
            }

            Debug.Assert(IsHandled);

            Action<IWriteableOperation> HandlerRedo = (IWriteableOperation operation) => ExecuteReplaceWithCycle(operation);
            Action<IWriteableOperation> HandlerUndo = (IWriteableOperation operation) => UndoReplaceWithCycle(operation);
            IFocusReplaceWithCycleOperation Operation = CreateReplaceWithCycleOperation(inner.Owner.Node, inner.PropertyName, BlockIndex, Index, cycleIndexList, cyclePosition, HandlerRedo, HandlerUndo, isNested: false);

            ExecuteReplaceWithCycle(Operation);

            nodeIndex = Operation.NewBrowsingIndex;
            SetLastOperation(Operation);

            CheckInvariant();
        }

        /// <summary></summary>
        private protected virtual void ExecuteReplaceWithCycle(IWriteableOperation operation)
        {
            IFocusReplaceWithCycleOperation ReplaceWithCycleOperation = (IFocusReplaceWithCycleOperation)operation;

            INode ParentNode = ReplaceWithCycleOperation.ParentNode;
            string PropertyName = ReplaceWithCycleOperation.PropertyName;
            IFocusInner<IFocusBrowsingChildIndex> Inner = GetInner(ParentNode, PropertyName) as IFocusInner<IFocusBrowsingChildIndex>;

            ReplaceState(ReplaceWithCycleOperation, Inner);

            IFocusNodeState NewState = StateTable[ReplaceWithCycleOperation.NewBrowsingIndex];
            NewState.RestoreCycleIndexList(ReplaceWithCycleOperation.CycleIndexList);

            NotifyStateReplaced(ReplaceWithCycleOperation);
        }

        /// <summary></summary>
        private protected virtual void UndoReplaceWithCycle(IWriteableOperation operation)
        {
            IFocusReplaceWithCycleOperation ReplaceWithCycleOperation = (IFocusReplaceWithCycleOperation)operation;
            ReplaceWithCycleOperation = ReplaceWithCycleOperation.ToInverseReplace() as IFocusReplaceWithCycleOperation;

            INode ParentNode = ReplaceWithCycleOperation.ParentNode;
            string PropertyName = ReplaceWithCycleOperation.PropertyName;
            IFocusInner<IFocusBrowsingChildIndex> Inner = GetInner(ParentNode, PropertyName) as IFocusInner<IFocusBrowsingChildIndex>;

            ReplaceState(ReplaceWithCycleOperation, Inner);

            IFocusNodeState NewState = StateTable[ReplaceWithCycleOperation.NewBrowsingIndex];
            NewState.RestoreCycleIndexList(ReplaceWithCycleOperation.CycleIndexList);

            NotifyStateReplaced(ReplaceWithCycleOperation);
        }
        #endregion

        #region Implementation
        /// <summary></summary>
        private protected override void CheckContextConsistency(IReadOnlyBrowseContext browseContext)
        {
            ((FocusBrowseContext)browseContext).CheckConsistency();
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxIndexNodeStateDictionary object.
        /// </summary>
        private protected override IReadOnlyIndexNodeStateDictionary CreateStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusIndexNodeStateDictionary();
        }

        /// <summary>
        /// Creates a IxxxIndexNodeStateReadOnlyDictionary object.
        /// </summary>
        private protected override IReadOnlyIndexNodeStateReadOnlyDictionary CreateStateTableReadOnly(IReadOnlyIndexNodeStateDictionary stateTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusIndexNodeStateReadOnlyDictionary((IFocusIndexNodeStateDictionary)stateTable);
        }

        /// <summary>
        /// Creates a IxxxInnerDictionary{string} object.
        /// </summary>
        private protected override IReadOnlyInnerDictionary<string> CreateInnerTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusInnerDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxInnerReadOnlyDictionary{string} object.
        /// </summary>
        private protected override IReadOnlyInnerReadOnlyDictionary<string> CreateInnerTableReadOnly(IReadOnlyInnerDictionary<string> innerTable)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusInnerReadOnlyDictionary<string>((IFocusInnerDictionary<string>)innerTable);
        }

        /// <summary>
        /// Creates a IxxxIndexNodeStateDictionary object.
        /// </summary>
        private protected override IReadOnlyIndexNodeStateDictionary CreateChildStateTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusIndexNodeStateDictionary();
        }

        /// <summary>
        /// Creates a IxxxxBrowseContext object.
        /// </summary>
        private protected override IReadOnlyBrowseContext CreateBrowseContext(IReadOnlyBrowseContext parentBrowseContext, IReadOnlyNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusBrowseContext((IFocusNodeState)state);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderInner{IxxxBrowsingPlaceholderNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyPlaceholderInner<IReadOnlyBrowsingPlaceholderNodeIndex> CreatePlaceholderInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingPlaceholderNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusPlaceholderInner<IFocusBrowsingPlaceholderNodeIndex, FocusBrowsingPlaceholderNodeIndex>((IFocusNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxOptionalInner{IxxxBrowsingOptionalNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyOptionalInner<IReadOnlyBrowsingOptionalNodeIndex> CreateOptionalInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingOptionalNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusOptionalInner<IFocusBrowsingOptionalNodeIndex, FocusBrowsingOptionalNodeIndex>((IFocusNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxListInner{IxxxBrowsingListNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyListInner<IReadOnlyBrowsingListNodeIndex> CreateListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingListNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusListInner<IFocusBrowsingListNodeIndex, FocusBrowsingListNodeIndex>((IFocusNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxBlockListInner{IxxxBrowsingBlockNodeIndex} object.
        /// </summary>
        private protected override IReadOnlyBlockListInner<IReadOnlyBrowsingBlockNodeIndex> CreateBlockListInner(IReadOnlyNodeState owner, IReadOnlyIndexCollection<IReadOnlyBrowsingBlockNodeIndex> nodeIndexCollection)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusBlockListInner<IFocusBrowsingBlockNodeIndex, FocusBrowsingBlockNodeIndex>((IFocusNodeState)owner, nodeIndexCollection.PropertyName);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        private protected override IReadOnlyPlaceholderNodeState CreateRootNodeState(IReadOnlyRootNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusPlaceholderNodeState<IFocusInner<IFocusBrowsingChildIndex>>((IFocusRootNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxWriteableInsertionOptionalNodeIndex object.
        /// </summary>
        private protected override IWriteableInsertionOptionalNodeIndex CreateNewOptionalNodeIndex(INode parentNode, string propertyName, INode node)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusInsertionOptionalNodeIndex(parentNode, propertyName, node);
        }

        /// <summary>
        /// Creates a IxxxInsertNodeOperation object.
        /// </summary>
        private protected override IWriteableInsertNodeOperation CreateInsertNodeOperation(INode parentNode, string propertyName, int blockIndex, int index, INode node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusInsertNodeOperation(parentNode, propertyName, blockIndex, index, node, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxInsertBlockOperation object.
        /// </summary>
        private protected override IWriteableInsertBlockOperation CreateInsertBlockOperation(INode parentNode, string propertyName, int blockIndex, IBlock block, INode node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusInsertBlockOperation(parentNode, propertyName, blockIndex, block, node, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveBlockOperation object.
        /// </summary>
        private protected override IWriteableRemoveBlockOperation CreateRemoveBlockOperation(INode parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusRemoveBlockOperation(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveBlockViewOperation object.
        /// </summary>
        private protected override IWriteableRemoveBlockViewOperation CreateRemoveBlockViewOperation(INode parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusRemoveBlockViewOperation(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxRemoveNodeOperation object.
        /// </summary>
        private protected override IWriteableRemoveNodeOperation CreateRemoveNodeOperation(INode parentNode, string propertyName, int blockIndex, int index, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusRemoveNodeOperation(parentNode, propertyName, blockIndex, index, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxReplaceOperation object.
        /// </summary>
        private protected override IWriteableReplaceOperation CreateReplaceOperation(INode parentNode, string propertyName, int blockIndex, int index, INode newNode, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusReplaceOperation(parentNode, propertyName, blockIndex, index, newNode, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxAssignmentOperation object.
        /// </summary>
        private protected override IWriteableAssignmentOperation CreateAssignmentOperation(INode parentNode, string propertyName, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusAssignmentOperation(parentNode, propertyName, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeNodeOperation object.
        /// </summary>
        private protected override IWriteableChangeNodeOperation CreateChangeNodeOperation(INode parentNode, string propertyName, int value, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusChangeNodeOperation(parentNode, propertyName, value, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxChangeBlockOperation object.
        /// </summary>
        private protected override IWriteableChangeBlockOperation CreateChangeBlockOperation(INode parentNode, string propertyName, int blockIndex, ReplicationStatus replication, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusChangeBlockOperation(parentNode, propertyName, blockIndex, replication, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxSplitBlockOperation object.
        /// </summary>
        private protected override IWriteableSplitBlockOperation CreateSplitBlockOperation(INode parentNode, string propertyName, int blockIndex, int index, IBlock newBlock, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusSplitBlockOperation(parentNode, propertyName, blockIndex, index, newBlock, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMergeBlocksOperation object.
        /// </summary>
        private protected override IWriteableMergeBlocksOperation CreateMergeBlocksOperation(INode parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusMergeBlocksOperation(parentNode, propertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMoveNodeOperation object.
        /// </summary>
        private protected override IWriteableMoveNodeOperation CreateMoveNodeOperation(INode parentNode, string propertyName, int blockIndex, int index, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusMoveNodeOperation(parentNode, propertyName, blockIndex, index, direction, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxxMoveBlockOperation object.
        /// </summary>
        private protected override IWriteableMoveBlockOperation CreateMoveBlockOperation(INode parentNode, string propertyName, int blockIndex, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusMoveBlockOperation(parentNode, propertyName, blockIndex, direction, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxExpandArgumentOperation object.
        /// </summary>
        private protected override IWriteableExpandArgumentOperation CreateExpandArgumentOperation(INode parentNode, string propertyName, IBlock block, INode node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusExpandArgumentOperation(parentNode, propertyName, block, node, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxGenericRefreshOperation object.
        /// </summary>
        private protected override IWriteableGenericRefreshOperation CreateGenericRefreshOperation(IWriteableNodeState refreshState, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusGenericRefreshOperation((IFocusNodeState)refreshState, handlerRedo, handlerUndo, isNested);
        }

        /// <summary>
        /// Creates a IxxxOperationGroupList object.
        /// </summary>
        private protected override IWriteableOperationGroupList CreateOperationGroupStack()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusOperationGroupList();
        }

        /// <summary>
        /// Creates a IxxxOperationGroupReadOnlyList object.
        /// </summary>
        private protected override IWriteableOperationGroupReadOnlyList CreateOperationGroupReadOnlyStack(IWriteableOperationGroupList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusOperationGroupReadOnlyList((IFocusOperationGroupList)list);
        }

        /// <summary>
        /// Creates a IxxxOperationList object.
        /// </summary>
        private protected override IWriteableOperationList CreateOperationList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusOperationList();
        }

        /// <summary>
        /// Creates a IxxxOperationReadOnlyList object.
        /// </summary>
        private protected override IWriteableOperationReadOnlyList CreateOperationReadOnlyList(IWriteableOperationList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusOperationReadOnlyList((IFocusOperationList)list);
        }

        /// <summary>
        /// Creates a IxxxOperationGroup object.
        /// </summary>
        private protected override IWriteableOperationGroup CreateOperationGroup(IWriteableOperationReadOnlyList operationList, IWriteableGenericRefreshOperation refresh)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusOperationGroup((IFocusOperationReadOnlyList)operationList, (IFocusGenericRefreshOperation)refresh);
        }

        /// <summary>
        /// Creates a IxxxReplaceWithCycleOperation object.
        /// </summary>
        private protected virtual IFocusReplaceWithCycleOperation CreateReplaceWithCycleOperation(INode parentNode, string propertyName, int blockIndex, int index, IFocusInsertionChildNodeIndexList cycleIndexList, int cyclePosition, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusController));
            return new FocusReplaceWithCycleOperation(parentNode, propertyName, blockIndex, index, cycleIndexList, cyclePosition, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
