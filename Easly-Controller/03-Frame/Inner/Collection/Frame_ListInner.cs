﻿using BaseNode;
using BaseNodeHelper;
using EaslyController.ReadOnly;
using EaslyController.Writeable;
using System;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Inner for a list of nodes.
    /// </summary>
    public interface IFrameListInner : IWriteableListInner, IFrameCollectionInner
    {
        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        new IFramePlaceholderNodeStateReadOnlyList StateList { get; }
    }

    /// <summary>
    /// Inner for a list of nodes.
    /// </summary>
    public interface IFrameListInner<out IIndex> : IWriteableListInner<IIndex>, IFrameCollectionInner<IIndex>
        where IIndex : IFrameBrowsingListNodeIndex
    {
        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        new IFramePlaceholderNodeStateReadOnlyList StateList { get; }
    }

    /// <summary>
    /// Inner for a list of nodes.
    /// </summary>
    public class FrameListInner<IIndex, TIndex> : WriteableListInner<IIndex, TIndex>, IFrameListInner<IIndex>, IFrameListInner
        where IIndex : IFrameBrowsingListNodeIndex
        where TIndex : FrameBrowsingListNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameListInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public FrameListInner(IFrameNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Parent containing the inner.
        /// </summary>
        public new IFrameNodeState Owner { get { return (IFrameNodeState)base.Owner; } }

        /// <summary>
        /// States of nodes in the list.
        /// </summary>
        public new IFramePlaceholderNodeStateReadOnlyList StateList { get { return (IFramePlaceholderNodeStateReadOnlyList)base.StateList; } }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        public new IFramePlaceholderNodeState FirstNodeState { get { return (IFramePlaceholderNodeState)base.FirstNodeState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateList object.
        /// </summary>
        protected override IReadOnlyPlaceholderNodeStateList CreateStateList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameListInner<IIndex, TIndex>));
            return new FramePlaceholderNodeStateList();
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateReadOnlyList object.
        /// </summary>
        protected override IReadOnlyPlaceholderNodeStateReadOnlyList CreateStateListReadOnly(IReadOnlyPlaceholderNodeStateList stateList)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameListInner<IIndex, TIndex>));
            return new FramePlaceholderNodeStateReadOnlyList((IFramePlaceholderNodeStateList)stateList);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        protected override IReadOnlyPlaceholderNodeState CreateNodeState(IReadOnlyNodeIndex nodeIndex)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameListInner<IIndex, TIndex>));
            return new FramePlaceholderNodeState((IFrameNodeIndex)nodeIndex);
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeState object.
        /// </summary>
        protected override IIndex CreateNodeIndex(IReadOnlyPlaceholderNodeState state, string propertyName, int index)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameListInner<IIndex, TIndex>));
            return (TIndex)new FrameBrowsingListNodeIndex(Owner.Node, state.Node, propertyName, index);
        }
        #endregion
    }
}