using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Piksel.Graphics.ColourPicker.Utilities
{
    using GDIGraphics = System.Drawing.Graphics;

    public class CursorDragDropHandler
    {
        public Cursor DragCursorMove { get; private set; }
        public Cursor DragCursorCopy { get; private set; }
        public Cursor DragCursorNo { get; private set; }
        public Cursor DragCursorLink { get; private set; }

        public bool CursorsCreated { get; private set; } = false;

        public void UpdateCursor(object sender, GiveFeedbackEventArgs fea)
        {
            if (!CursorsCreated)
            {
                return;
            }
            fea.UseDefaultCursors = false;

            switch (fea.Effect)
            {
                case DragDropEffects.Copy:
                    Cursor.Current = DragCursorCopy; break;

                case DragDropEffects.None:
                    Cursor.Current = DragCursorNo; break;

                case DragDropEffects.Link:
                    Cursor.Current = DragCursorLink; break;

                case DragDropEffects.Move:
                default:
                    Cursor.Current = DragCursorMove; break;
            }
        }

        public void CreateCursors(Bitmap image, Point hotSpot)
        {
            DragCursorMove = CursorUtils.CreateCursor(new Bitmap(image), hotSpot);
            DragCursorLink = DragCursorMove;
            //DragCursorLink = CursorUtil.CreateCursor((Bitmap)bm.Clone(), DragStart.X, DragStart.Y);
            DragCursorCopy = CursorUtils.CreateCursor(CursorUtils.AddCopySymbol(image), hotSpot);
            DragCursorNo = CursorUtils.CreateCursor(CursorUtils.AddNoSymbol(image), hotSpot);
            CursorsCreated = true;
        }
    }

    public static class CursorUtils
    {
        [DllImport("user32.dll")]
        public static extern IntPtr CreateIconIndirect(ref IconInfo icon);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr handle);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);

        public struct IconInfo
        {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }

        public static Cursor CreateCursor(Bitmap bmp, Point hotSpot)
        {
            IntPtr cursorPtr, iconHandle = IntPtr.Zero;
            IconInfo iconInfo = new IconInfo();
            try
            {
                iconHandle = bmp.GetHicon();
                GetIconInfo(iconHandle, ref iconInfo);
                iconInfo.xHotspot = hotSpot.X;
                iconInfo.yHotspot = hotSpot.Y;
                iconInfo.fIcon = false;
                cursorPtr = CreateIconIndirect(ref iconInfo);
            }
            finally
            {
                if (iconInfo.hbmColor != IntPtr.Zero) DeleteObject(iconInfo.hbmColor);
                if (iconInfo.hbmMask != IntPtr.Zero) DeleteObject(iconInfo.hbmMask);
                if (iconHandle != IntPtr.Zero) DestroyIcon(iconHandle);
            }

            return new Cursor(cursorPtr);
        }

        internal static Bitmap OverlayImage(Bitmap image, Action<GDIGraphics> drawOverlay)
        {
            var bitmap = new Bitmap(image.Width, image.Height);
            using (var g = GDIGraphics.FromImage(bitmap))
            {
                g.DrawImage(image, Point.Empty);
                drawOverlay(g);
            }
            return bitmap;
        }

        internal static Bitmap AddCopySymbol(Bitmap image)
        {
            return OverlayImage(image, g => { });
        }

        internal static Bitmap AddNoSymbol(Bitmap image)
        {
            return OverlayImage(image, g => { });
        }
    }
}
