﻿using BaseNode;
using BaseNodeHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Base interface for the state of a node.
    /// </summary>
    public interface IReadOnlyNodeState : IReadOnlyState
    {
        /// <summary>
        /// The node.
        /// </summary>
        INode Node { get; }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        IReadOnlyIndex ParentIndex { get; }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        IReadOnlyInner<IReadOnlyBrowsingChildIndex> ParentInner { get; }

        /// <summary>
        /// State of the parent.
        /// </summary>
        IReadOnlyNodeState ParentState { get; }

        /// <summary>
        /// Table for all inners in this state.
        /// </summary>
        IReadOnlyInnerReadOnlyDictionary<string> InnerTable { get; }

        /// <summary>
        /// Table of children that are not nodes.
        /// </summary>
        IReadOnlyDictionary<string, ValuePropertyType> ValuePropertyTypeTable { get; }

        /// <summary>
        /// Find children in the node tree.
        /// </summary>
        /// <param name="browseContext">The context used to browse the node tree.</param>
        void BrowseChildren(IReadOnlyBrowseContext browseContext);

        /// <summary>
        /// Initializes a newly created state.
        /// </summary>
        /// <param name="browseContext">The context that was used to find the node.</param>
        /// <param name="parentInner">Inner containing this state.</param>
        /// <param name="innerTable">Table for all inners in this state.</param>
        /// <param name="valuePropertyTable">Table of children that are not nodes.</param>
        void Init(IReadOnlyBrowseContext browseContext, IReadOnlyInner<IReadOnlyBrowsingChildIndex> parentInner, IReadOnlyInnerReadOnlyDictionary<string> innerTable, IReadOnlyDictionary<string, ValuePropertyType> valuePropertyTable);

        /// <summary>
        /// Gets the first inner corresponding to the first optional child node that is not assigned.
        /// </summary>
        /// <param name="inner">The inner corresponding to the first optional child node that is not assigned, or null if none.</param>
        /// <returns>True if an unassigned optional child was found, false if the state contains no optional node child or if they are all assigned.</returns>
        bool FindFirstUnassignedOptional(out IReadOnlyOptionalInner inner);

        /// <summary>
        /// Returns a list of states for all child nodes.
        /// </summary>
        IReadOnlyNodeStateReadOnlyList GetAllChildren();

        /// <summary>
        /// Returns a clone of the node of this state.
        /// </summary>
        /// <returns>The cloned node.</returns>
        INode CloneNode();
    }

    /// <summary>
    /// Base class for the state of a node.
    /// </summary>
    public abstract class ReadOnlyNodeState : IReadOnlyNodeState
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyNodeState"/> class.
        /// </summary>
        /// <param name="parentIndex">The index used to create the state.</param>
        public ReadOnlyNodeState(IReadOnlyIndex parentIndex)
        {
            Debug.Assert(parentIndex != null);

            ParentIndex = parentIndex;
            ValuePropertyTypeTable = null;
            IsInitialized = false;
        }

        /// <summary>
        /// Initializes a newly created state.
        /// </summary>
        /// <param name="browseContext">The context that was used to find the node.</param>
        /// <param name="parentInner">Inner containing this state.</param>
        /// <param name="innerTable">Table for all inners in this state.</param>
        /// <param name="valuePropertyTable">Table of children that are not nodes.</param>
        public virtual void Init(IReadOnlyBrowseContext browseContext, IReadOnlyInner<IReadOnlyBrowsingChildIndex> parentInner, IReadOnlyInnerReadOnlyDictionary<string> innerTable, IReadOnlyDictionary<string, ValuePropertyType> valuePropertyTable)
        {
            Debug.Assert(browseContext != null);
            Debug.Assert(innerTable != null);
            Debug.Assert(valuePropertyTable != null);
            Debug.Assert(!IsInitialized);
            Debug.Assert(ParentInner == null);
            Debug.Assert(InnerTable == null);
            Debug.Assert(ValuePropertyTypeTable == null);

            InitParentInner(parentInner);
            if (parentInner != null) // The root node doesn't have a parent inner.
                InitParentState(ParentInner.Owner);
            InitInnerTable(innerTable);

            Dictionary<string, ValuePropertyType> Table = new Dictionary<string, ValuePropertyType>();
            foreach (KeyValuePair<string, ValuePropertyType> Entry in valuePropertyTable)
                Table.Add(Entry.Key, Entry.Value);

            ValuePropertyTypeTable = Table;
            IsInitialized = true;

            CheckInvariant();
        }

        protected void InitParentState(IReadOnlyNodeState parentState)
        {
            Debug.Assert(parentState != null);
            Debug.Assert(ParentState == null);

            ParentState = parentState;
        }

        protected void InitParentInner(IReadOnlyInner<IReadOnlyBrowsingChildIndex> parentInner)
        {
            Debug.Assert(ParentInner == null);

            ParentInner = parentInner;
        }

        protected virtual void InitInnerTable(IReadOnlyInnerReadOnlyDictionary<string> innerTable)
        {
            Debug.Assert(innerTable != null);
            Debug.Assert(InnerTable == null);

            InnerTable = innerTable;
        }

        /// <summary>
        /// Find children in the node tree.
        /// </summary>
        /// <param name="browseContext">The context used to browse the node tree.</param>
        public virtual void BrowseChildren(IReadOnlyBrowseContext browseNodeContext)
        {
            IList<string> PropertyNames = NodeTreeHelper.EnumChildNodeProperties(Node);
            bool IsAssigned;
            INode ChildNode;
            Type ChildInterfaceType, ChildNodeType;
            IReadOnlyList<INode> ChildNodeList;
            IReadOnlyList<INodeTreeBlock> ChildBlockList;

            foreach (string PropertyName in PropertyNames)
            {
                if (NodeTreeHelper.IsChildNodeProperty(Node, PropertyName, out ChildNodeType))
                {
                    NodeTreeHelper.GetChildNode(Node, PropertyName, out IsAssigned, out ChildNode);
                    IReadOnlyBrowsingPlaceholderNodeIndex ChildNodeIndex = CreateChildNodeIndex(browseNodeContext, PropertyName, ChildNode);

                    // Create a collection containing one index for this child node.
                    IReadOnlyIndexCollection IndexCollection = CreatePlaceholderIndexCollection(browseNodeContext, PropertyName, ChildNodeIndex);
                    browseNodeContext.AddIndexCollection(IndexCollection);
                }

                else if (NodeTreeHelper.IsOptionalChildNodeProperty(Node, PropertyName, out ChildNodeType))
                {
                    IReadOnlyBrowsingOptionalNodeIndex OptionalNodeIndex = CreateOptionalNodeIndex(browseNodeContext, PropertyName);

                    // Create a collection containing one index for this optional node.
                    IReadOnlyIndexCollection IndexCollection = CreateOptionalIndexCollection(browseNodeContext, PropertyName, OptionalNodeIndex);
                    browseNodeContext.AddIndexCollection(IndexCollection);
                }

                else if (NodeTreeHelper.IsChildNodeList(Node, PropertyName, out ChildNodeType))
                {
                    NodeTreeHelper.GetChildNodeList(Node, PropertyName, out ChildNodeList);

                    // Create a collection containing indexes for each children.
                    IReadOnlyIndexCollection IndexCollection = BrowseNodeList(browseNodeContext, PropertyName, ChildNodeList);
                    browseNodeContext.AddIndexCollection(IndexCollection);
                }

                else if (NodeTreeHelper.IsChildBlockList(Node, PropertyName, out ChildInterfaceType, out ChildNodeType))
                {
                    NodeTreeHelper.GetChildBlockList(Node, PropertyName, out ChildBlockList);

                    // Create a collection containing indexes for each child blocks and their children.
                    IReadOnlyIndexCollection IndexCollection = BrowseNodeBlockList(browseNodeContext, PropertyName, ChildBlockList);
                    browseNodeContext.AddIndexCollection(IndexCollection);
                }

                else if (NodeTreeHelper.IsBooleanProperty(Node, PropertyName))
                    browseNodeContext.AddValueProperty(PropertyName, ValuePropertyType.Boolean);

                else if (NodeTreeHelper.IsEnumProperty(Node, PropertyName))
                    browseNodeContext.AddValueProperty(PropertyName, ValuePropertyType.Enum);

                else if (NodeTreeHelper.IsStringProperty(Node, PropertyName))
                    browseNodeContext.AddValueProperty(PropertyName, ValuePropertyType.String);

                else if (NodeTreeHelper.IsGuidProperty(Node, PropertyName))
                    browseNodeContext.AddValueProperty(PropertyName, ValuePropertyType.Guid);

                else if (NodeTreeHelper.IsDocumentProperty(Node, PropertyName))
                { } // Ignore the doc node.

                else
                    throw new ArgumentOutOfRangeException(nameof(PropertyName));
            }
        }

        protected virtual IReadOnlyIndexCollection BrowseNodeList(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyList<INode> childNodeList)
        {
            Debug.Assert(!string.IsNullOrEmpty(propertyName));

            IReadOnlyBrowsingListNodeIndexList NodeIndexList = CreateBrowsingListNodeIndexList();

            for (int Index = 0; Index < childNodeList.Count; Index++)
            {
                INode ChildNode = childNodeList[Index];

                IReadOnlyBrowsingListNodeIndex NewNodeIndex = CreateListNodeIndex(browseNodeContext, propertyName, ChildNode, Index);
                NodeIndexList.Add(NewNodeIndex);
            }

            return CreateListIndexCollection(browseNodeContext, propertyName, NodeIndexList);
        }

        protected virtual IReadOnlyIndexCollection BrowseNodeBlockList(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyList<INodeTreeBlock> childBlockList)
        {
            Debug.Assert(!string.IsNullOrEmpty(propertyName));

            IReadOnlyBrowsingBlockNodeIndexList NodeIndexList = CreateBrowsingBlockNodeIndexList();

            for (int BlockIndex = 0; BlockIndex < childBlockList.Count; BlockIndex++)
            {
                INodeTreeBlock ChildBlock = childBlockList[BlockIndex];
                BrowseBlock(browseNodeContext, propertyName, BlockIndex, ChildBlock, NodeIndexList);
            }

            return CreateBlockIndexCollection(browseNodeContext, propertyName, NodeIndexList);
        }

        protected virtual void BrowseBlock(IReadOnlyBrowseContext browseNodeContext, string propertyName, int blockIndex, INodeTreeBlock childBlock, IReadOnlyBrowsingBlockNodeIndexList nodeIndexList)
        {
            Debug.Assert(!string.IsNullOrEmpty(propertyName));

            for (int Index = 0; Index < childBlock.NodeList.Count; Index++)
            {
                INode ChildNode = childBlock.NodeList[Index];

                IReadOnlyBrowsingBlockNodeIndex NewNodeIndex;
                if (Index == 0) // For the first node, we use a IxxxBrowsingNewBlockNodeIndex, otherwise a IxxxBrowsingExistingBlockNodeIndex.
                    NewNodeIndex = CreateNewBlockNodeIndex(browseNodeContext, propertyName, childBlock, ChildNode, blockIndex);
                else
                    NewNodeIndex = CreateExistingBlockNodeIndex(browseNodeContext, propertyName, ChildNode, blockIndex, Index);

                nodeIndexList.Add(NewNodeIndex);
            }
        }

        protected bool IsInitialized { get; private set; }
        #endregion

        #region Properties
        /// <summary>
        /// The node.
        /// </summary>
        public abstract INode Node { get; }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public IReadOnlyIndex ParentIndex { get; }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        public IReadOnlyInner<IReadOnlyBrowsingChildIndex> ParentInner { get; private set; }

        /// <summary>
        /// State of the parent.
        /// </summary>
        public IReadOnlyNodeState ParentState { get; private set; }

        /// <summary>
        /// Table for all inners in this state.
        /// </summary>
        public IReadOnlyInnerReadOnlyDictionary<string> InnerTable { get; private set; }

        /// <summary>
        /// Table of children that are not nodes.
        /// </summary>
        public IReadOnlyDictionary<string, ValuePropertyType> ValuePropertyTypeTable { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Gets the inner corresponding to a property in the node.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        public virtual IReadOnlyInner<IReadOnlyBrowsingChildIndex> PropertyToInner(string propertyName)
        {
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(InnerTable.ContainsKey(propertyName));

            return InnerTable[propertyName];
        }

        /// <summary>
        /// Gets the first inner corresponding to the first optional child node that is not assigned.
        /// </summary>
        /// <param name="inner">The inner corresponding to the first optional child node that is not assigned, or null if none.</param>
        /// <returns>True if an unassigned optional child was found, false if the state contains no optional node child or if they are all assigned.</returns>
        public virtual bool FindFirstUnassignedOptional(out IReadOnlyOptionalInner inner)
        {
            if (ParentInner is IReadOnlyOptionalInner AsOptional)
                if (!AsOptional.IsAssigned)
                {
                    inner = AsOptional;
                    return true;
                }

            if (ParentState == null)
            {
                inner = null;
                return false;
            }

            return ParentState.FindFirstUnassignedOptional(out inner);
        }

        /// <summary>
        /// Returns a list of states for all child nodes.
        /// </summary>
        public IReadOnlyNodeStateReadOnlyList GetAllChildren()
        {
            IReadOnlyNodeStateList StateList = CreateNodeStateList();
            AddChildStates(StateList, this);

            return CreateNodeStateReadOnlyList(StateList);
        }

        private void AddChildStates(IReadOnlyNodeStateList stateList, IReadOnlyNodeState state)
        {
            stateList.Add(state);

            foreach (KeyValuePair<string, IReadOnlyInner<IReadOnlyBrowsingChildIndex>> Entry in state.InnerTable)
                AddChildInner(stateList, Entry.Value);
        }

        private void AddChildInner(IReadOnlyNodeStateList stateList, IReadOnlyInner<IReadOnlyBrowsingChildIndex> inner)
        {
            switch (inner)
            {
                case IReadOnlySingleInner AsSingle:
                    AddChildStates(stateList, AsSingle.ChildState);
                    break;

                case IReadOnlyListInner AsList:
                    foreach (IReadOnlyNodeState ChildState in AsList.StateList)
                        AddChildStates(stateList, ChildState);
                    break;

                case IReadOnlyBlockListInner AsBlockList:
                    foreach (IReadOnlyBlockState Block in AsBlockList.BlockStateList)
                    {
                        stateList.Add(Block.PatternState);
                        stateList.Add(Block.SourceState);

                        foreach (IReadOnlyNodeState ChildState in Block.StateList)
                            AddChildStates(stateList, ChildState);
                    }
                    break;

                default:
                    throw new InvalidCastException(nameof(inner));
            }
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Returns a clone of the node of this state.
        /// </summary>
        /// <returns>The cloned node.</returns>
        public INode CloneNode()
        {
            // Create a clone, initially empty and full of null references.
            INode NewNode = NodeHelper.CreateEmptyNode(Node.GetType());

            // Clone and assign reference to all nodes, optional or not, list and block lists.
            foreach (KeyValuePair<string, IReadOnlyInner<IReadOnlyBrowsingChildIndex>> Entry in InnerTable)
            {
                string PropertyName = Entry.Key;
                IReadOnlyInner<IReadOnlyBrowsingChildIndex> Inner = Entry.Value;
                Inner.CloneChildren(NewNode);
            }

            // Copy other properties.
            foreach (KeyValuePair<string, ValuePropertyType> Entry in ValuePropertyTypeTable)
            {
                string PropertyName = Entry.Key;
                ValuePropertyType Type = Entry.Value;

                switch (Type)
                {
                    case ValuePropertyType.Boolean:
                        NodeTreeHelper.CopyBooleanProperty(Node, NewNode, Entry.Key);
                        break;
                    case ValuePropertyType.Enum:
                        NodeTreeHelper.CopyEnumProperty(Node, NewNode, Entry.Key);
                        break;
                    case ValuePropertyType.String:
                        NodeTreeHelper.CopyStringProperty(Node, NewNode, Entry.Key);
                        break;
                    case ValuePropertyType.Guid:
                        NodeTreeHelper.CopyGuidProperty(Node, NewNode, Entry.Key);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Type));
                }
            }

            // Also copy comments.
            NodeTreeHelper.CopyDocumentation(Node, NewNode);

            return NewNode;
        }
        #endregion

        #region Invariant
        protected virtual void CheckInvariant()
        {
            InvariantAssert(IsInitialized);
            InvariantAssert(Node != null);
            InvariantAssert(InnerTable != null);

            foreach (KeyValuePair<string, IReadOnlyInner<IReadOnlyBrowsingChildIndex>> Entry in InnerTable)
            {
                IReadOnlyInner<IReadOnlyBrowsingChildIndex> Inner = Entry.Value;

                InvariantAssert((Inner is IReadOnlyBlockListInner) || (Inner is IReadOnlyListInner) || (Inner is IReadOnlyOptionalInner) || (Inner is IReadOnlyPlaceholderInner));
                InvariantAssert(Inner.Owner == this);
            }
        }

        protected void InvariantAssert(bool Condition)
        {
            Debug.Assert(Condition);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxNodeStateList object.
        /// </summary>
        protected virtual IReadOnlyNodeStateList CreateNodeStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxNodeStateReadOnlyList object.
        /// </summary>
        protected virtual IReadOnlyNodeStateReadOnlyList CreateNodeStateReadOnlyList(IReadOnlyNodeStateList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyNodeStateReadOnlyList(list);
        }

        /// <summary>
        /// Creates a IxxxBrowsingPlaceholderNodeIndex object.
        /// </summary>
        protected virtual IReadOnlyBrowsingPlaceholderNodeIndex CreateChildNodeIndex(IReadOnlyBrowseContext browseNodeContext, string propertyName, INode childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyBrowsingPlaceholderNodeIndex(Node, childNode, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingOptionalNodeIndex object.
        /// </summary>
        protected virtual IReadOnlyBrowsingOptionalNodeIndex CreateOptionalNodeIndex(IReadOnlyBrowseContext browseNodeContext, string propertyName)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyBrowsingOptionalNodeIndex(Node, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndex object.
        /// </summary>
        protected virtual IReadOnlyBrowsingListNodeIndex CreateListNodeIndex(IReadOnlyBrowseContext browseNodeContext, string propertyName, INode childNode, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyBrowsingListNodeIndex(Node, childNode, propertyName, index);
        }

        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        protected virtual IReadOnlyBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(IReadOnlyBrowseContext browseNodeContext, string propertyName, INodeTreeBlock childBlock, INode childNode, int blockIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyBrowsingNewBlockNodeIndex(Node, childNode, propertyName, blockIndex, childBlock.ReplicationPattern, childBlock.SourceIdentifier);
        }

        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        protected virtual IReadOnlyBrowsingExistingBlockNodeIndex CreateExistingBlockNodeIndex(IReadOnlyBrowseContext browseNodeContext, string propertyName, INode childNode, int blockIndex, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyBrowsingExistingBlockNodeIndex(Node, childNode, propertyName, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingPlaceholderNodeIndex.
        /// </summary>
        protected virtual IReadOnlyIndexCollection CreatePlaceholderIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingPlaceholderNodeIndex childNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyIndexCollection<IReadOnlyBrowsingPlaceholderNodeIndex>(propertyName, new List<IReadOnlyBrowsingPlaceholderNodeIndex>() { childNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingOptionalNodeIndex.
        /// </summary>
        protected virtual IReadOnlyIndexCollection CreateOptionalIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingOptionalNodeIndex optionalNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyIndexCollection<IReadOnlyBrowsingOptionalNodeIndex>(propertyName, new List<IReadOnlyBrowsingOptionalNodeIndex>() { optionalNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndexList object.
        /// </summary>
        protected virtual IReadOnlyBrowsingListNodeIndexList CreateBrowsingListNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyBrowsingListNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingListNodeIndex objects.
        /// </summary>
        protected virtual IReadOnlyIndexCollection CreateListIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingListNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyIndexCollection<IReadOnlyBrowsingListNodeIndex>(propertyName, nodeIndexList);
        }

        /// <summary>
        /// Creates a IxxxBrowsingBlockNodeIndexList object.
        /// </summary>
        protected virtual IReadOnlyBrowsingBlockNodeIndexList CreateBrowsingBlockNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyBrowsingBlockNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingBlockNodeIndex objects.
        /// </summary>
        protected virtual IReadOnlyIndexCollection CreateBlockIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingBlockNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyNodeState));
            return new ReadOnlyIndexCollection<IReadOnlyBrowsingBlockNodeIndex>(propertyName, nodeIndexList);
        }
        #endregion
    }
}