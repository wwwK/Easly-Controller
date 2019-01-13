﻿using EaslyController.Frame;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// View of a pattern state.
    /// </summary>
    public interface IFocusPatternStateView : IFramePatternStateView, IFocusPlaceholderNodeStateView
    {
        /// <summary>
        /// The pattern state.
        /// </summary>
        new IFocusPatternState State { get; }
    }

    /// <summary>
    /// View of a pattern state.
    /// </summary>
    public class FocusPatternStateView : FramePatternStateView, IFocusPatternStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusPatternStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The pattern state.</param>
        public FocusPatternStateView(IFocusControllerView controllerView, IFocusPatternState state)
            : base(controllerView, state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public new IFocusControllerView ControllerView { get { return (IFocusControllerView)base.ControllerView; } }

        /// <summary>
        /// The pattern state.
        /// </summary>
        public new IFocusPatternState State { get { return (IFocusPatternState)base.State; } }
        IFocusNodeState IFocusNodeStateView.State { get { return State; } }
        IFocusPlaceholderNodeState IFocusPlaceholderNodeStateView.State { get { return State; } }

        /// <summary>
        /// The template used to display the state.
        /// </summary>
        public new IFocusTemplate Template { get { return (IFocusTemplate)base.Template; } }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        public new IFocusCellView RootCellView { get { return (IFocusCellView)base.RootCellView; } }

        /// <summary>
        /// Table of cell views that are mutable lists of cells.
        /// </summary>
        public new IFocusAssignableCellViewReadOnlyDictionary<string> CellViewTable { get { return (IFocusAssignableCellViewReadOnlyDictionary<string>)base.CellViewTable; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Builds the cell view tree for this view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        public override void BuildRootCellView(IFrameCellViewTreeContext context)
        {
            if (((IFocusCellViewTreeContext)context).IsVisible)
                base.BuildRootCellView(context);
            else
                SetRootCellView(CreateEmptyCellView(((IFocusCellViewTreeContext)context).StateView));
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusPatternStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFocusPatternStateView AsPatternStateView))
                return false;

            if (!base.IsEqual(comparer, AsPatternStateView))
                return false;

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxAssignableCellViewDictionary{string} object.
        /// </summary>
        protected override IFrameAssignableCellViewDictionary<string> CreateCellViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPatternStateView));
            return new FocusAssignableCellViewDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxAssignableCellViewReadOnlyDictionary{string} object.
        /// </summary>
        protected override IFrameAssignableCellViewReadOnlyDictionary<string> CreateCellViewReadOnlyTable(IFrameAssignableCellViewDictionary<string> dictionary)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPatternStateView));
            return new FocusAssignableCellViewReadOnlyDictionary<string>((IFocusAssignableCellViewDictionary<string>)dictionary);
        }

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        protected virtual IFocusEmptyCellView CreateEmptyCellView(IFocusNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusPatternStateView));
            return new FocusEmptyCellView(stateView);
        }
        #endregion
    }
}