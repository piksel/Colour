using Piksel.Graphics.ColourPicker.Controls;
using System;
using System.Windows.Forms;

namespace ComponentTester
{
    public partial class ColourPickerTesterForm : Form
    {
        public ColourPickerTesterForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void colourSwatchPanel1_ColourSwatchSelected(object sender, ColourSelectedEventArgs e)
        {
            colourPanelControl2.Value = e.Colour;
        }
    }
}
