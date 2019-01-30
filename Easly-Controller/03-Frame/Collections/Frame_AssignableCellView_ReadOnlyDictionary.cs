﻿#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    /// <summary>
    /// Read-only dictionary of ..., IxxxInner
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public interface IFrameAssignableCellViewReadOnlyDictionary<TKey> : IReadOnlyDictionary<TKey, IFrameAssignableCellView>, IEqualComparable
    {
    }

    /// <summary>
    /// Read-only dictionary of ..., IxxxInner
    /// </summary>
    /// <typeparam name="TKey">Type of the key.</typeparam>
    public class FrameAssignableCellViewReadOnlyDictionary<TKey> : ReadOnlyDictionary<TKey, IFrameAssignableCellView>, IFrameAssignableCellViewReadOnlyDictionary<TKey>
    {
        public FrameAssignableCellViewReadOnlyDictionary(IFrameAssignableCellViewDictionary<TKey> dictionary)
            : base(dictionary)
        {
        }

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameAssignableCellViewReadOnlyDictionary{TKey}"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameAssignableCellViewReadOnlyDictionary<TKey> AsAssignableCellViewReadOnlyDictionary))
                return comparer.Failed();

            if (Count != AsAssignableCellViewReadOnlyDictionary.Count)
                return comparer.Failed();

            foreach (KeyValuePair<TKey, IFrameAssignableCellView> Entry in this)
            {
                Debug.Assert(Entry.Key != null);

                if (!AsAssignableCellViewReadOnlyDictionary.ContainsKey(Entry.Key))
                    return comparer.Failed();

                IFrameAssignableCellView OtherValue = AsAssignableCellViewReadOnlyDictionary[Entry.Key] as IFrameAssignableCellView;

                if (Entry.Value != null && OtherValue != null)
                {
                    if (!comparer.VerifyEqual(Entry.Value, OtherValue))
                        return comparer.Failed();
                }
                else if (Entry.Value != null || OtherValue != null)
                    return comparer.Failed();
            }

            return true;
        }
        #endregion
    }
}
