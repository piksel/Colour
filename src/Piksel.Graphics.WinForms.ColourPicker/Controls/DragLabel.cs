using Piksel.Graphics.ColourPicker.Utilities;
using System.Windows.Forms;

namespace Piksel.Graphics.ColourPicker.Controls
{
    public class DragLabel : Label
    {
        protected override void WndProc(ref Message m)
        {
            if (!Window.HandleHits(ref m))
            {
                base.WndProc(ref m);
            }
        }
    }
}
