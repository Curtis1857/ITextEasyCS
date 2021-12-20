using System;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;

// arc
namespace iTextEasyCS
{

    partial class PDFWriter
    {
        private float _CurrentX = 0f;
        private float _CurrentY = 0f;
        private float _LineWidth = 0.1f;
        private LineCapStyle _LineCap = LineCapStyle.None;
        private LineJoinStyle _LineJoin = LineJoinStyle.Miter;
        private LineDashStyle _LineDash = LineDashStyle.Solid;
        private bool _InLine;
        private System.Drawing.Color _ForeColor = System.Drawing.Color.Black;
        private System.Drawing.Color _FillColor = System.Drawing.Color.Black;

        private void _InitLineVars()
        {
            ForeColor = _ForeColor;
            FillColor = _FillColor;
            LineCap = _LineCap;
            LineJoin = _LineJoin;
            LineDash = _LineDash;
            LineWidth = LineWidth;
        }

        public void FinishLine()
        {
            if (_InLine) {
                try {
                    _Content.Stroke();
                } finally {
                    _InLine = false;
                }
            }
        }

        public void SetLineWidth(float width)
        {
            
            _Content.SetLineWidth(width);
        }

        public void FinishPolygon(bool border, bool fill, bool eoFill = false)
        {
            if (_InLine) {
                try {
                    if (border) {
                        if (eoFill) {
                            _Content.ClosePathEoFillStroke();
                        } else if (fill) {
                            _Content.ClosePathFillStroke();
                        } else {
                            _Content.ClosePathStroke();
                        }
                    } else if (eoFill) {
                        _Content.EoFill();
                    } else if (fill) {
                        _Content.Fill();
                    } else {
                        _Content.NewPath();
                    }
                } finally {
                    _InLine = false;
                }
            }
        }

        public System.Drawing.Color ForeColor {
            get {
                return _ForeColor;
            }

            set {
                FinishLine();
                _ForeColor = value;
                if (_Content is object)
                    _Content.SetColorStroke(_GetColor(value));
            }
        }

        public System.Drawing.Color FillColor {
            get {
                return _FillColor;
            }

            set {
                FinishLine();
                _FillColor = value;
                if (_Content is object)
                    _Content.SetColorFill(new iTextSharp.text.Color(value.R, value.G, value.B, value.A));
            }
        }

        public LineCapStyle LineCap {
            get {
                return _LineCap;
            }

            set {
                FinishLine();
                switch (value) {
                    case var @case when @case == LineCapStyle.None:
                    case var case1 when case1 == LineCapStyle.Round:
                    case var case2 when case2 == LineCapStyle.Square: {
                            _LineCap = value;
                            if (_Content is object)
                                _Content.SetLineCap((int)_LineCap);
                            break;
                        }

                    default: {
                            throw new ArgumentOutOfRangeException();
                            break;
                        }
                }
            }
        }

        public LineJoinStyle LineJoin {
            get {
                return _LineJoin;
            }

            set {
                FinishLine();
                switch (value) {
                    case var @case when @case == LineJoinStyle.Bevel:
                    case var case1 when case1 == LineJoinStyle.Miter:
                    case var case2 when case2 == LineJoinStyle.Rounded: {
                            _LineJoin = value;
                            if (_Content is object)
                                _Content.SetLineJoin((int)_LineJoin);
                            break;
                        }

                    default: {
                            throw new ArgumentOutOfRangeException();
                            break;
                        }
                }
            }
        }

        public LineDashStyle LineDash {
            get {
                return _LineDash;
            }

            set {
                if (value is null)
                    throw new ArgumentNullException();
                FinishLine();
                _LineDash = value;
                if (_Content is object)
                    _Content.SetLineDash(_LineDash.MultipliedArray(_LineWidth), _LineDash.MultipliedPhase(_LineWidth));
            }
        }

        public float CurrentX {
            get {
                return _TranslateRev(_CurrentX);
            }

            set {
                FinishLine();
                _CurrentX = _Translate(value);
            }
        }

        public float CurrentY {
            get {
                return _TranslateRev(_CurrentY);
            }

            set {
                FinishLine();
                _CurrentY = _Translate(value);
            }
        }

        public float LineWidth {
            get {
                return _TranslateRev(_LineWidth);
            }

            set {
                FinishLine();
                _LineWidth = _Translate(value);
                if (_Content is object) {
                    _Content.SetLineWidth(_LineWidth);
                    _Content.SetLineDash(_LineDash.MultipliedArray(_LineWidth), _LineDash.MultipliedPhase(_LineWidth));
                }
            }
        }

        public PointF Pos {
            get {
                return new PointF(_TranslateRev(_CurrentX), _TranslateRev(_CurrentY));
            }

            set {
                FinishLine();
                _CurrentX = _Translate(value.X);
                _CurrentY = _Translate(value.Y);
            }
        }

        public void MoveTo(bool step, float X, float Y)
        {
            FinishLine();
            if (step) {
                _CurrentX += _Translate(X);
                _CurrentY += _Translate(Y);
            } else {
                _CurrentX = _Translate(X);

                _CurrentY = _Translate(Y);
            }
        }

        public void SetLineCap(int cap)
        {
            _Content.SetLineCap(cap);
        }

