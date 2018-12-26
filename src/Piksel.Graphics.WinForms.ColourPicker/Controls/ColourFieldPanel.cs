using Piksel.Graphics.ColourPicker.Utilities;
using Piksel.Graphics.ColourSpaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


// TODO: use Offset() to offset point coordinates instead of subtracting SAMPLE_CIRCLE_RADIUS.

namespace Piksel.Graphics.ColourPicker.Controls
{

    /// <summary>
    /// Responsible for managing all functionality related to the Colour field.
    /// </summary>
    [DesignTimeVisible(false)]
    public class ColourFieldPanel : System.Windows.Forms.UserControl
    {

        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Timer cursorTimer;

        private Bitmap currentColourBitmap;
        private bool isMouseDown = false;
        private bool mousePressedWithinPanel;
        private char component;
        private Colour Colour;
        private IColourSpaceControl ColourSpace;
        private int selectedComponentValue;
        private int movingPointX;
        private int movingPointY;
        private int targetPointX;
        private int targetPointY;
        private Point currentPoint;
        private Point tempPoint;

        // constants
        private const int SAMPLE_CIRCLE_RADIUS = 5;

        [Browsable(true)]
        [DefaultValue("Black")]
        [EditorBrowsable]
        [Category("Appearance")]
        public Color BorderColor { get; set; } = Color.Black;

        private int borderWidth = 1;
        private BorderStyle borderStyle;
        public new BorderStyle BorderStyle
        {
            get => borderStyle;
            set
            {
                borderStyle = value;
                borderWidth =
                    value == BorderStyle.Fixed3D ? 2 :
                    value == BorderStyle.FixedSingle ? 1
                    : 0;
            }
        }


        #region Events and delegates

        public delegate void ColourSelectedHandler(object sender, ColourSelectedEventArgs e);
        public event ColourSelectedHandler ColourSelected;

        #endregion Events and delegates

