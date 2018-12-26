using Piksel.Graphics.ColourSpaces;

namespace Piksel.Graphics.ColourPicker.Controls
{

    /// <summary>
    /// RGB Colour space.
    /// </summary>

    internal class RgbColourSpaceControl : ColourSpaceControl<RGB>
    {

        /// data fields
        private ColourSpaceComponent csRed;
        private ColourSpaceComponent csGreen;
        private ColourSpaceComponent csBlue;

        /// <summary>
        /// Gets or sets a value containing the coordinates of this Colour 
        /// space.
        /// </summary>

        internal override RGB Structure
        {
            get => NativeStructure;
            set => NativeStructure = value;
        }

        public RGB NativeStructure
        {
            get => new RGB(csRed.ByteValue, csGreen.ByteValue, csBlue.ByteValue);

            set
            {
                csRed.Value = value.Red;
                csGreen.Value = value.Green;
                csBlue.Value = value.Blue;
            }
        }

        /// <summary>
        /// Constructor. Adds the Colour space components to the Colour space
        /// component collection.
        /// </summary>

        public RgbColourSpaceControl()
        {

            InitializeComponent();

            ColourSpaceComponents.Add(csRed);
            ColourSpaceComponents.Add(csBlue);
            ColourSpaceComponents.Add(csGreen);

        }

        /// <summary>
        /// Sets the default Colour space component.
        /// </summary>

        public override void SetDefaultSelection()
        {
            ChangeCurrentlySelectedComponent(csRed);
        }

        /// <summary>
        /// Converts the coordinates represented by this Colour space to its
        /// equivalent Colour representation.
        /// </summary>
        /// <returns>A Colour object.</returns>

        public override Colour GetColour() => new Colour(csRed.ByteValue, csGreen.ByteValue, csBlue.ByteValue);

        /// <summary>
        /// Converts the coordinates represented by this Colour space to its 
        /// equivalent HEX representation.
        /// </summary>
        /// <returns>A string containing a hexadecimal value.</returns>

        internal string ConvertToHex() => GetColour().ToHex(HexPrefix.Hash, HexFormatAlpha.Never);

        /// <summary>
        /// Updates the Colour space coordinate values.
        /// </summary>
        /// <param name="csStructure">A IColourSpaceStructure object containing 
        /// values that are to be mapped to the coordinates of this Colour 
        /// space.</param>

        protected override void UpdateValues(RGB csStructure)
        {

            RGB rgb = (RGB)csStructure;

            csRed.Value = rgb.Red;
            csGreen.Value = rgb.Green;
            csBlue.Value = rgb.Blue;

        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.csRed = new ColourSpaceComponent();
            this.csBlue = new ColourSpaceComponent();
            this.csGreen = new ColourSpaceComponent();
            // 
            // csRed
            // 
            this.csRed.BackColor = System.Drawing.SystemColors.Control;
            this.csRed.DisplayCharacter = 'R';
            this.csRed.ForeColor = System.Drawing.SystemColors.ControlText;
            this.csRed.Location = new System.Drawing.Point(2, 2);
            this.csRed.Name = "csRed";
            this.csRed.RadioButtonVisible = true;
            this.csRed.ReadOnly = false;
            this.csRed.Selected = false;
            this.csRed.Size = new System.Drawing.Size(96, 26);
            this.csRed.TabIndex = 0;
            this.csRed.Unit = Piksel.Graphics.ColourSpaces.ComponentUnit.Byte;
            this.csRed.Value = 0;
            this.csRed.ComponentTextKeyUp += new ColourSpaceComponentEventHandler(ComponentTextKeyUp);
            this.csRed.ComponentSelected += new ColourSpaceComponentEventHandler(this.ComponentSelected);
            // 
            // csBlue
            // 
            this.csBlue.BackColor = System.Drawing.SystemColors.Control;
            this.csBlue.DisplayCharacter = 'B';
            this.csBlue.ForeColor = System.Drawing.SystemColors.ControlText;
            this.csBlue.Location = new System.Drawing.Point(2, 56);
            this.csBlue.Name = "csBlue";
            this.csBlue.RadioButtonVisible = true;
            this.csBlue.ReadOnly = false;
            this.csBlue.Selected = false;
            this.csBlue.Size = new System.Drawing.Size(96, 26);
            this.csBlue.TabIndex = 2;
            this.csBlue.Unit = Piksel.Graphics.ColourSpaces.ComponentUnit.Byte;
            this.csBlue.Value = 0;
            this.csBlue.ComponentTextKeyUp += new ColourSpaceComponentEventHandler(ComponentTextKeyUp);
            this.csBlue.ComponentSelected += new ColourSpaceComponentEventHandler(this.ComponentSelected);
            // 
            // csGreen
            // 
            this.csGreen.BackColor = System.Drawing.SystemColors.Control;
            this.csGreen.DisplayCharacter = 'G';
            this.csGreen.ForeColor = System.Drawing.SystemColors.ControlText;
            this.csGreen.Location = new System.Drawing.Point(2, 29);
            this.csGreen.Name = "csGreen";
            this.csGreen.RadioButtonVisible = true;
            this.csGreen.ReadOnly = false;
            this.csGreen.Selected = false;
            this.csGreen.Size = new System.Drawing.Size(96, 25);
            this.csGreen.TabIndex = 1;
            this.csGreen.Unit = Piksel.Graphics.ColourSpaces.ComponentUnit.Byte;
            this.csGreen.Value = 0;
            this.csGreen.ComponentTextKeyUp += new ColourSpaceComponentEventHandler(ComponentTextKeyUp);
            this.csGreen.ComponentSelected += new ColourSpaceComponentEventHandler(this.ComponentSelected);
            // 
            // RgbColourSpace
            // 
            this.Controls.Add(this.csBlue);
            this.Controls.Add(this.csGreen);
            this.Controls.Add(this.csRed);
            this.Name = "RgbColourSpace";
            this.Size = new System.Drawing.Size(112, 90);
            this.ResumeLayout(false);

        }

        #endregion

    }

}
