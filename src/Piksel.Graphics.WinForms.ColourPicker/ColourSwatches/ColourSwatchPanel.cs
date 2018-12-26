using Piksel.Graphics.ColourPicker.ColourSwatches;
using Piksel.Graphics.ColourPicker.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Piksel.Graphics.ColourPicker.Controls
{
    using GDIGraphics = System.Drawing.Graphics;

    public class ColourSwatchPanel : Panel
    {
        private bool isDragging = false;
        private Rectangle dragThreshold;

        public int SwatchSize
        {
            get => SWATCH_HEIGHT;
            set
            {
                SWATCH_HEIGHT = value;
                SWATCH_WIDTH = value;
                InvalidateBitmap();
            }
        }

        private void InvalidateBitmap()
        {
            swatchBitmap = null;
            Invalidate();
        }

        private IContainer components;

        // events
        public event ColourSwatchSelectedHandler ColourSwatchSelected;

        // controls
        private ToolTip ColourTip;
        private readonly DragForm dragForm;
        private ContextMenu contextMenu;
        private MenuItem renameSwatchMenuItem;
        private MenuItem separatorMenuItem;
        private MenuItem deleteSwatchMenuItem;
        private MenuItem addNewSwatchMenuItem;

        // readonly members
        private readonly Rectangle swatchRegion = new Rectangle(0, 0, 164, 263);
        private readonly string customSwatchesFile = "CustomSwatches.xml";
        private readonly string customPresetsFile = "ColourPresets.toml";
        private ColourSwatch? swatchClicked = null;
        private readonly CursorDragDropHandler cursorDragDropHandler = new CursorDragDropHandler();

        // constants
        private const int PADDING = 2;
        private int SWATCH_WIDTH = 10;
        private int SWATCH_HEIGHT = 10;
        private const int OUTER_PADDING = 6;

        // member fields
        private List<ColourPreset> presets;
        private bool paintActiveEmptySwatch;
        private Bitmap swatchBitmap;
        private ColourSwatch[,] swatchArray;
        private ColourSwatch lastSwatch;
        private readonly int startX;
        private readonly int startY;
        private int swatchOuterRegionWidth;
        private int swatchOuterRegionHeight;
        private int totalNumberOfSwatches;
        private int numberOfEmptySwatches;
        private int swatchRowNum;
        private int swatchColumnNum;
        private Point nextEmptySwatchPoint;

        /// <summary>
        /// Constructor. 
        /// </summary>

        public ColourSwatchPanel()
        {

            InitializeComponent();

            startX = swatchRegion.X + OUTER_PADDING;
            startY = swatchRegion.Y + OUTER_PADDING;
            ColourTip.SetToolTip(this, "");

            AllowDrop = true;

            LoadSwatches();

        }

        // TODO: get rid of this.

        protected override void OnLayout(LayoutEventArgs levent)
        {

            base.OnLayout(levent);
            UpdateSize();

        }

        private void LoadSwatches()
        {
            presets = !File.Exists(customPresetsFile)
                ? ColourSwatchXml.ReadPresets("ColourSwatches.xml", true).ToList()
                : LicenseManager.UsageMode != LicenseUsageMode.Designtime
                    ? ColourPresetSerializer.LoadPresets(customPresetsFile).ToList()
                    : new List<ColourPreset>();
        }

        private void UpdateSize()
        {

            swatchOuterRegionWidth = Width - ((Width - (2 * OUTER_PADDING))) % (SWATCH_WIDTH + PADDING);
            swatchOuterRegionHeight = Height - ((Height - (2 * OUTER_PADDING))) % (SWATCH_HEIGHT + PADDING);

            int horizSwatches = (swatchOuterRegionWidth - (2 * OUTER_PADDING)) / (SWATCH_WIDTH + PADDING);
            int vertSwatches = (swatchOuterRegionHeight - (2 * OUTER_PADDING)) / (SWATCH_HEIGHT + PADDING);

            swatchArray = new ColourSwatch[horizSwatches, vertSwatches];

            //swatchBitmap = null;
            InvalidateBitmap();
        }

        private void CreateCustomSwatchesFile()
        {

            using (var resStream = Resources.GetFileResource<ColourSwatchPanel>("ColourSwatches.xml"))
            using (var custStream = File.Open(customSwatchesFile, FileMode.Create))
            {
                resStream.CopyTo(custStream);
            }

        }

        private Bitmap BuildSwatchBitmap()
        {

            int x = startX;
            int y = startY;
            bool isFull = false;

            int swatchColumnNum = 0;
            int swatchRowNum = 0;

            Bitmap swatchBitmap = new Bitmap(swatchOuterRegionWidth, swatchOuterRegionHeight);

            using (var g = GDIGraphics.FromImage(swatchBitmap))
            {

                foreach (var preset in presets)
                {
                    var swatch = new ColourSwatch(preset, new Point(x, y), new Size(SWATCH_WIDTH, SWATCH_HEIGHT));
                    swatchArray[swatchColumnNum++, swatchRowNum] = swatch;

                    using (SolidBrush sb = new SolidBrush(swatch.Colour))
                    {
                        g.FillRectangle(sb, x, y, SWATCH_WIDTH, SWATCH_HEIGHT);
                    }

                    g.DrawRectangle(Pens.Black, x, y, SWATCH_WIDTH, SWATCH_HEIGHT);

                    UpdatePositions(ref x, ref y, ref swatchColumnNum, ref swatchRowNum);

                    if (y + SWATCH_HEIGHT > swatchOuterRegionHeight)
                    {
                        isFull = true;
                        break;
                    }

                    totalNumberOfSwatches++;

                }

                if (!isFull)
                {

                    numberOfEmptySwatches = 0;

                    while (true)
                    {

                        if (++numberOfEmptySwatches == 1)
                        {
                            nextEmptySwatchPoint = new Point(x, y);
                        }

                        g.DrawRectangle(Pens.DarkGray, x, y, SWATCH_WIDTH, SWATCH_HEIGHT);
                        UpdatePositions(ref x, ref y, ref swatchColumnNum, ref swatchRowNum);

                        if (y + SWATCH_HEIGHT > swatchOuterRegionHeight)
                        {
                            isFull = true;
                            break;
                        }

                        totalNumberOfSwatches++;


                    }

                }

            }

            return swatchBitmap;

        }

        private void UpdatePositions(ref int x, ref int y, ref int swatchColumnNum, ref int swatchRowNum)
        {

            if (x + 2 * (SWATCH_WIDTH + PADDING) > swatchOuterRegionWidth)
            {

                x = startX;
                y += SWATCH_HEIGHT + PADDING;

                swatchColumnNum = 0;
                swatchRowNum++;

            }
            else
            {
                x += SWATCH_WIDTH + PADDING;
            }

        }

        protected override void OnPaint(PaintEventArgs pe)
        {

            ControlPaint.DrawBorder3D(pe.Graphics, new Rectangle(0, 0, swatchOuterRegionWidth, swatchOuterRegionHeight));

            if (swatchArray == null)
            {
                UpdateSize();
            }

            if (swatchBitmap == null)
            {
                swatchBitmap = BuildSwatchBitmap();
            }
            pe.Graphics.DrawImage(swatchBitmap, new Rectangle(new Point(0, 0), new Size(swatchBitmap.Width, swatchBitmap.Height)));

            if (paintActiveEmptySwatch)
            {

                Rectangle rect = new Rectangle(nextEmptySwatchPoint, new Size(SWATCH_WIDTH, SWATCH_HEIGHT));
                pe.Graphics.DrawRectangle(Pens.Yellow, rect);

            }

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!isDragging && swatchClicked is ColourSwatch swatch && !dragThreshold.Contains(e.Location))
                {
                    DragForm.ShowDragBox(swatch.Colour, PointToScreen(swatch.Location), swatch.Size);

                    isDragging = true;

                    // initiate drag
                    DoDragDrop(swatch.Colour.ToDataObject(), DragDropEffects.Move);
                }
            }
            else
            {
                isDragging = false;
            }


            int width = swatchOuterRegionWidth - (2 * OUTER_PADDING);
            int height = swatchOuterRegionHeight - (2 * OUTER_PADDING);
            int x = e.X - OUTER_PADDING;
            int y = e.Y - OUTER_PADDING;

            Rectangle r = new Rectangle(startX, startY, width, height);
            Rectangle c = new Rectangle(e.X, e.Y, 1, 1);

            if (c.IntersectsWith(r))
            {

                int swatchColumnIndex = (x / (SWATCH_WIDTH + PADDING));
                int swatchRowIndex = (y / (SWATCH_HEIGHT + PADDING));

                ColourSwatch potentialSwatch = swatchArray[swatchColumnIndex, swatchRowIndex];
                Rectangle potentialSwatchRectangle = new Rectangle(potentialSwatch.Location, potentialSwatch.Size);
                Point cursorPoint = new Point(e.X, e.Y);
                Rectangle cursorRectangle = new Rectangle(cursorPoint, new Size(1, 1));

                if (cursorRectangle.IntersectsWith(potentialSwatchRectangle))
                {

                    // hides the tooltip when moving from swatch to swatch.
                    if (!lastSwatch.Equals(potentialSwatch))
                    {
                        this.ColourTip.Active = false;
                    }

                    if (!potentialSwatch.Description.Equals(ColourTip.GetToolTip(this)))
                    {
                        this.ColourTip.SetToolTip(this, potentialSwatch.Description);
                    }

                    this.ColourTip.Active = true;
                    lastSwatch = potentialSwatch;
                    this.Cursor = Cursors.Hand;

                }
                else
                {

                    this.Cursor = Cursors.Default;
                    this.ColourTip.SetToolTip(this, "");
                    this.ColourTip.Active = false;


                }

            }

        }

        const int DRAG_THRESH = 10;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            int width = swatchOuterRegionWidth - (2 * OUTER_PADDING);
            int height = swatchOuterRegionHeight - (2 * OUTER_PADDING);
            int x = e.X - OUTER_PADDING;
            int y = e.Y - OUTER_PADDING;

            isDragging = false;

            Rectangle r = new Rectangle(startX, startY, width, height);

            dragThreshold = new Rectangle(e.X - DRAG_THRESH, e.Y - DRAG_THRESH, DRAG_THRESH * 2, DRAG_THRESH * 2);

            if (r.Contains(e.Location))
            {

                int swatchColumnIndex = (x / (SWATCH_WIDTH + PADDING));
                int swatchRowIndex = (y / (SWATCH_HEIGHT + PADDING));

                var potentialSwatch = swatchArray[swatchColumnIndex, swatchRowIndex];

                swatchClicked = (potentialSwatch.Rectangle.Contains(e.Location))
                    ? (ColourSwatch?)potentialSwatch
                    : null;

            }

            if (e.Button == MouseButtons.Left && swatchClicked is ColourSwatch swatch)
            {
                /*
                var hotSpot = new Point(SWATCH_WIDTH / 2, SWATCH_HEIGHT / 2);

                var bitmap = new Bitmap(SWATCH_WIDTH, SWATCH_HEIGHT);
                using (var g = GDIGraphics.FromImage(bitmap))
                {
                    var rect = new Rectangle(Point.Empty, new Size(SWATCH_WIDTH, SWATCH_HEIGHT));
                    g.FillRectangle(new SolidBrush(swatch.Colour), rect);
                }

                cursorDragDropHandler.CreateCursors(bitmap, hotSpot);
                */

                // update form properties


            }

            base.OnMouseDown(e);
        }

        protected override void OnQueryContinueDrag(QueryContinueDragEventArgs qcdevent)
            => DragForm.UpdateDragBox(qcdevent);

        protected override void OnMouseUp(MouseEventArgs e)
        {



            if (e.Button == MouseButtons.Right)
            {
                deleteSwatchMenuItem.Enabled = swatchClicked.HasValue;
                renameSwatchMenuItem.Enabled = swatchClicked.HasValue;
                contextMenu.Show(this, e.Location);
            }
            else if (e.Button == MouseButtons.Left && !isDragging && swatchClicked is ColourSwatch swatch)
            {
                ColourSwatchSelected?.Invoke(this, new ColourSelectedEventArgs(swatch.Colour));
            }

            isDragging = false;

            base.OnMouseUp(e);

        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {

            ToggleEmptySwatchState(true);
            base.OnDragEnter(drgevent);

        }

        protected override void OnDragLeave(EventArgs e)
        {
            ToggleEmptySwatchState(false);
            base.OnDragLeave(e);
        }


        protected override void OnDragOver(DragEventArgs e)
        {
            e.Effect = isDragging ? DragDropEffects.None : DragDropEffects.Move;
            base.OnDragOver(e);
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            if (!isDragging)
            {
                Colour c = (Colour)e.Data.GetData(typeof(Colour));
                AddColour(c);
                e.Effect = DragDropEffects.None;
            }

            isDragging = false;

            base.OnDragDrop(e);

        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs gfbevent)
        {
            cursorDragDropHandler.UpdateCursor(this, gfbevent);
            base.OnGiveFeedback(gfbevent);
        }

        private void AddColour(Colour c)
        {

            bool writeSwatchesToFile = false;

            if (presets.FirstOrDefault(cs => cs.Colour == c) is ColourPreset cp && cp.Colour == c)
            {
                MessageBox.Show(Parent, $"Colour already exists with the name \"{cp.Name}\"", "Cannot add colour to swatches!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (numberOfEmptySwatches <= 0)
            {
                MessageBox.Show(Parent, "There are no empty swatches available.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                using (AddNewColourSwatchForm ColourForm = new AddNewColourSwatchForm(c))
                {

                    ColourForm.StartPosition = FormStartPosition.CenterParent;
                    ColourForm.ShowInTaskbar = false;

                    if (ColourForm.ShowDialog() == DialogResult.OK)
                    {

                        if (swatchBitmap != null)
                        {

                            int x = nextEmptySwatchPoint.X;
                            int y = nextEmptySwatchPoint.Y;

                            using (var g = GDIGraphics.FromImage(swatchBitmap))
                            {

                                using (SolidBrush sb = new SolidBrush(c))
                                {
                                    g.FillRectangle(sb, x, y, SWATCH_WIDTH, SWATCH_HEIGHT);
                                }

                                g.DrawRectangle(Pens.Black, x, y, SWATCH_WIDTH, SWATCH_HEIGHT);

                            }

                            numberOfEmptySwatches--;

                            var preset = new ColourPreset(c, ColourForm.ColourDescription);
                            var cs = new ColourSwatch(preset, new Point(x, y), new Size(SWATCH_WIDTH, SWATCH_HEIGHT));

                            swatchArray[swatchColumnNum++, swatchRowNum] = cs;
                            presets.Add(preset);
                            writeSwatchesToFile = true;

                            UpdatePositions(ref x, ref y, ref swatchColumnNum, ref swatchRowNum);
                            nextEmptySwatchPoint = new Point(x, y);

                        }

                    }

                }

                // This should not be necessary...
                //InvalidateBitmap();

            }

            if (writeSwatchesToFile)
            {
                //ColourSwatchXml.WriteSwatches(customSwatchesFile, presets);
                ColourPresetSerializer.SavePresets(presets, customPresetsFile);
            }

            paintActiveEmptySwatch = false;
            this.InvalidateEmptySwatch(nextEmptySwatchPoint);

        }

        private void ToggleEmptySwatchState(bool isActive)
        {

            if (numberOfEmptySwatches > 0)
            {

                paintActiveEmptySwatch = isActive;
                InvalidateEmptySwatch();

            }
            else
            {
                paintActiveEmptySwatch = false;
            }

        }

        private void InvalidateEmptySwatch()
        {
            InvalidateEmptySwatch(nextEmptySwatchPoint);
        }

        private void InvalidateEmptySwatch(Point swatchPoint)
        {

            Rectangle invalidationRegion = new Rectangle(swatchPoint, new Size(SWATCH_WIDTH, SWATCH_HEIGHT));
            invalidationRegion.Inflate(1, 1);

            Invalidate(invalidationRegion);

        }

        #region Dispose

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                dragForm?.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion Dispose

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

            this.components = new System.ComponentModel.Container();
            this.ColourTip = new System.Windows.Forms.ToolTip(this.components);
            this.addNewSwatchMenuItem = new MenuItem("Add new swatch");
            this.separatorMenuItem = new MenuItem("-");
            this.deleteSwatchMenuItem = new MenuItem("Delete swatch");
            this.renameSwatchMenuItem = new MenuItem("Rename swatch");
            this.contextMenu = new System.Windows.Forms.ContextMenu(new MenuItem[] { addNewSwatchMenuItem, separatorMenuItem, renameSwatchMenuItem, deleteSwatchMenuItem });
            // 
            // ColourTip
            // 
            this.ColourTip.Active = false;

        }
        #endregion

    } // ColourSwatchPanel

}
