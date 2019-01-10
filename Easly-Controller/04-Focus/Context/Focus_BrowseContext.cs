﻿using EaslyController.ReadOnly;
using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Context for browsing child nodes of a parent node.
    /// </summary>
    public interface IFocusBrowseContext : IFrameBrowseContext
    {
        /// <summary>
        /// State this context is browsing.
        /// </summary>
        new IFocusNodeState State { get; }

        /// <summary>
        /// List of index collections that have been added during browsing.
        /// </summary>
        new IFocusIndexCollectionReadOnlyList IndexCollectionList { get; }
    }

    /// <summary>
    /// Context for browsing child nodes of a parent node.
    /// </summary>
    public class FocusBrowseContext : FrameBrowseContext, IFocusBrowseContext
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FocusBrowseContext"/>.
        /// </summary>
        /// <param name="state">The state that will be browsed.</param>
        public FocusBrowseContext(IFocusNodeState state)
            : base(state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// State this context is browsing.
        /// </summary>
        public new IFocusNodeState State { get { return (IFocusNodeState)base.State; } }

        /// <summary>
        /// List of index collections that have been added during browsing.
        /// </summary>
        public new IFocusIndexCollectionReadOnlyList IndexCollectionList { get { return (IFocusIndexCollectionReadOnlyList)base.IndexCollectionList; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCollectionList object.
        /// </summary>
        protected override IReadOnlyIndexCollectionList CreateIndexCollectionList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBrowseContext));
            return new FocusIndexCollectionList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollectionReadOnlyList object.
        /// </summary>
        protected override IReadOnlyIndexCollectionReadOnlyList CreateIndexCollectionListReadOnly(IReadOnlyIndexCollectionList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBrowseContext));
            return new FocusIndexCollectionReadOnlyList((IFocusIndexCollectionList)list);
        }
        #endregion
    }
}
