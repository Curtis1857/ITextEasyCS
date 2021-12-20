using System;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
namespace iTextEasyCS
{
    partial class PDFWriter
    {
        public System.Drawing.Font Font = new System.Drawing.Font("helvetica", 12);
        
        public TextAlignment FontAlignment = TextAlignment.LeftBaseline;
        public float FontLineSpacing = 1f;

        public void WriteLinesAt(bool step, float X, float Y, float Width, float NextLineLeftIndent, float NextLineRightIndent, string str, bool justify, bool newLine)
        {
            FinishLine();
            switch (FontAlignment) {
                case var @case when @case == TextAlignment.CenterBaseline:
                case var case1 when case1 == TextAlignment.CenterBottom:
                case var case2 when case2 == TextAlignment.CenterCenter:
                case var case3 when case3 == TextAlignment.CenterTop:
                case var case4 when case4 == TextAlignment.RightBaseline:
                case var case5 when case5 == TextAlignment.RightBottom:
                case var case6 when case6 == TextAlignment.RightCenter:
                case var case7 when case7 == TextAlignment.RightTop: {
                        throw new InvalidOperationException("Invalid FontAlignment; use a left-aligned alignment");
                        break;
                }
            }

            if (step) {
                _CurrentX += _Translate(X);
                _CurrentY += _Translate(Y);
            } else {
                _CurrentX = _Translate(X);
                _CurrentY = _Translate(Y);
            }

            if (string.IsNullOrEmpty(str)) {
                if (newLine)
                    Write(null, true);
                return;
            }

            Width = _Translate(Width);
            str = WriteLine(Width, str, justify, newLine);
            if (str is null)
                return;
            Width = Width - _Translate(NextLineLeftIndent) - _Translate(NextLineRightIndent);
            _CurrentX += _Translate(NextLineLeftIndent);
            do
                str = WriteLine(Width, str, justify, newLine);
            while (!(str is null));
            if (newLine)
                _CurrentX -= _Translate(NextLineLeftIndent);
        }

        public string WriteLine(float width, string str, bool justify, bool newLine)
        {
            // Always goes to new line if there is more text to be written
            switch (FontAlignment) {
                case var @case when @case == TextAlignment.CenterBaseline:
                case var case1 when case1 == TextAlignment.CenterBottom:
                case var case2 when case2 == TextAlignment.CenterCenter:
                case var case3 when case3 == TextAlignment.CenterTop:
                case var case4 when case4 == TextAlignment.RightBaseline:
                case var case5 when case5 == TextAlignment.RightBottom:
                case var case6 when case6 == TextAlignment.RightCenter:
                case var case7 when case7 == TextAlignment.RightTop: {
                        throw new InvalidOperationException("Invalid FontAlignment; use a left-aligned alignment");
                        break;
                    }
            }

            return Write2(str, newLine, width, true, justify);
        }

        public void WriteAt(bool step, float X, float Y, string str, bool newLine = false)
        {
            FinishLine();
            if (step) {
                _CurrentX += _Translate(X);
                _CurrentY += _Translate(Y);
            } else {
                _CurrentX = _Translate(X);
                _CurrentY = _Translate(Y);
            }

            Write(str, newLine);
        }

        public void WriteFont(string str, Font f)
        {
            //Encoding.RegisterProvider
            //var font = new Font("Agency FB", 12, FontStyle.Bold);
            
            _Content.SetFontAndSize(f.BaseFont, 12);
            Write(str);
        }

        public void Write(string str, bool NewLine = false)
        {
            Write2(str, NewLine, 0f, false, false);
        }

