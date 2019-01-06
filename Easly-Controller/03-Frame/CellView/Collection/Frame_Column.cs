﻿using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// A collection of cell views organized in a column.
    /// </summary>
    public interface IFrameColumn : IFrameCellViewCollection
    {
    }

    /// <summary>
    /// A collection of cell views organized in a column.
    /// </summary>
    public class FrameColumn : FrameCellViewCollection, IFrameColumn
    {
        #region Init
        /// <summary>
        /// Initializes an instance of <see cref="FrameColumn"/>.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="cellViewList">The list of child cell views.</param>
        public FrameColumn(IFrameNodeStateView stateView, IFrameCellViewList cellViewList)
            : base(stateView, cellViewList)
        {
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update line numbers in the cell view.
        /// </summary>
        /// <param name="lineNumber">The current line number, updated upon return.</param>
        /// <param name="columnNumber">The current column number, updated upon return.</param>
        public override void UpdateLineNumbers(ref int lineNumber, ref int columnNumber)
        {
            int StartColumnNumber = columnNumber;
            int MaxColumnNumber = columnNumber;

            foreach (IFrameCellView CellView in CellViewList)
            {
                int ChildColumnNumber = StartColumnNumber;
                RecalculateChildLineNumbers(CellView, ref lineNumber, ref ChildColumnNumber);

                if (MaxColumnNumber < ChildColumnNumber)
                    MaxColumnNumber = ChildColumnNumber;
            }

            columnNumber = MaxColumnNumber;
        }
        #endregion

        #region Descendant Interface
        /// <summary>
        /// Update line numbers in the cell view from the update in a child cell.
        /// </summary>
        /// <param name="lineNumber">The child cell view.</param>
        /// <param name="lineNumber">The current line number, updated upon return.</param>
        /// <param name="columnNumber">The current column number, updated upon return.</param>
        protected virtual void RecalculateChildLineNumbers(IFrameCellView cell, ref int lineNumber, ref int columnNumber)
        {
            cell.UpdateLineNumbers(ref lineNumber, ref columnNumber);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameCellView"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameColumn AsColumn))
                return false;

            if (!base.IsEqual(comparer, AsColumn))
                return false;

            return true;
        }

        public override string PrintTree(int indentation, bool printFull)
        {
            string Result = "";
            for (int i = 0; i < indentation; i++)
                Result += " ";

            Result += $" Column, {CellViewList.Count} cell(s)\n";

            foreach (IFrameCellView Item in CellViewList)
                Result += Item.PrintTree(indentation + 1, printFull);

            return Result;
        }
        #endregion
    }
}