        public ColourFieldPanel()
        {


            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                UpdateColour(new HsbColourSpaceControl(), 'H', false);
            }

            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.UserPaint, true);
            cursorTimer.Enabled = false;



        }

        private void InvalidateNewAndOldRegions(Point oldPoint, Point newPoint)
        {

            Rectangle oldRegion = new Rectangle(
                oldPoint.X - SAMPLE_CIRCLE_RADIUS,
                oldPoint.Y - SAMPLE_CIRCLE_RADIUS,
                SAMPLE_CIRCLE_RADIUS * 2,
                SAMPLE_CIRCLE_RADIUS * 2);

            // give the region a little buffer
            oldRegion.Inflate(4, 4);
            this.Invalidate(oldRegion);

            Rectangle newRegion = new Rectangle(
                newPoint.X - SAMPLE_CIRCLE_RADIUS,
                newPoint.Y - SAMPLE_CIRCLE_RADIUS,
                SAMPLE_CIRCLE_RADIUS * 2,
                SAMPLE_CIRCLE_RADIUS * 2);

            // give the region a little buffer
            newRegion.Inflate(4, 4);
            this.Invalidate(newRegion);

        }

        protected override void OnLoad(EventArgs e)
        {

            base.OnLoad(e);
            UpdateCurrentColourBitmap(false);

        }

        protected override void OnMouseDown(MouseEventArgs e)
        {

            isMouseDown = true;
            mousePressedWithinPanel = true;

            Point newPoint = new Point(e.X, e.Y);
            this.InvalidateNewAndOldRegions(currentPoint, newPoint);
            currentPoint = newPoint;
            tempPoint = currentPoint;

            ColourSelected(this, new ColourSelectedEventArgs(CalculateSelectedColour(newPoint)));

        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            isMouseDown = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {

            base.OnMouseMove(e);

            if (isMouseDown)
            {

                if (e.X >= 0 && e.X <= 255 || e.Y >= 0 && e.Y <= 255)
                {
                    var x = e.X.ClampByte();
                    var y = e.Y.ClampByte();

                    movingPointX = currentPoint.X;
                    movingPointY = currentPoint.Y;
                    targetPointX = x;
                    targetPointY = y;
                    Point midPoint = new Point(x, y);

                    //InvalidateNewAndOldRegions( currentPoint, midPoint );
                    this.Invalidate();
                    currentPoint = midPoint;

                    if (!cursorTimer.Enabled)
                    {
                        cursorTimer.Start();
                    }

                    //				} else {
                    //
                }

            }

        }

        protected override void OnMouseEnter(EventArgs e)
            => Cursor = CustomCursor;

        private Cursor customCursor = null;
        private Cursor CustomCursor =>
            customCursor ?? (customCursor = new Cursor(Resources.GetFileResource("ColourFieldPanelCursor.cur", this)));

        protected override void OnMouseLeave(EventArgs e)
        {

            if (cursorTimer.Enabled)
            {
                cursorTimer.Stop();
            }

            Cursor.Show();
            this.ParentForm.Cursor = Cursors.Default;

        }

        protected override void OnPaint(PaintEventArgs e)
        {

            if (currentColourBitmap == null)
            {
                UpdateCurrentColourBitmap(false);
            }

            // make sure this painting occurs only during run time.
            if (true || !this.DesignMode)
            {

                var g = e.Graphics;

                var borderOffset = 256 + borderWidth;
                var borderRect = new Rectangle(0, 0, borderOffset, borderOffset);
                g.DrawRectangle(new Pen(BorderColor), borderRect);

                var fieldRect = new Rectangle(borderWidth, borderWidth, 256, 256);
                g.DrawImage(currentColourBitmap, fieldRect);

                int circ = 0; // circ = circumference

                //				if ( mousePressedWithinPanel ) {

                Colour c = currentColourBitmap.GetPixel(currentPoint.X, currentPoint.Y);

                Point circlePoint = new Point(
                    currentPoint.X - SAMPLE_CIRCLE_RADIUS,
                    currentPoint.Y - SAMPLE_CIRCLE_RADIUS);

                int average = (c.Red + c.Green + c.Blue) / 3;
                circ = SAMPLE_CIRCLE_RADIUS * 2; // c = circumference

                if (average > 175)
                {
                    g.DrawEllipse(Pens.Black, new Rectangle(circlePoint, new Size(circ, circ)));
                }
                else
                {
                    g.DrawEllipse(Pens.White, new Rectangle(circlePoint, new Size(circ, circ)));
                }

                //				}

            }

        }

        private void UpdateCurrentColourBitmap(bool resetPreviouslyPickedPoint)
        {

            if (!isMouseDown)
            {

                int x = 0;
                int y = 0;

                if (currentColourBitmap != null)
                {
                    currentColourBitmap.Dispose();
                }

                if (ColourSpace is HsbColourSpaceControl hsbCtrl)
                {

                    HSB hsb = hsbCtrl.Structure;

                    if (component == 'H')
                    {

                        Colour c = Colour;
                        if (c.Equals(Colour.FromArgb(0, 0, 0, 0)) | c.Equals(new Colour(0, 0, 0)))
                        {
                            c = new Colour(255, 0, 0);
                        }

                        currentColourBitmap = ColourRenderingHelper.GetHueColourField(c);

                        x = (int)Math.Round(hsb.Saturation * 2.55);
                        y = 255 - (int)Math.Round(hsb.Brightness * 2.55);

                    }
                    else if (component == 'S')
                    {

                        currentColourBitmap = ColourRenderingHelper.GetSaturationColourField(selectedComponentValue);

                        x = (int)Math.Ceiling(hsb.Hue * ((double)255 / 360));
                        y = (int)(255 - (hsb.Brightness * 2.55));

                    }
                    else if (component == 'B')
                    {

                        currentColourBitmap = ColourRenderingHelper.GetBrightnessColourField(selectedComponentValue);

                        x = (int)Math.Ceiling(hsb.Hue * ((double)255 / 360));
                        y = 255 - (int)Math.Round(hsb.Saturation * 2.55);

                    }
                    else
                    {
                        throw new Exception($"Unknown component '{component}'");
                    }

                }
                else if (ColourSpace is RgbColourSpaceControl rgb)
                {

                    var c = ColourSpace.GetColour();

                    if (component == 'R')
                    {

                        currentColourBitmap = ColourRenderingHelper.GetRedColourField(selectedComponentValue);

                        x = c.Blue;
                        y = 255 - c.Green;

                    }
                    else if (component == 'G')
                    {

                        currentColourBitmap = ColourRenderingHelper.GetGreenColourField(selectedComponentValue);

                        x = c.Blue;
                        y = 255 - c.Red;

                    }
                    else if (component == 'B')
                    {

                        currentColourBitmap = ColourRenderingHelper.GetBlueColourField(selectedComponentValue);

                        x = c.Red;
                        y = 255 - c.Green;

                    }

                }
                else
                {
                    throw new Exception($"Unknown Colourspace '{ColourSpace?.GetType().Name ?? "<null>"}'");
                }

                if (resetPreviouslyPickedPoint)
                {
                    currentPoint = new Point(x, y);
                }

            }

        }

        public void UpdateColour(IColourSpaceControl colourSpace, char component, Colour colour, bool resetPreviouslyPickedPoint)
        {

            Colour = colour;
            UpdateColour(colourSpace, component, resetPreviouslyPickedPoint);

        }

        public void UpdateColour(IColourSpaceControl colourSpace, char component, int selectedComponentValue, bool resetPreviouslyPickedPoint)
        {

            Colour = Color.Empty;
            this.selectedComponentValue = selectedComponentValue;
            UpdateColour(colourSpace, component, resetPreviouslyPickedPoint);

        }

        private void UpdateColour(IColourSpaceControl colourSpace, char component, bool resetPreviouslyPickedPoint)
        {

            ColourSpace = colourSpace;
            this.component = component;

            mousePressedWithinPanel = resetPreviouslyPickedPoint;
            this.UpdateCurrentColourBitmap(resetPreviouslyPickedPoint);
            this.Invalidate();

        }

        #region Dispose
        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        #endregion Dispose

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cursorTimer = new Timer(components)
            {
                Interval = 10
            };
            this.cursorTimer.Tick += new System.EventHandler(this.cursorTimer_Tick);
            // 
            // ColourFieldPanel
            // 
            this.Name = "ColourFieldPanel";
            this.Size = new System.Drawing.Size(256, 256);

        }
        #endregion

        private void cursorTimer_Tick(object sender, EventArgs e)
        {

            if (isMouseDown)
            {

                int diffX = movingPointX - targetPointX;
                int diffY = movingPointY - targetPointY;
                int nextXValue = (int)Math.Round((double)diffX / 2);
                int nextYValue = (int)Math.Round((double)diffY / 2);
                bool stopTimer = false;

                // X
                if (nextXValue == 0)
                {
                    movingPointX = targetPointX;
                    stopTimer = true;
                }
                else
                {
                    movingPointX += -nextXValue;
                }

                // Y
                if (nextYValue == 0)
                {
                    movingPointY = targetPointY;

                    if (stopTimer)
                    {
                        cursorTimer.Stop();
                    }

                }
                else
                {
                    movingPointY += -nextYValue;
                }

                ColourSelected?.Invoke(this, new ColourSelectedEventArgs(CalculateSelectedColour(new Point(movingPointX, movingPointY))));

            }

        }

        private Colour CalculateSelectedColour(Point p)
        {

            IColourSpace selectedColour = null;

            if (ColourSpace is HsbColourSpaceControl hsbCtrl)
            {

                HSB hsb = hsbCtrl.Structure;

                if (component == 'H')
                {

                    byte brightness = (byte)(((double)255 - p.Y) / 2.55);
                    byte saturation = (byte)((double)p.X / 2.55);

                    selectedColour = new HSB(hsb.Hue, saturation, brightness);

                }
                else if (component == 'S')
                {

                    ushort hue = (ushort)(p.X * ((double)360 / 255));
                    byte brightness = (byte)(((double)255 - p.Y) / 2.55);

                    if (hue == 360)
                    {
                        hue = 0;
                    }

                    selectedColour = new HSB(hue, hsb.Saturation, brightness);

                }
                else if (component == 'B')
                {

                    ushort hue = (ushort)(p.X * ((double)360 / 255));
                    byte saturation = (byte)(((double)255 - p.Y) / 2.55);

                    if (hue == 360)
                    {
                        hue = 0;
                    }

                    selectedColour = new HSB(hue, saturation, hsb.Brightness);

                }

            }
            else if (ColourSpace is RgbColourSpaceControl rgbCtrl)
            {

                var rgb = rgbCtrl.Structure;

                if (component == 'R')
                {
                    selectedColour = new RGB(rgb.Red, (byte)(255 - p.Y), (byte)(p.X));
                }
                else if (component == 'G')
                {
                    selectedColour = new RGB((byte)(255 - p.Y), rgb.Green, (byte)(p.X));
                }
                else if (component == 'B')
                {
                    selectedColour = new RGB((byte)(p.X), (byte)(255 - p.Y), rgb.Blue);
                }

            }

            RGB crgb = selectedColour is HSB hsbSelectedColour
                ? ColourConverter.HsbToRgb(hsbSelectedColour)
                : (RGB)selectedColour;

            return ColourConverter.RgbToColour(crgb);

        }

    }

}
