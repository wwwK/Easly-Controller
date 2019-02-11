﻿namespace EaslyController.Layout
{
    using System;
    using System.Diagnostics;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// Base frame.
    /// </summary>
    public interface ILayoutFrame : IFocusFrame
    {
    }

    /// <summary>
    /// Base frame.
    /// </summary>
    public abstract class LayoutFrame : FocusFrame, ILayoutFrame
    {
        #region Init
        private class LayoutRootFrame : ILayoutFrame
        {
            public LayoutRootFrame()
            {
                UpdateParent(null, null);
                Debug.Assert(ParentTemplate == null);
                Debug.Assert(ParentFrame == null);
                Debug.Assert(!IsValid(null, null));

                IFrameFrame AsFrameFrame = this;
                Debug.Assert(AsFrameFrame.ParentTemplate == null);
                Debug.Assert(AsFrameFrame.ParentFrame == null);

                IFocusFrame AsFocusFrame = this;
                Debug.Assert(AsFocusFrame.ParentTemplate == null);
                Debug.Assert(AsFocusFrame.ParentFrame == null);
            }

            public ILayoutTemplate ParentTemplate { get; }
            IFocusTemplate IFocusFrame.ParentTemplate { get { return ParentTemplate; } }
            IFrameTemplate IFrameFrame.ParentTemplate { get { return ParentTemplate; } }
            public ILayoutFrame ParentFrame { get; }
            IFocusFrame IFocusFrame.ParentFrame { get { return ParentFrame; } }
            IFrameFrame IFrameFrame.ParentFrame { get { return ParentFrame; } }
            public bool IsValid(Type nodeType, IFrameTemplateReadOnlyDictionary nodeTemplateTable) { return false; }
            public void UpdateParent(IFrameTemplate parentTemplate, IFrameFrame parentFrame) { }
        }

        /// <summary>
        /// Singleton object representing the root of a tree of frames.
        /// </summary>
        public static ILayoutFrame LayoutRoot { get; } = new LayoutRootFrame();
        #endregion
    }
}
