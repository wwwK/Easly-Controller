﻿#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Read-only list of IxxxIndexCollection
    /// </summary>
    internal interface IFrameIndexCollectionReadOnlyList : IWriteableIndexCollectionReadOnlyList, IReadOnlyList<IFrameIndexCollection>
    {
        new int Count { get; }
        new IFrameIndexCollection this[int index] { get; }
        bool Contains(IFrameIndexCollection value);
        int IndexOf(IFrameIndexCollection value);
        new IEnumerator<IFrameIndexCollection> GetEnumerator();
    }

    /// <summary>
    /// Read-only list of IxxxIndexCollection
    /// </summary>
    internal class FrameIndexCollectionReadOnlyList : ReadOnlyCollection<IFrameIndexCollection>, IFrameIndexCollectionReadOnlyList
    {
        public FrameIndexCollectionReadOnlyList(IFrameIndexCollectionList list)
            : base(list)
        {
        }

        #region ReadOnly
        IReadOnlyIndexCollection IReadOnlyList<IReadOnlyIndexCollection>.this[int index] { get { return this[index]; } }
        public bool Contains(IReadOnlyIndexCollection value) { return base.Contains((IFrameIndexCollection)value); }
        public int IndexOf(IReadOnlyIndexCollection value) { return base.IndexOf((IFrameIndexCollection)value); }
        IEnumerator<IReadOnlyIndexCollection> IEnumerable<IReadOnlyIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteableIndexCollection IWriteableIndexCollectionReadOnlyList.this[int index] { get { return this[index]; } }
        IWriteableIndexCollection IReadOnlyList<IWriteableIndexCollection>.this[int index] { get { return this[index]; } }
        public bool Contains(IWriteableIndexCollection value) { return base.Contains((IFrameIndexCollection)value); }
        public int IndexOf(IWriteableIndexCollection value) { return base.IndexOf((IFrameIndexCollection)value); }
        IEnumerator<IWriteableIndexCollection> IWriteableIndexCollectionReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteableIndexCollection> IEnumerable<IWriteableIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
