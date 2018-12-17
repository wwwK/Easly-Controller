﻿using EaslyController.ReadOnly;
using System.Collections.Generic;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Collection of node indexes.
    /// </summary>
    public interface IWriteableIndexCollection : IReadOnlyIndexCollection
    {
    }

    /// <summary>
    /// Collection of node indexes.
    /// </summary>
    public interface IWriteableIndexCollection<out IIndex> : IReadOnlyIndexCollection<IIndex>
        where IIndex : IWriteableBrowsingChildIndex
    {
    }

    /// <summary>
    /// Collection of node indexes.
    /// </summary>
    public class WriteableIndexCollection<IIndex> : ReadOnlyIndexCollection<IIndex>, IWriteableIndexCollection<IIndex>, IWriteableIndexCollection
        where IIndex : IWriteableBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableIndexCollection{IIndex}"/> class.
        /// </summary>
        /// <param name="propertyName">Property indexed for all nodes in the collection.</param>
        /// <param name="nodeIndexList">Collection of node indexes.</param>
        public WriteableIndexCollection(string propertyName, IReadOnlyList<IIndex> nodeIndexList)
            : base(propertyName, nodeIndexList)
        {
        }
        #endregion
    }
}