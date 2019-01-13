﻿using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Base for all operations modifying the node tree.
    /// </summary>
    public interface IFocusOperation : IFrameOperation
    {
    }

    /// <summary>
    /// Base for all operations modifying the node tree.
    /// </summary>
    public class FocusOperation : FrameOperation, IFocusOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of a <see cref="FocusOperation"/> object.
        /// </summary>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusOperation(bool isNested)
            : base(isNested)
        {
        }
        #endregion
    }
}