﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// List of IxxxNodeState
    /// </summary>
    public interface IReadOnlyNodeStateList : IList<IReadOnlyNodeState>, IReadOnlyList<IReadOnlyNodeState>
    {
        new int Count { get; }
        new IReadOnlyNodeState this[int index] { get; set; }
    }

    /// <summary>
    /// List of IxxxNodeState
    /// </summary>
    public class ReadOnlyNodeStateList : Collection<IReadOnlyNodeState>, IReadOnlyNodeStateList
    {
    }
}