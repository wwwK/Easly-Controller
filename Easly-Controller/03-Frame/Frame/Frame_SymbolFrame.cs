﻿namespace EaslyController.Frame
{
    using EaslyController.Constants;

    /// <summary>
    /// Frame for decoration purpose only.
    /// </summary>
    public interface IFrameSymbolFrame : IFrameStaticFrame
    {
        /// <summary>
        /// Free symbol.
        /// (Set in Xaml)
        /// </summary>
        Symbols Symbol { get; set; }
    }

    /// <summary>
    /// Frame for decoration purpose only.
    /// </summary>
    public class FrameSymbolFrame : FrameStaticFrame, IFrameSymbolFrame
    {
        #region Properties
        /// <summary>
        /// Free symbol.
        /// (Set in Xaml)
        /// </summary>
        public Symbols Symbol { get; set; }
        #endregion
    }
}
