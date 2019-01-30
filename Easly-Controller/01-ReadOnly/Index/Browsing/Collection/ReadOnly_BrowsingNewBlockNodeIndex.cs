﻿namespace EaslyController.ReadOnly
{
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;

    /// <summary>
    /// Index for the first node in a block.
    /// </summary>
    public interface IReadOnlyBrowsingNewBlockNodeIndex : IReadOnlyBrowsingBlockNodeIndex
    {
        /// <summary>
        /// The parent node.
        /// </summary>
        INode ParentNode { get; }

        /// <summary>
        /// Replication pattern in the block.
        /// </summary>
        IPattern PatternNode { get; }

        /// <summary>
        /// Source identifier in the block.
        /// </summary>
        IIdentifier SourceNode { get; }

        /// <summary>
        /// Gets the index for this node in an existing block.
        /// </summary>
        IReadOnlyBrowsingExistingBlockNodeIndex ToExistingBlockIndex();
    }

    /// <summary>
    /// Index for the first node in a block.
    /// </summary>
    internal class ReadOnlyBrowsingNewBlockNodeIndex : ReadOnlyBrowsingBlockNodeIndex, IReadOnlyBrowsingNewBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBrowsingNewBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="node">First node in the block.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="patternNode">Replication pattern in the block.</param>
        /// <param name="sourceNode">Source identifier in the block.</param>
        public ReadOnlyBrowsingNewBlockNodeIndex(INode parentNode, INode node, string propertyName, int blockIndex, IPattern patternNode, IIdentifier sourceNode)
            : base(node, propertyName, blockIndex)
        {
            Debug.Assert(parentNode != null);
            Debug.Assert(node != null);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(blockIndex >= 0);
            Debug.Assert(patternNode != null);
            Debug.Assert(sourceNode != null);
            Debug.Assert(NodeTreeHelperBlockList.IsBlockChildNode(parentNode, propertyName, blockIndex, 0, node));
            Debug.Assert(NodeTreeHelperBlockList.IsBlockPatternNode(parentNode, propertyName, blockIndex, patternNode));
            Debug.Assert(NodeTreeHelperBlockList.IsBlockSourceNode(parentNode, propertyName, blockIndex, sourceNode));

            ParentNode = parentNode;
            PatternNode = patternNode;
            SourceNode = sourceNode;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The parent node.
        /// </summary>
        public INode ParentNode { get; }

        /// <summary>
        /// Replication pattern in the block.
        /// </summary>
        public IPattern PatternNode { get; }

        /// <summary>
        /// Source identifier in the block.
        /// </summary>
        public IIdentifier SourceNode { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Gets the index for this node in an existing block.
        /// </summary>
        public virtual IReadOnlyBrowsingExistingBlockNodeIndex ToExistingBlockIndex()
        {
            return CreateExistingBlockIndex();
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IReadOnlyIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IReadOnlyBrowsingNewBlockNodeIndex AsNewBlockNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsNewBlockNodeIndex))
                return comparer.Failed();

            if (ParentNode != AsNewBlockNodeIndex.ParentNode)
                return comparer.Failed();

            if (PatternNode != AsNewBlockNodeIndex.PatternNode)
                return comparer.Failed();

            if (SourceNode != AsNewBlockNodeIndex.SourceNode)
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        private protected virtual IReadOnlyBrowsingExistingBlockNodeIndex CreateExistingBlockIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBrowsingNewBlockNodeIndex));
            return new ReadOnlyBrowsingExistingBlockNodeIndex(ParentNode, Node, PropertyName, BlockIndex, 0);
        }
        #endregion
    }
}