        private string Write2(string str, bool newLine, float width, bool wordWrap, bool justify)
        {
            string Write2Ret = default;
            FinishLine();
            if (string.IsNullOrEmpty(str) & !newLine)
                return null;
            iTextSharp.text.Font f = _GetFont(Font);
            BaseFont bf = f.GetCalculatedBaseFont(true);
            var TextWidth = default(float);
            if (!string.IsNullOrEmpty(str)) {
                _Content.SaveState();
                try {
                    _Content.BeginText();
                    _Content.SetFontAndSize(bf, f.CalculatedSize);
                    float XOffset = default, YOffset = default;
                    if (wordWrap) {
                        int I = str.IndexOf(' ');
                        if (I == -1) {
                            wordWrap = false; // use newLine variable and don't kern text
                            Write2Ret = null;
                        } else {
                            int J, spaces;
                            float K, strlen, spacelen;
                            string str2;
                            J = 0;
                            strlen = _Content.GetEffectiveStringWidth(str.Substring(0, I), false);
                            spacelen = _Content.GetEffectiveStringWidth(" ", false);
                            spaces = 0;
                            do {
                                J = str.IndexOf(' ', I + 1); //no error thrown when startIndex = str.Length, but always returns -1 (perfect!)
                                if (J == -1) {
                                    str2 = str;
                                }
                                else {
                                    str2 = str.Substring(0, J);
                                 
                                }

                                K = _Content.GetEffectiveStringWidth(str2.TrimEnd(), false);
                                if (K <= width) {
                                    // enough room; continue with loop
                                    if (J == -1) {
                                        // enough room for entire string
                                        wordWrap = false; // use newLine variable and don't kern text
                                        Write2Ret = null;
                                        break;
                                    } else {
                                        I = J;
                                        strlen = K;
                                        spaces += 1;
                                    }
                                } else {
                                    // not enough room; go back to last valid string
                                    Write2Ret = str.Substring(I + 1); // everything after the space
                                    str = str.Substring(0, I).TrimEnd(); // everything before the space, trimmed in case of extra spaces
                                                                         // wordWrap = True 'write a new line and kern text
                                    if (justify)
                                        _Content.SetWordSpacing((width - strlen) / spaces);
                                    break;
                                }
                            }
                            while (true);
                        }
                    } else {
                        Write2Ret = null;
                    }

                    switch (FontAlignment) {
                        case var @case when @case == TextAlignment.LeftTop:
                        case var case1 when case1 == TextAlignment.LeftCenter:
                        case var case2 when case2 == TextAlignment.LeftBaseline:
                        case var case3 when case3 == TextAlignment.LeftBottom: {
                                break;
                            }

                        case var case4 when case4 == TextAlignment.CenterTop:
                        case var case5 when case5 == TextAlignment.CenterCenter:
                        case var case6 when case6 == TextAlignment.CenterBaseline:
                        case var case7 when case7 == TextAlignment.CenterBottom: {
                                XOffset = -_Content.GetEffectiveStringWidth(str, false) / 2;
                                break;
                            }

                        case var case8 when case8 == TextAlignment.RightTop:
                        case var case9 when case9 == TextAlignment.RightCenter:
                        case var case10 when case10 == TextAlignment.RightBaseline:
                        case var case11 when case11 == TextAlignment.RightBottom: {
                                XOffset = -_Content.GetEffectiveStringWidth(str, false);
                                break;
                            }
                    }

                    switch (FontAlignment) {
                        case var case12 when case12 == TextAlignment.LeftTop:
                        case var case13 when case13 == TextAlignment.CenterTop:
                        case var case14 when case14 == TextAlignment.RightTop: {
                                YOffset += bf.GetFontDescriptor(BaseFont.AWT_ASCENT, f.CalculatedSize);
                                break;
                            }

                        case var case15 when case15 == TextAlignment.LeftCenter:
                        case var case16 when case16 == TextAlignment.CenterCenter:
                        case var case17 when case17 == TextAlignment.RightCenter: {
                                YOffset += bf.GetFontDescriptor(BaseFont.CAPHEIGHT, f.CalculatedSize) / 2;
                                break;
                            }

                        case var case18 when case18 == TextAlignment.LeftBaseline:
                        case var case19 when case19 == TextAlignment.CenterBaseline:
                        case var case20 when case20 == TextAlignment.RightBaseline: {
                                break;
                            }

                        case var case21 when case21 == TextAlignment.LeftBottom:
                        case var case22 when case22 == TextAlignment.CenterBottom:
                        case var case23 when case23 == TextAlignment.RightBottom: {
                                YOffset += bf.GetFontDescriptor(BaseFont.AWT_DESCENT, f.CalculatedSize); // negative value
                                YOffset -= bf.GetFontDescriptor(BaseFont.AWT_LEADING, f.CalculatedSize);
                                break;
                            }
                    }

                    if ((f.CalculatedStyle & iTextSharp.text.Font.ITALIC) == iTextSharp.text.Font.ITALIC) {
                        _Content.SetTextMatrix(1, 0, 0.21256f, -1, _CurrentX + XOffset, _CurrentY + YOffset);
                    } else {
                        _Content.SetTextMatrix(1, 0, 0, -1, _CurrentX + XOffset, _CurrentY + YOffset);
                    }

                    if ((f.CalculatedStyle & iTextSharp.text.Font.BOLD) == iTextSharp.text.Font.BOLD) {
                        _Content.SetTextRenderingMode(PdfContentByte.TEXT_RENDER_MODE_FILL_STROKE);
                        _Content.SetColorStroke(f.Color);
                        _Content.SetLineCap(PdfContentByte.LINE_CAP_PROJECTING_SQUARE);
                        _Content.SetLineDash(0);
                        _Content.SetLineJoin(PdfContentByte.LINE_JOIN_MITER);
                        _Content.SetLineWidth(f.Size / 30);
                    }

                    _Content.SetColorFill(f.Color);
                    _Content.MoveText(0, 0);
                    _Content.ShowText(str);
                    // If NewLine Then _Content.NewlineText() 'unused; newline code is below.  (doesn't update CurrentY)
                    if ((f.CalculatedStyle & iTextSharp.text.Font.BOLD) == iTextSharp.text.Font.BOLD) {
                        _Content.SetTextRenderingMode(PdfContentByte.TEXT_RENDER_MODE_FILL);
                    }

                    TextWidth = _Content.GetEffectiveStringWidth(str, false);
                    _Content.EndText();
                    if ((f.CalculatedStyle & iTextSharp.text.Font.UNDERLINE) == iTextSharp.text.Font.UNDERLINE) {
                        _Content.Rectangle(_CurrentX + XOffset, _CurrentY + YOffset + f.CalculatedSize / 4, TextWidth, -f.CalculatedSize / 15);
                        _Content.Fill();
                    }

                    if ((f.CalculatedStyle & iTextSharp.text.Font.STRIKETHRU) == iTextSharp.text.Font.STRIKETHRU) {
                        _Content.Rectangle(_CurrentX + XOffset, _CurrentY + YOffset - f.CalculatedSize / 3, TextWidth, -f.CalculatedSize / 15);
                        _Content.Fill();
                    }
                } finally {
                    _Content.RestoreState();
                }
            } else {
                Write2Ret = null;
            }

            if (newLine | wordWrap) // justify, at this point, is equilavent to (Not Write2 Is Nothing) -- indicating whether there is text to kern or not
            {
                float TextHeight = bf.GetFontDescriptor(iTextSharp.text.pdf.BaseFont.AWT_ASCENT, f.CalculatedSize);
                TextHeight -= bf.GetFontDescriptor(iTextSharp.text.pdf.BaseFont.AWT_DESCENT, f.CalculatedSize);
                TextHeight += bf.GetFontDescriptor(iTextSharp.text.pdf.BaseFont.AWT_LEADING, f.CalculatedSize);
                _CurrentY += TextHeight * FontLineSpacing;
            } else {
                switch (FontAlignment) {
                    case var case24 when case24 == TextAlignment.LeftTop:
                    case var case25 when case25 == TextAlignment.LeftCenter:
                    case var case26 when case26 == TextAlignment.LeftBaseline:
                    case var case27 when case27 == TextAlignment.LeftBottom: {
                            _CurrentX += TextWidth;
                            break;
                        }

                    case var case28 when case28 == TextAlignment.CenterTop:
                    case var case29 when case29 == TextAlignment.CenterCenter:
                    case var case30 when case30 == TextAlignment.CenterBaseline:
                    case var case31 when case31 == TextAlignment.CenterBottom: {
                            _CurrentX += TextWidth / 2f;
                            break;
                        }

                    case var case32 when case32 == TextAlignment.RightTop:
                    case var case33 when case33 == TextAlignment.RightCenter:
                    case var case34 when case34 == TextAlignment.RightBaseline:
                    case var case35 when case35 == TextAlignment.RightBottom: {
                            _CurrentX -= TextWidth;
                            break;
                        }
                }
            }

            return Write2Ret;
        }

