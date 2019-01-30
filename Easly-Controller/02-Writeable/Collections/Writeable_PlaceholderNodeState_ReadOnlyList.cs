﻿#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Read-only list of IxxxPlaceholderNodeState
    /// </summary>
    public interface IWriteablePlaceholderNodeStateReadOnlyList : IReadOnlyPlaceholderNodeStateReadOnlyList, IReadOnlyList<IWriteablePlaceholderNodeState>
    {
        new int Count { get; }
        new IWriteablePlaceholderNodeState this[int index] { get; }
        bool Contains(IWriteablePlaceholderNodeState value);
        int IndexOf(IWriteablePlaceholderNodeState value);
        new IEnumerator<IWriteablePlaceholderNodeState> GetEnumerator();
    }

    /// <summary>
    /// Read-only list of IxxxPlaceholderNodeState
    /// </summary>
    internal class WriteablePlaceholderNodeStateReadOnlyList : ReadOnlyCollection<IWriteablePlaceholderNodeState>, IWriteablePlaceholderNodeStateReadOnlyList
    {
        public WriteablePlaceholderNodeStateReadOnlyList(IWriteablePlaceholderNodeStateList list)
            : base(list)
        {
        }

        #region ReadOnly
        public new IReadOnlyPlaceholderNodeState this[int index] { get { return base[index]; } }
        public bool Contains(IReadOnlyPlaceholderNodeState value) { return base.Contains((IWriteablePlaceholderNodeState)value); }
        public int IndexOf(IReadOnlyPlaceholderNodeState value) { return base.IndexOf((IWriteablePlaceholderNodeState)value); }
        public new IEnumerator<IReadOnlyPlaceholderNodeState> GetEnumerator() { return base.GetEnumerator(); }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IWriteablePlaceholderNodeStateReadOnlyList"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IWriteablePlaceholderNodeStateReadOnlyList AsPlaceholderNodeStateReadOnlyList))
                return comparer.Failed();

            if (Count != AsPlaceholderNodeStateReadOnlyList.Count)
                return comparer.Failed();

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsPlaceholderNodeStateReadOnlyList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion
    }
}
