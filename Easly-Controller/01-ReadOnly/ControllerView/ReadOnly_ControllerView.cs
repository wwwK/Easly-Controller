﻿using System;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public interface IReadOnlyControllerView
    {
        /// <summary>
        /// The controller.
        /// </summary>
        IReadOnlyController Controller { get; }

        /// <summary>
        /// Table of views of each state in the controller.
        /// </summary>
        IReadOnlyStateViewDictionary StateViewTable { get; }
    }

    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public class ReadOnlyControllerView : IReadOnlyControllerView
    {
        #region Init
        /// <summary>
        /// Creates and initializes a new instance of a <see cref="ReadOnlyControllerView"/> object.
        /// </summary>
        /// <param name="controller">The controller on which the view is attached.</param>
        public static IReadOnlyControllerView Create(IReadOnlyController controller)
        {
            ReadOnlyControllerView View = new ReadOnlyControllerView(controller);
            View.Init();
            return View;
        }

        /// <summary>
        /// Initializes a new instance of a <see cref="ReadOnlyControllerView"/> object.
        /// </summary>
        /// <param name="controller">The controller on which the view is attached.</param>
        protected ReadOnlyControllerView(IReadOnlyController controller)
        {
            Controller = controller;

            StateViewTable = CreateStateViewTable();
        }

        /// <summary>
        /// Initializes the view by attaching it to the controller.
        /// </summary>
        protected virtual void Init()
        {
            IReadOnlyAttachCallbackSet CallbackSet = CreateCallbackSet();
            Controller.Attach(this, CallbackSet);

            Debug.Assert(StateViewTable.Count == Controller.Stats.NodeCount);

            Controller.StateCreated += OnNodeStateCreated;
            Controller.StateInitialized += OnNodeStateInitialized;
            Controller.StateRemoved += OnNodeStateRemoved;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller.
        /// </summary>
        public IReadOnlyController Controller { get; }

        /// <summary>
        /// Table of views of each state in the controller.
        /// </summary>
        public IReadOnlyStateViewDictionary StateViewTable { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Handler called every time a state is created in the controller.
        /// </summary>
        /// <param name="state">The state created.</param>
        public virtual void OnNodeStateCreated(IReadOnlyNodeState state)
        {
            Debug.Assert(state != null);
            Debug.Assert(!StateViewTable.ContainsKey(state));

            IReadOnlyNodeStateView StateView;

            switch (state)
            {
                case IReadOnlyPatternState AsPatternState:
                    StateView = CreatePatternStateView(AsPatternState);
                    break;

                case IReadOnlySourceState AsSourceState:
                    StateView = CreateSourceStateView(AsSourceState);
                    break;

                case IReadOnlyPlaceholderNodeState AsPlaceholderState:
                    StateView = CreatePlaceholderNodeStateView(AsPlaceholderState);
                    break;

                case IReadOnlyOptionalNodeState AsOptionalState:
                    StateView = CreateOptionalNodeStateView(AsOptionalState);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(state));
            }

            StateViewTable.Add(state, StateView);
        }

        /// <summary>
        /// Handler called every time a state is initialized in the controller.
        /// </summary>
        /// <param name="state">The state initialized.</param>
        public virtual void OnNodeStateInitialized(IReadOnlyNodeState state)
        {
            Debug.Assert(state != null);
            Debug.Assert(StateViewTable.ContainsKey(state));
        }

        /// <summary>
        /// Handler called every time a state is removed in the controller.
        /// </summary>
        /// <param name="state">The state removed.</param>
        public virtual void OnNodeStateRemoved(IReadOnlyNodeState state)
        {
            Debug.Assert(state != null);
            Debug.Assert(StateViewTable.ContainsKey(state));
        }

        /// <summary>
        /// Handler called every time a block list inner is created in the controller.
        /// </summary>
        /// <param name="state">The block list inner created.</param>
        public virtual void OnBlockListInnerCreated(IReadOnlyBlockListInner inner)
        {
            inner.BlockStateCreated += OnBlockStateCreated;
            inner.BlockStateRemoved += OnBlockStateRemoved;
        }

        /// <summary>
        /// Handler called every time a block state is created in the controller.
        /// </summary>
        /// <param name="state">The block state created.</param>
        public virtual void OnBlockStateCreated(IReadOnlyBlockState blockState)
        {
            Debug.Assert(blockState != null);
        }

        /// <summary>
        /// Handler called every time a block state is removed in the controller.
        /// </summary>
        /// <param name="state">The block state removed.</param>
        public virtual void OnBlockStateRemoved(IReadOnlyBlockState blockState)
        {
            Debug.Assert(blockState != null);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxNodeStateViewDictionary object.
        /// </summary>
        protected virtual IReadOnlyStateViewDictionary CreateStateViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyControllerView));
            return new ReadOnlyStateViewDictionary();
        }

        /// <summary>
        /// Creates a IxxxAttachCallbackSet object.
        /// </summary>
        protected virtual IReadOnlyAttachCallbackSet CreateCallbackSet()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyControllerView));
            return new ReadOnlyAttachCallbackSet()
            {
                NodeStateAttachedHandler = OnNodeStateCreated,
                BlockListInnerAttachedHandler = OnBlockListInnerCreated,
                BlockStateAttachedHandler = OnBlockStateCreated,
            };
        }

        /// <summary>
        /// Creates a IxxxPlaceholderNodeStateView object.
        /// </summary>
        protected virtual IReadOnlyPlaceholderNodeStateView CreatePlaceholderNodeStateView(IReadOnlyPlaceholderNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyControllerView));
            return new ReadOnlyPlaceholderNodeStateView(state);
        }

        /// <summary>
        /// Creates a IxxxOptionalNodeStateView object.
        /// </summary>
        protected virtual IReadOnlyOptionalNodeStateView CreateOptionalNodeStateView(IReadOnlyOptionalNodeState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyControllerView));
            return new ReadOnlyOptionalNodeStateView(state);
        }

        /// <summary>
        /// Creates a IxxxPatternStateView object.
        /// </summary>
        protected virtual IReadOnlyPatternStateView CreatePatternStateView(IReadOnlyPatternState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyControllerView));
            return new ReadOnlyPatternStateView(state);
        }

        /// <summary>
        /// Creates a IxxxSourceStateView object.
        /// </summary>
        protected virtual IReadOnlySourceStateView CreateSourceStateView(IReadOnlySourceState state)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyControllerView));
            return new ReadOnlySourceStateView(state);
        }
        #endregion
    }
}