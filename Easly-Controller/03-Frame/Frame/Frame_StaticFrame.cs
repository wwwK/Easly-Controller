﻿namespace EaslyController.Frame
{
    /// <summary>
    /// Frame for decoration purpose only.
    /// </summary>
    public interface IFrameStaticFrame : IFrameFrame
    {
    }

    /// <summary>
    /// Frame for decoration purpose only.
    /// </summary>
    public class FrameStaticFrame : FrameFrame, IFrameStaticFrame
    {
        #region Client Interface
        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="controllerView">The view in cells are created.</param>
        /// <param name="stateView">The state view for which to create cells.</param>
        public override IFrameCellView BuildCells(IFrameControllerView controllerView, IFrameNodeStateView stateView)
        {
            return CreateFrameCellView(stateView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxVisibleCellView object.
        /// </summary>
        protected virtual IFrameVisibleCellView CreateFrameCellView(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameStaticFrame));
            return new FrameVisibleCellView(stateView);
        }
        #endregion
    }
}
