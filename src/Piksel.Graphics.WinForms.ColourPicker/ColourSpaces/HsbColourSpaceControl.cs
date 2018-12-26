using Piksel.Graphics.ColourSpaces;
using System;

namespace Piksel.Graphics.ColourPicker.Controls
{

    /// <summary>
    /// HSB Colour space.
    /// </summary>

    internal class HsbColourSpaceControl : ColourSpaceControl<HSB>
    {

        // data fields
        private ColourSpaceComponent csBrightness;
        private ColourSpaceComponent csSaturation;
        private ColourSpaceComponent csHue;

        /// <summary>
        /// Gets or sets a value containing the coordinates of this Colour 
        /// space.
        /// </summary>

        internal sealed override HSB Structure
        {

            get
            {
                return new HSB(csHue.Value, csSaturation.ByteValue, csBrightness.ByteValue);
            }

            set
            {

                HSB hsb = (HSB)value;

                csHue.Value = hsb.Hue;
                csSaturation.Value = hsb.Saturation;
                csBrightness.Value = hsb.Brightness;

            }

        }

        /// <summary>
        /// Constructor. Adds the Colour space components to the Colour space
        /// component collection.
        /// </summary>

        public HsbColourSpaceControl()
        {

            InitializeComponent();

            ColourSpaceComponents.Add(csHue);
            ColourSpaceComponents.Add(csSaturation);
            ColourSpaceComponents.Add(csBrightness);

        }

        /// <summary>
        /// Sets the default Colour space component.
        /// </summary>

        public override void SetDefaultSelection()
        {
            ChangeCurrentlySelectedComponent(csHue);
        }

        /// <summary>
        /// Updates the Colour space coordinate values.
        /// </summary>
        /// <param name="csStructure">A IColourSpaceStructure object containing 
        /// the values that are to be mapped to the coordinates of this Colour 
        /// space.</param>

        protected override void UpdateValues(HSB csStructure)
        {

            HSB hsb = (HSB)csStructure;

            csHue.Value = hsb.Hue;
            csSaturation.Value = hsb.Saturation;
            csBrightness.Value = hsb.Brightness;

        }

        public override Colour GetColour()
        {

            var rgb = ColourConverter.HsbToRgb(new HSB(csHue.Value, csSaturation.ByteValue, csBrightness.ByteValue));
            return new Colour(rgb.Red, rgb.Green, rgb.Blue);

        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            csBrightness = new ColourSpaceComponent();
            csSaturation = new ColourSpaceComponent();
            csHue = new ColourSpaceComponent();
            // 
            // csBrightness
            // 
            this.csBrightness.DisplayCharacter = 'B';
            this.csBrightness.Location = new System.Drawing.Point(2, 56);
            this.csBrightness.MaximumValue = 100;
            this.csBrightness.Name = "csBrightness";
            this.csBrightness.RadioButtonVisible = true;
            this.csBrightness.ReadOnly = false;
            this.csBrightness.Selected = false;
            this.csBrightness.Size = new System.Drawing.Size(96, 26);
            this.csBrightness.TabIndex = 2;
            this.csBrightness.Unit = Piksel.Graphics.ColourSpaces.ComponentUnit.Percentage;
            this.csBrightness.Value = 0;
            this.csBrightness.MaximumValue = 100;
            this.csBrightness.ComponentTextKeyUp += new ColourSpaceComponentEventHandler(this.ComponentTextKeyUp);
            this.csBrightness.ComponentSelected += new ColourSpaceComponentEventHandler(ComponentSelected);
            // 
            // csSaturation
            // 
            this.csSaturation.DisplayCharacter = 'S';
            this.csSaturation.Location = new System.Drawing.Point(2, 29);
            this.csSaturation.MaximumValue = 100;
            this.csSaturation.Name = "csSaturation";
            this.csSaturation.RadioButtonVisible = true;
            this.csSaturation.ReadOnly = false;
            this.csSaturation.Selected = false;
            this.csSaturation.Size = new System.Drawing.Size(96, 26);
            this.csSaturation.TabIndex = 1;
            this.csSaturation.Unit = Piksel.Graphics.ColourSpaces.ComponentUnit.Percentage;
            this.csSaturation.Value = 0;
            this.csSaturation.MaximumValue = 100;
            this.csSaturation.ComponentTextKeyUp += new ColourSpaceComponentEventHandler(this.ComponentTextKeyUp);
            this.csSaturation.ComponentSelected += new ColourSpaceComponentEventHandler(ComponentSelected);
            // 
            // csHue
            // 
            this.csHue.DisplayCharacter = 'H';
            this.csHue.Location = new System.Drawing.Point(2, 2);
            this.csHue.MaximumValue = 360;
            this.csHue.Name = "csHue";
            this.csHue.RadioButtonVisible = true;
            this.csHue.ReadOnly = false;
            this.csHue.Selected = false;
            this.csHue.Size = new System.Drawing.Size(96, 26);
            this.csHue.TabIndex = 0;
            this.csHue.Unit = Piksel.Graphics.ColourSpaces.ComponentUnit.Degree;
            this.csHue.Value = 0;
            this.csHue.MaximumValue = 360;
            this.csHue.ComponentTextKeyUp += new ColourSpaceComponentEventHandler(this.ComponentTextKeyUp);
            this.csHue.ComponentSelected += new ColourSpaceComponentEventHandler(ComponentSelected);
            // 
            // HsbColourSpace
            // 
            this.Controls.Add(this.csBrightness);
            this.Controls.Add(this.csSaturation);
            this.Controls.Add(this.csHue);
            this.Name = "HsbColourSpace";
            this.Size = new System.Drawing.Size(112, 90);
            this.ResumeLayout(false);

        }

        private void csBrightness_ComponentTextKeyUp(ColourSpaceComponent sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

    } // HsbColourSpace

}
