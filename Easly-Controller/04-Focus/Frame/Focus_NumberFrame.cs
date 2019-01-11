﻿using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Focus describing a number string value property in a node.
    /// </summary>
    public interface IFocusNumberFrame : IFrameNumberFrame, IFocusTextValueFrame
    {
    }

    /// <summary>
    /// Focus describing a number string value property in a node.
    /// </summary>
    public class FocusNumberFrame : FrameNumberFrame, IFocusNumberFrame
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
        /// Creates a IxxxTextFocusableCellView object.
        /// </summary>
        protected override IFrameVisibleCellView CreateFrameCellView(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusNumberFrame));
            return new FocusTextFocusableCellView((IFocusNodeStateView)stateView, this, PropertyName);
        }
        #endregion
    }
}
