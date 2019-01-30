﻿#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    /// <summary>
    /// Read-only list of IxxxCellView
    /// </summary>
    public interface IFrameCellViewReadOnlyList : IReadOnlyList<IFrameCellView>, IEqualComparable
    {
        bool Contains(IFrameCellView value);
        int IndexOf(IFrameCellView value);
    }

    /// <summary>
    /// Read-only list of IxxxCellView
    /// </summary>
    public class FrameCellViewReadOnlyList : ReadOnlyCollection<IFrameCellView>, IFrameCellViewReadOnlyList
    {
        public FrameCellViewReadOnlyList(IFrameCellViewList list)
            : base(list)
        {
        }

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameCellViewReadOnlyList"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameCellViewReadOnlyList AsCellViewList))
                return comparer.Failed();

            if (Count != AsCellViewList.Count)
                return comparer.Failed();

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsCellViewList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion
    }
}
