
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Piksel.Graphics.ColourPicker.Controls
{

    /// <summary>
    /// A specially designed textbox class that ensures that only HEX values 
    /// are entered and displayed.
    /// </summary>

    internal class HexTextBox : ColourSpaceComponentTextBox
    {

        // constants
        private const int MINIMUVALUE = 0x0;
        private const int MAXIMUVALUE = 0xFFFFFF;

        /// <summary>
        /// Overrides the Text property to set the DefaultValue attribute.
        /// </summary>

        [DefaultValue("#000000")]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        string formatError;
        public string FormatError
        {
            get => formatError; set
            {
                formatError = value;
                // TODO: Add error tooltip
                BackColor = formatError != null ? Color.MistyRose : SystemColors.Window;
                //Invalidate();
            }
        }

        /// <summary>
        /// Constructor. Sets the max length to 6 and the text property to an 
        /// empty string.
        /// </summary>

        public HexTextBox() : base()
        {

            MaxLength = 7;
            Text = string.Empty;

        }

        /// <summary>
        /// Determines whether or not the specified character is to be 
        /// designated for preprocessing or sent directly to the control.
        /// </summary>
        /// <param name="charCode">The character to be evaluated.</param>
        /// <returns>True if the key should be sent directly to the control, 
        /// false if it should be preprocessed.</returns>

        protected override bool IsInputChar(char charCode)
        {

            bool isInputChar = base.IsInputChar(charCode);

            if ((charCode >= (int)Keys.A && charCode <= (int)Keys.F)
              || (charCode == '#' && Text.Length == 0))
            {
                isInputChar = true;
            }

            return isInputChar;

        }

        /// <summary>
        /// Preprocessing leading up to the raising of the KeyDown event.
        /// </summary>
        /// <param name="e">A KeyEventArgs containing the event data.</param>

        protected override void OnKeyDown(KeyEventArgs e)
        {

            base.OnKeyDown(e);

            if (Text.Length > 0 &&
                 ((e.KeyData | Keys.Shift) == (Keys.Up | Keys.Shift) ||
                 (e.KeyData | Keys.Shift) == (Keys.Down | Keys.Shift)))
            {

                //string textHexValue = $"0x{Text.Substring(1)}";
                int hexValue = Convert.ToInt32(Text.Substring(1), 16);
                int incrementValue = 0;

                if ((e.KeyData & Keys.Shift) == Keys.Shift)
                {
                    incrementValue = 0x00000A;
                }
                else
                {
                    incrementValue = 0x000001;
                }

                if ((e.KeyData & Keys.Up) == Keys.Up)
                {

                    if (hexValue + incrementValue <= MAXIMUVALUE)
                    {
                        hexValue += incrementValue;
                    }
                    else
                    {
                        hexValue = MAXIMUVALUE;
                    }

                }
                else
                {

                    if (hexValue - incrementValue >= MINIMUVALUE)
                    {
                        hexValue -= incrementValue;
                    }
                    else
                    {
                        hexValue = MINIMUVALUE;
                    }

                }

                //Text = "#" + hexValue.ToString("x6").ToUpper();

                // this is a hack to fix some of the problems with the key 
                // combination selecting part of the text (only happens
                // with shift+down).
                //
                // TODO: see if there is a better way to do this.

                if (e.KeyData == (Keys.Shift | Keys.Down))
                {
                    SelectionStart = Text.Length;
                }

            }

        }

    }

}
