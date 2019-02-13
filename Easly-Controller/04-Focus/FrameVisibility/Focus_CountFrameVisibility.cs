﻿namespace EaslyController.Focus
{
    using System;
    using BaseNodeHelper;

    /// <summary>
    /// Frame visibility that depends if a collection has at least one item.
    /// </summary>
    public interface IFocusCountFrameVisibility : IFocusFrameVisibility, IFocusNodeFrameVisibility
    {
        /// <summary>
        /// Name of the collection property.
        /// (Set in Xaml)
        /// </summary>
        string PropertyName { get; }
    }

    /// <summary>
    /// Frame visibility that depends if a collection has at least one item.
    /// </summary>
    public class FocusCountFrameVisibility : FocusFrameVisibility, IFocusCountFrameVisibility
    {
        #region Properties
        /// <summary>
        /// True if the visibility depends on the show/hidden state of the view with the focus.
        /// </summary>
        public virtual bool IsVolatile { get { return true; } }

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
            bool IsValid = true;

            IsValid &= !string.IsNullOrEmpty(PropertyName);

            Type ChildInterfaceType, ChildNodeType;
            IsValid &= NodeTreeHelperList.IsNodeListProperty(nodeType, PropertyName, out ChildNodeType) || NodeTreeHelperBlockList.IsBlockListProperty(nodeType, PropertyName, out ChildInterfaceType, out ChildNodeType);

            return IsValid;
        }

        /// <summary>
        /// Is the associated frame visible.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="frame">The frame with the associated visibility.</param>
        public virtual bool IsVisible(IFocusCellViewTreeContext context, IFocusNodeFrameWithVisibility frame)
        {
            bool IsVisible = true;

            IsVisible &= context.ControllerView.CollectionHasItems(context.StateView, PropertyName);

            return IsVisible;
        }
        #endregion
    }
}
