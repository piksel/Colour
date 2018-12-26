using System.Windows.Forms;

namespace Piksel.Graphics.ColourPicker.Utilities
{
    public static class MouseEventArgsExtensions
    {
        public static MouseEventArgs Translated(this MouseEventArgs e, Control control)
        {
            var transPos = control.PointToClient(e.Location);
            return new MouseEventArgs(e.Button, e.Clicks, transPos.X, transPos.Y, e.Delta);
        }

    }
}
