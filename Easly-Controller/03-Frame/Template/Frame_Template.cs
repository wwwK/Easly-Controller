﻿namespace EaslyController.Frame
{
    using System;

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public interface IFrameTemplate
    {
        /// <summary>
        /// Type of the node associated to this template (an interface type).
        /// (Set in Xaml)
        /// </summary>
        Type NodeType { get; set; }

        /// <summary>
        /// Root frame.
        /// (Set in Xaml)
        /// </summary>
        IFrameFrame Root { get; set; }

        /// <summary>
        /// Checks that a template and all its frames are valid.
        /// </summary>
        bool IsValid { get; }
    }

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public abstract class FrameTemplate : IFrameTemplate
    {
        #region Properties
        /// <summary>
        /// Type of the node associated to this template (an interface type).
        /// (Set in Xaml)
        /// </summary>
        public Type NodeType { get; set; }

        /// <summary>
        /// Root frame.
        /// (Set in Xaml)
        /// </summary>
        public IFrameFrame Root { get; set; }

        /// <summary>
        /// Checks that a template and all its frames are valid.
        /// </summary>
        public virtual bool IsValid
        {
            get
            {
                if (NodeType == null)
                    return false;

                if (Root == null)
                    return false;

                if (!IsRootValid)
                    return false;

                return true;
            }
        }

        /// <summary></summary>
        private protected virtual bool IsRootValid { get { return (Root.ParentFrame == FrameFrame.FrameRoot); } }
        #endregion
    }
}
