﻿using BaseNodeHelper;
using EaslyController.Frame;
using System;
using System.Diagnostics;
using System.Windows.Markup;

namespace EaslyController.Focus
{
    /// <summary>
    /// Focus for bringing the focus to an insertion point.
    /// </summary>
    public interface IFocusInsertFrame : IFrameInsertFrame, IFocusStaticFrame
    {
        /// <summary>
        /// Type to use when creating a new item in the associated block list or list. Only when it's an abstract type.
        /// (Set in Xaml)
        /// </summary>
        Type ItemType { get; set; }

        /// <summary>
        /// Type to use when inserting a new item in the collection.
        /// </summary>
        Type InsertType { get; }

        /// <summary>
        /// Returns the inner for the collection associated to this frame, for a given state.
        /// </summary>
        /// <param name="state">The state, modified if <see cref="CollectionName"/> points to a different state.</param>
        /// <param name="inner">The inner associated to the collection in <paramref name="state"/>.</param>
        void CollectionNameToInner(ref IFocusNodeState state, ref IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex> inner);
    }

    /// <summary>
    /// Focus for bringing the focus to an insertion point.
    /// </summary>
    [ContentProperty("CollectionName")]
    public class FocusInsertFrame : FrameInsertFrame, IFocusInsertFrame
    {
        #region Properties
        /// <summary>
        /// Parent template.
        /// </summary>
        public new IFocusTemplate ParentTemplate { get { return (IFocusTemplate)base.ParentTemplate; } }

        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        public new IFocusFrame ParentFrame { get { return (IFocusFrame)base.ParentFrame; } }

        /// <summary>
        /// Node frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public IFocusNodeFrameVisibility Visibility { get; set; }

        /// <summary>
        /// Indicates that this is the preferred frame when restoring the focus.
        /// (Set in Xaml)
        /// </summary>
        public bool IsPreferred { get; set; }

        /// <summary>
        /// Type to use when creating a new item in the associated block list or list. Only when it's an abstract type.
        /// (Set in Xaml)
        /// </summary>
        public Type ItemType { get; set; }

        /// <summary>
        /// Type to use when inserting a new item in the collection.
        /// </summary>
        public Type InsertType { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        /// <param name="nodeTemplateTable">Table of templates with all frames.</param>
        public override bool IsValid(Type nodeType, IFrameTemplateReadOnlyDictionary nodeTemplateTable)
        {
            if (!base.IsValid(nodeType, nodeTemplateTable))
                return false;

            if (Visibility != null && !Visibility.IsValid(nodeType))
                return false;

            Debug.Assert(InsertType != null);

            if (InsertType.IsInterface)
                return false;

            if (InsertType.IsAbstract)
                return false;

            return true;
        }

        /// <summary></summary>
        protected override void UpdateInterfaceType(Type nodeType)
        {
            base.UpdateInterfaceType(nodeType);

            Debug.Assert(InterfaceType != null);
            Type EstimatedItemType = NodeTreeHelper.InterfaceTypeToNodeType(InterfaceType);
            Debug.Assert(EstimatedItemType != null);

            if (ItemType == null)
                InsertType = EstimatedItemType;
            else
                InsertType = ItemType;
        }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="parentCellView">The parent cell view.</param>
        public override IFrameCellView BuildNodeCells(IFrameCellViewTreeContext context, IFrameCellViewCollection parentCellView)
        {
            ((IFocusCellViewTreeContext)context).UpdateNodeFrameVisibility(this, out bool OldFrameVisibility);

            IFocusCellView Result;
            if (((IFocusCellViewTreeContext)context).IsVisible)
                Result = base.BuildNodeCells(context, parentCellView) as IFocusCellView;
            else
                Result = CreateEmptyCellView(((IFocusCellViewTreeContext)context).StateView);

            ((IFocusCellViewTreeContext)context).RestoreFrameVisibility(OldFrameVisibility);

            return Result;
        }

        /// <summary>
        /// Returns the frame associated to a property if can have selectors.
        /// </summary>
        /// <param name="propertyName">Name of the property to look for.</param>
        /// <param name="frame">Frame found upon return. Null if not matching <paramref name="propertyName"/>.</param>
        public virtual bool FrameSelectorForProperty(string propertyName, out IFocusNodeFrameWithSelector frame)
        {
            frame = null;
            return false;
        }

        /// <summary>
        /// Gets preferred frames to receive the focus when the source code is changed.
        /// </summary>
        /// <param name="firstPreferredFrame">The first preferred frame found.</param>
        /// <param name="lastPreferredFrame">The first preferred frame found.</param>
        public virtual void GetPreferredFrame(ref IFocusNodeFrame firstPreferredFrame, ref IFocusNodeFrame lastPreferredFrame)
        {
            if (Visibility == null || Visibility.IsVolatile || IsPreferred)
            {
                if (firstPreferredFrame == null || IsPreferred)
                    firstPreferredFrame = this;

                lastPreferredFrame = this;
            }
        }

        /// <summary>
        /// Returns the inner for the collection associated to this frame, for a given state.
        /// </summary>
        /// <param name="state">The state, modified if <see cref="CollectionName"/> points to a different state.</param>
        /// <param name="inner">The inner associated to the collection in <paramref name="state"/>.</param>
        public virtual void CollectionNameToInner(ref IFocusNodeState state, ref IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex> inner)
        {
            Debug.Assert(inner == null);

            string[] Split = CollectionName.Split('.');

            for (int i = 0; i < Split.Length; i++)
            {
                string PropertyName = Split[i];

                if (i + 1 < Split.Length)
                {
                    Debug.Assert(state.InnerTable.ContainsKey(PropertyName));

                    switch (state.InnerTable[PropertyName])
                    {
                        case IFocusPlaceholderInner<IFocusBrowsingPlaceholderNodeIndex> AsPlaceholderInner:
                            state = AsPlaceholderInner.ChildState;
                            break;

                        case IFocusOptionalInner<IFocusBrowsingOptionalNodeIndex> AsOptionalInner:
                            Debug.Assert(AsOptionalInner.IsAssigned);
                            state = AsOptionalInner.ChildState;
                            break;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(PropertyName));
                    }
                }
                else
                {
                    Debug.Assert(state.InnerTable.ContainsKey(PropertyName));
                    inner = state.InnerTable[PropertyName] as IFocusCollectionInner<IFocusBrowsingCollectionNodeIndex>;
                }
            }

            Debug.Assert(inner != null);
            Debug.Assert(state.InnerTable.ContainsKey(inner.PropertyName));
            Debug.Assert(state.InnerTable[inner.PropertyName] == inner);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxFocusableCellView object.
        /// </summary>
        protected override IFrameVisibleCellView CreateFrameCellView(IFrameNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusInsertFrame));
            return new FocusFocusableCellView((IFocusNodeStateView)stateView, this);
        }

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        protected virtual IFocusEmptyCellView CreateEmptyCellView(IFocusNodeStateView stateView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusInsertFrame));
            return new FocusEmptyCellView(stateView);
        }
        #endregion
    }
}
