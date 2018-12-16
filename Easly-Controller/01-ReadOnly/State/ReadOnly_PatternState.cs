﻿using BaseNode;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// State of an replication pattern node.
    /// </summary>
    public interface IReadOnlyPatternState : IReadOnlyPlaceholderNodeState
    {
        /// <summary>
        /// The replication pattern node.
        /// </summary>
        new IPattern Node { get; }

        /// <summary>
        /// The parent block state.
        /// </summary>
        IReadOnlyBlockState ParentBlockState { get; }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new IReadOnlyBrowsingPatternIndex ParentIndex { get; }

        /// <summary>
        /// Returns a clone of the node of this state.
        /// </summary>
        /// <returns>The cloned node.</returns>
        new IPattern CloneNode();
    }

    /// <summary>
    /// State of an replication pattern node.
    /// </summary>
    public class ReadOnlyPatternState : ReadOnlyPlaceholderNodeState, IReadOnlyPatternState
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyPatternState"/> class.
        /// </summary>
        /// <param name="parentBlockState">The parent block state.</param>
        /// <param name="index">The index used to create the state.</param>
        public ReadOnlyPatternState(IReadOnlyBlockState parentBlockState, IReadOnlyBrowsingPatternIndex index)
            : base(index)
        {
            Debug.Assert(parentBlockState != null);

            ParentBlockState = parentBlockState;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The replication pattern node.
        /// </summary>
        public new IPattern Node { get { return (IPattern)base.Node; } }

        /// <summary>
        /// The parent block state.
        /// </summary>
        public IReadOnlyBlockState ParentBlockState { get; }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new IReadOnlyBrowsingPatternIndex ParentIndex { get { return (IReadOnlyBrowsingPatternIndex)base.ParentIndex; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Returns a clone of the node of this state.
        /// </summary>
        /// <returns>The cloned node.</returns>
        public new IPattern CloneNode() { return (IPattern)base.CloneNode(); }
        #endregion
    }
}