﻿#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;

    /// <summary>
    /// List of IxxxKeywordFrame
    /// </summary>
    public interface IFocusKeywordFrameList : IFrameKeywordFrameList, IList<IFocusKeywordFrame>, IReadOnlyList<IFocusKeywordFrame>
    {
        new int Count { get; }
        new IFocusKeywordFrame this[int index] { get; set; }
        new IEnumerator<IFocusKeywordFrame> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxKeywordFrame
    /// </summary>
    internal class FocusKeywordFrameList : Collection<IFocusKeywordFrame>, IFocusKeywordFrameList
    {
        #region Frame
        IFrameKeywordFrame IFrameKeywordFrameList.this[int index] { get { return this[index]; } set { this[index] = (IFocusKeywordFrame)value; } }
        IFrameKeywordFrame IList<IFrameKeywordFrame>.this[int index] { get { return this[index]; } set { this[index] = (IFocusKeywordFrame)value; } }
        IFrameKeywordFrame IReadOnlyList<IFrameKeywordFrame>.this[int index] { get { return this[index]; } }
        void ICollection<IFrameKeywordFrame>.Add(IFrameKeywordFrame item) { Add((IFocusKeywordFrame)item); }
        void IList<IFrameKeywordFrame>.Insert(int index, IFrameKeywordFrame item) { Insert(index, (IFocusKeywordFrame)item); }
        bool ICollection<IFrameKeywordFrame>.Remove(IFrameKeywordFrame item) { return Remove((IFocusKeywordFrame)item); }
        void ICollection<IFrameKeywordFrame>.CopyTo(IFrameKeywordFrame[] array, int index) { CopyTo((IFocusKeywordFrame[])array, index); }
        bool ICollection<IFrameKeywordFrame>.IsReadOnly { get { return ((ICollection<IFocusKeywordFrame>)this).IsReadOnly; } }
        bool ICollection<IFrameKeywordFrame>.Contains(IFrameKeywordFrame value) { return Contains((IFocusKeywordFrame)value); }
        int IList<IFrameKeywordFrame>.IndexOf(IFrameKeywordFrame value) { return IndexOf((IFocusKeywordFrame)value); }
        IEnumerator<IFrameKeywordFrame> IFrameKeywordFrameList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IFrameKeywordFrame> IEnumerable<IFrameKeywordFrame>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
