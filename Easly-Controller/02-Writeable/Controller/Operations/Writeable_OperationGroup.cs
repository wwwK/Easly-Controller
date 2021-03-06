﻿namespace EaslyController.Writeable
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Group of operations to make some tasks atomic.
    /// </summary>
    public interface IWriteableOperationGroup
    {
        /// <summary>
        /// List of operations belonging to this group.
        /// </summary>
        IWriteableOperationReadOnlyList OperationList { get; }

        /// <summary>
        /// The main operation for this group.
        /// </summary>
        IWriteableOperation MainOperation { get; }

        /// <summary>
        /// Optional refresh operation to execute at the end of undo and redo.
        /// </summary>
        IWriteableGenericRefreshOperation Refresh { get; }

        /// <summary>
        /// Execute all operations in the group.
        /// </summary>
        void Redo();

        /// <summary>
        /// Undo all operations in the group.
        /// </summary>
        void Undo();
    }

    /// <summary>
    /// Group of operations to make some tasks atomic.
    /// </summary>
    internal class WriteableOperationGroup : IWriteableOperationGroup
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableOperationGroup"/> class.
        /// </summary>
        /// <param name="operationList">List of operations belonging to this group.</param>
        /// <param name="refresh">Optional refresh operation to execute at the end of undo and redo.</param>
        public WriteableOperationGroup(IWriteableOperationReadOnlyList operationList, IWriteableGenericRefreshOperation refresh)
        {
            Debug.Assert(operationList != null);
            Debug.Assert(operationList.Count > 0);

            OperationList = operationList;
            Refresh = refresh;
        }
        #endregion

        #region Properties
        /// <summary>
        /// List of operations belonging to this group.
        /// </summary>
        public IWriteableOperationReadOnlyList OperationList { get; }

        /// <summary>
        /// The main operation for this group.
        /// </summary>
        public IWriteableOperation MainOperation { get { return OperationList[0]; } }

        /// <summary>
        /// Optional refresh operation to execute at the end of undo and redo.
        /// </summary>
        public IWriteableGenericRefreshOperation Refresh { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Execute all operations in the group.
        /// </summary>
        public virtual void Redo()
        {
            for (int i = 0; i < OperationList.Count; i++)
            {
                IWriteableOperation Operation = OperationList[i];
                Operation.Redo();
            }

            if (Refresh != null)
                Refresh.Redo();
        }

        /// <summary>
        /// Undo all operations in the group.
        /// </summary>
        public virtual void Undo()
        {
            for (int i = OperationList.Count; i > 0; i--)
            {
                IWriteableOperation Operation = OperationList[i - 1];
                Operation.Undo();
            }

            if (Refresh != null)
                Refresh.Redo();
        }
        #endregion
    }
}
