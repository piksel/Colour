using System.Windows.Forms;

namespace Piksel.Graphics.ColourPicker.Designer
{
    public partial class ColourDialog : Form
    {
        public static Colour ShowDialog(Colour value)
        {
            var cef = new ColourDialog();
            cef.colourPanelControl2.Value = value;
            if (cef.ShowDialog() == DialogResult.OK)
            {
                return cef.colourPanelControl2.Value;
            }
            else
            {
                return value;
            }
        }


        internal ColourDialog()
        {
            InitializeComponent();
        }
    }
}