        public float TextWidth(string str)
        {
            // width of specified text
            Font f = _GetFont(Font);
            return _TranslateRev(f.GetCalculatedBaseFont(false).GetWidthPoint(str, f.CalculatedSize));
        }

        public float TextHeight()
        {
            // height of a single line of text, including space between rows
            // (ascent + descent + leading)
            if (Font is null)
                throw new InvalidOperationException();
            Font f = _GetFont(Font);
            BaseFont bf = f.GetCalculatedBaseFont(false);
            float s = bf.GetFontDescriptor(iTextSharp.text.pdf.BaseFont.AWT_ASCENT, f.CalculatedSize);
            s -= bf.GetFontDescriptor(iTextSharp.text.pdf.BaseFont.AWT_DESCENT, f.CalculatedSize);
            s += bf.GetFontDescriptor(iTextSharp.text.pdf.BaseFont.AWT_LEADING, f.CalculatedSize);
            return _TranslateRev(s);
        }

        public float TextCapHeight()
        {
            // distance between the baseline and the top of capital letters
            if (Font is null)
                throw new InvalidOperationException();
            Font f = _GetFont(Font);
            BaseFont bf = f.GetCalculatedBaseFont(false);
            return _TranslateRev(bf.GetFontDescriptor(iTextSharp.text.pdf.BaseFont.CAPHEIGHT, f.CalculatedSize));
        }

        public float TextAscent()
        {
            // distance between the baseline and the top of the highest letters
            if (Font is null)
                throw new InvalidOperationException();
            Font f = _GetFont(Font);
            BaseFont bf = f.GetCalculatedBaseFont(false);
            return _TranslateRev(bf.GetFontDescriptor(iTextSharp.text.pdf.BaseFont.AWT_ASCENT, f.CalculatedSize));
        }

        public float TextDescent()
        {
            // distance between the baseline and the bottom of the lowest letters (j's, etc)
            if (Font is null)
                throw new InvalidOperationException();
            Font f = _GetFont(Font);
            BaseFont bf = f.GetCalculatedBaseFont(false);
            return _TranslateRev(-bf.GetFontDescriptor(iTextSharp.text.pdf.BaseFont.AWT_DESCENT, f.CalculatedSize));
        }

        public float TextLeading()
        {
            // additional line space in addition to the ascent and descent
            if (Font is null)
                throw new InvalidOperationException();
            Font f = _GetFont(Font);
            BaseFont bf = f.GetCalculatedBaseFont(false);
            return _TranslateRev(bf.GetFontDescriptor(iTextSharp.text.pdf.BaseFont.AWT_LEADING, f.CalculatedSize));
        }
    }
}
