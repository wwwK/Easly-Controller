﻿using System.Diagnostics;
using EaslyController.Constants;
using EaslyController.Controller;
using EaslyController.Layout;

namespace TestDebug
{
    public class LayoutDrawContext : ILayoutDrawContext
    {
        public static LayoutDrawContext Default = new LayoutDrawContext();

        public Measure LineHeight { get { return new Measure() { Draw = 12, Print = 1 }; } }
        public Measure TabulationWidth { get { return new Measure() { Draw = 24, Print = 2 }; } }
        
        public Measure GetHorizontalSeparatorWidth(HorizontalSeparators separator)
        {
            return Measure.Zero;
        }

        public Measure GetVerticalSeparatorHeight(VerticalSeparators separator)
        {
            return Measure.Zero;
        }

        public Size MeasureSymbolSize(Symbols symbol)
        {
            switch (symbol)
            {
                default:
                case Symbols.LeftArrow:
                case Symbols.Dot:
                case Symbols.InsertSign:
                    return new Size(new Measure() { Draw = 20 }, LineHeight);
                case Symbols.LeftBracket:
                case Symbols.RightBracket:
                case Symbols.LeftCurlyBracket:
                case Symbols.RightCurlyBracket:
                case Symbols.LeftParenthesis:
                case Symbols.RightParenthesis:
                    return new Size(new Measure() { Draw = 20 }, Measure.Floating);
                case Symbols.HorizontalLine:
                    return new Size(Measure.Floating, new Measure() { Draw = 20 });
            }
        }

        public Size MeasureTextSize(string text, TextStyles textStyle, Measure maxTextWidth)
        {
            return new Size(new Measure() { Draw = text.Length * 20, Print = text.Length }, LineHeight);
        }

        public void UpdatePadding(Margins leftMargin, Margins rightMargin, ref Size size, out Padding padding)
        {
            padding = Padding.Empty;
        }

        public virtual void DrawSelectionText(string text, Point origin, TextStyles textStyle, int start, int end)
        {
        }

        public void DrawText(string text, Point origin, TextStyles textStyle, bool isFocused)
        {
        }

        public void DrawSymbol(Symbols symbol, Point origin, Size size, Padding padding, bool isFocused)
        {
        }

        public void DrawHorizontalSeparator(HorizontalSeparators separator, Point origin, Measure height)
        {
        }

        public void DrawVerticalSeparator(VerticalSeparators separator, Point origin, Measure width)
        {
        }

        public void ShowCaret(Point origin, string text, TextStyles textStyle, CaretModes mode, int position)
        {
            Debug.Assert(position >= 0 && ((mode == CaretModes.Insertion && position <= text.Length) || (mode == CaretModes.Override && position < text.Length)));
        }

        public void HideCaret()
        {
        }

        public void DrawCommentIcon(Rect region)
        {
        }

        public virtual int GetCaretPositionInText(double x, string text, TextStyles textStyle, CaretModes mode, Measure maxTextWidth)
        {
            return 0;
        }

        public virtual void ToRelativeLocation(ref double x, ref double y)
        {
        }

        public virtual void DrawSelectionRectangle(Rect rect)
        {
        }
    }
}
