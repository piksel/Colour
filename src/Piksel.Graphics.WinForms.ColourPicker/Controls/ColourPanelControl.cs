using Piksel.Graphics.ColourPicker.Designer;
using Piksel.Graphics.ColourPicker.Utilities;
using Piksel.Graphics.ColourSpaces;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Piksel.Graphics.ColourPicker.Controls
{

    [ComVisible(false)]
    public class ColourPanelControl : UserControl
    {
        [Browsable(true)]
        //[TypeConverter(typeof(ColourTypeConverter))]
        [Editor(typeof(ColourComponentEditor), typeof(UITypeEditor))]
        public Colour Value
        {
            get => rgbColourSpace.GetColour();
            set
            {
                ColourSelectedInternal(value, true);
                pbOriginalColour.BackColor = value;
            }
        }

        IColourSpaceControl defaultColourSpace;
        public IColourSpaceControl DefaultColourSpace
        {
            get => defaultColourSpace;
            set
            {
                value.SetDefaultSelection();
                defaultColourSpace = value;
            }
        }

        private IContainer components;

        // controls
        private ColourFieldPanel ColourFieldPanel;
        private HexTextBox hexTextBox;
        private HsbColourSpaceControl hsbColourSpace;
        private Label hexLabel;
        private PictureBox pbCurrentColour;
        private ColourSlider picColourSlider;
        private RgbColourSpaceControl rgbColourSpace;
        private System.Windows.Forms.Timer sliderTimer;

        // member fields
        private bool isLeftMouseButtonDown;
        private bool isLeftMouseButtonDownAndMoving;

        private int currentYValue;
        private int targetYValue;
        private double fOldValue;
        //private Rectangle selectedColourRegion;
        //private Rectangle selectedColourColourRegion;

        private IColourSpaceControl currentColourSpace;
        private BitVector32 panelState;

        // rectangles for drawing.
        private Rectangle ColourSliderOuterRegion;
        private Rectangle ColourSliderInnerRegion;
        private readonly Rectangle ColourFieldOuterRegion = new Rectangle(12, 4, 265, 265);
        private readonly Rectangle ColourFieldInnerRegion = new Rectangle(15, 7, 257, 257);
        private readonly Rectangle valueFieldOuterRegion = new Rectangle(295, 4, 27, 265);
        private readonly Rectangle valueFieldInnerRegion = new Rectangle(298, 7, 19, 257);
        private readonly Rectangle swatchRegion = new Rectangle(464, 4, 164, 263);

        // constants
        private const int PANELSTATE_isLeftMouseDown = 0x1;                             // 1
        private const int PANELSTATE_isLeftMouseDownAndMoving = 0x8;                    // 8
        private const int ARROW_HEIGHT = 10;
        private PictureBox pbOriginalColour;
        private const int ARROW_WIDTH = 6;

        /// <summary>
        /// Constructor. Initializes all of the components and member fields 
        /// and configures the control for double buffering support.
        /// </summary>

        public ColourPanelControl()
        {

            InitializeComponent();


            if (defaultColourSpace == null)
            {
                DefaultColourSpace = hsbColourSpace;
            }

            ColourSliderInnerRegion = new Rectangle(picColourSlider.Location, picColourSlider.Size);
            ColourSliderOuterRegion = new Rectangle(
                ColourSliderInnerRegion.X - ARROW_WIDTH,
                ColourSliderInnerRegion.Y,
                ColourSliderInnerRegion.Width + (ARROW_WIDTH * 2),
                ColourSliderInnerRegion.Height);



            //selectedColourRegion = new Rectangle( 344, 8, 88, 40 );
            //selectedColourColourRegion = new Rectangle( 346, 10, 85, 37 );

            panelState = new BitVector32(0);

            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint, true);

            AllowDrop = true;

        }

        /// <summary>
        /// Returns the hex value of the Colours in the RGB Colour space.
        /// </summary>

        private string HexValue => rgbColourSpace.ConvertToHex();

        /// <summary>
        /// Overrides the base class' OnLoad method and instantiates a new
        /// DragForm object that will be used to create the visual drag effect
        /// when adding the currently selected Colour to the Colour swatch panel.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>

        protected override void OnLoad(EventArgs e)
        {
            DragForm.Initialize();

        }

        /// <summary>
        /// Overrides the panel's OnPaint method to performs all of the painting 
        /// operations.
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data.</param>

        protected override void OnPaint(PaintEventArgs e)
        {

            base.OnPaint(e);

            // using defines a scope at the end of which the graphics object is disposed.
            using (var g = e.Graphics)
            {



                //ControlPaint.DrawBorder3D( g, ColourFieldInnerRegion);
                //g.DrawRectangle(SystemPens.ControlDarkDark, ColourFieldInnerRegion);

                //ControlPaint.DrawBorder3D( g, valueFieldOuterRegion);
                //g.DrawRectangle(SystemPens.InactiveBorder, valueFieldInnerRegion );
                //g.DrawRectangle(Pens.Red, ColourSliderOuterRegion );

                ControlPaint.DrawBorder3D(g, new Rectangle(
                    pbCurrentColour.Location.X - 1,
                    pbCurrentColour.Location.Y - 1,
                    pbCurrentColour.Width + 2,
                    pbCurrentColour.Height + 2));

            }

        }


        private void SliderDown(int y)
        {
            isLeftMouseButtonDown = true;

            CheckCursorYRegion(y, true);

            UpdateColourPanels(false, false, true);
        }

        private void SliderMove(int y)
        {
            if (isLeftMouseButtonDown)
            {
                isLeftMouseButtonDownAndMoving = true;
                CheckCursorYRegion(y);
            }
        }

        private void SliderUp()
        {
            if (isLeftMouseButtonDown)
            {
                UpdateColourField(false);
            }

            isLeftMouseButtonDown = false;
            isLeftMouseButtonDownAndMoving = false;
        }

        #region Overridden methods

        protected override void OnMouseMove(MouseEventArgs e)
        {

            SliderMove(e.Y);

            base.OnMouseMove(e);

        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left && ColourSliderOuterRegion.Contains(e.Location))
            {
                SliderDown(e.Y);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {

            base.OnMouseUp(e);

            SliderUp();

        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (e.Delta != 0 && ColourSliderOuterRegion.Contains(e.Location))
            {
                var amount = ModifierKeys.HasFlag(Keys.Shift) ? 12 : 120;

                //CheckCursorYRegion(currentColourSliderArrowYLocation + (e.Delta / amount), true);

            }
        }



        #endregion Overridden methods

        /// <summary>
        /// Handles the MouseDown event raised by the picColourSlider object.
        /// When the user clicks on the Colour slider, the arrow regions are
        /// updated to assume the clicked y-coordinate as their new
        /// vertical position.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">A MouseEventArgs containing event data.</param>

        private void PicColourSlider_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SliderDown(e.Y + picColourSlider.Top);
            }
        }

        private void PicColourSlider_MouseMove(object sender, MouseEventArgs e)
        {
            SliderMove(picColourSlider.Top + e.Y);
        }

        private void PicColourSlider_MouseUp(object sender, MouseEventArgs e)
        {
            SliderUp();
        }

        /// <summary>
        /// Handles the SelectedColourSpaceComponentChanged event raised by the
        /// Colour spaces. When this occurs, the Colour slider arrow regions and 
        /// the Colour panels are updated.
        /// </summary>
        /// <param name="sender">The ColourSpace object that raised the event.</param>
        /// <param name="e">An EventArgs containing the event data.</param>

        private void SelectedColourSpaceComponentChanged(IColourSpaceControl sender, EventArgs e)
        {

            if (sender is RgbColourSpaceControl)
            {
                hsbColourSpace.ResetComponents();
            }
            else if (sender is HsbColourSpaceControl)
            {
                rgbColourSpace.ResetComponents();
            }

            currentColourSpace = sender;

            UpdateColourPanels(true, true, true);

        }

        /// <summary>
        /// Handles the ComponentValueChanged event that the ColourSpace raises 
        /// when the value of one of its components is changed by way of a
        /// keyboard user input. The Colour spaces are synced up and the Colour
        /// panels updated.
        /// </summary>
        /// <param name="sender">The ColourSpace object that raised the event.</param>
        /// <param name="e">An EventArgs object containing the event data.</param>

        private void ColourSpaceComponentValueChanged(IColourSpaceControl sender, EventArgs e)
        {
            UpdateSecondaryColourSpace(sender);

            UpdateColourPanels(true, true, true);

        }

        /// <summary>
        /// Handles the Tick event raised by the sliderTimer object. This is
        /// used to guarantee fluidity between two different points on the 
        /// Colour slider when the arrows are being dragged. Without this, the
        /// currently selected Colour panel and the updated structure values
        /// (RGB/HEX) jump awkwardly between points.
        /// 
        /// This will probably be changed to a threaded solution since FXCop
        /// is complaining about the tick frequency.
        /// </summary>
        /// <param name="sender">The object that raised this event.</param>
        /// <param name="e">An EventArgs containing the event data.</param>

        private void SliderTimer_Tick(object sender, EventArgs e)
        {

            int difference = Math.Abs(currentYValue - targetYValue);
            int nextValue = (int)Math.Round((double)difference / 2);

            if (nextValue == 0)
            {

                currentYValue = targetYValue;
                sliderTimer.Stop();

            }
            else
            {

                if (currentYValue < targetYValue)
                {
                    currentYValue += nextValue;
                }
                else
                {
                    currentYValue += -nextValue;
                }

            }

            UpdateSecondaryColourSpace(currentColourSpace);

            UpdateColourPanels(false, false, true);

        }

        private void UpdateSecondaryColourSpace(IColourSpaceControl primary)
        {
            if (primary is RgbColourSpaceControl)
            {
                hsbColourSpace.Structure = ColourConverter.RgbToHsb(rgbColourSpace.Structure);
            }
            else if (primary is HsbColourSpaceControl)
            {
                rgbColourSpace.Structure = ColourConverter.HsbToRgb(hsbColourSpace.Structure);
            }
        }

        /// <summary>
        /// Processes the selected Colour.
        /// </summary>
        /// <param name="Colour">A Colour object containing the Colour that was
        /// recently selected.</param>

        private void ColourSelectedInternal(Colour Colour, bool updateSliderPosition)
        {

            // make sure the Colour that was just clicked isn't the Colour that
            // is currently displayed (performance enhancement).

            if (!ColourConverter.ColourToRgb(Colour).Equals(rgbColourSpace.Structure))
            {

                RGB rgb = ColourConverter.ColourToRgb(Colour);

                rgbColourSpace.Structure = rgb;
                hsbColourSpace.Structure = ColourConverter.RgbToHsb(rgb);

                if (updateSliderPosition)
                {
                    picColourSlider.UpdateColourSliderArrowRegions();
                }

                UpdateColourPanels(true, true, true);

            }

        }

        private void ColourSwatchPanel_ColourSwatchSelected(object sender, ColourSelectedEventArgs e)
        {
            ColourSelectedInternal(e.Colour, true);
        }

        private void ColourFieldPanel_ColourSelected(object sender, ColourSelectedEventArgs e)
        {
            ColourSelectedInternal(e.Colour, false);
        }

        #region Events and delegates

        public delegate void ColourSelectedHandler(object sender, ColourSelectedEventArgs e);
        public event ColourSelectedHandler ColourSelected;

        #endregion Events and delegates

        #region Helper methods

        private void CheckCursorYRegion(int y, bool force = false)
        {
#if false

            int mValue = y;

            if (force || (isLeftMouseButtonDown && !isLeftMouseButtonDownAndMoving))
            {

                if (y < ColourSliderInnerRegion.Top || y >= ColourSliderInnerRegion.Bottom)
                {
                    return;
                }

            }
            else if (isLeftMouseButtonDownAndMoving)
            {

                if (y < ColourSliderInnerRegion.Top)
                {
                    mValue = ColourSliderInnerRegion.Top;
                }
                else if (y >= ColourSliderInnerRegion.Bottom)
                {
                    mValue = ColourSliderInnerRegion.Bottom - 1;
                }

            }
            else
            {
                return;
            }

            picColourSlider.Value = mValue;

            //ValueChanged( 255 - ( mValue - ColourSliderInnerRegion.Top ) , force );
#endif
        }


        private void ValueChanged(object sender, EventArgs e)
        {
            if (sender is ColourSlider cs)
            {
                double fValue = cs.Value;

                var newValue = currentColourSpace.SelectedComponent.SetSliderValue(fValue);// Value = mValue;

                if (fOldValue != fValue)
                {

                    targetYValue = newValue;
                    //currentYValue = oldValue;
                    fOldValue = fValue;
                    sliderTimer.Start();

                }
                else
                {
                    UpdateColourField(false);
                }
            }

        }

        private void SetCurrentSliderArrowYLocation(double sValue)
        {
            picColourSlider.Value = (int)(ColourSliderInnerRegion.Top + (255 - sValue));
        }






        /// <summary>
        /// Updates the Colour panels and the hex value.
        /// </summary>
        /// <param name="updateSlider">A boolean value indicating whether or 
        /// not the Colour slider should be updated.</param>
        /// <param name="resetPreviouslyPickedPointOnColourField">A boolean 
        /// value indicating whether or not the previously picked point on the 
        /// Colour field should be reset.</param>
        /// <param name="updateHexValue">A boolean value indicating whether or
        /// not the hex value should be updated.</param>

        private void UpdateColourPanels(bool updateSlider, bool resetPreviouslyPickedPointOnColourField, bool updateHexValue)
        {
            var Colour = rgbColourSpace.GetColour();
            pbCurrentColour.BackColor = Colour;

            if (updateSlider)
            {
                UpdateColourSlider();
            }

            UpdateColourField(resetPreviouslyPickedPointOnColourField);

            if (updateHexValue)
            {
                UpdateHexValue();
            }


            ColourSelected?.Invoke(this, new ColourSelectedEventArgs(Colour));

        }

        /// <summary>
        /// Updates the hexadecimal text value.
        /// </summary>

        private void UpdateHexValue()
        {
            hexTextBox.Text = this.HexValue;
            hexTextBox.SelectionStart = this.HexValue.Length;
        }

        /// <summary>
        /// Updates the Colour field panel.
        /// </summary>
        /// <param name="resetPreviouslyPickedPoint">A boolean value indicating
        /// whether or not the previously picked point should be reset.</param>

        private void UpdateColourField(bool resetPreviouslyPickedPoint)
        {

            int sValue = currentColourSpace.SelectedComponent.Value;
            char component = currentColourSpace.SelectedComponent.DisplayCharacter;

            if (currentColourSpace is HsbColourSpaceControl)
            {

                if (component == 'H')
                {

                    Colour Colour = picColourSlider.GetPixel();
                    ColourFieldPanel.UpdateColour(currentColourSpace, component, Colour, resetPreviouslyPickedPoint);

                }
                else if (component == 'S' || component == 'B')
                {
                    ColourFieldPanel.UpdateColour(currentColourSpace, component, sValue, resetPreviouslyPickedPoint);
                }

            }
            else if (currentColourSpace is RgbColourSpaceControl)
            {
                ColourFieldPanel.UpdateColour(currentColourSpace, component, sValue, resetPreviouslyPickedPoint);
            }

        }

        /// <summary>
        /// Updates the Colour slider.
        /// </summary>

        private void UpdateColourSlider()
        {

            picColourSlider.ColourSpace = currentColourSpace;// = ColourSliderBitmap;
            picColourSlider.Invalidate();

        }

        #endregion Helper methods

        /// <summary>
        /// Handles the MouseDown event raised by the picCurrentColour object.
        /// </summary>

        private void PicCurrentColour_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {


                DragForm.ShowDragBox(pbCurrentColour.BackColor, PointToScreen(pbCurrentColour.Location), pbCurrentColour.Size);

                // initiate drag
                pbCurrentColour.DoDragDrop(rgbColourSpace.GetColour().ToDataObject(), DragDropEffects.Move);

            }

        }

        /// <summary>
        /// Handles the KeyUp event raised by the hexTextBox object.
        /// </summary>

        private void HexTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            UpdateColourSpacesWithNewHexValue();
        }

        /// <summary>
        /// Handles the KeyDown event raised by the hexTextBox object.
        /// </summary>
        /// <param name="sender"> The object that raised the event.</param>
        /// <param name="e">A KeyEventArgs containing the event data.</param>

        private void HexTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            UpdateColourSpacesWithNewHexValue();
        }

        /// <summary>
        /// Updates the Colour spaces with the new hex value and makes sure that
        /// the necessary components are updated (Colour field, Colour slider, 
        /// etc).
        /// </summary>

        private void UpdateColourSpacesWithNewHexValue()
        {
            try
            {
                RGB rgb = ColourConverter.HexToRgb(hexTextBox.Text);
                hexTextBox.FormatError = null;

                rgbColourSpace.Structure = rgb;
                hsbColourSpace.Structure = ColourConverter.RgbToHsb(rgb);

                // reset the location of the arrows in the value region.
                this.UpdateColourPanels(true, true, false);
            }
            catch (Exception x)
            {
                hexTextBox.FormatError = x.Message;
            }
        }

        /// <summary>
        /// Handles the GetFeedback event raised by picCurrentColour. We don't
        /// want to use the default cursors (I think they are intrusive). 
        /// Setting the UseDefaultCursors property of the GiveFeedbackEventArgs
        /// object and setting the cursor to the hand cursor takes care of this
        /// for us.
        /// </summary>
        /// <param name="sender"> The object that raised the event.</param>
        /// <param name="e">A GiveFeedbackEventArgs containing the event data.</param>

        private void PicCurrentColour_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {

            //e.UseDefaultCursors = false;
            //Cursor.Current = Cursors.Hand;

        }

        /// <summary>
        /// Handles the QueryContinueDrag event raised by picCurrentColour. If
        /// the Action is anything other than Cancel or Drop, the location of
        /// the transparent form is updated.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">A QueryContinueDragEventArgs containing the event
        /// data.</param>

        private void PicCurrentColour_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            DragForm.UpdateDragBox(e);
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

        #endregion

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pbCurrentColour = new System.Windows.Forms.PictureBox();
            this.hexLabel = new System.Windows.Forms.Label();
            this.sliderTimer = new System.Windows.Forms.Timer(this.components);
            this.pbOriginalColour = new System.Windows.Forms.PictureBox();
            this.hexTextBox = new Piksel.Graphics.ColourPicker.Controls.HexTextBox();
            this.ColourFieldPanel = new Piksel.Graphics.ColourPicker.Controls.ColourFieldPanel();
            this.rgbColourSpace = new Piksel.Graphics.ColourPicker.Controls.RgbColourSpaceControl();
            this.hsbColourSpace = new Piksel.Graphics.ColourPicker.Controls.HsbColourSpaceControl();
            this.picColourSlider = new Piksel.Graphics.ColourPicker.Controls.ColourSlider();
            ((System.ComponentModel.ISupportInitialize)(this.pbCurrentColour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOriginalColour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picColourSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // pbCurrentColour
            // 
            this.pbCurrentColour.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbCurrentColour.Location = new System.Drawing.Point(328, 5);
            this.pbCurrentColour.Name = "pbCurrentColour";
            this.pbCurrentColour.Size = new System.Drawing.Size(80, 40);
            this.pbCurrentColour.TabIndex = 42;
            this.pbCurrentColour.TabStop = false;
            this.pbCurrentColour.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.PicCurrentColour_GiveFeedback);
            this.pbCurrentColour.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.PicCurrentColour_QueryContinueDrag);
            this.pbCurrentColour.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PicCurrentColour_MouseDown);
            // 
            // hexLabel
            // 
            this.hexLabel.Location = new System.Drawing.Point(308, 228);
            this.hexLabel.Name = "hexLabel";
            this.hexLabel.Size = new System.Drawing.Size(52, 23);
            this.hexLabel.TabIndex = 40;
            this.hexLabel.Text = "HEX:";
            this.hexLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // sliderTimer
            // 
            this.sliderTimer.Interval = 10;
            this.sliderTimer.Tick += new System.EventHandler(this.SliderTimer_Tick);
            // 
            // pbOriginalColour
            // 
            this.pbOriginalColour.Location = new System.Drawing.Point(368, 5);
            this.pbOriginalColour.Name = "pbOriginalColour";
            this.pbOriginalColour.Size = new System.Drawing.Size(40, 40);
            this.pbOriginalColour.TabIndex = 51;
            this.pbOriginalColour.TabStop = false;
            // 
            // hexTextBox
            // 
            this.hexTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.hexTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hexTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hexTextBox.FormatError = null;
            this.hexTextBox.Location = new System.Drawing.Point(342, 229);
            this.hexTextBox.MaxLength = 7;
            this.hexTextBox.Name = "hexTextBox";
            this.hexTextBox.Size = new System.Drawing.Size(79, 23);
            this.hexTextBox.TabIndex = 0;
            this.hexTextBox.Text = "";
            this.hexTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.hexTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HexTextBox_KeyDown);
            this.hexTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HexTextBox_KeyUp);
            // 
            // ColourFieldPanel
            // 
            this.ColourFieldPanel.BorderColor = System.Drawing.Color.Black;
            this.ColourFieldPanel.Location = new System.Drawing.Point(2, 2);
            this.ColourFieldPanel.Name = "ColourFieldPanel";
            this.ColourFieldPanel.Size = new System.Drawing.Size(258, 258);
            this.ColourFieldPanel.TabIndex = 52;
            this.ColourFieldPanel.ColourSelected += new Piksel.Graphics.ColourPicker.Controls.ColourFieldPanel.ColourSelectedHandler(this.ColourFieldPanel_ColourSelected);
            // 
            // rgbColourSpace
            // 
            this.rgbColourSpace.DisplayRadioButtons = true;
            this.rgbColourSpace.Location = new System.Drawing.Point(317, 131);
            this.rgbColourSpace.Name = "rgbColourSpace";
            this.rgbColourSpace.ReadOnly = false;
            this.rgbColourSpace.Size = new System.Drawing.Size(100, 90);
            this.rgbColourSpace.TabIndex = 48;
            this.rgbColourSpace.ComponentValueChanged += new Piksel.Graphics.ColourPicker.Controls.ColourSpaceEventHandler(this.ColourSpaceComponentValueChanged);
            this.rgbColourSpace.SelectedComponentChanged += new Piksel.Graphics.ColourPicker.Controls.ColourSpaceEventHandler(this.SelectedColourSpaceComponentChanged);
            // 
            // hsbColourSpace
            // 
            this.hsbColourSpace.DisplayRadioButtons = true;
            this.hsbColourSpace.Location = new System.Drawing.Point(317, 51);
            this.hsbColourSpace.Name = "hsbColourSpace";
            this.hsbColourSpace.ReadOnly = false;
            this.hsbColourSpace.Size = new System.Drawing.Size(100, 90);
            this.hsbColourSpace.TabIndex = 47;
            this.hsbColourSpace.ComponentValueChanged += new Piksel.Graphics.ColourPicker.Controls.ColourSpaceEventHandler(this.ColourSpaceComponentValueChanged);
            this.hsbColourSpace.SelectedComponentChanged += new Piksel.Graphics.ColourPicker.Controls.ColourSpaceEventHandler(this.SelectedColourSpaceComponentChanged);
            // 
            // picColourSlider
            // 
            this.picColourSlider.ColourSpace = null;
            this.picColourSlider.CursorHeight = 6;
            this.picColourSlider.CursorWidth = 8;
            this.picColourSlider.Location = new System.Drawing.Point(262, 0);
            this.picColourSlider.Margin = new System.Windows.Forms.Padding(0);
            this.picColourSlider.Name = "picColourSlider";
            this.picColourSlider.Size = new System.Drawing.Size(40, 262);
            this.picColourSlider.TabIndex = 50;
            this.picColourSlider.TabStop = false;
            this.picColourSlider.Value = 1D;
            this.picColourSlider.ValueChanged += new System.EventHandler(this.ValueChanged);
            this.picColourSlider.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PicColourSlider_MouseDown);
            this.picColourSlider.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PicColourSlider_MouseMove);
            this.picColourSlider.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PicColourSlider_MouseUp);
            // 
            // ColourPanelControl
            // 
            this.Controls.Add(this.hexTextBox);
            this.Controls.Add(this.ColourFieldPanel);
            this.Controls.Add(this.rgbColourSpace);
            this.Controls.Add(this.hsbColourSpace);
            this.Controls.Add(this.pbOriginalColour);
            this.Controls.Add(this.picColourSlider);
            this.Controls.Add(this.hexLabel);
            this.Controls.Add(this.pbCurrentColour);
            this.Name = "ColourPanelControl";
            this.Size = new System.Drawing.Size(428, 262);
            ((System.ComponentModel.ISupportInitialize)(this.pbCurrentColour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOriginalColour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picColourSlider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion Component Designer generated code


    } // ColourPanel

}
