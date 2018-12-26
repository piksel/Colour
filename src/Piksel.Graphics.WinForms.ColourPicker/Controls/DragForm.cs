
using Piksel.Graphics.ColourPicker.Utilities;
using System.Drawing;
using System.Windows.Forms;

namespace Piksel.Graphics.ColourPicker.Controls
{

    internal class DragForm : Form
    {
        private static DragForm instance;

        public static void Initialize()
            => instance = new DragForm();

        public static void ShowDragBox(Colour colour, Point location, Size size)
            => instance.showDragBox(colour, location, size);

        public static void UpdateDragBox(QueryContinueDragEventArgs e)
            => instance.updateDragBox(e);

        private Size clientRectangleSize;
        private Point cursorDifference;
        private Rectangle drawRect;
        private readonly DragLabel label;
        private readonly Bitmap labelBitmap;

        /// <summary>
        /// Overloaded constructor that allows you to set the size property.
        /// </summary>
        /// <param name="size">A Size object representing the desired size.</param>

        private DragForm() : base()
        {

            ShowInTaskbar = false;
            TopMost = true;
            FormBorderStyle = FormBorderStyle.None;

            Size = new Size(0, 0);
            drawRect = new Rectangle(Point.Empty, Size);

            // Since the second parameter of the ShowWindow method in ShowForm
            // is ignored the first time the form is shown, work around this by
            // quickly showing and then hiding the form.

            ShowForm();
            Hide();


            label = new DragLabel()
            {
                Location = new Point(1, 1),
                Name = "label",
                Font = Font,
                Text = "text",
                ForeColor = Color.Black,
                BackColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false,
            };


            Controls.Add(label);


        }

        /// <summary>
        /// Sets the client size of the form equal to the Size parameter.
        /// </summary>
        /// <param name="newSize">The desired client area size.</param>

        internal void ChangeSize(Size newSize)
        {
            if (newSize.Width < 60)
                newSize.Width = 60;

            clientRectangleSize = newSize;
            Size = newSize + new Size(6, 22);
            drawRect = new Rectangle(new Point(3, 19), newSize);
            Region = new Region(drawRect);

            label.Location = new Point(drawRect.X + 1, drawRect.Y + 1);
            label.Size = new Size(drawRect.Width - 2, drawRect.Height - 2);
            //label.Size = drawRect.Size;
        }

        /// <summary>
        /// Sets the location of the form relative to the top left corner of
        /// the client area as opposed to the top left corner of the window.
        /// </summary>
        /// <param name="newLocation">The new location of the form.</param>

        internal void UpdateLocation(Point newLocation)
        {
            Location = new Point(newLocation.X - 3, newLocation.Y - 19);
        }

        /// <summary>
        /// Displays the form with a transparency of .5, but does not make 
        /// it active.
        /// </summary>

        internal void ShowForm()
        {

            Opacity = .5f;

            this.ShowWindow(Window.ShowWindowMessages.SW_SHOWNOACTIVATE);
            //Window.ShowWindow(label.Handle, Window.ShowWindowMessages.SW_SHOWNOACTIVATE);
        }

        /// <summary>
        /// Paints a thin 1f border around the boundaries of the client area.
        /// </summary>
        /// <param name="e">A PaintEventArgs that contains the event data.</param>

        protected override void OnPaint(PaintEventArgs e)
        {
            //e.Graphics.FillRectangle(new SolidBrush(Color.Black), 1, 1, Width - 1, Height - 1);
            //e.Graphics.DrawRectangle(Pens.Black, drawRect.X, drawRect.Y, drawRect.Width - 1, drawRect.Height - 1);

            if (labelBitmap != null)
            {
                e.Graphics.DrawImage(labelBitmap, drawRect.Location);
            }
        }

        /// <summary>
        /// Overrides the Form's WndProc method to return HTTRANSPARENT when the
        /// WNCHITTEST message is returned.
        /// </summary>
        /// <param name="m">A message object containing the message data.</param>

        protected override void WndProc(ref Message m)
        {
            if (!Window.HandleHits(ref m))
            {
                base.WndProc(ref m);
            }
        }

        private void updateDragBox(QueryContinueDragEventArgs e)
        {
            if (e.Action == DragAction.Cancel || e.Action == DragAction.Drop)
            {
                Hide();
            }
            else
            {
                var cp = Cursor.Position;
                Location = new Point(cp.X - cursorDifference.X, cp.Y - cursorDifference.Y);
            }
        }

        private void showDragBox(Colour colour, Point location, Size size)
        {
            UpdateLocation(location);
            var cp = Cursor.Position;
            cursorDifference = new Point(cp.X - Location.X, cp.Y - Location.Y);

            /*
            label.Location = Point.Empty;
            label.Size = size;
            //label.AutoSize = true;
            //label.Anchor = AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right & AnchorStyles.Top;
            label.Font = Font;
            label.Text = "text";

            BackColor = Color.Black;
            */
            ChangeSize(size);

            label.ForeColor = Color.Black;
            label.BackColor = colour;
            var labelBitmap = new Bitmap(drawRect.Width, drawRect.Height);
            label.Visible = true;
            //label.DrawToBitmap(labelBitmap, drawRect);
            //label.Visible = false;
            label.Text = colour.ToHex(HexPrefix.Hash, HexFormatAlpha.Never);

            BackColor = colour;
            //Invalidate();
            //TopMost = true;
            ShowForm();
        }
    }

}
