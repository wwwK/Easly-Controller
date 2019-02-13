﻿namespace EaslyController.Frame
{
    using System;
    using System.Diagnostics;
    using BaseNodeHelper;

    /// <summary>
    /// Base frame for frames that describe property in a node.
    /// </summary>
    public interface IFrameNamedFrame : IFrameFrame
    {
        /// <summary>
        /// The property name.
        /// (Set in Xaml)
        /// </summary>
        string PropertyName { get; }
    }

    /// <summary>
    /// Base frame for frames that describe property in a node.
    /// </summary>
    public abstract class FrameNamedFrame : FrameFrame, IFrameNamedFrame
    {
        #region Properties
        /// <summary>
        /// The property name.
        /// (Set in Xaml)
        /// </summary>
        public string PropertyName { get; set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        /// <param name="nodeTemplateTable">Table of templates with all frames.</param>
        public override bool IsValid(Type nodeType, IFrameTemplateReadOnlyDictionary nodeTemplateTable)
        {
            bool IsValid = true;

            IsValid &= base.IsValid(nodeType, nodeTemplateTable);
            IsValid &= !string.IsNullOrEmpty(PropertyName) && NodeTreeHelper.GetPropertyOf(nodeType, PropertyName) != null;

            Debug.Assert(IsValid);
            return IsValid;
        }
        #endregion
    }
}
