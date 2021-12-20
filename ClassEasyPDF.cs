using System;
using System.Drawing;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
namespace iTextEasyCS
{
    public enum PictureAlignment
    {
        LeftTop,
        LeftCenter,
        LeftBottom,
        CenterTop,
        CenterCenter,
        CenterBottom,
        RightTop,
        RightCenter,
        RightBottom
    }

    public enum TextAlignment
    {
        LeftTop,
        LeftCenter,
        LeftBaseline,
        LeftBottom,
        CenterTop,
        CenterCenter,
        CenterBaseline,
        CenterBottom,
        RightTop,
        RightCenter,
        RightBaseline,
        RightBottom
    }

    public enum LineCapStyle
    {
        None = iTextSharp.text.pdf.PdfContentByte.LINE_CAP_BUTT,
        Square = iTextSharp.text.pdf.PdfContentByte.LINE_CAP_PROJECTING_SQUARE,
        Round = iTextSharp.text.pdf.PdfContentByte.LINE_CAP_ROUND
    }

    public enum LineJoinStyle
    {
        Miter = iTextSharp.text.pdf.PdfContentByte.LINE_JOIN_MITER,
        Rounded = iTextSharp.text.pdf.PdfContentByte.LINE_JOIN_ROUND,
        Bevel = iTextSharp.text.pdf.PdfContentByte.LINE_JOIN_BEVEL
    }

    public partial class LineDashStyle
    {
        private float[] _array;
        private float _phase;

        public LineDashStyle(float unitsOn, float phase) : this(new float[] { unitsOn, unitsOn }, phase)
        {
        }

        public LineDashStyle(float unitsOn, float unitsOff, float phase) : this(new float[] { unitsOn, unitsOff }, phase)
        {
        }

        public LineDashStyle(float[] array, float phase)
        {
            if (array is null)
                throw new ArgumentNullException();
            _array = array;
            _phase = phase;
        }

        public float[] Array {
            get {
                return (float[])_array.Clone();
            }
        }

        public float[] MultipliedArray(float multiplier)
        {
            float[] x = (float[])_array.Clone();
            for (int i = 0, loopTo = x.GetUpperBound(0); i <= loopTo; i++)
                x[i] *= multiplier;
            return x;
        }

        public float Phase {
            get {
                return _phase;
            }
        }

        public float MultipliedPhase(float multiplier)
        {
            return _phase * multiplier;
        }

        public static readonly LineDashStyle Solid = new LineDashStyle(new float[] { }, 0f);
        public static readonly LineDashStyle Dash = new LineDashStyle(6f, 6f, 3f);
        public static readonly LineDashStyle Dot = new LineDashStyle(2f, 3f, 0f);
        public static readonly LineDashStyle DashDotDot = new LineDashStyle(new float[] { 6f, 3f, 2f, 3f, 2f, 3f }, 0f);
    }

    public partial class PDFWriter
    {
        private Stream _Stream;
        private Document _Document;
        private iTextSharp.text.pdf.PdfWriter _Writer;
        private PdfContentByte _Content;
        private ScaleModes _ScaleMode = ScaleModes.Hundredths;
        private SizeF _PageSize;
        private PointF _Margins;

        static PDFWriter()
        {
            FontFactory.RegisterDirectories();
            //FontFactory.RegisterDirectory(@"C:\Windows\Fonts");
            //FontFactory.RegisterDirectory(@"C:\Users\Curtis\APPDATA\LOCAL\Microsoft\Windows\Fonts");
        }

        public PDFWriter()
        {
            _Stream = new MemoryStream();
        }

        public PDFWriter(Stream s)
        {
            _Stream = s;
        }

        public PdfContentByte GetDirectContent()
        {
            return _Content;
        }

        public iTextSharp.text.pdf.PdfWriter GetWriter()
        {
            return _Writer;
        }

        public Document GetDocument()
        {
            return _Document;
        }

