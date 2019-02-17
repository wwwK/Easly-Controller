﻿namespace EaslyController.Controller
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Implements a structure that is used to describe a rectangular region.
    /// </summary>
    public struct Rect : IFormattable
    {
        #region Init
        static Rect()
        {
            Empty = new Rect(0, 0, 0, 0);
        }

        /// <summary>
        /// The empty (0,0,0,0) region.
        /// </summary>
        public static Rect Empty { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rect"/> struct.
        /// </summary>
        /// <param name="x">The horizontal coordinate of the top left corner.</param>
        /// <param name="y">The vertical coordinate of the top left corner.</param>
        /// <param name="width">The region width.</param>
        /// <param name="height">The region height.</param>
        public Rect(double x, double y, double width, double height)
        {
            Debug.Assert(x >= 0 && y >= 0 && width >= 0 && height >= 0);

            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rect"/> struct.
        /// </summary>
        /// <param name="origin">The location of the top left corner.</param>
        /// <param name="size">The region size.</param>
        public Rect(Point origin, Size size)
        {
            Debug.Assert(origin.X >= 0 && origin.Y >= 0 && size.Width >= 0 && size.Height >= 0);

            X = origin.X;
            Y = origin.Y;
            Width = size.Width;
            Height = size.Height;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Horizontal coordinate of the top left corner.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Vertical coordinate of the top left corner.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Origin of the top left corner.
        /// </summary>
        public Point Origin { get { return new Point(X, Y); } }

        /// <summary>
        /// The region Width.
        /// </summary>
        public double Width { get; }

        /// <summary>
        /// The region Height.
        /// </summary>
        public double Height { get; }

        /// <summary>
        /// The region size
        /// </summary>
        public Size Size { get { return new Size(Width, Height); } }

        /// <summary>
        /// True if the object is the empty region.
        /// </summary>
        public bool IsEmpty { get { return X == 0 && Y == 0 && Width == 0 && Height == 0; } }

        /// <summary>
        /// True if the object represents a visible region.
        /// </summary>
        public bool IsVisible { get { return Width > 0 && Height > 0; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Compares two regions.
        /// </summary>
        /// <param name="rect1">The first region.</param>
        /// <param name="rect2">The second region.</param>
        public static bool IsEqual(Rect rect1, Rect rect2)
        {
            double DiffX = Math.Abs(rect2.X - rect1.X);
            double DiffY = Math.Abs(rect2.Y - rect1.Y);
            double DiffCX = Math.Abs(rect2.Width - rect1.Width);
            double DiffCY = Math.Abs(rect2.Height - rect1.Height);

            return DiffX <= RegionHelper.Tolerance && DiffY <= RegionHelper.Tolerance && DiffCX <= RegionHelper.Tolerance && DiffCY <= RegionHelper.Tolerance;
        }

        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// Returns a formatted string representation of this instance.
        /// </summary>
        /// <param name="provider">A format provider.</param>
        public string ToString(IFormatProvider provider)
        {
            return ToString(null, provider);
        }

        /// <summary>
        /// Returns a formatted string representation of this instance.
        /// </summary>
        /// <param name="format">A format.</param>
        /// <param name="formatProvider">A format provider.</param>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return $"{X},{Y},{Width},{Height}";
        }
        #endregion
    }
}