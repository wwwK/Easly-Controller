﻿using EaslyController.Writeable;
using System;

namespace EaslyController.Frame
{
    /// <summary>
    /// Operation details for replacing a node.
    /// </summary>
    public interface IFrameReplaceOperation : IWriteableReplaceOperation, IFrameOperation
    {
        /// <summary>
        /// Inner where the replacement is taking place.
        /// </summary>
        new IFrameInner<IFrameBrowsingChildIndex> Inner { get; }

        /// <summary>
        /// Position where the node is replaced.
        /// </summary>
        new IFrameInsertionChildIndex ReplacementIndex { get; }

        /// <summary>
        /// Index of the state before it's replaced.
        /// </summary>
        new IFrameBrowsingChildIndex OldBrowsingIndex { get; }

        /// <summary>
        /// Index of the state after it's replaced.
        /// </summary>
        new IFrameBrowsingChildIndex NewBrowsingIndex { get; }

        /// <summary>
        /// The old state.
        /// </summary>
        new IFrameNodeState OldChildState { get; }

        /// <summary>
        /// The new state.
        /// </summary>
        new IFrameNodeState NewChildState { get; }
    }

    /// <summary>
    /// Operation details for replacing a node.
    /// </summary>
    public class FrameReplaceOperation : WriteableReplaceOperation, IFrameReplaceOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FrameReplaceOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the replacement is taking place.</param>
        /// <param name="replacementIndex">Position where the node is replaced.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameReplaceOperation(IFrameInner<IFrameBrowsingChildIndex> inner, IFrameInsertionChildIndex replacementIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(inner, replacementIndex, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the replacement is taking place.
        /// </summary>
        public new IFrameInner<IFrameBrowsingChildIndex> Inner { get { return (IFrameInner<IFrameBrowsingChildIndex>)base.Inner; } }

        /// <summary>
        /// Position where the node is replaced.
        /// </summary>
        public new IFrameInsertionChildIndex ReplacementIndex { get { return (IFrameInsertionChildIndex)base.ReplacementIndex; } }

        /// <summary>
        /// Index of the state before it's replaced.
        /// </summary>
        public new IFrameBrowsingChildIndex OldBrowsingIndex { get { return (IFrameBrowsingChildIndex)base.OldBrowsingIndex; } }

        /// <summary>
        /// Index of the state after it's replaced.
        /// </summary>
        public new IFrameBrowsingChildIndex NewBrowsingIndex { get { return (IFrameBrowsingChildIndex)base.NewBrowsingIndex; } }

        /// <summary>
        /// The old state.
        /// </summary>
        public new IFrameNodeState OldChildState { get { return (IFrameNodeState)base.OldChildState; } }

        /// <summary>
        /// The new state.
        /// </summary>
        public new IFrameNodeState NewChildState { get { return (IFrameNodeState)base.NewChildState; } }
        #endregion
    }
}
