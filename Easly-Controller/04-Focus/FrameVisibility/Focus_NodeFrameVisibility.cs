﻿namespace EaslyController.Focus
{
    /// <summary>
    /// Base frame visibility for node frames.
    /// </summary>
    public interface IFocusNodeFrameVisibility : IFocusFrameVisibility
    {
        /// <summary>
        /// Is the associated frame visible.
        /// </summary>
        /// <param name="controllerView">The view in cells are created.</param>
        /// <param name="stateView">The state view for which to create cells.</param>
        /// <param name="frame">The frame with the associated visibility.</param>
        bool IsVisible(IFocusControllerView controllerView, IFocusNodeStateView stateView, IFocusNodeFrame frame);
    }
}
