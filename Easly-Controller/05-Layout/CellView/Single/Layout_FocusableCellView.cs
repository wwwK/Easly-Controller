﻿namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Focus;

    /// <summary>
    /// Cell view for discrete elements that can receive the focus but are not always the component of a node (insertion points, keywords and other decorations)
    /// </summary>
    public interface ILayoutFocusableCellView : IFocusFocusableCellView, ILayoutVisibleCellView
    {
    }

    /// <summary>
    /// Cell view for discrete elements that can receive the focus but are not always the component of a node (insertion points, keywords and other decorations)
    /// </summary>
    internal class LayoutFocusableCellView : FocusFocusableCellView, ILayoutFocusableCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutFocusableCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="frame">The frame that created this cell view.</param>
        public LayoutFocusableCellView(ILayoutNodeStateView stateView, ILayoutFrame frame)
            : base(stateView, frame)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        public new ILayoutNodeStateView StateView { get { return (ILayoutNodeStateView)base.StateView; } }

        /// <summary>
        /// The frame that created this cell view.
        /// </summary>
        public new ILayoutFrame Frame { get { return (ILayoutFrame)base.Frame; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ILayoutCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutFocusableCellView AsFocusableCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsFocusableCellView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}