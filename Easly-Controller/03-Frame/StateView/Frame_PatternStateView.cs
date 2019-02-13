﻿namespace EaslyController.Frame
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Writeable;

    /// <summary>
    /// View of a pattern state.
    /// </summary>
    public interface IFramePatternStateView : IWriteablePatternStateView, IFrameNodeStateView
    {
        /// <summary>
        /// The pattern state.
        /// </summary>
        new IFramePatternState State { get; }
    }

    /// <summary>
    /// View of a pattern state.
    /// </summary>
    internal class FramePatternStateView : WriteablePatternStateView, IFramePatternStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FramePatternStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The pattern state.</param>
        public FramePatternStateView(IFrameControllerView controllerView, IFramePatternState state)
            : base(controllerView, state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public new IFrameControllerView ControllerView { get { return (IFrameControllerView)base.ControllerView; } }

        /// <summary>
        /// The pattern state.
        /// </summary>
        public new IFramePatternState State { get { return (IFramePatternState)base.State; } }
        IFrameNodeState IFrameNodeStateView.State { get { return State; } }

        /// <summary>
        /// The template used to display the state.
        /// </summary>
        public IFrameTemplate Template
        {
            get
            {
                Type InterfaceType = typeof(IPattern);
                return ControllerView.TemplateSet.NodeTypeToTemplate(InterfaceType);
            }
        }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        public IFrameCellView RootCellView { get; private set; }

        /// <summary>
        /// Table of cell views that are mutable lists of cells.
        /// </summary>
        public IFrameAssignableCellViewReadOnlyDictionary<string> CellViewTable { get; private set; }
        private IFrameAssignableCellViewDictionary<string> _CellViewTable;

        /// <summary>
        /// True if the node view contain at least one visible cell view.
        /// </summary>
        public virtual bool HasVisibleCellView
        {
            get
            {
                Debug.Assert(RootCellView != null);
                return RootCellView.HasVisibleCellView;
            }
        }

        /// <summary>
        /// The cell view that is embedding this state view. Can be null.
        /// </summary>
        public IFrameCellView ParentContainer { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Builds the cell view tree for this view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        public virtual void BuildRootCellView(IFrameCellViewTreeContext context)
        {
            Debug.Assert(context.StateView == this);
            Debug.Assert(State.InnerTable.Count == 0);
            Debug.Assert(RootCellView == null);

            InitCellViewTable();

            IFrameNodeTemplate NodeTemplate = Template as IFrameNodeTemplate;
            Debug.Assert(NodeTemplate != null);

            SetRootCellView(NodeTemplate.BuildNodeCells(context));

            SealCellViewTable();
        }

        /// <summary></summary>
        private protected virtual void InitCellViewTable()
        {
            _CellViewTable = CreateCellViewTable();
        }

        /// <summary></summary>
        private protected virtual void SetRootCellView(IFrameCellView cellView)
        {
            Debug.Assert(cellView != null);
            Debug.Assert(RootCellView == null);

            RootCellView = cellView;
        }

        /// <summary></summary>
        private protected virtual void SealCellViewTable()
        {
            CellViewTable = _CellViewTable.ToReadOnly();
        }

        /// <summary>
        /// Set the container for this state view.
        /// </summary>
        /// <param name="parentContainer">The cell view where the tree is restarted.</param>
        public virtual void SetContainerCellView(IFrameCellView parentContainer)
        {
            ParentContainer = parentContainer;
        }

        /// <summary>
        /// Clears the cell view tree for this view.
        /// </summary>
        public virtual void ClearRootCellView()
        {
            if (RootCellView != null)
                RootCellView.ClearCellTree();

            RootCellView = null;
            _CellViewTable = null;
            CellViewTable = null;
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
        public void EnumerateVisibleCellViews(IFrameVisibleCellViewList list)
        {
            Debug.Assert(list != null);

            Debug.Assert(RootCellView != null);
            RootCellView.EnumerateVisibleCellViews(list);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFramePatternStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FramePatternStateView AsPatternStateView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsPatternStateView))
                return comparer.Failed();

            if (!comparer.IsSameReference(Template, AsPatternStateView.Template))
                return comparer.Failed();

            if (!comparer.IsTrue((RootCellView == null || AsPatternStateView.RootCellView != null) && (RootCellView != null || AsPatternStateView.RootCellView == null)))
                return comparer.Failed();

            if (RootCellView != null)
            {
                Debug.Assert(CellViewTable != null);
                Debug.Assert(AsPatternStateView.CellViewTable != null);

                if (!comparer.VerifyEqual(RootCellView, AsPatternStateView.RootCellView))
                    return comparer.Failed();

                if (!comparer.VerifyEqual(CellViewTable, AsPatternStateView.CellViewTable))
                    return comparer.Failed();
            }

            return true;
        }

        /// <summary>
        /// Checks if the tree of cell views under this state is valid.
        /// </summary>
        public virtual bool IsCellViewTreeValid()
        {
            bool IsValid = true;

            IsValid &= RootCellView != null;

            if (IsValid && !(RootCellView is IFrameEmptyCellView))
            {
                IFrameAssignableCellViewDictionary<string> ActualCellViewTable = CreateCellViewTable();
                IsValid &= RootCellView.IsCellViewTreeValid(CellViewTable, ActualCellViewTable);

                Debug.Assert(ActualCellViewTable.Count == 0);
            }

            return IsValid;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxAssignableCellViewDictionary{string} object.
        /// </summary>
        private protected virtual IFrameAssignableCellViewDictionary<string> CreateCellViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePatternStateView));
            return new FrameAssignableCellViewDictionary<string>();
        }
        #endregion
    }
}
