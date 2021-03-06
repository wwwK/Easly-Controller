﻿#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Read-only list of IxxxBlockState
    /// </summary>
    public interface IFocusBlockStateReadOnlyList : IFrameBlockStateReadOnlyList, IReadOnlyList<IFocusBlockState>
    {
        new IFocusBlockState this[int index] { get; }
        new int Count { get; }
        bool Contains(IFocusBlockState value);
        new IEnumerator<IFocusBlockState> GetEnumerator();
        int IndexOf(IFocusBlockState value);
    }

    /// <summary>
    /// Read-only list of IxxxBlockState
    /// </summary>
    internal class FocusBlockStateReadOnlyList : ReadOnlyCollection<IFocusBlockState>, IFocusBlockStateReadOnlyList
    {
        public FocusBlockStateReadOnlyList(IFocusBlockStateList list)
            : base(list)
        {
        }

        #region ReadOnly
        bool IReadOnlyBlockStateReadOnlyList.Contains(IReadOnlyBlockState value) { return Contains((IFocusBlockState)value); }
        int IReadOnlyBlockStateReadOnlyList.IndexOf(IReadOnlyBlockState value) { return IndexOf((IFocusBlockState)value); }
        IEnumerator<IReadOnlyBlockState> IEnumerable<IReadOnlyBlockState>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyBlockState IReadOnlyList<IReadOnlyBlockState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteableBlockState IWriteableBlockStateReadOnlyList.this[int index] { get { return this[index]; } }
        bool IWriteableBlockStateReadOnlyList.Contains(IWriteableBlockState value) { return Contains((IFocusBlockState)value); }
        IEnumerator<IWriteableBlockState> IWriteableBlockStateReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        int IWriteableBlockStateReadOnlyList.IndexOf(IWriteableBlockState value) { return IndexOf((IFocusBlockState)value); }
        IEnumerator<IWriteableBlockState> IEnumerable<IWriteableBlockState>.GetEnumerator() { return GetEnumerator(); }
        IWriteableBlockState IReadOnlyList<IWriteableBlockState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Frame
        IFrameBlockState IFrameBlockStateReadOnlyList.this[int index] { get { return this[index]; } }
        bool IFrameBlockStateReadOnlyList.Contains(IFrameBlockState value) { return Contains((IFocusBlockState)value); }
        IEnumerator<IFrameBlockState> IFrameBlockStateReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        int IFrameBlockStateReadOnlyList.IndexOf(IFrameBlockState value) { return IndexOf((IFocusBlockState)value); }
        IEnumerator<IFrameBlockState> IEnumerable<IFrameBlockState>.GetEnumerator() { return GetEnumerator(); }
        IFrameBlockState IReadOnlyList<IFrameBlockState>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
