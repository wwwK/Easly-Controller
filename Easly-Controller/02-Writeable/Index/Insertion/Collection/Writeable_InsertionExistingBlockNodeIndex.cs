﻿using BaseNode;
using BaseNodeHelper;
using EaslyController.ReadOnly;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Index for inserting a node in an existing block of a block list.
    /// </summary>
    public interface IWriteableInsertionExistingBlockNodeIndex : IWriteableInsertionBlockNodeIndex
    {
        /// <summary>
        /// Position of the block in the block list.
        /// </summary>
        int BlockIndex { get; }

        /// <summary>
        /// Position where the node is inserted in the block.
        /// </summary>
        int Index { get; }
    }

    /// <summary>
    /// Index for inserting a node in an existing block of a block list.
    /// </summary>
    public class WriteableInsertionExistingBlockNodeIndex : WriteableInsertionBlockNodeIndex, IWriteableInsertionExistingBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableInsertionExistingBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list..</param>
        /// <param name="node">Inserted node.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="index">Position where to insert <see cref="node"/> in the block.</param>
        public WriteableInsertionExistingBlockNodeIndex(INode parentNode, string propertyName, INode node, int blockIndex, int index)
            : base(parentNode, propertyName, node)
        {
            Debug.Assert(blockIndex >= 0);
            Debug.Assert(index >= 0); // You can insert at position 0, contrary to a browsing index that only supports positions other than 0.
            Debug.Assert(NodeTreeHelperBlockList.GetLastBlockChildIndex(parentNode, propertyName, blockIndex, out int LastIndex) && index <= LastIndex);

            BlockIndex = blockIndex;
            Index = index;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Position of the block in the block list.
        /// </summary>
        public int BlockIndex { get; }

        /// <summary>
        /// Position where the node is inserted in the block.
        /// </summary>
        public int Index { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Creates a browsing index from an insertion index.
        /// To call after the insertion operation has been completed.
        /// </summary>
        public override IWriteableBrowsingChildIndex ToBrowsingIndex()
        {
            return CreateBrowsingIndex();
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IReadOnlyIndex"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IWriteableInsertionExistingBlockNodeIndex AsInsertionExistingBlockNodeIndex))
                return false;

            if (!base.IsEqual(comparer, AsInsertionExistingBlockNodeIndex))
                return false;

            if (BlockIndex != AsInsertionExistingBlockNodeIndex.BlockIndex)
                return false;

            if (Index != AsInsertionExistingBlockNodeIndex.Index)
                return false;

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        protected virtual IWriteableBrowsingExistingBlockNodeIndex CreateBrowsingIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableInsertionExistingBlockNodeIndex));
            return new WriteableBrowsingExistingBlockNodeIndex(ParentNode, Node, PropertyName, BlockIndex, Index);
        }
        #endregion
    }
}
