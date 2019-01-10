﻿using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Focus for a placeholder node in a block list displayed vertically.
    /// </summary>
    public interface IFocusVerticalCollectionPlaceholderFrame : IFrameVerticalCollectionPlaceholderFrame, IFocusCollectionPlaceholderFrame
    {
    }

    /// <summary>
    /// Focus for a placeholder node in a block list displayed vertically.
    /// </summary>
    public class FocusVerticalCollectionPlaceholderFrame : FrameVerticalCollectionPlaceholderFrame, IFocusVerticalCollectionPlaceholderFrame
    {
        #region Properties
        /// <summary>
        /// Parent template.
        /// </summary>
        public new IFocusTemplate ParentTemplate { get { return (IFocusTemplate)base.ParentTemplate; } }

        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        public new IFocusFrame ParentFrame { get { return (IFocusFrame)base.ParentFrame; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusVerticalCollectionPlaceholderFrame));
            return new FocusColumn((IFocusNodeStateView)stateView, (IFocusCellViewList)list);
        }
        #endregion
    }
}
