﻿namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Constants;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// Frame for a placeholder node in a block list displayed vertically.
    /// </summary>
    public interface ILayoutVerticalCollectionPlaceholderFrame : IFocusVerticalCollectionPlaceholderFrame, ILayoutCollectionPlaceholderFrame, ILayoutFrameWithSelector, ILayoutFrameWithVerticalSeparator
    {
    }

    /// <summary>
    /// Frame for a placeholder node in a block list displayed vertically.
    /// </summary>
    public class LayoutVerticalCollectionPlaceholderFrame : FocusVerticalCollectionPlaceholderFrame, ILayoutVerticalCollectionPlaceholderFrame
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
        /// Block frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public new ILayoutBlockFrameVisibility BlockVisibility { get { return (ILayoutBlockFrameVisibility)base.BlockVisibility; } set { base.BlockVisibility = value; } }

        /// <summary>
        /// List of optional selectors.
        /// (Set in Xaml)
        /// </summary>
        public new ILayoutFrameSelectorList Selectors { get { return (ILayoutFrameSelectorList)base.Selectors; } }

        /// <summary>
        /// Vertical separator.
        /// (Set in Xaml)
        /// </summary>
        public VerticalSeparators Separator { get; set; }
        #endregion

        #region Implementation
        /// <summary></summary>
        private protected override void ValidateEmbeddingCellView(IFrameCellViewTreeContext context, IFrameCellViewCollection embeddingCellView)
        {
            Debug.Assert(((ILayoutCellViewCollection)embeddingCellView).StateView == ((ILayoutCellViewTreeContext)context).StateView);
            ILayoutCellViewCollection ParentCellView = ((ILayoutCellViewCollection)embeddingCellView).ParentCellView;
        }

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
        /// Creates a IxxxCellViewList object.
        /// </summary>
        private protected override IFrameCellViewList CreateCellViewList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutVerticalCollectionPlaceholderFrame));
            return new LayoutCellViewList();
        }

        /// <summary>
        /// Creates a IxxxContainerCellView object.
        /// </summary>
        private protected override IFrameContainerCellView CreateFrameCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameNodeStateView childStateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutVerticalCollectionPlaceholderFrame));
            return new LayoutContainerCellView((ILayoutNodeStateView)stateView, (ILayoutCellViewCollection)parentCellView, (ILayoutNodeStateView)childStateView, this);
        }

        /// <summary>
        /// Creates a IxxxCellViewCollection object.
        /// </summary>
        private protected override IFrameCellViewCollection CreateEmbeddingCellView(IFrameNodeStateView stateView, IFrameCellViewCollection parentCellView, IFrameCellViewList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutVerticalCollectionPlaceholderFrame));
            return new LayoutColumn((ILayoutNodeStateView)stateView, (ILayoutCellViewCollection)parentCellView, (ILayoutCellViewList)list, this);
        }

        /// <summary>
        /// Creates a IxxxFrameSelectorList object.
        /// </summary>
        private protected override IFocusFrameSelectorList CreateEmptySelectorList()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutVerticalCollectionPlaceholderFrame));
            return new LayoutFrameSelectorList();
        }
        #endregion
    }
}
