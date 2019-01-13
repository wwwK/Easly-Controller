﻿using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Cell view for discrete components that can receive the focus and be modified (enum, bool...)
    /// </summary>
    public interface IFrameDiscreteContentFocusableCellView : IFrameContentFocusableCellView
    {
    }

    /// <summary>
    /// Cell view for discrete components that can receive the focus and be modified (enum, bool...)
    /// </summary>
    public class FrameDiscreteContentFocusableCellView : FrameContentFocusableCellView, IFrameDiscreteContentFocusableCellView
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FrameDiscreteContentFocusableCellView"/>.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="frame">The frame that created this cell view.</param>
        /// <param name="propertyName">Property corresponding to the component of the node.</param>
        public FrameDiscreteContentFocusableCellView(IFrameNodeStateView stateView, IFrameFrame frame, string propertyName)
            : base(stateView, frame, propertyName)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameDiscreteContentFocusableCellView AsMultiDiscreteFocusableCellView))
                return false;

            if (!base.IsEqual(comparer, AsMultiDiscreteFocusableCellView))
                return false;

            if (PropertyName != AsMultiDiscreteFocusableCellView.PropertyName)
                return false;

            return true;
        }
        #endregion
    }
}