using iTextSharp.text;
using iTextSharp.text.pdf;

namespace iTextEasyCS
{
    partial class PDFWriter
    {
        public PictureAlignment PictureAlignment;

        public void PaintPicture(iTextSharp.text.Image img)
        {
            if (img.DpiX == 0 | img.DpiY == 0) {
                PaintPictureAbs(img, _Translate(img.Width / 96, ScaleModes.Inches), _Translate(img.Height / 96, ScaleModes.Inches));
            } else {
                PaintPictureAbs(img, _Translate(img.Width / img.DpiX, ScaleModes.Inches), _Translate(img.Height / img.DpiY, ScaleModes.Inches));
            }
        }

        public void PaintPicture(iTextSharp.text.Image img, float width, float height)
        {
            PaintPictureAbs(img, _Translate(width), _Translate(height));
        }

        public void PaintPicture(iTextSharp.text.Image img, bool step, float X, float Y)
        {
            if (step) {
                _CurrentX += _Translate(X);
                _CurrentY += _Translate(Y);
            } else {
                _CurrentX = _Translate(X);
                _CurrentY = _Translate(Y);
            }

            PaintPicture(img);
        }

        public void PaintPicture(iTextSharp.text.Image img, bool step, float X, float Y, float width, float height)
        {
            if (step) {
                _CurrentX += _Translate(X);
                _CurrentY += _Translate(Y);
            } else {
                _CurrentX = _Translate(X);
                _CurrentY = _Translate(Y);
            }

            PaintPicture(img, width, height);
        }

        private void PaintPictureAbs(iTextSharp.text.Image img, float width, float height)
        {
            FinishLine();
            float OffsetX = default, OffsetY = default;
            switch (PictureAlignment) {
                case var @case when @case == PictureAlignment.LeftTop:
                case var case1 when case1 == PictureAlignment.LeftCenter:
                case var case2 when case2 == PictureAlignment.LeftBottom: {
                        break;
                    }

                case var case3 when case3 == PictureAlignment.CenterTop:
                case var case4 when case4 == PictureAlignment.CenterCenter:
                case var case5 when case5 == PictureAlignment.CenterBottom: {
                        OffsetX -= width / 2f;
                        break;
                    }

                case var case6 when case6 == PictureAlignment.RightTop:
                case var case7 when case7 == PictureAlignment.RightCenter:
                case var case8 when case8 == PictureAlignment.RightBottom: {
                        OffsetX -= width;
                        break;
                    }
            }

            switch (PictureAlignment) {
                case var case9 when case9 == PictureAlignment.LeftTop:
                case var case10 when case10 == PictureAlignment.CenterTop:
                case var case11 when case11 == PictureAlignment.RightTop: {
                        OffsetY += height;
                        break;
                    }

                case var case12 when case12 == PictureAlignment.LeftCenter:
                case var case13 when case13 == PictureAlignment.CenterCenter:
                case var case14 when case14 == PictureAlignment.RightCenter: {
                        OffsetY += height / 2f;
                        break;
                    }

                case var case15 when case15 == PictureAlignment.LeftBottom:
                case var case16 when case16 == PictureAlignment.CenterBottom:
                case var case17 when case17 == PictureAlignment.RightBottom: {
                        break;
                    }
            }

            _Content.AddImage(img, width, 0, 0, -height, _CurrentX + OffsetX, _CurrentY + OffsetY);
        }
    }
}
