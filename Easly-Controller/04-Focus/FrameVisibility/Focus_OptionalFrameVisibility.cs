﻿using BaseNodeHelper;
using System;

namespace EaslyController.Focus
{
    /// <summary>
    /// Frame visibility that depends if an optional node is assigned.
    /// </summary>
    public interface IFocusOptionalFrameVisibility : IFocusFrameVisibility, IFocusNodeFrameVisibility
    {
        /// <summary>
        /// Name of the collection property.
        /// (Set in Xaml)
        /// </summary>
        string PropertyName { get; set; }
    }

    /// <summary>
    /// Frame visibility that depends if an optional node is assigned.
    /// </summary>
    public class FocusOptionalFrameVisibility : FocusFrameVisibility, IFocusOptionalFrameVisibility
    {
        #region Properties
        /// <summary>
        /// True if the visibility depends on the show/hidden state of the view with the focus.
        /// </summary>
        public override bool IsVolatile { get { return true; } }

        /// <summary>
        /// Name of the collection property.
        /// (Set in Xaml)
        /// </summary>
        public string PropertyName { get; set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame visibility is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame visibility can describe.</param>
        public override bool IsValid(Type nodeType)
        {
            if (string.IsNullOrEmpty(PropertyName))
                return false;

            if (!NodeTreeHelperOptional.IsOptionalChildNodeProperty(nodeType, PropertyName, out Type ChildNodeType))
                return false;

            return true;
        }

        /// <summary>
        /// Is the associated frame visible.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="frame">The frame with the associated visibility.</param>
        public virtual bool IsVisible(IFocusCellViewTreeContext context, IFocusNodeFrameWithVisibility frame)
        {
            if (!context.ControllerView.IsOptionalNodeAssigned(context.StateView, PropertyName))
                return false;

            return true;
        }
        #endregion
    }
}