﻿#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// List of IxxxOperation
    /// </summary>
    public interface IWriteableOperationList : IList<IWriteableOperation>, IReadOnlyList<IWriteableOperation>
    {
        new int Count { get; }
        new IWriteableOperation this[int index] { get; set; }
        new IEnumerator<IWriteableOperation> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxOperation
    /// </summary>
    internal class WriteableOperationList : Collection<IWriteableOperation>, IWriteableOperationList
    {
    }
}