        private System.Drawing.Printing.PaperSize _GetPaperSize(System.Drawing.Printing.PaperKind paperKind)
        {
            switch (paperKind) {
                case var @case when @case == System.Drawing.Printing.PaperKind.Letter: {
                        return new System.Drawing.Printing.PaperSize(paperKind.ToString(), 850, 1100);
                    }

                case var case1 when case1 == System.Drawing.Printing.PaperKind.Legal: {
                        return new System.Drawing.Printing.PaperSize(paperKind.ToString(), 850, 1400);
                    }

                case var case2 when case2 == System.Drawing.Printing.PaperKind.Ledger: {
                        return new System.Drawing.Printing.PaperSize(paperKind.ToString(), 1100, 1700);
                    }

                default: {
                        throw new NotImplementedException();
                        break;
                    }
            }
        }

        public void NewPage(System.Drawing.Printing.PaperKind paperKind, bool landscape)
        {
            NewPage(_GetPaperSize(paperKind), landscape);
        }

        public void NewPage(System.Drawing.Printing.PaperKind paperKind, bool landscape, float marginLeft, float marginTop)
        {
            NewPage(_GetPaperSize(paperKind), landscape, marginLeft, marginTop);
        }

        public void NewPage(System.Drawing.Printing.PaperKind paperKind, bool landscape, System.Drawing.Printing.Margins m)
        {
            NewPage(_GetPaperSize(paperKind), landscape, m);
        }

        public void NewPage(float width, float height, bool Landscape)
        {
            NewPageAbs(_Translate(width), _Translate(height), 0f, 0f, Landscape);
        }

        public void NewPage(float width, float height, bool Landscape, float marginLeft, float marginTop)
        {
            NewPageAbs(_Translate(width), _Translate(height), _Translate(marginLeft), _Translate(marginTop), Landscape);
        }

        public void NewPage(System.Drawing.Printing.PaperSize ps, bool Landscape)
        {
            NewPageAbs(_Translate(ps.Width, ScaleModes.Hundredths), _Translate(ps.Height, ScaleModes.Hundredths), 0, 0, Landscape);
        }

        public void NewPage(System.Drawing.Printing.PaperSize ps, bool Landscape, float MarginLeft, float MarginTop)
        {
            NewPageAbs(_Translate(ps.Width, ScaleModes.Hundredths), _Translate(ps.Height, ScaleModes.Hundredths), _Translate(MarginLeft), _Translate(MarginTop), Landscape);
        }

        public void NewPage(System.Drawing.Printing.PaperSize ps, bool Landscape, System.Drawing.Printing.Margins m)
        {
            NewPageAbs(_Translate(ps.Width, ScaleModes.Hundredths), _Translate(ps.Height, ScaleModes.Hundredths), _Translate(m.Left, ScaleModes.Hundredths), _Translate(m.Top, ScaleModes.Hundredths), Landscape);
        }
        // Private Sub NewPageAbs(ByVal PageWidth As Single, ByVal PageHeight As Single, ByVal MarginLeft As Single, ByVal MarginRight As Single, ByVal MarginTop As Single, ByVal MarginBottom As Single)
        private void NewPageAbs(float PageWidth, float PageHeight, float MarginLeft, float MarginTop, bool Landscape)
        {
            FinishLine();
            if (_Document is null) {
                // _Document = New Document(New iTextSharp.text.Rectangle(PageWidth, PageHeight), _
                // MarginLeft, MarginRight, MarginTop, MarginBottom)
                if (Landscape) {
                    var rotated = new iTextSharp.text.Rectangle(PageWidth, PageHeight).Rotate();
                    _Document = new Document(rotated);
                } else {
                    _Document = new Document(new iTextSharp.text.Rectangle(PageWidth, PageHeight));
                }

                _Writer = iTextSharp.text.pdf.PdfWriter.GetInstance(_Document, _Stream);
                _Document.Open();
                _Content = _Writer.DirectContent;
            } else {
                if (Landscape) {
                    _Document.SetPageSize(new iTextSharp.text.Rectangle(PageWidth, PageHeight).Rotate());
                } else {
                    _Document.SetPageSize(new iTextSharp.text.Rectangle(PageWidth, PageHeight));
                }
                // _Document.SetMargins(MarginLeft, MarginRight, MarginTop, MarginBottom)
                _Document.NewPage();
            }

            _Writer.PageEmpty = false;
            if (Landscape) {
                _Content.ConcatCTM(1, 0, 0, -1, MarginLeft, PageWidth - MarginTop);
                _PageSize = new SizeF(PageHeight, PageWidth);
            } else {
                _Content.ConcatCTM(1, 0, 0, -1, MarginLeft, PageHeight - MarginTop);
                _PageSize = new SizeF(PageWidth, PageHeight);
            }
            // If MarginLeft <> 0 Or MarginRight <> 0 Or MarginTop <> 0 Or MarginBottom <> 0 Then
            // _Content.MoveTo(0, 0)
            // _Content.LineTo(PageWidth - MarginLeft - MarginRight, 0)
            // _Content.LineTo(PageWidth - MarginLeft - MarginRight, PageHeight - MarginTop - MarginBottom)
            // _Content.LineTo(0, PageHeight - MarginTop - MarginBottom)
            // _Content.Clip()
            // _Content.NewPath()
            // End If
            _InitLineVars();
            _CurrentX = 0;
            _CurrentY = 0;
        }


