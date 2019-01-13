﻿using BaseNode;
using EaslyController.Frame;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Index for the root node of the node tree.
    /// </summary>
    public interface IFocusRootNodeIndex : IFrameRootNodeIndex, IFocusNodeIndex
    {
    }

    /// <summary>
    /// Index for the root node of the node tree.
    /// </summary>
    public class FocusRootNodeIndex : FrameRootNodeIndex, IFocusRootNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusRootNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed root node.</param>
        public FocusRootNodeIndex(INode node)
            : base(node)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFocusRootNodeIndex AsRootNodeIndex))
                return false;

            if (!base.IsEqual(comparer, AsRootNodeIndex))
                return false;

            return true;
        }
        #endregion
    }
}