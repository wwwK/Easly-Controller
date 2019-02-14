﻿namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Controller;
    using EaslyController.Focus;

    /// <summary>
    /// A collection of cell views organized in a column.
    /// </summary>
    public interface ILayoutLine : IFocusLine, ILayoutCellViewCollection
    {
    }

    /// <summary>
    /// A collection of cell views organized in a column.
    /// </summary>
    internal class LayoutLine : FocusLine, ILayoutLine
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutLine"/> class.
        /// </summary>
        /// <param name="stateView">The state view containing the tree with this cell.</param>
        /// <param name="parentCellView">The collection of cell views containing this view. Null for the root of the cell tree.</param>
        /// <param name="cellViewList">The list of child cell views.</param>
        /// <param name="frame">Frame providing the horizontal separator to insert between cells. Can be null.</param>
        public LayoutLine(ILayoutNodeStateView stateView, ILayoutCellViewCollection parentCellView, ILayoutCellViewList cellViewList, ILayoutFrame frame)
            : base(stateView, parentCellView, cellViewList, frame)
        {
            CellOrigin = ArrangeHelper.InvalidOrigin;
            CellSize = MeasureHelper.InvalidSize;
            CellPadding = Padding.Empty;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The state view containing the tree with this cell.
        /// </summary>
        public new ILayoutNodeStateView StateView { get { return (ILayoutNodeStateView)base.StateView; } }

        /// <summary>
        /// The collection of cell views containing this view. Null for the root of the cell tree.
        /// </summary>
        public new ILayoutCellViewCollection ParentCellView { get { return (ILayoutCellViewCollection)base.ParentCellView; } }

        /// <summary>
        /// The collection of child cells.
        /// </summary>
        public new ILayoutCellViewList CellViewList { get { return (ILayoutCellViewList)base.CellViewList; } }

        /// <summary>
        /// The frame that was used to create this cell. Can be null.
        /// </summary>
        public new ILayoutFrame Frame { get { return (ILayoutFrame)base.Frame; } }

        /// <summary>
        /// Location of the cell.
        /// </summary>
        public Point CellOrigin { get; private set; }

        /// <summary>
        /// Size of the cell.
        /// </summary>
        public Size CellSize { get; private set; }

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

            Debug.Assert(StateView != null);
            Debug.Assert(StateView.ControllerView != null);

            ILayoutDrawContext DrawContext = StateView.ControllerView.DrawContext;
            Debug.Assert(DrawContext != null);

            GetSeparatorOverrides(ref collectionWithSeparator, ref referenceContainer, ref separatorLength, out bool OverrideCollectionWithSeparator, out bool OverrideReferenceContainer);

            double Width = 0;
            double Height = double.NaN;

            for (int i = 0; i < CellViewList.Count; i++)
            {
                ILayoutCellView CellView = CellViewList[i];

                ApplySeparatorOverrides(i, CellView, OverrideCollectionWithSeparator, ref OverrideReferenceContainer, ref collectionWithSeparator, ref referenceContainer, ref separatorLength, ref Width);

                Debug.Assert(collectionWithSeparator != this || CellViewList.IndexOf(referenceContainer) == i);
                CellView.Measure(collectionWithSeparator, referenceContainer, separatorLength);

                Size NestedCellSize = CellView.CellSize;
                Debug.Assert(MeasureHelper.IsValid(NestedCellSize));

                bool IsFixed = MeasureHelper.IsFixed(NestedCellSize);
                bool IsStretched = MeasureHelper.IsStretchedVertically(NestedCellSize);
                Debug.Assert(IsFixed || IsStretched);

                Debug.Assert(!double.IsNaN(NestedCellSize.Width));
                Width += NestedCellSize.Width;

                if (IsFixed)
                {
                    Debug.Assert(!double.IsNaN(NestedCellSize.Height));
                    if (double.IsNaN(Height) || Height < NestedCellSize.Height)
                        Height = NestedCellSize.Height;
                }
            }

            if (Width == 0)
                CellSize = Size.Empty;
            else
                CellSize = new Size(Width, Height);

            Debug.Assert(MeasureHelper.IsValid(CellSize));
        }

        /// <summary>
        /// Arranges the cell.
        /// </summary>
        /// <param name="collectionWithSeparator">A collection that can draw separators around the cell.</param>
        /// <param name="referenceContainer">The cell view in <paramref name="collectionWithSeparator"/> that contains this cell.</param>
        /// <param name="separatorLength">The length of the separator in <paramref name="collectionWithSeparator"/>.</param>
        /// <param name="origin">The cell location.</param>
        public virtual void Arrange(ILayoutCellViewCollection collectionWithSeparator, ILayoutCellView referenceContainer, double separatorLength, Point origin)
        {
            CellOrigin = origin;

            Debug.Assert(StateView != null);
            Debug.Assert(StateView.ControllerView != null);

            ILayoutDrawContext DrawContext = StateView.ControllerView.DrawContext;
            Debug.Assert(DrawContext != null);

            double OriginX = origin.X;
            double OriginY = origin.Y;

            //GetSeparatorOverrides(ref collectionWithSeparator, ref referenceContainer, ref separatorLength, out bool OverrideCollectionWithSeparator, out bool OverrideReferenceContainer);

            for (int i = 0; i < CellViewList.Count; i++)
            {
                ILayoutCellView CellView = CellViewList[i];

                //ApplySeparatorOverrides(OverrideCollectionWithSeparator, OverrideReferenceContainer, i, CellView, ref collectionWithSeparator, ref referenceContainer, ref separatorLength, ref OriginX);

                if (i > 0)
                    OriginX += CellView.SeparatorLength;

                Point ColumnOrigin = new Point(OriginX, OriginY);
                CellView.Arrange(collectionWithSeparator, referenceContainer, separatorLength, ColumnOrigin);

                Size NestedCellSize = CellView.CellSize;
                Debug.Assert(!double.IsNaN(NestedCellSize.Width));
                OriginX += NestedCellSize.Width;
            }

            Point FinalOrigin = new Point(OriginX, OriginY);
            Point ExpectedOrigin = new Point(CellOrigin.X + CellSize.Width, CellOrigin.Y);
            bool IsEqual = Point.IsEqual(FinalOrigin, ExpectedOrigin);
            Debug.Assert(IsEqual);
        }

        /// <summary></summary>
        protected virtual void GetSeparatorOverrides(ref ILayoutCellViewCollection collectionWithSeparator, ref ILayoutCellView referenceContainer, ref double separatorLength, out bool overrideCollectionWithSeparator, out bool overrideReferenceContainer)
        {
            ILayoutFrameWithHorizontalSeparator AsFrameWithHorizontalSeparator = Frame as ILayoutFrameWithHorizontalSeparator;
            Debug.Assert(AsFrameWithHorizontalSeparator != null);

            //overrideCollectionWithSeparator = (collectionWithSeparator == null && referenceContainer == null) || !(Frame is ILayoutPanelFrame) || AsFrameWithHorizontalSeparator.Separator != Constants.HorizontalSeparators.None;
            overrideCollectionWithSeparator = true;
            overrideReferenceContainer = false;

            // Ensures arguments of Arrange() are valid.
            if (collectionWithSeparator == null && referenceContainer == null)
            {
                collectionWithSeparator = this;
                overrideReferenceContainer = true;
                separatorLength = 0;
            }
        }

        /// <summary></summary>
        protected virtual void ApplySeparatorOverrides(int cellViewIndex, ILayoutCellView cellView, bool overrideCollectionWithSeparator, ref bool overrideReferenceContainer, ref ILayoutCellViewCollection collectionWithSeparator, ref ILayoutCellView referenceContainer, ref double separatorLength, ref double length)
        {
            if (cellViewIndex > 0)
            {
                if (cellViewIndex == 1 && overrideCollectionWithSeparator)
                {
                    collectionWithSeparator = this;
                    overrideReferenceContainer = true;

                    ILayoutDrawContext DrawContext = StateView.ControllerView.DrawContext;
                    Debug.Assert(DrawContext != null);

                    ILayoutFrameWithHorizontalSeparator AsFrameWithHorizontalSeparator = Frame as ILayoutFrameWithHorizontalSeparator;
                    Debug.Assert(AsFrameWithHorizontalSeparator != null);

                    separatorLength = DrawContext.GetHorizontalSeparatorWidth(AsFrameWithHorizontalSeparator.Separator);
                }

                length += separatorLength;
            }

            if (overrideReferenceContainer)
                referenceContainer = cellView;
        }

        /// <summary>
        /// Draws container or separator before an element of a collection.
        /// </summary>
        /// <param name="drawContext">The context used to draw the cell.</param>
        /// <param name="cellView">The cell to draw.</param>
        /// <param name="origin">The location where to start drawing.</param>
        /// <param name="size">The drawing size, padding included.</param>
        /// <param name="padding">The padding to use when drawing.</param>
        public virtual void DrawBeforeItem(ILayoutDrawContext drawContext, ILayoutCellView cellView, Point origin, Size size, Padding padding)
        {
            int Index = CellViewList.IndexOf(cellView);
            Debug.Assert(Index >= 0);

            ILayoutFrameWithHorizontalSeparator AsFrameWithHorizontalSeparator = Frame as ILayoutFrameWithHorizontalSeparator;
            Debug.Assert(AsFrameWithHorizontalSeparator != null);

            if (Index > 0)
                drawContext.DrawHorizontalSeparator(AsFrameWithHorizontalSeparator.Separator, cellView.CellOrigin, cellView.CellSize.Height);
        }

        /// <summary>
        /// Draws container or separator after an element of a collection.
        /// </summary>
        /// <param name="drawContext">The context used to draw the cell.</param>
        /// <param name="cellView">The cell to draw.</param>
        /// <param name="origin">The location where to start drawing.</param>
        /// <param name="size">The drawing size, padding included.</param>
        /// <param name="padding">The padding to use when drawing.</param>
        public virtual void DrawAfterItem(ILayoutDrawContext drawContext, ILayoutCellView cellView, Point origin, Size size, Padding padding)
        {
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

            if (!comparer.IsSameType(other, out LayoutLine AsLine))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsLine))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
