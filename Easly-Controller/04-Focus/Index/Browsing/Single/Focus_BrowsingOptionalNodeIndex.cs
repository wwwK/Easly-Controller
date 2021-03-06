﻿namespace EaslyController.Focus
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <summary>
    /// Index for an optional node.
    /// </summary>
    public interface IFocusBrowsingOptionalNodeIndex : IFrameBrowsingOptionalNodeIndex, IFocusBrowsingChildIndex, IFocusBrowsingInsertableIndex
    {
    }

    /// <summary>
    /// Index for an optional node.
    /// </summary>
    internal class FocusBrowsingOptionalNodeIndex : FrameBrowsingOptionalNodeIndex, IFocusBrowsingOptionalNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBrowsingOptionalNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the indexed optional node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed optional node.</param>
        public FocusBrowsingOptionalNodeIndex(INode parentNode, string propertyName)
            : base(parentNode, propertyName)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusBrowsingOptionalNodeIndex AsBrowsingOptionalNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBrowsingOptionalNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertionPlaceholderNodeIndex object.
        /// </summary>
        private protected override IWriteableInsertionOptionalNodeIndex CreateInsertionOptionalNodeIndex(INode parentNode, INode node)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBrowsingOptionalNodeIndex));
            return new FocusInsertionOptionalNodeIndex(parentNode, PropertyName, node);
        }

        /// <summary>
        /// Creates a IxxxInsertionOptionalClearIndex object.
        /// </summary>
        private protected override IWriteableInsertionOptionalClearIndex CreateInsertionOptionalClearIndex(INode parentNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBrowsingOptionalNodeIndex));
            return new FocusInsertionOptionalClearIndex(parentNode, PropertyName);
        }
        #endregion
    }
}
