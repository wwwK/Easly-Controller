﻿using EaslyController.Writeable;
using System;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// View of a block state.
    /// </summary>
    public interface IFrameBlockStateView : IWriteableBlockStateView
    {
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        new IFrameControllerView ControllerView { get; }

        /// <summary>
        /// The block state.
        /// </summary>
        new IFrameBlockState BlockState { get; }

        /// <summary>
        /// The template used to display the block state.
        /// </summary>
        IFrameTemplate Template { get; }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        IFrameCellView RootCellView { get; }

        /// <summary>
        /// List of cell views for each child node.
        /// </summary>
        IFrameCellViewCollection EmbeddingCellView { get; }

        /// <summary>
        /// True if the block view contain at least one visible cell view.
        /// </summary>
        bool HasVisibleCellView { get; }

        /// <summary>
        /// Builds the cell view tree for this view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        void BuildRootCellView(IFrameCellViewTreeContext context);

        /// <summary>
        /// Assign the cell view for each child node.
        /// </summary>
        /// <param name="embeddingCellView">The assigned cell view list.</param>
        void AssignEmbeddingCellView(IFrameCellViewCollection embeddingCellView);

        /// <summary>
        /// Clears the cell view tree for this view.
        /// </summary>
        /// <param name="stateView">The state view for which to delete cells.</param>
        void ClearRootCellView(IFrameNodeStateView stateView);

        /// <summary>
        /// Update line numbers in the root cell view.
        /// </summary>
        /// <param name="lineNumber">The current line number, updated upon return.</param>
        /// <param name="maxLineNumber">The maximum line number observed, updated upon return.</param>
        /// <param name="columnNumber">The current column number, updated upon return.</param>
        /// <param name="maxColumnNumber">The maximum column number observed, updated upon return.</param>
        void UpdateLineNumbers(ref int lineNumber, ref int maxLineNumber, ref int columnNumber, ref int maxColumnNumber);

        /// <summary>
        /// Enumerate all visible cell views.
        /// </summary>
        /// <param name="list">The list of visible cell views upon return.</param>
        void EnumerateVisibleCellViews(IFrameVisibleCellViewList list);

        /// <summary>
        /// Checks if the tree of cell views under this state is valid.
        /// </summary>
        bool IsCellViewTreeValid();
    }

    /// <summary>
    /// View of a block state.
    /// </summary>
    public class FrameBlockStateView : WriteableBlockStateView, IFrameBlockStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBlockStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="blockState">The block state.</param>
        public FrameBlockStateView(IFrameControllerView controllerView, IFrameBlockState blockState)
            : base(controllerView, blockState)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public new IFrameControllerView ControllerView { get { return (IFrameControllerView)base.ControllerView; } }

        /// <summary>
        /// The block state.
        /// </summary>
        public new IFrameBlockState BlockState { get { return (IFrameBlockState)base.BlockState; } }

        /// <summary>
        /// The template used to display the block state.
        /// </summary>
        public IFrameTemplate Template
        {
            get
            {
                Type BlockType = BlockState.ParentInner.BlockType;
                return ControllerView.TemplateSet.BlockTypeToTemplate(BlockType);
            }
        }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        public IFrameCellView RootCellView { get; private set; }

        /// <summary>
        /// List of cell views for each child node.
        /// </summary>
        public IFrameCellViewCollection EmbeddingCellView { get; private set; }

        /// <summary>
        /// True if the block view contain at least one visible cell view.
        /// </summary>
        public virtual bool HasVisibleCellView
        {
            get
            {
                Debug.Assert(RootCellView != null);
                return RootCellView.HasVisibleCellView;
            }
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Builds the cell view tree for this view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        public virtual void BuildRootCellView(IFrameCellViewTreeContext context)
        {
            Debug.Assert(context.BlockStateView == this);

            IFrameBlockTemplate BlockTemplate = Template as IFrameBlockTemplate;
            Debug.Assert(BlockTemplate != null);

            SetRootCellView(BlockTemplate.BuildBlockCells(context));

            Debug.Assert(EmbeddingCellView != null);
        }

        /// <summary></summary>
        protected virtual void SetRootCellView(IFrameCellView cellView)
        {
            Debug.Assert(cellView != null);
            Debug.Assert(RootCellView == null);

            RootCellView = cellView;
        }

        /// <summary>
        /// Assign the cell view for each child node.
        /// </summary>
        /// <param name="embeddingCellView">The assigned cell view list.</param>
        public virtual void AssignEmbeddingCellView(IFrameCellViewCollection embeddingCellView)
        {
            Debug.Assert(embeddingCellView != null);
            Debug.Assert(EmbeddingCellView == null);

            EmbeddingCellView = embeddingCellView;
        }

        /// <summary>
        /// Clears the cell view tree for this view.
        /// </summary>
        /// <param name="stateView">The state view for which to delete cells.</param>
        public virtual void ClearRootCellView(IFrameNodeStateView stateView)
        {
            if (RootCellView != null)
                RootCellView.ClearCellTree();

            RootCellView = null;
            EmbeddingCellView = null;
        }

        /// <summary>
        /// Update line numbers in the root cell view.
        /// </summary>
        /// <param name="lineNumber">The current line number, updated upon return.</param>
        /// <param name="maxLineNumber">The maximum line number observed, updated upon return.</param>
        /// <param name="columnNumber">The current column number, updated upon return.</param>
        /// <param name="maxColumnNumber">The maximum column number observed, updated upon return.</param>
        public virtual void UpdateLineNumbers(ref int lineNumber, ref int maxLineNumber, ref int columnNumber, ref int maxColumnNumber)
        {
            Debug.Assert(RootCellView != null);

            RootCellView.UpdateLineNumbers(ref lineNumber, ref maxLineNumber, ref columnNumber, ref maxColumnNumber);
        }

        /// <summary>
        /// Enumerate all visible cell views.
        /// </summary>
        /// <param name="list">The list of visible cell views upon return.</param>
        public virtual void EnumerateVisibleCellViews(IFrameVisibleCellViewList list)
        {
            Debug.Assert(list != null);

            Debug.Assert(RootCellView != null);
            RootCellView.EnumerateVisibleCellViews(list);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameBlockStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameBlockStateView AsBlockStateView))
                return false;

            if (!base.IsEqual(comparer, AsBlockStateView))
                return false;

            if (Template != AsBlockStateView.Template)
                return false;

            if (!IsRootCellViewEqual(comparer, AsBlockStateView))
                return false;

            if (!IsEmbeddingCellViewEqual(comparer, AsBlockStateView))
                return false;

            return true;
        }

        /// <summary></summary>
        protected virtual bool IsRootCellViewEqual(CompareEqual comparer, IFrameBlockStateView other)
        {
            if (RootCellView != null)
            {
                if (other.RootCellView == null)
                    return false;
                else
                {
                    if (!comparer.VerifyEqual(RootCellView, other.RootCellView))
                        return false;
                }
            }
            else if (other.RootCellView != null)
                return false;

            return true;
        }

        /// <summary></summary>
        protected virtual bool IsEmbeddingCellViewEqual(CompareEqual comparer, IFrameBlockStateView other)
        {
            if (EmbeddingCellView != null)
            {
                if (other.EmbeddingCellView == null)
                    return false;
                else
                {
                    if (!comparer.VerifyEqual(EmbeddingCellView, other.EmbeddingCellView))
                        return false;
                }
            }
            else if (other.EmbeddingCellView != null)
                return false;

            return true;
        }

        /// <summary>
        /// Checks if the tree of cell views under this state is valid.
        /// </summary>
        public virtual bool IsCellViewTreeValid()
        {
            if (RootCellView == null)
                return false;

            IFrameAssignableCellViewDictionary<string> EmptyCellViewTable = CreateCellViewTable();
            IFrameAssignableCellViewReadOnlyDictionary<string> ExpectedCellViewTable = CreateCellViewReadOnlyTable(EmptyCellViewTable);
            IFrameAssignableCellViewDictionary<string> ActualCellViewTable = CreateCellViewTable();
            if (!RootCellView.IsCellViewTreeValid(ExpectedCellViewTable, ActualCellViewTable))
                return false;

            if (ActualCellViewTable.Count != 0)
                return false;

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxAssignableCellViewDictionary{string} object.
        /// </summary>
        protected virtual IFrameAssignableCellViewDictionary<string> CreateCellViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBlockStateView));
            return new FrameAssignableCellViewDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxAssignableCellViewReadOnlyDictionary{string} object.
        /// </summary>
        protected virtual IFrameAssignableCellViewReadOnlyDictionary<string> CreateCellViewReadOnlyTable(IFrameAssignableCellViewDictionary<string> dictionary)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBlockStateView));
            return new FrameAssignableCellViewReadOnlyDictionary<string>(dictionary);
        }
        #endregion
    }
}
