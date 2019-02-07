﻿namespace EaslyController.Frame
{
    using System.Diagnostics;

    /// <summary>
    /// Frame for decoration purpose only.
    /// </summary>
    public interface IFrameStaticFrame : IFrameFrame, IFrameNodeFrame
    {
    }

    /// <summary>
    /// Frame for decoration purpose only.
    /// </summary>
    public abstract class FrameStaticFrame : FrameFrame, IFrameStaticFrame
    {
        #region Properties
        /// <summary></summary>
        private protected abstract bool IsFrameFocusable { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public virtual IFrameCellView BuildNodeCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            IFrameVisibleCellView CellView;

            if (IsFrameFocusable)
                CellView = CreateFocusableCellView(context.StateView);
            else
                CellView = CreateVisibleCellView(context.StateView);

            ValidateVisibleCellView(context, CellView);

            return CellView;
        }
        #endregion

        #region Implementation
        /// <summary></summary>
        private protected virtual void ValidateVisibleCellView(IFrameCellViewTreeContext context, IFrameVisibleCellView cellView)
        {
            Debug.Assert(cellView.StateView == context.StateView);
            Debug.Assert(cellView.Frame == this);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxFocusableCellView object.
        /// </summary>
        private protected virtual IFrameFocusableCellView CreateFocusableCellView(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameStaticFrame));
            return new FrameFocusableCellView(stateView, this);
        }

        /// <summary>
        /// Creates a IxxxVisibleCellView object.
        /// </summary>
        private protected virtual IFrameVisibleCellView CreateVisibleCellView(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameStaticFrame));
            return new FrameVisibleCellView(stateView, this);
        }
        #endregion
    }
}
