﻿#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <summary>
    /// Read-only list of IxxxOperationGroup
    /// </summary>
    public interface IFocusOperationGroupReadOnlyList : IFrameOperationGroupReadOnlyList, IReadOnlyList<IFocusOperationGroup>
    {
        new int Count { get; }
        new IFocusOperationGroup this[int index] { get; }
        bool Contains(IFocusOperationGroup value);
        int IndexOf(IFocusOperationGroup value);
        new IEnumerator<IFocusOperationGroup> GetEnumerator();
    }

    /// <summary>
    /// Read-only list of IxxxOperationGroup
    /// </summary>
    internal class FocusOperationGroupReadOnlyList : ReadOnlyCollection<IFocusOperationGroup>, IFocusOperationGroupReadOnlyList
    {
        public FocusOperationGroupReadOnlyList(IFocusOperationGroupList list)
            : base(list)
        {
        }

        #region Writeable
        IWriteableOperationGroup IReadOnlyList<IWriteableOperationGroup>.this[int index] { get { return this[index]; } }
        public bool Contains(IWriteableOperationGroup value) { return base.Contains((IFocusOperationGroup)value); }
        public int IndexOf(IWriteableOperationGroup value) { return base.IndexOf((IFocusOperationGroup)value); }
        IEnumerator<IWriteableOperationGroup> IEnumerable<IWriteableOperationGroup>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Frame
        IFrameOperationGroup IFrameOperationGroupReadOnlyList.this[int index] { get { return this[index]; } }
        IFrameOperationGroup IReadOnlyList<IFrameOperationGroup>.this[int index] { get { return this[index]; } }
        public bool Contains(IFrameOperationGroup value) { return base.Contains((IFocusOperationGroup)value); }
        public int IndexOf(IFrameOperationGroup value) { return base.IndexOf((IFocusOperationGroup)value); }
        IEnumerator<IFrameOperationGroup> IFrameOperationGroupReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IFrameOperationGroup> IEnumerable<IFrameOperationGroup>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
