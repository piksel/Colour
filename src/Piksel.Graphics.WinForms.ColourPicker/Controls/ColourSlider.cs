using Piksel.Graphics.ColourSpaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Piksel.Graphics.ColourPicker.Controls
{
    using Piksel.Graphics.ColourPicker.Utilities;
    using static Piksel.Graphics.ColourPicker.Utilities.StaticFunctions;
    using GDIGraphics = System.Drawing.Graphics;

    public class ColourSlider : Control, ISupportInitialize
    {
        private Image backBitmap;
        private bool _needsUpdate;
        private bool _initializing;
        private readonly bool _sizeChanged = true;
        private bool mouseDown = false;

        private Rectangle bitmapRegion;
        private Rectangle borderRect;
        private int cursorWidth = 8;
        private int cursorHeight = 6;
        private int arrowYLocation;
        private Rectangle leftArrowRegion;
        private Rectangle rightArrowRegion;


        public event EventHandler ValueChanged;
        public virtual void OnValueChanged()
            => ValueChanged?.Invoke(this, EventArgs.Empty);

        public int CursorWidth
        {
            get => cursorWidth;
            set => cursorWidth = value;
        }

        public int CursorHeight
        {
            get => cursorHeight;
            set => cursorHeight = value;
        }

        private IColourSpaceControl _colourSpace;
        public IColourSpaceControl ColourSpace
        {
            get => _colourSpace;
            set
            {
                if (value == null) return;
                _colourSpace = value;
                Value = value.SelectedComponent.GetSliderValue();
            }
        }

        private double _value = 1;
        public double Value
        {
            get => _value;
            set
            {
                _value = value;
                arrowYLocation = (int)(Math.Ceiling((1 - value) * (backBitmap.Height - 1)));
            }
        }

        public ColourSlider()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }

        private void UpdateRegion()
        {
            bitmapRegion = new Rectangle(cursorWidth + 1, (cursorHeight / 2), Width - ((cursorWidth + 1) * 2), Height - (cursorHeight));
            borderRect = new Rectangle(bitmapRegion.X - 1, bitmapRegion.Y - 1, bitmapRegion.Width + 1, bitmapRegion.Height + 1);
            backBitmap = new Bitmap(bitmapRegion.Width, bitmapRegion.Height);
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);
            _needsUpdate = true;
            UpdateRegion();
        }

        readonly Color cTest = Color.Magenta;

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_initializing) return;

            if (_needsUpdate) GenerateBitmap();

            //e.Graphics.FillRectangle(new SolidBrush(cTest), e.ClipRectangle);
            //e.Graphics.FillRectangle(Brushes.Cyan, bitmapRegion);

            e.Graphics.DrawImage(backBitmap, bitmapRegion);


            Point[] leftTriangle = CreateLeftTrianglePointer(arrowYLocation + bitmapRegion.Y);
            Point[] rightTriangle = CreateRightTrianglePointer(arrowYLocation + bitmapRegion.Y);

            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.DrawPolygon(Pens.Black, leftTriangle);
            e.Graphics.DrawPolygon(Pens.Black, rightTriangle);

            if (_sizeChanged)
            {
                //_sizeChanged = false;
                e.Graphics.DrawRectangle(SystemPens.ControlDarkDark, borderRect);
            }
        }

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            //base.OnInvalidated(e);
        }

        private void GenerateBitmap()
        {
            var region = new Rectangle(Point.Empty, backBitmap.Size);
            using (var g = GDIGraphics.FromImage(backBitmap))
            {
                var cs = ColourSpace;
                if (cs is RgbColourSpaceControl csRgb)
                {

                    var rgb = csRgb.NativeStructure;
                    char dChar = cs.SelectedComponent.DisplayCharacter;
                    int red = rgb.Red;
                    int green = rgb.Green;
                    int blue = rgb.Blue;

                    Colour startColour;
                    Colour endColour;

                    switch (dChar)
                    {

                        case 'R':
                            startColour = Color.FromArgb(0, green, blue);
                            endColour = Color.FromArgb(255, green, blue);
                            break;

                        case 'G':
                            startColour = Color.FromArgb(red, 0, blue);
                            endColour = Color.FromArgb(red, 255, blue);
                            break;

                        default:
                            startColour = Color.FromArgb(red, green, 0);
                            endColour = Color.FromArgb(red, green, 255);
                            break;

                    }

                    using (LinearGradientBrush lgb = new LinearGradientBrush(new Rectangle(region.X, region.Y - 1, region.Width, region.Height), startColour, endColour, 270f))
                    {
                        g.FillRectangle(lgb, region);
                    }

                }
                else if (cs is HsbColourSpaceControl csHsb)
                {

                    var hsb = (HSB)csHsb.Structure;

                    if (csHsb.SelectedComponent.DisplayCharacter == 'H')
                    {

                        Rectangle rect = new Rectangle(0, 0, 20, 256);

                        using (LinearGradientBrush brBrush = new LinearGradientBrush(rect, Color.Blue, Color.Red, 90f, false))
                        {

                            Color[] ColourArray = { Color.Red, Color.Magenta, Color.Blue, Color.Cyan, Color.Lime, Color.Yellow, Color.Red };
                            float[] posArray = { 0.0f, 0.1667f, 0.3372f, 0.502f, 0.6686f, 0.8313f, 1.0f };

                            var ColourBlend = new ColorBlend()
                            {
                                Colors = ColourArray,
                                Positions = posArray
                            };
                            brBrush.InterpolationColors = ColourBlend;

                            g.FillRectangle(brBrush, region);

                        }

                    }
                    else if (csHsb.SelectedComponent.DisplayCharacter == 'B')
                    {

                        RGB sRgb = ColourConverter.HsbToRgb(new HSB(hsb.Hue, hsb.Saturation, 100));
                        RGB eRgb = ColourConverter.HsbToRgb(new HSB(hsb.Hue, hsb.Saturation, 0));

                        using (LinearGradientBrush lgb = new LinearGradientBrush(region,
                            Color.FromArgb(sRgb.Red, sRgb.Green, sRgb.Blue),
                            Color.FromArgb(eRgb.Red, eRgb.Green, eRgb.Blue), 90f))
                        {
                            g.FillRectangle(lgb, region);
                        }

                    }
                    else
                    {

                        RGB sRgb = ColourConverter.HsbToRgb(new HSB(hsb.Hue, 100, hsb.Brightness));
                        RGB eRgb = ColourConverter.HsbToRgb(new HSB(hsb.Hue, 0, hsb.Brightness));

                        using (LinearGradientBrush lgb = new LinearGradientBrush(region,
                            Color.FromArgb(sRgb.Red, sRgb.Green, sRgb.Blue),
                            Color.FromArgb(eRgb.Red, eRgb.Green, eRgb.Blue), 90f))
                        {
                            g.FillRectangle(lgb, region);
                        }

                    }

                }
                else
                {
                    g.FillRectangle(Brushes.Cyan, region);
                    g.DrawString(cs.GetType().Name, DefaultFont, Brushes.Black, Point.Empty);
                }

            }
        }

        public void UpdateColourSliderArrowRegions()
        {

            InvalidateColourSliderArrowRegions();



            // TODO: We should probably clear the old arrows here
            UpdateArrowInvalidationRegions();

            InvalidateColourSliderArrowRegions();

        }


        private void UpdateArrowInvalidationRegions()
        {
            var halfArrow = cursorHeight / 2;

            rightArrowRegion = new Rectangle(
                backBitmap.Width + cursorWidth,
                arrowYLocation - (halfArrow) - 1,
                cursorWidth + 2,
                cursorHeight + 3);

            leftArrowRegion = new Rectangle(new Point(0, rightArrowRegion.Y), rightArrowRegion.Size);

        }

        /// <summary>
        /// Invalidate the Colour slider arrow regions.
        /// </summary>

        private void InvalidateColourSliderArrowRegions()
        {

            this.Invalidate(leftArrowRegion);
            this.Invalidate(rightArrowRegion);

        }

        private Point[] CreateLeftTrianglePointer(int y)
            => new[] {
                new Point( 0, y - ( cursorHeight / 2 ) ),
                new Point( cursorWidth, y ),
                new Point( 0, y + ( cursorHeight / 2 ) )
            };

        private Point[] CreateRightTrianglePointer(int y)
            => new[] {
                new Point( Width - 1, y - ( cursorHeight / 2 ) ),
                new Point( Width - cursorWidth - 1, y ),
                new Point( Width - 1, y + ( cursorHeight / 2 ) )
            };

        internal Colour GetPixel(int y)
            => (backBitmap as Bitmap).GetPixel(backBitmap.Width / 2, y);

        public Colour GetPixel()
        {
            return GetPixel(arrowYLocation);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            mouseDown = true;

            SelectedValueChanged(e.Y);

            base.OnMouseDown(e);
        }

        private void SelectedValueChanged(int y)
        {
            var yVal = (y - bitmapRegion.Y).Clamp(0, bitmapRegion.Height - 1);

            arrowYLocation = yVal;

            UpdateValue();
            Invalidate();
            OnValueChanged();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (mouseDown)
            {
                SelectedValueChanged(e.Y);
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void UpdateValue()
        {
            _value = 1 - ((double)(arrowYLocation) / bitmapRegion.Height);
            Clamp(ref _value, 0, 1);
        }



        public void BeginInit()
        {
            _initializing = true;
        }

        public void EndInit()
        {
            if (this.IsInDesignMode())
            {
                var previewSpace = new HsbColourSpaceControl();
                previewSpace.SetDefaultSelection();
                _colourSpace = previewSpace;
            }
            //this.Top -= bitmapRegion.Y;
            //this.Height += (bitmapRegion.Y * 2);
            _initializing = false;
            Value = _value;
        }

        protected override void Dispose(bool disposing)
        {
            if (backBitmap != null)
            {
                backBitmap.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}