﻿using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// View of a block state.
    /// </summary>
    public interface IWriteableBlockStateView : IReadOnlyBlockStateView
    {
        /// <summary>
        /// The block state.
        /// </summary>
        new IWriteableBlockState BlockState { get; }
    }

    /// <summary>
    /// View of a block state.
    /// </summary>
    public class WriteableBlockStateView : ReadOnlyBlockStateView, IWriteableBlockStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBlockStateView"/> class.
        /// </summary>
        /// <param name="blockState">The block state.</param>
        public WriteableBlockStateView(IWriteableBlockState blockState)
            : base(blockState)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The block state.
        /// </summary>
        public new IWriteableBlockState BlockState { get { return (IWriteableBlockState)base.BlockState; } }
        #endregion
    }
}