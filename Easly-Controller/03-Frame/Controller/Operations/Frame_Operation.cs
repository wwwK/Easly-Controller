﻿using EaslyController.Writeable;

namespace EaslyController.Frame
{
    /// <summary>
    /// Base for all operations modifying the node tree.
    /// </summary>
    public interface IFrameOperation : IWriteableOperation
    {
    }

    /// <summary>
    /// Base for all operations modifying the node tree.
    /// </summary>
    public class FrameOperation : WriteableOperation, IFrameOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of a <see cref="FrameOperation"/> object.
        /// </summary>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameOperation(bool isNested)
            : base(isNested)
        {
        }
        #endregion
    }
}