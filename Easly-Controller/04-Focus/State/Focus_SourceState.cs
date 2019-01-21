﻿using BaseNode;
using BaseNodeHelper;
using EaslyController.ReadOnly;
using EaslyController.Frame;
using System.Collections.Generic;
using System.Diagnostics;
using Easly;
using System;

namespace EaslyController.Focus
{
    /// <summary>
    /// State of a source identifier node.
    /// </summary>
    public interface IFocusSourceState : IFrameSourceState, IFocusPlaceholderNodeState
    {
        /// <summary>
        /// The parent block state.
        /// </summary>
        new IFocusBlockState ParentBlockState { get; }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new IFocusBrowsingSourceIndex ParentIndex { get; }
    }

    /// <summary>
    /// State of a source identifier node.
    /// </summary>
    public class FocusSourceState : FrameSourceState, IFocusSourceState
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusSourceState"/> class.
        /// </summary>
        /// <param name="parentBlockState">The parent block state.</param>
        /// <param name="index">The index used to create the state.</param>
        public FocusSourceState(IFocusBlockState parentBlockState, IFocusBrowsingSourceIndex index)
            : base(parentBlockState, index)
        {
            CycleIndexList = null;
            CycleCurrentPosition = -1;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The parent block state.
        /// </summary>
        public new IFocusBlockState ParentBlockState { get { return (IFocusBlockState)base.ParentBlockState; } }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new IFocusBrowsingSourceIndex ParentIndex { get { return (IFocusBrowsingSourceIndex)base.ParentIndex; } }
        IFocusIndex IFocusNodeState.ParentIndex { get { return ParentIndex; } }
        IFocusNodeIndex IFocusPlaceholderNodeState.ParentIndex { get { return ParentIndex; } }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        public new IFocusInner<IFocusBrowsingChildIndex> ParentInner { get { return (IFocusInner<IFocusBrowsingChildIndex>)base.ParentInner; } }

        /// <summary>
        /// State of the parent.
        /// </summary>
        public new IFocusNodeState ParentState { get { return (IFocusNodeState)base.ParentState; } }

        /// <summary>
        /// Table for all inners in this state.
        /// </summary>
        public new IFocusInnerReadOnlyDictionary<string> InnerTable { get { return (IFocusInnerReadOnlyDictionary<string>)base.InnerTable; } }

        /// <summary>
        /// List of node indexes that can replace the current node. Can be null.
        /// Applies only to bodies and features.
        /// </summary>
        public IFocusInsertionChildIndexList CycleIndexList { get; private set; }

        /// <summary>
        /// Position of the current node in <see cref="CycleIndexList"/>.
        /// </summary>
        public int CycleCurrentPosition { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Adds a new node to the list of nodes that can replace the current one. Does nothing if all types of nodes have been added.
        /// Applies only to bodies and features.
        /// </summary>
        public virtual void AddNodeToCycle()
        {
            Debug.Assert((Node is IBody) || (Node is IFeature));

            // If it's the first time we're cycling through this node, initialize it.
            if (CycleIndexList == null)
            {
                IFocusInsertionChildIndex ThisIndex = (IFocusInsertionChildIndex)ParentIndex.ToInsertionIndex(ParentState.Node, Node);
                CycleIndexList = CreateInsertionChildIndexList();
                CycleIndexList.Add(ThisIndex);
            }

            switch (Node)
            {
                case IBody AsBody:
                    AddBodyToCycle();
                    break;

                case IFeature AsFeature:
                    AddFeatureToCycle();
                    break;
            }

            for (int i = 0; i < CycleIndexList.Count; i++)
                if (CycleIndexList[i].Node == Node)
                {
                    CycleCurrentPosition = i;
                    break;
                }

            Debug.Assert(CycleCurrentPosition >= 0 && CycleCurrentPosition < CycleIndexList.Count);
        }

        /// <summary></summary>
        protected virtual void AddBodyToCycle()
        {
            Debug.Assert(CycleIndexList != null);
            Debug.Assert(CycleIndexList.Count > 0);

            IDocument Documentation = null;
            IBlockList<IAssertion, Assertion> RequireBlocks = null;
            IBlockList<IAssertion, Assertion> EnsureBlocks = null;
            IBlockList<IIdentifier, Identifier> ExceptionIdentifierBlocks = null;
            IBlockList<IEntityDeclaration, EntityDeclaration> EntityDeclarationBlocks = null;
            IBlockList<IInstruction, Instruction> BodyInstructionBlocks = null;
            IBlockList<IExceptionHandler, ExceptionHandler> ExceptionHandlerBlocks = null;
            IOptionalReference<IObjectType> AncestorType = null;

            List<Type> BodyTypeList = new List<Type>() { typeof(EffectiveBody), typeof(DeferredBody), typeof(ExternBody), typeof(PrecursorBody) };
            foreach (IFocusInsertionChildIndex Index in CycleIndexList)
            {
                IBody Body = Index.Node as IBody;
                Debug.Assert(Body != null);

                if (BodyTypeList.Contains(Body.GetType()))
                    BodyTypeList.Remove(Body.GetType());

                switch (Body)
                {
                    case IEffectiveBody AsEffective:
                        Documentation = AsEffective.Documentation;
                        RequireBlocks = AsEffective.RequireBlocks;
                        EnsureBlocks = AsEffective.EnsureBlocks;
                        ExceptionIdentifierBlocks = AsEffective.ExceptionIdentifierBlocks;
                        EntityDeclarationBlocks = AsEffective.EntityDeclarationBlocks;
                        BodyInstructionBlocks = AsEffective.BodyInstructionBlocks;
                        ExceptionHandlerBlocks = AsEffective.ExceptionHandlerBlocks;
                        break;

                    case IDeferredBody AsDeferred:
                        Documentation = AsDeferred.Documentation;
                        RequireBlocks = AsDeferred.RequireBlocks;
                        EnsureBlocks = AsDeferred.EnsureBlocks;
                        ExceptionIdentifierBlocks = AsDeferred.ExceptionIdentifierBlocks;
                        break;

                    case IExternBody AsExtern:
                        Documentation = AsExtern.Documentation;
                        RequireBlocks = AsExtern.RequireBlocks;
                        EnsureBlocks = AsExtern.EnsureBlocks;
                        ExceptionIdentifierBlocks = AsExtern.ExceptionIdentifierBlocks;
                        break;

                    case IPrecursorBody AsPrecursor:
                        Documentation = AsPrecursor.Documentation;
                        RequireBlocks = AsPrecursor.RequireBlocks;
                        EnsureBlocks = AsPrecursor.EnsureBlocks;
                        ExceptionIdentifierBlocks = AsPrecursor.ExceptionIdentifierBlocks;
                        AncestorType = AsPrecursor.AncestorType;
                        break;
                }
            }

            // If the list is full, no need to add more nodes to the cycle.
            if (BodyTypeList.Count > 0)
            {
                Type NodeType = BodyTypeList[0];

                INode NewBody = NodeHelper.CreateInitializedBody(NodeType, Documentation, RequireBlocks, EnsureBlocks, ExceptionIdentifierBlocks, EntityDeclarationBlocks, BodyInstructionBlocks, ExceptionHandlerBlocks, AncestorType);

                IFocusInsertionChildIndex InsertionIndex = (IFocusInsertionChildIndex)ParentIndex.ToInsertionIndex(ParentState.Node, NewBody);
                CycleIndexList.Add(InsertionIndex);
            }
        }

        /// <summary></summary>
        protected virtual void AddFeatureToCycle()
        {
            Debug.Assert(CycleIndexList != null);
            Debug.Assert(CycleIndexList.Count > 0);

            IDocument Documentation = null;
            IIdentifier ExportIdentifier = null;
            ExportStatus Export = ExportStatus.Exported;
            IName EntityName = null;
            IObjectType EntityType = null;
            IBlockList<IAssertion, Assertion> EnsureBlocks = null;
            IExpression ConstantValue = null;
            IBlockList<ICommandOverload, CommandOverload> CommandOverloadBlocks = null;
            OnceChoice Once = OnceChoice.Normal;
            IBlockList<IQueryOverload, QueryOverload> QueryOverloadBlocks = null;
            UtilityType PropertyKind = UtilityType.ReadWrite;
            IBlockList<IIdentifier, Identifier> ModifiedQueryBlocks = null;
            IOptionalReference<IBody> GetterBody = null;
            IOptionalReference<IBody> SetterBody = null;
            IBlockList<IEntityDeclaration, EntityDeclaration> IndexParameterBlocks = null;
            ParameterEndStatus ParameterEnd = ParameterEndStatus.Closed;

            List<Type> FeatureTypeList = new List<Type>() { typeof(AttributeFeature), typeof(ConstantFeature), typeof(CreationFeature), typeof(FunctionFeature), typeof(ProcedureFeature), typeof(PropertyFeature), typeof(IndexerFeature) };
            foreach (IFocusInsertionChildIndex Index in CycleIndexList)
            {
                IFeature Feature = Index.Node as IFeature;
                Debug.Assert(Feature != null);

                if (FeatureTypeList.Contains(Feature.GetType()))
                    FeatureTypeList.Remove(Feature.GetType());

                switch (Feature)
                {
                    case IAttributeFeature AsAttribute:
                        Documentation = AsAttribute.Documentation;
                        ExportIdentifier = AsAttribute.ExportIdentifier;
                        Export = AsAttribute.Export;
                        EntityName = AsAttribute.EntityName;
                        EntityType = AsAttribute.EntityType;
                        EnsureBlocks = AsAttribute.EnsureBlocks;
                        break;

                    case IConstantFeature AsConstant:
                        Documentation = AsConstant.Documentation;
                        ExportIdentifier = AsConstant.ExportIdentifier;
                        Export = AsConstant.Export;
                        EntityName = AsConstant.EntityName;
                        EntityType = AsConstant.EntityType;
                        ConstantValue = AsConstant.ConstantValue;
                        break;

                    case ICreationFeature AsCreation:
                        Documentation = AsCreation.Documentation;
                        ExportIdentifier = AsCreation.ExportIdentifier;
                        Export = AsCreation.Export;
                        EntityName = AsCreation.EntityName;
                        CommandOverloadBlocks = AsCreation.OverloadBlocks;
                        break;

                    case IFunctionFeature AsFunction:
                        Documentation = AsFunction.Documentation;
                        ExportIdentifier = AsFunction.ExportIdentifier;
                        Export = AsFunction.Export;
                        EntityName = AsFunction.EntityName;
                        Once = AsFunction.Once;
                        QueryOverloadBlocks = AsFunction.OverloadBlocks;
                        break;

                    case IProcedureFeature AsProcedure:
                        Documentation = AsProcedure.Documentation;
                        ExportIdentifier = AsProcedure.ExportIdentifier;
                        Export = AsProcedure.Export;
                        EntityName = AsProcedure.EntityName;
                        CommandOverloadBlocks = AsProcedure.OverloadBlocks;
                        break;

                    case IPropertyFeature AsProperty:
                        Documentation = AsProperty.Documentation;
                        ExportIdentifier = AsProperty.ExportIdentifier;
                        Export = AsProperty.Export;
                        EntityName = AsProperty.EntityName;
                        EntityType = AsProperty.EntityType;
                        PropertyKind = AsProperty.PropertyKind;
                        ModifiedQueryBlocks = AsProperty.ModifiedQueryBlocks;
                        GetterBody = AsProperty.GetterBody;
                        SetterBody = AsProperty.SetterBody;
                        break;

                    case IIndexerFeature AsIndexer:
                        Documentation = AsIndexer.Documentation;
                        ExportIdentifier = AsIndexer.ExportIdentifier;
                        Export = AsIndexer.Export;
                        EntityType = AsIndexer.EntityType;
                        IndexParameterBlocks = AsIndexer.IndexParameterBlocks;
                        ParameterEnd = AsIndexer.ParameterEnd;
                        ModifiedQueryBlocks = AsIndexer.ModifiedQueryBlocks;
                        GetterBody = AsIndexer.GetterBody;
                        SetterBody = AsIndexer.SetterBody;
                        break;
                }
            }

            // If the list is full, no need to add more nodes to the cycle.
            if (FeatureTypeList.Count > 0)
            {
                Type NodeType = FeatureTypeList[0];

                INode NewFeature = NodeHelper.CreateInitializedFeature(NodeType, Documentation, ExportIdentifier, Export, EntityName, EntityType, EnsureBlocks, ConstantValue, CommandOverloadBlocks, Once, QueryOverloadBlocks, PropertyKind, ModifiedQueryBlocks, GetterBody, SetterBody, IndexParameterBlocks, ParameterEnd);

                IFocusInsertionChildIndex InsertionIndex = (IFocusInsertionChildIndex)ParentIndex.ToInsertionIndex(ParentState.Node, NewFeature);
                CycleIndexList.Add(InsertionIndex);
            }
        }

        /// <summary>
        /// Restores the cycle index list from which this state was created.
        /// </summary>
        /// <param name="cycleIndexList">The list to restore.</param>
        public virtual void RestoreCycleIndexList(IFocusInsertionChildIndexList cycleIndexList)
        {
            Debug.Assert(cycleIndexList != null && cycleIndexList.Count >= 2);
            Debug.Assert(CycleIndexList == null);

            CycleIndexList = cycleIndexList;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxNodeStateList object.
        /// </summary>
        protected override IReadOnlyNodeStateList CreateNodeStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusSourceState));
            return new FocusNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxNodeStateReadOnlyList object.
        /// </summary>
        protected override IReadOnlyNodeStateReadOnlyList CreateNodeStateReadOnlyList(IReadOnlyNodeStateList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusSourceState));
            return new FocusNodeStateReadOnlyList((IFocusNodeStateList)list);
        }

        /// <summary>
        /// Creates a IxxxBrowsingPlaceholderNodeIndex object.
        /// </summary>
        protected override IReadOnlyBrowsingPlaceholderNodeIndex CreateChildNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName, INode childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusSourceState));
            return new FocusBrowsingPlaceholderNodeIndex(node, childNode, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingOptionalNodeIndex object.
        /// </summary>
        protected override IReadOnlyBrowsingOptionalNodeIndex CreateOptionalNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusSourceState));
            return new FocusBrowsingOptionalNodeIndex(node, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndex object.
        /// </summary>
        protected override IReadOnlyBrowsingListNodeIndex CreateListNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName, INode childNode, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusSourceState));
            return new FocusBrowsingListNodeIndex(node, childNode, propertyName, index);
        }

        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        protected override IReadOnlyBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName, INodeTreeBlock childBlock, int blockIndex, INode childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusSourceState));
            return new FocusBrowsingNewBlockNodeIndex(node, childNode, propertyName, blockIndex, childBlock.ReplicationPattern, childBlock.SourceIdentifier);
        }

        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        protected override IReadOnlyBrowsingExistingBlockNodeIndex CreateExistingBlockNodeIndex(IReadOnlyBrowseContext browseNodeContext, INode node, string propertyName, int blockIndex, int index, INode childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusSourceState));
            return new FocusBrowsingExistingBlockNodeIndex(node, childNode, propertyName, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingPlaceholderNodeIndex.
        /// </summary>
        protected override IReadOnlyIndexCollection CreatePlaceholderIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingPlaceholderNodeIndex childNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusSourceState));
            return new FocusIndexCollection<IFocusBrowsingPlaceholderNodeIndex>(propertyName, new List<IFocusBrowsingPlaceholderNodeIndex>() { (IFocusBrowsingPlaceholderNodeIndex)childNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingOptionalNodeIndex.
        /// </summary>
        protected override IReadOnlyIndexCollection CreateOptionalIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingOptionalNodeIndex optionalNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusSourceState));
            return new FocusIndexCollection<IFocusBrowsingOptionalNodeIndex>(propertyName, new List<IFocusBrowsingOptionalNodeIndex>() { (IFocusBrowsingOptionalNodeIndex)optionalNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndexList object.
        /// </summary>
        protected override IReadOnlyBrowsingListNodeIndexList CreateBrowsingListNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusSourceState));
            return new FocusBrowsingListNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingListNodeIndex objects.
        /// </summary>
        protected override IReadOnlyIndexCollection CreateListIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingListNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusSourceState));
            return new FocusIndexCollection<IFocusBrowsingListNodeIndex>(propertyName, (IFocusBrowsingListNodeIndexList)nodeIndexList);
        }

        /// <summary>
        /// Creates a IxxxBrowsingBlockNodeIndexList object.
        /// </summary>
        protected override IReadOnlyBrowsingBlockNodeIndexList CreateBrowsingBlockNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusSourceState));
            return new FocusBrowsingBlockNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingBlockNodeIndex objects.
        /// </summary>
        protected override IReadOnlyIndexCollection CreateBlockIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingBlockNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusSourceState));
            return new FocusIndexCollection<IFocusBrowsingBlockNodeIndex>(propertyName, (IFocusBrowsingBlockNodeIndexList)nodeIndexList);
        }

        /// <summary>
        /// Creates a IxxxInsertionChildIndexList object.
        /// </summary>
        protected virtual IFocusInsertionChildIndexList CreateInsertionChildIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusSourceState));
            return new FocusInsertionChildIndexList();
        }
        #endregion
    }
}
