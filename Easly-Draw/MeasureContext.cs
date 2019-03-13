﻿namespace EaslyDraw
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows.Media;
    using BaseNodeHelper;
    using EaslyController.Constants;
    using EaslyController.Controller;
    using EaslyController.Layout;

    /// <summary>
    /// An implementation of IxxxMeasureContext for WPF.
    /// </summary>
    public class MeasureContext : ILayoutMeasureContext
    {
        #region Init
        /// <summary>
        /// Creates and initializes a new context.
        /// </summary>
        public static MeasureContext CreateMeasureContext()
        {
            MeasureContext Result = new MeasureContext();
            Result.Update();
            return Result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureContext"/> class.
        /// </summary>
        protected MeasureContext()
        {
            Typeface = new Typeface("Consolas");
            FontSize = 10;
            SubscriptRatio = 0.7;
            Culture = CultureInfo.CurrentCulture;
            FlowDirection = System.Windows.FlowDirection.LeftToRight;
            CommaSeparatorString = ", ";
            DotSeparatorString = "·";

            SolidColorBrush SelectionBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x99, 0xC9, 0xEF));

            BrushTable = new Dictionary<BrushSettings, Brush>
            {
                { BrushSettings.Default, Brushes.Black },
                { BrushSettings.Keyword, Brushes.Blue },
                { BrushSettings.Symbol, Brushes.Blue },
                { BrushSettings.Character, Brushes.Orange },
                { BrushSettings.Discrete, Brushes.DarkRed },
                { BrushSettings.NumberSignificand, Brushes.Green },
                { BrushSettings.NumberExponent, Brushes.Green },
                { BrushSettings.NumberInvalid, Brushes.Red },
                { BrushSettings.TypeIdentifier, new SolidColorBrush(Color.FromArgb(0xFF, 0x2B, 0x91, 0xAF)) },
                { BrushSettings.CommentBackground, Brushes.LightGreen },
                { BrushSettings.CommentForeground, Brushes.Black },
                { BrushSettings.CaretInsertion, Brushes.Black },
                { BrushSettings.CaretOverride, Brushes.DarkGray },
                { BrushSettings.Selection, SelectionBrush },
            };

            PenTable = new Dictionary<PenSettings, Pen>
            {
                { PenSettings.Default, null },
                { PenSettings.Comment, new Pen(Brushes.DarkGreen, 1) },
                { PenSettings.CaretInsertion, null },
                { PenSettings.CaretOverride, null },
                { PenSettings.SelectionText, null },
                { PenSettings.SelectionNode, new Pen(Brushes.Black, 1) { DashStyle = new DashStyle(new double[] { 1 }, 1) } },
                { PenSettings.SelectionNodeList, new Pen(Brushes.Black, 1) { DashStyle = new DashStyle(new double[] { 3 }, 3) } },
                { PenSettings.SelectionBlockList, new Pen(Brushes.Black, 1) { DashStyle = new DashStyle(new double[] { 3 }, 1) } },
                { PenSettings.BlockGeometry, new Pen(Brushes.Gray, 1) },
            };
        }
        #endregion

        #region Properties
        /// <summary>
        /// Width of a tabulation margin.
        /// </summary>
        public Measure TabulationWidth { get; private set; }

        /// <summary>
        /// Width of a vertical block geometry.
        /// </summary>
        public Measure BlockGeometryWidth { get; private set; }

        /// <summary>
        /// Height of an horizontal block geometry.
        /// </summary>
        public Measure BlockGeometryHeight { get; private set; }

        /// <summary>
        /// Height of a line of text.
        /// </summary>
        public Measure LineHeight { get; private set; }

        /// <summary>
        /// The font family, weight, style and stretch the text should be formatted with.
        /// </summary>
        public Typeface Typeface { get; set; }

        /// <summary>
        /// The font size.
        /// </summary>
        public double FontSize { get; set; }

        /// <summary>
        /// The em size.
        /// </summary>
        public double EmSize { get { return FontSize * 128.0 / 96.0; } }

        /// <summary>
        /// The ratio to normal size for subscript characters.
        /// </summary>
        public double SubscriptRatio { get; set; }

        /// <summary>
        /// The specific culture of the text.
        /// </summary>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// The direction the text is read.
        /// </summary>
        public System.Windows.FlowDirection FlowDirection { get; set; }

        /// <summary>
        /// The string used as comma separator.
        /// </summary>
        public string CommaSeparatorString { get; set; }

        /// <summary>
        /// The string used as dot separator.
        /// </summary>
        public string DotSeparatorString { get; set; }

        /// <summary>
        /// The insertion caret width.
        /// </summary>
        public double InsertionCaretWidth { get; private set; }

        /// <summary>
        /// Table of brushes to use when drawing.
        /// </summary>
        public IDictionary<BrushSettings, Brush> BrushTable { get; }

        /// <summary>
        /// Table of pens to use when drawing.
        /// </summary>
        public IDictionary<PenSettings, Pen> PenTable { get; }
        #endregion

        #region Implementation of IxxxMeasureContext
        /// <summary>
        /// Gets the width corresponding to a separator between cells in a line.
        /// </summary>
        /// <param name="separator">The separator.</param>
        public virtual Measure GetHorizontalSeparatorWidth(HorizontalSeparators separator)
        {
            return HorizontalSeparatorWidthTable[separator];
        }

        /// <summary>
        /// Gets the height corresponding to a separator between cells in a column.
        /// </summary>
        /// <param name="separator">The separator.</param>
        public virtual Measure GetVerticalSeparatorHeight(VerticalSeparators separator)
        {
            return VerticalSeparatorHeightTable[separator];
        }

        /// <summary>
        /// Measures a string that is not a number.
        /// </summary>
        /// <param name="text">The string to measure.</param>
        /// <param name="textStyle">Style to use for the text.</param>
        /// <param name="maxTextWidth">The maximum width for a line of text. NaN means no limit.</param>
        /// <returns>The size of the string.</returns>
        public virtual Size MeasureText(string text, TextStyles textStyle, Measure maxTextWidth)
        {
            Brush Brush = BrushTable[StyleToForegroundBrush(textStyle)];
            FormattedText ft = new FormattedText(text, Culture, FlowDirection, Typeface, EmSize, Brush);
            if (!maxTextWidth.IsFloating)
                ft.MaxTextWidth = maxTextWidth.Draw;

            double TextWidth = ft.WidthIncludingTrailingWhitespace;
            if (TextWidth < InsertionCaretWidth)
                TextWidth = InsertionCaretWidth;

            Measure Width = new Measure() { Draw = TextWidth, Print = text.Length };
            Measure Height = LineHeight;
            return new Size(Width, Height);
        }

        /// <summary>
        /// Measures a number string.
        /// </summary>
        /// <param name="text">The string to measure.</param>
        /// <returns>The size of the string.</returns>
        public virtual Size MeasureNumber(string text)
        {
            IFormattedNumber fn = FormattedNumber.Parse(text);
            string SignificandString = fn.SignificandString;
            string ExponentString = fn.ExponentString;
            string InvalidText = fn.InvalidText;

            Brush Brush;

            Brush = BrushTable[BrushSettings.NumberSignificand];
            FormattedText ftSignificand = new FormattedText(SignificandString, Culture, FlowDirection, Typeface, EmSize, Brush);

            Brush = BrushTable[BrushSettings.NumberExponent];
            FormattedText ftExponent = new FormattedText(ExponentString, Culture, FlowDirection, Typeface, EmSize * SubscriptRatio, Brush);

            Brush = BrushTable[BrushSettings.NumberInvalid];
            FormattedText ftInvalid = new FormattedText(InvalidText, Culture, FlowDirection, Typeface, EmSize, Brush);

            Measure Width = new Measure() { Draw = ftSignificand.WidthIncludingTrailingWhitespace + ftExponent.WidthIncludingTrailingWhitespace + ftInvalid.WidthIncludingTrailingWhitespace, Print = text.Length };
            Measure Height = LineHeight;
            return new Size(Width, Height);
        }

        /// <summary>
        /// Measures a symbol.
        /// </summary>
        /// <param name="symbol">The symbol to measure.</param>
        /// <returns>The size of the symbol.</returns>
        public virtual Size MeasureSymbolSize(Symbols symbol)
        {
            string Text = SymbolToText(symbol);
            FormattedText ft = new FormattedText(Text, Culture, FlowDirection, Typeface, EmSize, BrushTable[BrushSettings.Symbol]);

            switch (symbol)
            {
                default:
                case Symbols.LeftArrow:
                case Symbols.Dot:
                case Symbols.InsertSign:
                    return new Size(new Measure() { Draw = ft.Width, Print = Text.Length }, LineHeight);
                case Symbols.LeftBracket:
                case Symbols.RightBracket:
                case Symbols.LeftCurlyBracket:
                case Symbols.RightCurlyBracket:
                case Symbols.LeftParenthesis:
                case Symbols.RightParenthesis:
                    return new Size(new Measure() { Draw = ft.Width, Print = Text.Length }, Measure.Floating);
                case Symbols.HorizontalLine:
                    return new Size(Measure.Floating, VerticalSeparatorHeightTable[VerticalSeparators.Line]);
            }
        }

        /// <summary></summary>
        protected virtual BrushSettings StyleToForegroundBrush(TextStyles textStyle)
        {
            switch (textStyle)
            {
                default:
                case TextStyles.Default:
                    return BrushSettings.Default;
                case TextStyles.Character:
                    return BrushSettings.Character;
                case TextStyles.Discrete:
                    return BrushSettings.Discrete;
                case TextStyles.Keyword:
                    return BrushSettings.Keyword;
                case TextStyles.Type:
                    return BrushSettings.TypeIdentifier;
                case TextStyles.Comment:
                    return BrushSettings.CommentForeground;
            }
        }

        /// <summary></summary>
        protected virtual string SymbolToText(Symbols symbol)
        {
            string Text = null;

            switch (symbol)
            {
                case Symbols.LeftArrow:
                    Text = "←";
                    break;
                case Symbols.Dot:
                    Text = DotSeparatorString;
                    break;
                case Symbols.InsertSign:
                    Text = "◄";
                    break;
                case Symbols.LeftBracket:
                    Text = "[";
                    break;
                case Symbols.RightBracket:
                    Text = "]";
                    break;
                case Symbols.LeftCurlyBracket:
                    Text = "{";
                    break;
                case Symbols.RightCurlyBracket:
                    Text = "}";
                    break;
                case Symbols.LeftParenthesis:
                    Text = "(";
                    break;
                case Symbols.RightParenthesis:
                    Text = ")";
                    break;
                case Symbols.HorizontalLine:
                    Text = "-";
                    break;
            }

            Debug.Assert(Text != null);
            return Text;
        }

        /// <summary>
        /// Extends a size according to the left and right margin settings.
        /// </summary>
        /// <param name="leftMargin">The left margin setting.</param>
        /// <param name="rightMargin">The right margin setting.</param>
        /// <param name="size">The size to extend with the calculated padding.</param>
        /// <param name="padding">The padding calculated from <paramref name="leftMargin"/> and <paramref name="rightMargin"/>.</param>
        public virtual void UpdatePadding(Margins leftMargin, Margins rightMargin, ref Size size, out Padding padding)
        {
            double LeftPadding = 0;
            double RightPadding = 0;
            int LeftSpacing = 0;
            int RightSpacing = 0;

            switch (leftMargin)
            {
                case Margins.ThinSpace:
                    LeftPadding = WhitespaceWidth / 2;
                    break;
                case Margins.Whitespace:
                    LeftPadding = WhitespaceWidth;
                    LeftSpacing = 1;
                    break;
            }

            switch (rightMargin)
            {
                case Margins.ThinSpace:
                    RightPadding = WhitespaceWidth / 2;
                    break;
                case Margins.Whitespace:
                    RightPadding = WhitespaceWidth;
                    RightSpacing = 1;
                    break;
            }

            size = new Size(new Measure() { Draw = size.Width.Draw + LeftPadding + RightPadding, Print = size.Width.Print + LeftSpacing + RightSpacing }, size.Height);
            padding = new Padding(new Measure() { Draw = LeftPadding, Print = LeftSpacing }, Measure.Zero, new Measure() { Draw = RightPadding, Print = RightSpacing }, Measure.Zero);
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Recalculate internal constants.
        /// To call after a property was changed.
        /// </summary>
        public virtual void Update()
        {
            FormattedText ft;

            ft = new FormattedText(" ", Culture, FlowDirection, Typeface, EmSize, BrushTable[BrushSettings.Default]);
            WhitespaceWidth = ft.WidthIncludingTrailingWhitespace;
            InsertionCaretWidth = WhitespaceWidth / 4;
            LineHeight = new Measure() { Draw = ft.Height, Print = 1 };
            int TabulationLength = 3;
            TabulationWidth = new Measure() { Draw = WhitespaceWidth * TabulationLength, Print = TabulationLength };
            BlockGeometryWidth = new Measure() { Draw = WhitespaceWidth, Print = 1 };
            BlockGeometryHeight = new Measure() { Draw = LineHeight.Draw, Print = 1 };
            VerticalSeparatorHeightTable[VerticalSeparators.Line] = new Measure() { Draw = LineHeight.Draw * 3, Print = 3 };

            ft = new FormattedText(CommaSeparatorString, Culture, FlowDirection, Typeface, EmSize, BrushTable[BrushSettings.Symbol]);
            HorizontalSeparatorWidthTable[HorizontalSeparators.Comma] = new Measure() { Draw = ft.WidthIncludingTrailingWhitespace, Print = CommaSeparatorString.Length };

            ft = new FormattedText(DotSeparatorString, Culture, FlowDirection, Typeface, EmSize, BrushTable[BrushSettings.Symbol]);
            HorizontalSeparatorWidthTable[HorizontalSeparators.Dot] = new Measure() { Draw = ft.WidthIncludingTrailingWhitespace, Print = DotSeparatorString.Length };
        }

        /// <summary></summary>
        protected double WhitespaceWidth { get; private set; }

        /// <summary></summary>
        protected Dictionary<HorizontalSeparators, Measure> HorizontalSeparatorWidthTable { get; } = new Dictionary<HorizontalSeparators, Measure>()
        {
            { HorizontalSeparators.None, Measure.Zero },
            { HorizontalSeparators.Comma, Measure.Zero },
            { HorizontalSeparators.Dot, Measure.Zero },
        };

        /// <summary></summary>
        protected Dictionary<VerticalSeparators, Measure> VerticalSeparatorHeightTable { get; } = new Dictionary<VerticalSeparators, Measure>()
        {
            { VerticalSeparators.None, Measure.Zero },
            { VerticalSeparators.Line, Measure.Zero },
        };
        #endregion
    }
}
