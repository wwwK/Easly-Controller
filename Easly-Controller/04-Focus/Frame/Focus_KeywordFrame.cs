﻿namespace EaslyController.Focus
{
    using System;
    using System.Diagnostics;
    using System.Windows.Markup;
    using EaslyController.Frame;

    /// <summary>
    /// Frame for decoration purpose only.
    /// </summary>
    public interface IFocusKeywordFrame : IFrameKeywordFrame, IFocusFrame, IFocusBlockFrame, IFocusNodeFrameWithVisibility
    {
    }

    /// <summary>
    /// Frame for decoration purpose only.
    /// </summary>
    [ContentProperty("Text")]
    public class FocusKeywordFrame : FrameKeywordFrame, IFocusKeywordFrame
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

        /// <summary>
        /// Node frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public IFocusNodeFrameVisibility Visibility { get; set; }

        /// <summary>
        /// Block frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public IFocusBlockFrameVisibility BlockVisibility { get; set; }

        /// <summary>
        /// Indicates that this is the preferred frame when restoring the focus.
        /// (Set in Xaml)
        /// </summary>
        public bool IsPreferred { get; set; }
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
            IsValid &= Visibility == null || Visibility.IsValid(nodeType);
            IsValid &= BlockVisibility == null || BlockVisibility.IsValid(nodeType);

            Debug.Assert(IsValid);
            return IsValid;
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public override IFrameCellView BuildNodeCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            ((IFocusCellViewTreeContext)context).UpdateNodeFrameVisibility(this, out bool OldFrameVisibility);

            IFocusCellView Result;
            if (((IFocusCellViewTreeContext)context).IsVisible)
                Result = base.BuildNodeCells(context, parentCellView) as IFocusCellView;
            else
            {
                IFocusEmptyCellView EmptyCellView = CreateEmptyCellView(((IFocusCellViewTreeContext)context).StateView, (IFocusCellViewCollection)parentCellView);
                ValidateEmptyCellView((IFocusCellViewTreeContext)context, EmptyCellView);

                Result = EmptyCellView;
            }

            ((IFocusCellViewTreeContext)context).RestoreFrameVisibility(OldFrameVisibility);

            return Result;
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        public override IFrameCellView BuildBlockCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            ((IFocusCellViewTreeContext)context).UpdateBlockFrameVisibility(this, out bool OldFrameVisibility);

            IFocusCellView Result;
            if (((IFocusCellViewTreeContext)context).IsVisible)
                Result = base.BuildBlockCells(context, parentCellView) as IFocusCellView;
            else
            {
                IFocusEmptyCellView EmptyCellView = CreateEmptyCellView(((IFocusCellViewTreeContext)context).StateView, (IFocusCellViewCollection)parentCellView);
                ValidateEmptyCellView((IFocusCellViewTreeContext)context, EmptyCellView);

                Result = EmptyCellView;
            }

            ((IFocusCellViewTreeContext)context).RestoreFrameVisibility(OldFrameVisibility);

            return Result;
        }

        /// <summary>
        /// Gets preferred frames to receive the focus when the source code is changed.
        /// </summary>
        /// <param name="firstPreferredFrame">The first preferred frame found.</param>
        /// <param name="lastPreferredFrame">The last preferred frame found.</param>
        public virtual void GetPreferredFrame(ref IFocusNodeFrame firstPreferredFrame, ref IFocusNodeFrame lastPreferredFrame)
        {
        }
        #endregion

        #region Implementation
        /// <summary></summary>
        private protected override void ValidateVisibleCellView(IFrameCellViewTreeContext context, IFrameVisibleCellView cellView)
        {
            Debug.Assert(((IFocusVisibleCellView)cellView).StateView == ((IFocusCellViewTreeContext)context).StateView);
            Debug.Assert(((IFocusVisibleCellView)cellView).Frame == this);
        }

        /// <summary></summary>
        private protected virtual void ValidateEmptyCellView(IFocusCellViewTreeContext context, IFocusEmptyCellView emptyCellView)
        {
            Debug.Assert(emptyCellView.StateView == context.StateView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxFocusableCellView object.
        /// </summary>
        private protected override IFrameFocusableCellView CreateFocusableCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusKeywordFrame));
            return new FocusFocusableCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, this);
        }

        /// <summary>
        /// Creates a IxxxVisibleCellView object.
        /// </summary>
        private protected override IFrameVisibleCellView CreateVisibleCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusKeywordFrame));
            return new FocusVisibleCellView((IFocusNodeStateView)stateView, (IFocusCellViewCollection)parentCellView, this);
        }

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        private protected virtual IFocusEmptyCellView CreateEmptyCellView(IFocusNodeStateView stateView, IFocusCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusKeywordFrame));
            return new FocusEmptyCellView(stateView, parentCellView);
        }
        #endregion
    }
}
