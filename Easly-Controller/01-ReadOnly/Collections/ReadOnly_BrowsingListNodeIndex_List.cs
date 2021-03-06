﻿#pragma warning disable 1591

namespace EaslyController.ReadOnly
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// List of IxxxBrowsingListNodeIndex
    /// </summary>
    public interface IReadOnlyBrowsingListNodeIndexList : IList<IReadOnlyBrowsingListNodeIndex>, IReadOnlyList<IReadOnlyBrowsingListNodeIndex>
    {
        new IReadOnlyBrowsingListNodeIndex this[int index] { get; set; }
        new int Count { get; }
    }

    /// <summary>
    /// List of IxxxBrowsingListNodeIndex
    /// </summary>
    internal class ReadOnlyBrowsingListNodeIndexList : Collection<IReadOnlyBrowsingListNodeIndex>, IReadOnlyBrowsingListNodeIndexList
    {
    }
}
