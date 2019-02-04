﻿#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Read-only list of IxxxIndexCollection
    /// </summary>
    internal interface IFocusIndexCollectionReadOnlyList : IFrameIndexCollectionReadOnlyList, IReadOnlyList<IFocusIndexCollection>
    {
        new int Count { get; }
        new IFocusIndexCollection this[int index] { get; }
        bool Contains(IFocusIndexCollection value);
        int IndexOf(IFocusIndexCollection value);
        new IEnumerator<IFocusIndexCollection> GetEnumerator();
    }

    /// <summary>
    /// Read-only list of IxxxIndexCollection
    /// </summary>
    internal class FocusIndexCollectionReadOnlyList : ReadOnlyCollection<IFocusIndexCollection>, IFocusIndexCollectionReadOnlyList
    {
        public FocusIndexCollectionReadOnlyList(IFocusIndexCollectionList list)
            : base(list)
        {
        }

        #region ReadOnly
        IReadOnlyIndexCollection IReadOnlyList<IReadOnlyIndexCollection>.this[int index] { get { return this[index]; } }
        public bool Contains(IReadOnlyIndexCollection value) { return base.Contains((IFocusIndexCollection)value); }
        public int IndexOf(IReadOnlyIndexCollection value) { return base.IndexOf((IFocusIndexCollection)value); }
        IEnumerator<IReadOnlyIndexCollection> IEnumerable<IReadOnlyIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteableIndexCollection IWriteableIndexCollectionReadOnlyList.this[int index] { get { return this[index]; } }
        IWriteableIndexCollection IReadOnlyList<IWriteableIndexCollection>.this[int index] { get { return this[index]; } }
        public bool Contains(IWriteableIndexCollection value) { return base.Contains((IFocusIndexCollection)value); }
        public int IndexOf(IWriteableIndexCollection value) { return base.IndexOf((IFocusIndexCollection)value); }
        IEnumerator<IWriteableIndexCollection> IWriteableIndexCollectionReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteableIndexCollection> IEnumerable<IWriteableIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Frame
        IFrameIndexCollection IFrameIndexCollectionReadOnlyList.this[int index] { get { return this[index]; } }
        IFrameIndexCollection IReadOnlyList<IFrameIndexCollection>.this[int index] { get { return this[index]; } }
        public bool Contains(IFrameIndexCollection value) { return base.Contains((IFocusIndexCollection)value); }
        public int IndexOf(IFrameIndexCollection value) { return base.IndexOf((IFocusIndexCollection)value); }
        IEnumerator<IFrameIndexCollection> IFrameIndexCollectionReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IFrameIndexCollection> IEnumerable<IFrameIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
