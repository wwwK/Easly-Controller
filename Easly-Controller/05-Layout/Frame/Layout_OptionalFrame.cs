﻿namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using NodeController;

    /// <summary>
    /// Layout for describing an optional child node.
    /// </summary>
    public interface ILayoutOptionalFrame : IFocusOptionalFrame, ILayoutNamedFrame, ILayoutNodeFrameWithVisibility, ILayoutNodeFrameWithSelector, ILayoutSelectorPropertyFrame
    {
        /// <summary>
        /// Margin the right side of the cell.
        /// (Set in Xaml)
        /// </summary>
        Margins RightMargin { get; }

        /// <summary>
        /// Margin the left side of the cell.
        /// (Set in Xaml)
        /// </summary>
        Margins LeftMargin { get; }
    }

    /// <summary>
    /// Layout for describing an optional child node.
    /// </summary>
    public class LayoutOptionalFrame : FocusOptionalFrame, ILayoutOptionalFrame
    {
        #region Properties
        /// <summary>
        /// Parent template.
        /// </summary>
        public new ILayoutTemplate ParentTemplate { get { return (ILayoutTemplate)base.ParentTemplate; } }

        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        public new ILayoutFrame ParentFrame { get { return (ILayoutFrame)base.ParentFrame; } }

        /// <summary>
        /// Node frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public new ILayoutNodeFrameVisibility Visibility { get { return (ILayoutNodeFrameVisibility)base.Visibility; } set { base.Visibility = value; } }

        /// <summary>
        /// List of optional selectors.
        /// (Set in Xaml)
        /// </summary>
        public new ILayoutFrameSelectorList Selectors { get { return (ILayoutFrameSelectorList)base.Selectors; } }

        /// <summary>
        /// Margin the right side of the cell.
        /// (Set in Xaml)
        /// </summary>
        public Margins RightMargin { get; set; }

        /// <summary>
        /// Margin the left side of the cell.
        /// (Set in Xaml)
        /// </summary>
        public Margins LeftMargin { get; set; }
        #endregion

        #region Implementation
        /// <summary></summary>
        private protected override void ValidateContainerCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView, IFrameContainerCellView containerCellView)
        {
            Debug.Assert(((ILayoutContainerCellView)containerCellView).StateView == (ILayoutNodeStateView)stateView);
            Debug.Assert(((ILayoutContainerCellView)containerCellView).ParentCellView == (ILayoutCellViewCollection)parentCellView);
            Debug.Assert(((ILayoutContainerCellView)containerCellView).ChildStateView == (ILayoutNodeStateView)childStateView);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        private protected override IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutOptionalFrame));
            return new LayoutContainerCellView((ILayoutNodeStateView)stateView, (ILayoutCellViewCollection)parentCellView, (ILayoutNodeStateView)childStateView);
        }

        /// <summary>
        /// Creates a IxxxFrameSelectorList object.
        /// </summary>
        private protected override IFocusFrameSelectorList CreateEmptySelectorList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutOptionalFrame));
            return new LayoutFrameSelectorList();
        }
        #endregion
    }
}
