﻿using EaslyController.Writeable;

namespace EaslyController.Frame
{
    /// <summary>
    /// View of a node state.
    /// </summary>
    public interface IFrameNodeStateView : IWriteableNodeStateView
    {
        /// <summary>
        /// The node state.
        /// </summary>
        new IFrameNodeState State { get; }

        /// <summary>
        /// The template used to display the state.
        /// </summary>
        IFrameTemplate Template { get; }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        IFrameCellView RootCellView { get; }

        void RecalculateLineNumbers(IFrameController controller, ref int lineNumber, ref int columnNumber);
    }

    /// <summary>
    /// View of a node state.
    /// </summary>
    public abstract class FrameNodeStateView : WriteableNodeStateView, IFrameNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameNodeStateView"/> class.
        /// </summary>
        /// <param name="state">The node state.</param>
        /// <param name="templateSet">The template set used to display the state.</param>
        public FrameNodeStateView(IFrameNodeState state, IFrameTemplateSet templateSet)
            : base(state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The node state.
        /// </summary>
        public new IFrameNodeState State { get { return (IFrameNodeState)base.State; } }

        /// <summary>
        /// The template used to display the state.
        /// </summary>
        public abstract IFrameTemplate Template { get; }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        public abstract IFrameCellView RootCellView { get; }
        #endregion

        #region Client Interface
        public abstract void RecalculateLineNumbers(IFrameController controller, ref int lineNumber, ref int columnNumber);
        #endregion
    }
}
