﻿namespace EaslyController.Focus
{
    using System.Diagnostics;
    using EaslyController.Frame;

    /// <summary>
    /// Base interface for collection of cell views.
    /// </summary>
    public interface IFocusCellViewCollection : IFrameCellViewCollection, IFocusCellView, IFocusAssignableCellView
    {
        /// <summary>
        /// The collection of child cells.
        /// </summary>
        new IFocusCellViewList CellViewList { get; }

        /// <summary>
        /// The frame that was used to create this cell. Can be null.
        /// </summary>
        new IFocusFrame Frame { get; }
    }

    /// <summary>
    /// Base interface for collection of cell views.
    /// </summary>
    internal abstract class FocusCellViewCollection : FrameCellViewCollection, IFocusCellViewCollection
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusCellViewCollection"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        /// <param name="cellViewList">The list of child cell views.</param>
        /// <param name="frame">The frame that was used to create this cell. Can be null.</param>
        public FocusCellViewCollection(IFocusNodeStateView stateView, IFocusCellViewCollection parentCellView, IFocusCellViewList cellViewList, IFocusFrame frame)
            : base(stateView, parentCellView, cellViewList, frame)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        public new IFocusNodeStateView StateView { get { return (IFocusNodeStateView)base.StateView; } }

        /// <summary>
        /// The collection of cell views containing this view. Null for the root of the cell tree.
        /// </summary>
        public new IFocusCellViewCollection ParentCellView { get { return (IFocusCellViewCollection)base.ParentCellView; } }

        /// <summary>
        /// The collection of child cells.
        /// </summary>
        public new IFocusCellViewList CellViewList { get { return (IFocusCellViewList)base.CellViewList; } }

        /// <summary>
        /// The frame that was used to create this cell. Can be null.
        /// </summary>
        public new IFocusFrame Frame { get { return (IFocusFrame)base.Frame; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Updates the focus chain with cells in the tree.
        /// </summary>
        /// <param name="focusChain">The list of focusable cell views found in the tree.</param>
        public virtual void UpdateFocusChain(IFocusFocusableCellViewList focusChain)
        {
            foreach (IFocusCellView Item in CellViewList)
                Item.UpdateFocusChain(focusChain);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusCellViewCollection AsCellViewCollection))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsCellViewCollection))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}