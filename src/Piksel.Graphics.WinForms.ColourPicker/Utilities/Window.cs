
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Piksel.Graphics.ColourPicker.Utilities
{

    public static class Window
    {

        [DllImport("User32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hwnd, ShowWindowMessages cmdShow);

        public static bool ShowWindow(this System.Windows.Forms.Form form, ShowWindowMessages cmdShow)
            => ShowWindow(form.Handle, cmdShow);

        public enum Messages
        {

            WNCHITTEST = 0x0084,
            WKEYDOWN = 0x01FF,
            WKEYUP = 0x02FF,

        }

        public static Messages Message(this System.Windows.Forms.Message msg)
            => (Messages)msg.Msg;

        /// <summary>
        /// Enumeration of HitTest values.
        /// 
        /// See https://docs.microsoft.com/en-us/windows/desktop/inputdev/wm-nchittest
        /// for a description of each value.
        /// </summary>
        public enum HitTestValues
        {

            HTERROR = -2,
            HTTRANSPARENT = -1,
            HTNOWHERE = 0,
            HTCLIENT = 1,
            HTCAPTION = 2,
            HTSYSMENU = 3,
            HTGROWBOX = 4,
            HTMENU = 5,
            HTHSCROLL = 6,
            HTVSCROLL = 7,
            HTMINBUTTON = 8,
            HTMAXBUTTON = 9,
            HTLEFT = 10,
            HTRIGHT = 11,
            HTTOP = 12,
            HTTOPLEFT = 13,
            HTTOPRIGHT = 14,
            HTBOTTOM = 15,
            HTBOTTOMLEFT = 16,
            HTBOTTOMRIGHT = 17,
            HTBORDER = 18,
            HTOBJECT = 19,
            HTCLOSE = 20,
            HTHELP = 21,

        }

        public enum ShowWindowMessages : int
        {
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,

        }

        public static class Caret
        {

            [DllImport("user32.dll")]
            public static extern bool HideCaret(IntPtr hwnd);

            [DllImport("user32.dll")]
            public static extern bool ShowCaret(IntPtr hwnd);

        }

        internal static bool HandleHits(ref Message m)
        {
            if (m.Message() == Messages.WNCHITTEST)
            {
                m.Result = (IntPtr)HitTestValues.HTTRANSPARENT;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}