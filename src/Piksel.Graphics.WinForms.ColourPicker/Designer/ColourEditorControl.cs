using System.Drawing;
using System.Windows.Forms;

namespace Piksel.Graphics.ColourPicker.Designer
{
    public partial class ColourEditorControl : UserControl
    {
        private Colour value;

        public ColourEditorControl(Colour value)
        {
            this.value = value;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.DarkBlue), 0, 0, this.Width, this.Height);
        }
    }
}
