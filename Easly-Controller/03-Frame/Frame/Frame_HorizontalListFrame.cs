﻿namespace EaslyController.Frame
{
    /// <summary>
    /// Base frame for a list of nodes displayed horizontally.
    /// </summary>
    public interface IFrameHorizontalListFrame : IFrameListFrame
    {
    }

    /// <summary>
    /// Base frame for a list of nodes displayed horizontally.
    /// </summary>
    public class FrameHorizontalListFrame : FrameListFrame, IFrameHorizontalListFrame
    {
        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameHorizontalListFrame));
            return new FrameLine(stateView, list);
        }
        #endregion
    }
}