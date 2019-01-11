﻿using BaseNodeHelper;
using System;

namespace EaslyController.Frame
{
    /// <summary>
    /// Frame describing a string value property in a node.
    /// </summary>
    public interface IFrameTextValueFrame : IFrameValueFrame
    {
    }

    /// <summary>
    /// Frame describing a string value property in a node.
    /// </summary>
    public class FrameTextValueFrame : FrameValueFrame, IFrameTextValueFrame
    {
        #region Client Interface
        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="controllerView">The view in cells are created.</param>
        /// <param name="stateView">The state view for which to create cells.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public override IFrameCellView BuildNodeCells(IFrameControllerView controllerView, IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView)
        {
            IFrameVisibleCellView EmbeddingCellView = CreateFrameCellView(stateView);
            return EmbeddingCellView;
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        public override bool IsValid(Type nodeType)
        {
            if (!base.IsValid(nodeType))
                return false;

            if (!NodeTreeHelper.IsStringProperty(nodeType, PropertyName))
                return false;

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxTextFocusableCellView object.
        /// </summary>
        protected virtual IFrameVisibleCellView CreateFrameCellView(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameTextValueFrame));
            return new FrameTextFocusableCellView(stateView, this, PropertyName);
        }
        #endregion
    }
}
