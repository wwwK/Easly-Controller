﻿using BaseNode;
using BaseNodeHelper;
using EaslyController.ReadOnly;
using EaslyController.Writeable;
using System.Collections.Generic;

namespace EaslyController.Frame
{
    /// <summary>
    /// State of an child node.
    /// </summary>
    public interface IFramePlaceholderNodeState : IWriteablePlaceholderNodeState, IFrameNodeState
    {
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new IFrameNodeIndex ParentIndex { get; }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        new IFrameInner<IFrameBrowsingChildIndex> ParentInner { get; }
    }

    /// <summary>
    /// State of an child node.
    /// </summary>
    public class FramePlaceholderNodeState : WriteablePlaceholderNodeState, IFramePlaceholderNodeState
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FramePlaceholderNodeState"/> class.
        /// </summary>
        /// <param name="parentIndex">The index used to create the state.</param>
        public FramePlaceholderNodeState(IFrameNodeIndex parentIndex)
            : base(parentIndex)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new IFrameNodeIndex ParentIndex { get { return (IFrameNodeIndex)base.ParentIndex; } }
        IFrameIndex IFrameNodeState.ParentIndex { get { return ParentIndex; } }

        /// <summary>
        /// Inner containing this state.
        /// </summary>
        public new IFrameInner<IFrameBrowsingChildIndex> ParentInner { get { return (IFrameInner<IFrameBrowsingChildIndex>)base.ParentInner; } }

        /// <summary>
        /// State of the parent.
        /// </summary>
        public new IFrameNodeState ParentState { get { return (IFrameNodeState)base.ParentState; } }

        /// <summary>
        /// Table for all inners in this state.
        /// </summary>
        public new IFrameInnerReadOnlyDictionary<string> InnerTable { get { return (IFrameInnerReadOnlyDictionary<string>)base.InnerTable; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxNodeStateList object.
        /// </summary>
        protected override IReadOnlyNodeStateList CreateNodeStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePlaceholderNodeState));
            return new FrameNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxNodeStateReadOnlyList object.
        /// </summary>
        protected override IReadOnlyNodeStateReadOnlyList CreateNodeStateReadOnlyList(IReadOnlyNodeStateList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePlaceholderNodeState));
            return new FrameNodeStateReadOnlyList((IFrameNodeStateList)list);
        }

        /// <summary>
        /// Creates a IxxxBrowsingPlaceholderNodeIndex object.
        /// </summary>
        protected override IReadOnlyBrowsingPlaceholderNodeIndex CreateChildNodeIndex(IReadOnlyBrowseContext browseNodeContext, string propertyName, INode childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePlaceholderNodeState));
            return new FrameBrowsingPlaceholderNodeIndex(Node, childNode, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingOptionalNodeIndex object.
        /// </summary>
        protected override IReadOnlyBrowsingOptionalNodeIndex CreateOptionalNodeIndex(IReadOnlyBrowseContext browseNodeContext, string propertyName)
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePlaceholderNodeState));
            return new FrameBrowsingOptionalNodeIndex(Node, propertyName);
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndex object.
        /// </summary>
        protected override IReadOnlyBrowsingListNodeIndex CreateListNodeIndex(IReadOnlyBrowseContext browseNodeContext, string propertyName, INode childNode, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePlaceholderNodeState));
            return new FrameBrowsingListNodeIndex(Node, childNode, propertyName, index);
        }

        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        protected override IReadOnlyBrowsingNewBlockNodeIndex CreateNewBlockNodeIndex(IReadOnlyBrowseContext browseNodeContext, string propertyName, INodeTreeBlock childBlock, int blockIndex, INode childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePlaceholderNodeState));
            return new FrameBrowsingNewBlockNodeIndex(Node, childNode, propertyName, blockIndex, childBlock.ReplicationPattern, childBlock.SourceIdentifier);
        }

        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        protected override IReadOnlyBrowsingExistingBlockNodeIndex CreateExistingBlockNodeIndex(IReadOnlyBrowseContext browseNodeContext, string propertyName, int blockIndex, int index, INode childNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePlaceholderNodeState));
            return new FrameBrowsingExistingBlockNodeIndex(Node, childNode, propertyName, blockIndex, index);
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingPlaceholderNodeIndex.
        /// </summary>
        protected override IReadOnlyIndexCollection CreatePlaceholderIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingPlaceholderNodeIndex childNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePlaceholderNodeState));
            return new FrameIndexCollection<IFrameBrowsingPlaceholderNodeIndex>(propertyName, new List<IFrameBrowsingPlaceholderNodeIndex>() { (IFrameBrowsingPlaceholderNodeIndex)childNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with one IxxxBrowsingOptionalNodeIndex.
        /// </summary>
        protected override IReadOnlyIndexCollection CreateOptionalIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingOptionalNodeIndex optionalNodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePlaceholderNodeState));
            return new FrameIndexCollection<IFrameBrowsingOptionalNodeIndex>(propertyName, new List<IFrameBrowsingOptionalNodeIndex>() { (IFrameBrowsingOptionalNodeIndex)optionalNodeIndex });
        }

        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndexList object.
        /// </summary>
        protected override IReadOnlyBrowsingListNodeIndexList CreateBrowsingListNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePlaceholderNodeState));
            return new FrameBrowsingListNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingListNodeIndex objects.
        /// </summary>
        protected override IReadOnlyIndexCollection CreateListIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingListNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePlaceholderNodeState));
            return new FrameIndexCollection<IFrameBrowsingListNodeIndex>(propertyName, (IFrameBrowsingListNodeIndexList)nodeIndexList);
        }

        /// <summary>
        /// Creates a IxxxBrowsingBlockNodeIndexList object.
        /// </summary>
        protected override IReadOnlyBrowsingBlockNodeIndexList CreateBrowsingBlockNodeIndexList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePlaceholderNodeState));
            return new FrameBrowsingBlockNodeIndexList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollection with IxxxBrowsingBlockNodeIndex objects.
        /// </summary>
        protected override IReadOnlyIndexCollection CreateBlockIndexCollection(IReadOnlyBrowseContext browseNodeContext, string propertyName, IReadOnlyBrowsingBlockNodeIndexList nodeIndexList)
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePlaceholderNodeState));
            return new FrameIndexCollection<IFrameBrowsingBlockNodeIndex>(propertyName, (IFrameBrowsingBlockNodeIndexList)nodeIndexList);
        }
        #endregion
    }
}
