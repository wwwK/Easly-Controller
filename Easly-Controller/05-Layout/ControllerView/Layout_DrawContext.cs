﻿namespace EaslyController.Layout
{
    using EaslyController.Constants;
    using EaslyController.Controller;

    /// <summary>
    /// Context for measuring, arranging and drawing cells in a view.
    /// </summary>
    public interface ILayoutDrawContext : ILayoutMeasureContext
    {
        /// <summary>
        /// Draws a string, at the location specified in <paramref name="origin"/>.
        /// </summary>
        /// <param name="text">The text to draw.</param>
        /// <param name="origin">The location where to start drawing.</param>
        /// <param name="textStyle">Style to use for the text.</param>
        /// <param name="isFocused">true if the whole text has the focus.</param>
        void DrawText(string text, Point origin, TextStyles textStyle, bool isFocused);

        /// <summary>
        /// Draws a symbol, at the location specified in <paramref name="origin"/>.
        /// </summary>
        /// <param name="symbol">The symbol to draw.</param>
        /// <param name="origin">The location where to start drawing.</param>
        /// <param name="size">The drawing size, padding included.</param>
        /// <param name="padding">The padding to use when drawing.</param>
        /// <param name="isFocused">true if the symbol text has the focus.</param>
        void DrawSymbol(Symbols symbol, Point origin, Size size, Padding padding, bool isFocused);

        /// <summary>
        /// Draws the horizontal separator left of the specified origin and with the specified height.
        /// </summary>
        /// <param name="separator">The separator to draw.</param>
        /// <param name="origin">The location where to draw.</param>
        /// <param name="height">The separator height.</param>
        void DrawHorizontalSeparator(HorizontalSeparators separator, Point origin, double height);

        /// <summary>
        /// Draws the vertical separator above the specified origin and with the specified width.
        /// </summary>
        /// <param name="separator">The separator to draw.</param>
        /// <param name="origin">The location where to draw.</param>
        /// <param name="width">The separator width.</param>
        void DrawVerticalSeparator(VerticalSeparators separator, Point origin, double width);

        /// <summary>
        /// Shows the caret.
        /// </summary>
        /// <param name="origin">Location of the cell with the caret.</param>
        /// <param name="text">The full cell text.</param>
        /// <param name="textStyle">The text style.</param>
        /// <param name="mode">The caret mode.</param>
        /// <param name="position">The position of the caret in <paramref name="text"/>.</param>
        void ShowCaret(Point origin, string text, TextStyles textStyle,  CaretModes mode, int position);

        /// <summary>
        /// Hides the caret.
        /// </summary>
        void HideCaret();

        /// <summary>
        /// Draws the vertical separator above the specified origin and with the specified width.
        /// </summary>
        /// <param name="region">The region corresponding to the node that has a comment.</param>
        void DrawCommentIcon(Rect region);

        /// <summary>
        /// Draws the background of a selected text.
        /// </summary>
        /// <param name="text">The text</param>
        /// <param name="origin">The location where to start drawing.</param>
        /// <param name="textStyle">The style used to measure selected text.</param>
        /// <param name="start">The starting point of the selection.</param>
        /// <param name="end">The ending point of the selection.</param>
        void DrawSelectionText(string text, Point origin, TextStyles textStyle, int start, int end);

        /// <summary>
        /// Draws the background of a selected rectangle.
        /// </summary>
        /// <param name="rect">The rectangle to draw.</param>
        void DrawSelectionRectangle(Rect rect);

        /// <summary>
        /// Get the location where draw occurs corresponding to the specified absolute location.
        /// </summary>
        /// <param name="origin">The absolute location.</param>
        /// <returns>The relative location where things would be drawn.</returns>
        Point ToRelativeLocation(Point origin);

        /// <summary>
        /// Get the caret position corresponding to <paramref name="origin"/> in <paramref name="text"/>.
        /// </summary>
        /// <param name="origin">The location.</param>
        /// <param name="text">The text</param>
        /// <param name="textStyle">The style used to measure <paramref name="text"/>.</param>
        /// <param name="mode">The caret mode.</param>
        /// <param name="maxTextWidth">The maximum width for a line of text. NaN means no limit.</param>
        /// <returns>The position of the caret.</returns>
        int GetCaretPositionInText(Point origin, string text, TextStyles textStyle, CaretModes mode, double maxTextWidth);
    }
}
