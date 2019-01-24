﻿using EaslyController.Frame;
using EaslyController.Writeable;
using System;

namespace EaslyController.Focus
{
    /// <summary>
    /// Operation details for splitting a block in a block list.
    /// </summary>
    public interface IFocusSplitBlockOperation : IFrameSplitBlockOperation, IFocusOperation
    {
        /// <summary>
        /// Inner where the block is split.
        /// </summary>
        new IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// Index of the last node to stay in the old block.
        /// </summary>
        new IFocusBrowsingExistingBlockNodeIndex NodeIndex { get; }

        /// <summary>
        /// Block state inserted.
        /// </summary>
        new IFocusBlockState BlockState { get; }
    }

    /// <summary>
    /// Operation details for splitting a block in a block list.
    /// </summary>
    public class FocusSplitBlockOperation : FrameSplitBlockOperation, IFocusSplitBlockOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FocusSplitBlockOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the block is split.</param>
        /// <param name="nodeIndex">Index of the last node to stay in the old block.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusSplitBlockOperation(IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> inner, IFocusBrowsingExistingBlockNodeIndex nodeIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(inner, nodeIndex, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the block is split.
        /// </summary>
        public new IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> Inner { get { return (IFocusBlockListInner<IFocusBrowsingBlockNodeIndex>)base.Inner; } }

        /// <summary>
        /// Index of the last node to stay in the old block.
        /// </summary>
        public new IFocusBrowsingExistingBlockNodeIndex NodeIndex { get { return (IFocusBrowsingExistingBlockNodeIndex)base.NodeIndex; } }

        /// <summary>
        /// Block state inserted.
        /// </summary>
        public new IFocusBlockState BlockState { get { return (IFocusBlockState)base.BlockState; } }
        #endregion
    }
}
