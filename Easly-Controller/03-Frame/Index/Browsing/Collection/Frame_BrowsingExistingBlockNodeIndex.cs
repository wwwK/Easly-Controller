﻿using BaseNode;
using BaseNodeHelper;
using EaslyController.Writeable;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Index for a node in a block that is not the first.
    /// </summary>
    public interface IFrameBrowsingExistingBlockNodeIndex : IWriteableBrowsingExistingBlockNodeIndex, IFrameBrowsingBlockNodeIndex
    {
    }

    /// <summary>
    /// Index for a node in a block that is not the first.
    /// </summary>
    public class FrameBrowsingExistingBlockNodeIndex : WriteableBrowsingExistingBlockNodeIndex, IFrameBrowsingExistingBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBrowsingExistingBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="node">Indexed node in the block.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="index">Position of the node in the block.</param>
        public FrameBrowsingExistingBlockNodeIndex(INode parentNode, INode node, string propertyName, int blockIndex, int index)
            : base(parentNode, node, propertyName, blockIndex, index)
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

            if (!(other is IFrameBrowsingExistingBlockNodeIndex AsBrowsingExistingBlockNodeIndex))
                return false;

            if (!base.IsEqual(comparer, AsBrowsingExistingBlockNodeIndex))
                return false;

            return true;
        }
        #endregion
    }
}