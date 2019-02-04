﻿#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using EaslyController.Frame;

    /// <summary>
    /// List of IxxxVisibleCellView
    /// </summary>
    public interface IFocusVisibleCellViewList : IFrameVisibleCellViewList, IList<IFocusVisibleCellView>, IReadOnlyList<IFocusVisibleCellView>, IEqualComparable
    {
        new int Count { get; }
        new IFocusVisibleCellView this[int index] { get; set; }
        new IEnumerator<IFocusVisibleCellView> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxVisibleCellView
    /// </summary>
    public class FocusVisibleCellViewList : Collection<IFocusVisibleCellView>, IFocusVisibleCellViewList
    {
        #region Frame
        IFrameVisibleCellView IFrameVisibleCellViewList.this[int index] { get { return this[index]; } set { this[index] = (IFocusVisibleCellView)value; } }
        IFrameVisibleCellView IList<IFrameVisibleCellView>.this[int index] { get { return this[index]; } set { this[index] = (IFocusVisibleCellView)value; } }
        IFrameVisibleCellView IReadOnlyList<IFrameVisibleCellView>.this[int index] { get { return this[index]; } }
        public void Add(IFrameVisibleCellView item) { base.Add((IFocusVisibleCellView)item); }
        public void Insert(int index, IFrameVisibleCellView item) { base.Insert(index, (IFocusVisibleCellView)item); }
        public bool Remove(IFrameVisibleCellView item) { return base.Remove((IFocusVisibleCellView)item); }
        public void CopyTo(IFrameVisibleCellView[] array, int index) { base.CopyTo((IFocusVisibleCellView[])array, index); }
        bool ICollection<IFrameVisibleCellView>.IsReadOnly { get { return ((ICollection<IFocusVisibleCellView>)this).IsReadOnly; } }
        public bool Contains(IFrameVisibleCellView value) { return base.Contains((IFocusVisibleCellView)value); }
        public int IndexOf(IFrameVisibleCellView value) { return base.IndexOf((IFocusVisibleCellView)value); }
        IEnumerator<IFrameVisibleCellView> IFrameVisibleCellViewList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IFrameVisibleCellView> IEnumerable<IFrameVisibleCellView>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusVisibleCellViewList"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusVisibleCellViewList AsVisibleCellViewList))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsVisibleCellViewList.Count))
                return comparer.Failed();

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsVisibleCellViewList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion
    }
}
