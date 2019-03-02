﻿namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Controller;
    using EaslyController.Focus;

    /// <summary>
    /// A cell view for a block state.
    /// </summary>
    public interface ILayoutBlockCellView : IFocusBlockCellView, ILayoutCellView
    {
        /// <summary>
        /// The collection of cell views containing this view.
        /// </summary>
        new ILayoutCellViewCollection ParentCellView { get; }

        /// <summary>
        /// The block state view of the state associated to this cell.
        /// </summary>
        new ILayoutBlockStateView BlockStateView { get; }
    }

    /// <summary>
    /// A leaf of the cell view tree for a child state.
    /// </summary>
    internal class LayoutBlockCellView : FocusBlockCellView, ILayoutBlockCellView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBlockCellView"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view.</param>
        /// <param name="blockStateView">The block state view of the state associated to this cell.</param>
        public LayoutBlockCellView(ILayoutNodeStateView stateView, ILayoutCellViewCollection parentCellView, ILayoutBlockStateView blockStateView)
            : base(stateView, parentCellView, blockStateView)
        {
            CellOrigin = RegionHelper.InvalidOrigin;
            CellSize = RegionHelper.InvalidSize;
            ActualCellSize = RegionHelper.InvalidSize;
            CellPadding = Padding.Empty;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The collection of cell views containing this view.
        /// </summary>
        public new ILayoutCellViewCollection ParentCellView { get { return (ILayoutCellViewCollection)base.ParentCellView; } }

        /// <summary>
        /// The block state view of the state associated to this cell.
        /// </summary>
        public new ILayoutBlockStateView BlockStateView { get { return (ILayoutBlockStateView)base.BlockStateView; } }

        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        public new ILayoutNodeStateView StateView { get { return (ILayoutNodeStateView)base.StateView; } }

        /// <summary>
        /// Location of the cell.
        /// </summary>
        public Point CellOrigin { get; private set; }

        /// <summary>
        /// Floating size of the cell.
        /// </summary>
        public Size CellSize { get; private set; }

        /// <summary>
        /// Actual size of the cell.
        /// </summary>
        public Size ActualCellSize { get; private set; }

        /// <summary>
        /// Rectangular region for the cell.
        /// </summary>
        public Rect CellRect { get { return new Rect(CellOrigin, ActualCellSize); } }

        /// <summary>
        /// Padding inside the cell.
        /// </summary>
        public Padding CellPadding { get; private set; }

        /// <summary>
        /// The collection that can add separators around this item.
        /// </summary>
        public ILayoutCellViewCollection CollectionWithSeparator { get; private set; }

        /// <summary>
        /// The reference when displaying separators.
        /// </summary>
        public ILayoutCellView ReferenceContainer { get; private set; }

        /// <summary>
        /// The length of the separator.
        /// </summary>
        public double SeparatorLength { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Measures the cell.
        /// </summary>
        /// <param name="collectionWithSeparator">A collection that can draw separators around the cell.</param>
        /// <param name="referenceContainer">The cell view in <paramref name="collectionWithSeparator"/> that contains this cell.</param>
        /// <param name="separatorLength">The length of the separator in <paramref name="collectionWithSeparator"/>.</param>
        public virtual void Measure(ILayoutCellViewCollection collectionWithSeparator, ILayoutCellView referenceContainer, double separatorLength)
        {
            CollectionWithSeparator = collectionWithSeparator;
            ReferenceContainer = referenceContainer;
            SeparatorLength = separatorLength;

            Debug.Assert(BlockStateView != null);
            BlockStateView.MeasureCells(collectionWithSeparator, referenceContainer, separatorLength);

            CellSize = BlockStateView.CellSize;
            ActualCellSize = RegionHelper.InvalidSize;

            Debug.Assert(RegionHelper.IsValid(CellSize));
        }

        /// <summary>
        /// Arranges the cell.
        /// </summary>
        /// <param name="origin">The cell location.</param>
        public virtual void Arrange(Point origin)
        {
            CellOrigin = origin;

            Point OriginWithPadding = origin.Moved(CellPadding.Left, 0);

            Debug.Assert(BlockStateView != null);
            BlockStateView.ArrangeCells(OriginWithPadding);

            Debug.Assert(RegionHelper.IsValid(CellOrigin));
        }

        /// <summary>
        /// Updates the actual size of the cell.
        /// </summary>
        public virtual void UpdateActualSize()
        {
            Debug.Assert(BlockStateView != null);
            BlockStateView.UpdateActualCellsSize();

            Debug.Assert(RegionHelper.IsValid(BlockStateView.ActualCellSize));
            ActualCellSize = BlockStateView.ActualCellSize;

            Debug.Assert(Size.IsEqual(CellRect.Size, ActualCellSize));
        }

        /// <summary>
        /// Draws the cell.
        /// </summary>
        public virtual void Draw()
        {
            Debug.Assert(RegionHelper.IsValid(ActualCellSize));

            Debug.Assert(BlockStateView != null);
            BlockStateView.DrawCells();
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ILayoutCellView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutBlockCellView AsBlockCellView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBlockCellView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
