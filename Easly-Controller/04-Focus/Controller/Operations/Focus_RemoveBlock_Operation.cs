﻿using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Operation details for removing a block from a block list.
    /// </summary>
    public interface IFocusRemoveBlockOperation : IFrameRemoveBlockOperation, IFocusRemoveOperation
    {
        /// <summary>
        /// Inner where the block removal is taking place.
        /// </summary>
        new IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the removed block.
        /// </summary>
        new IFocusBrowsingExistingBlockNodeIndex BlockIndex { get; }

        /// <summary>
        /// Block state removed.
        /// </summary>
        new IFocusBlockState BlockState { get; }
    }

    /// <summary>
    /// Operation details for removing a block from a block list.
    /// </summary>
    public class FocusRemoveBlockOperation : FrameRemoveBlockOperation, IFocusRemoveBlockOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FocusRemoveBlockOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the block removal is taking place.</param>
        /// <param name="blockIndex">index of the removed block.</param>
        public FocusRemoveBlockOperation(IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> inner, IFocusBrowsingExistingBlockNodeIndex blockIndex)
            : base(inner, blockIndex)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the block removal is taking place.
        /// </summary>
        public new IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> Inner { get { return (IFocusBlockListInner<IFocusBrowsingBlockNodeIndex>)base.Inner; } }

        /// <summary>
        /// Index of the removed block.
        /// </summary>
        public new IFocusBrowsingExistingBlockNodeIndex BlockIndex { get { return (IFocusBrowsingExistingBlockNodeIndex)base.BlockIndex; } }

        /// <summary>
        /// Block state removed.
        /// </summary>
        public new IFocusBlockState BlockState { get { return (IFocusBlockState)base.BlockState; } }
        #endregion
    }
}
