﻿using BaseNode;
using EaslyController.Writeable;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Index for inserting a node in an existing block of a block list.
    /// </summary>
    public interface IFrameInsertionExistingBlockNodeIndex : IWriteableInsertionExistingBlockNodeIndex, IFrameInsertionBlockNodeIndex
    {
    }

    /// <summary>
    /// Index for inserting a node in an existing block of a block list.
    /// </summary>
    public class FrameInsertionExistingBlockNodeIndex : WriteableInsertionExistingBlockNodeIndex, IFrameInsertionExistingBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameInsertionExistingBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list..</param>
        /// <param name="node">Inserted node.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="index">Position where to insert <paramref name="node"/> in the block.</param>
        public FrameInsertionExistingBlockNodeIndex(INode parentNode, string propertyName, INode node, int blockIndex, int index)
            : base(parentNode, propertyName, node, blockIndex, index)
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

            if (!(other is IFrameInsertionExistingBlockNodeIndex AsInsertionExistingBlockNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsInsertionExistingBlockNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        protected override IWriteableBrowsingExistingBlockNodeIndex CreateBrowsingIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameInsertionExistingBlockNodeIndex));
            return new FrameBrowsingExistingBlockNodeIndex(ParentNode, Node, PropertyName, BlockIndex, Index);
        }
        #endregion
    }
}