        public byte[] ToArray()
        {
            if (!(_Stream is MemoryStream))
                throw new InvalidOperationException();
            Close();
            return ((MemoryStream)_Stream).ToArray();
        }

        public void Close() // closes underlying stream
        {
            if (_Content is object)
                FinishLine();
            _Content = null;
            if (_Document is object) {
                _Document.Close();
                _Document = null;
            }

            if (_Writer is object) {
                _Writer.Close();
                _Writer = null;
            }
        }

        public ScaleModes ScaleMode {
            get {
                return _ScaleMode;
            }

            set {
                switch (value) {
                    case ScaleModes.Hundredths:
                    case ScaleModes.Inches:
                    case ScaleModes.Points: {
                            _ScaleMode = value;
                            break;
                        }

                    default: {
                            throw new ArgumentOutOfRangeException();
                            break;
                        }
                }
            }
        }


        public void RegisterFont(string name, string path)
        {
            var codePages = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(codePages);

            if (!FontFactory.IsRegistered(name)) {
                FontFactory.Register(path, name);
            }
        }

        private float _Translate(float num)
        {
            return _Translate(num, _ScaleMode);
        }

        private static float _Translate(float num, ScaleModes scaleMode)
        {
            switch (scaleMode) {
                case ScaleModes.Points: {
                        return num;
                    }

                case ScaleModes.Hundredths: {
                        return num * 72f / 100f;
                    }

                case ScaleModes.Inches: {
                        return num * 72f;
                    }

                default: {
                        throw new ArgumentOutOfRangeException();
                        break;
                    }
            }
        }

        private float _TranslateRev(float num)
        {
            return _TranslateRev(num, ScaleMode);
        }

        private static float _TranslateRev(float num, ScaleModes scaleMode)
        {
            switch (scaleMode) {
                case ScaleModes.Points: {
                        return num;
                    }

                case ScaleModes.Hundredths: {
                        return num * 100f / 72f;
                    }

                case ScaleModes.Inches: {
                        return num / 72f;
                    }

                default: {
                        throw new ArgumentOutOfRangeException();
                        break;
                    }
            }
        }

        private static iTextSharp.text.Color _GetColor(System.Drawing.Color c)
        {
            return new iTextSharp.text.Color(c.R, c.G, c.B, c.A);
        }

        private iTextSharp.text.Font _GetFont(System.Drawing.Font font)
        {
            if (Font is null)
                throw new InvalidOperationException();
            iTextSharp.text.Font f;
            var s = default(int);
            if (font.Bold)
                s = s | iTextSharp.text.Font.BOLD;
            if (font.Italic)
                s = s | iTextSharp.text.Font.ITALIC;
            if (font.Underline)
                s = s | iTextSharp.text.Font.UNDERLINE;
            if (font.Strikeout)
                s = s | iTextSharp.text.Font.STRIKETHRU;

            //if font doesnt exist then throws error
            if (!FontFactory.IsRegistered(font.OriginalFontName)) {
                throw new ArgumentNullException($"{font.OriginalFontName} doesnt exist please register using RegisterFont");
                //var t = FontFactory.RegisteredFonts;
            }

            f = FontFactory.GetFont(font.OriginalFontName, BaseFont.CP1252, true, font.SizeInPoints, s, _GetColor(ForeColor));
            return f;
        }
    }

    public enum ScaleModes
    {
        Inches,
        Hundredths,
        Points
    }
}