        public void LineTo(bool step, float X, float Y)
        {
            X = _Translate(X);
            Y = _Translate(Y);
            if (step) {
                X += _CurrentX;
                Y += _CurrentY;
            }

            if (!_InLine)
                _Content.MoveTo(_CurrentX, _CurrentY);
            _Content.LineTo(X, Y);
            _InLine = true;
            _CurrentX = X;
            _CurrentY = Y;
        }

        public void Line(bool step1, float X1, float Y1, bool step2, float X2, float Y2)
        {
            FinishLine();
            MoveTo(step1, X1, Y1);
            LineTo(step2, X2, Y2);
        }

        public void RectangleDualOffSet(bool step1, float X1, float Y1, bool step2, float X2, float Y2, float offSet, float R = 0f, bool Fill = false, bool Border = true)
        {
            Rectangle(step1, X1, Y1, step2, X2, Y2);
            Rectangle(step1, X1 + offSet, Y1 + offSet, step2, X2 - (offSet * 2), Y2 - (offSet * 2));
        }

        public void Rectangle(bool step1, float X1, float Y1, bool step2, float X2, float Y2, float R = 0f, bool Fill = false, bool Border = true)
        {
            FinishLine();
            X1 = _Translate(X1);
            Y1 = _Translate(Y1);
            if (step1) {
                X1 += _CurrentX;
                Y1 += _CurrentY;
            }

            X2 = _Translate(X2);
            Y2 = _Translate(Y2);
            if (step2) {
                X2 += X1;
                Y2 += Y1;
            }

            _CurrentX = _TranslateRev(X2);
            _CurrentY = _TranslateRev(Y2);
            if (!(Fill | Border))
                return;
            if (R == 0f) {
                //_Content.Rectangle(X1, X2, X2 - X1, Y2 - Y1);
                _Content.Rectangle(X1, Y1, X2 - X1, Y2 - Y1);
            } else {
                //_Content.RoundRectangle(X1, X2, X2 - X1, Y2 - Y1, _Translate(R));
                _Content.RoundRectangle(X1, Y1, X2 - X1, Y2 - Y1, _Translate(R));
            }

            if (Border) {
                if (Fill)
                    _Content.FillStroke();
                else
                    _Content.Stroke();
            } else {
                // If Fill Then _Content.Fill() Else _Content.NewPath() 'redundant; Fill will always be True
                _Content.Fill();
            }
        }
        // Public Sub Arc(ByVal X1!, ByVal Y1!, ByVal X2!, ByVal Y2!, ByVal startAng!, ByVal extent!)
        // _Content.Arc(_Translate(X1), _Translate(Y1), _Translate(X2), _Translate(Y2), startAng, extent)
        // End Sub

        // 0.5522847498307936f = (float)(4d * (Math.Pow(2d, 0.5d) - 1d) / 3d);
        public void CornerTo(bool step, float X, float Y, bool FromSide, float bulge = 0.5522847498307936f)
        {
            CornerTo(step, X, Y, FromSide, bulge, bulge);
        }

        public void CornerTo(bool step, float X, float Y, bool FromSide, float bulgeHorizontal, float bulgeVertical)
        {
            X = _Translate(X);
            Y = _Translate(Y);
            if (step) {
                X += _CurrentX;
                Y += _CurrentY;
            }

            if (!_InLine)
                _Content.MoveTo(_CurrentX, _CurrentY);
            if (FromSide) {
                _Content.CurveTo(_CurrentX, (Y - _CurrentY) * bulgeVertical + _CurrentY, X - (X - _CurrentX) * bulgeHorizontal, Y, X, Y);
            } else {
                _Content.CurveTo((X - _CurrentX) * bulgeHorizontal + _CurrentX, _CurrentY, X, Y - (Y - _CurrentY) * bulgeVertical, X, Y);
            }

            _InLine = true;
            _CurrentX = X;
            _CurrentY = Y;
        }

        public void BezierTo(float X2, float Y2, float X3, float Y3, float X4, float Y4)
        {
            if (!_InLine)
                _Content.MoveTo(_CurrentX, _CurrentY);
            _Content.CurveTo(_Translate(X2), _Translate(Y2), _Translate(X3), _Translate(Y3), _Translate(X4), _Translate(Y4));
            _InLine = true;
            _CurrentX = X4;
            _CurrentY = Y4;
        }

        public void BezierTo(float X2, float Y2, float X4, float Y4)
        {
            if (!_InLine)
                _Content.MoveTo(_CurrentX, _CurrentY);
            _Content.CurveTo(_Translate(X2), _Translate(Y2), _Translate(X4), _Translate(Y4));
            _InLine = true;
            _CurrentX = X4;
            _CurrentY = Y4;
        }

        public void Circle(bool step, float X, float Y, float R, bool border = true, bool fill = false)
        {
            FinishLine();
            if (!(border | fill))
                return;
            X = _Translate(X);
            Y = _Translate(Y);
            if (step) {
                X += _CurrentX;
                Y += _CurrentY;
            }

            _CurrentX = X;
            _CurrentY = Y;
            _Content.Circle(X, Y, _Translate(R));
            if (border) {
                if (fill)
                    _Content.FillStroke();
                else
                    _Content.Stroke();
            } else {
                // If Fill Then _Content.Fill() Else _Content.NewPath() 'redundant; Fill will always be True
                _Content.Fill();
            }
        }
    }
}
