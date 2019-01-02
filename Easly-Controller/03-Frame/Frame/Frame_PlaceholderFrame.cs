﻿using BaseNodeHelper;
using System;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Frame for describing an child node.
    /// </summary>
    public interface IFramePlaceholderFrame : IFrameNamedFrame
    {
    }

    /// <summary>
    /// Frame for describing an child node.
    /// </summary>
    public class FramePlaceholderFrame : FrameNamedFrame, IFramePlaceholderFrame
    {
        #region Client Interface
        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        public override bool IsValid(Type nodeType)
        {
            if (!base.IsValid(nodeType))
                return false;

            if (!NodeTreeHelperChild.IsChildNodeProperty(nodeType, PropertyName, out Type ChildNodeType))
                return false;

            return true;
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="controllerView">The view in cells are created.</param>
        /// <param name="stateView">The state view for which to create cells.</param>
        public override IFrameCellView BuildCells(IFrameControllerView controllerView, IFrameNodeStateView stateView)
        {
            IFrameNodeState State = stateView.State;
            Debug.Assert(State != null);
            Debug.Assert(State.InnerTable != null);
            Debug.Assert(State.InnerTable.ContainsKey(PropertyName));

            IFramePlaceholderInner<IFrameBrowsingPlaceholderNodeIndex> Inner = State.InnerTable[PropertyName] as IFramePlaceholderInner<IFrameBrowsingPlaceholderNodeIndex>;
            Debug.Assert(Inner != null);
            Debug.Assert(Inner.ChildState != null);
            IFrameNodeState ChildState = Inner.ChildState;

            IFrameStateViewDictionary StateViewTable = controllerView.StateViewTable;
            Debug.Assert(StateViewTable.ContainsKey(ChildState));

            IFrameNodeStateView ChildStateView = StateViewTable[ChildState];

            return CreateFrameCellView(stateView, ChildStateView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        protected virtual IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameStaticFrame));
            return new FrameContainerCellView(stateView, childStateView);
        }
        #endregion
    }
}
