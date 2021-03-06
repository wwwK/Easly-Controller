﻿namespace EaslyController.Frame
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Writeable;

    /// <summary>
    /// Index for a node in a list of nodes.
    /// </summary>
    public interface IFrameBrowsingListNodeIndex : IWriteableBrowsingListNodeIndex, IFrameBrowsingCollectionNodeIndex, IFrameBrowsingInsertableIndex
    {
    }

    /// <summary>
    /// Index for a node in a list of nodes.
    /// </summary>
    internal class FrameBrowsingListNodeIndex : WriteableBrowsingListNodeIndex, IFrameBrowsingListNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBrowsingListNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the list.</param>
        /// <param name="node">Indexed node in the list</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the list.</param>
        /// <param name="index">Position of the node in the list.</param>
        public FrameBrowsingListNodeIndex(INode parentNode, INode node, string propertyName, int index)
            : base(parentNode, node, propertyName, index)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FrameBrowsingListNodeIndex AsBrowsingListNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBrowsingListNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertionListNodeIndex object.
        /// </summary>
        private protected override IWriteableInsertionListNodeIndex CreateInsertionIndex(INode parentNode, INode node)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBrowsingListNodeIndex));
            return new FrameInsertionListNodeIndex(parentNode, PropertyName, node, Index);
        }
        #endregion
    }
}
