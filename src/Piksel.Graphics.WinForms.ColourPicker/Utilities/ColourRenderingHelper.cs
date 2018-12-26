using Piksel.Graphics.ColourSpaces;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Piksel.Graphics.ColourPicker.Utilities
{
    using GDIGraphics = System.Drawing.Graphics;

    internal static class ColourRenderingHelper
    {
        private static Bitmap gradientBitmap;
        private static Bitmap GradientBitmap
            => gradientBitmap ?? (gradientBitmap = DrawBitmap(DrawMultiGradient));

        public static void DrawRGBColourField(GDIGraphics g, ColourField field, byte value)
        {

            byte y = 255;
            Rectangle rect;

            var getColour = ColourFieldMap.GetFieldMapColourDelegate(field, value);

            for (byte i = 0; ; i++)
            {

                rect = new Rectangle(0, i, 256, 1);

                using (var lgb = new LinearGradientBrush(rect, getColour(0, y), getColour(255, y), LinearGradientMode.Horizontal))
                {
                    g.FillRectangle(lgb, rect);
                }

                if (i == byte.MaxValue) break;

                y--;
            }

        }

        public static Bitmap GetGreenColourField(int green)
            => DrawBitmap(g => DrawRGBColourField(g, ColourField.Green, (byte)green));

        public static Bitmap GetBlueColourField(int blue)
            => DrawBitmap(g => DrawRGBColourField(g, ColourField.Blue, (byte)blue));

        public static Bitmap GetRedColourField(int red)
            => DrawBitmap(g => DrawRGBColourField(g, ColourField.Red, (byte)red));

        public static void DrawHueColourField(GDIGraphics g, Colour endColour)
        {
            double redIndex = (double)(255 - endColour.Red) / 255;
            double blueIndex = (double)(255 - endColour.Blue) / 255;
            double greenIndex = (double)(255 - endColour.Green) / 255;

            double cR = 255;
            double cG = 255;
            double cB = 255;

            for (int x = 0; x < 256; x++)
            {
                var colStart = new Colour((byte)Math.Round(cR), (byte)Math.Round(cG), (byte)Math.Round(cB));
                using (var lgb = new LinearGradientBrush(new Rectangle(x, 0, 1, 256), colStart, new Colour(0, 0, 0), 90f, false))
                {
                    g.FillRectangle(lgb, new Rectangle(x, 0, 1, 256));
                }

                cR = cR - redIndex;
                cG = cG - greenIndex;
                cB = cB - blueIndex;
            }
        }

        public static Bitmap GetHueColourField(Colour colour)
            => DrawBitmap(g => DrawHueColourField(g, colour));

        public static void DrawSaturationColourField(GDIGraphics g, int saturation)
        {
            var rect = new Rectangle(0, 0, 256, 256);
            byte saturatedColourValue = (byte)(255 - Math.Round(255 * ((double)saturation / 100)));

            g.DrawImage(GradientBitmap, 0, 0);

            var startColour = Colour.FromArgb(saturatedColourValue, 255, 255, 255);
            using (LinearGradientBrush lgb = new LinearGradientBrush(rect, startColour, Color.Black, 90f))
            {
                g.FillRectangle(lgb, rect);
            }
        }

        public static Bitmap GetSaturationColourField(int saturation)
            => DrawBitmap(g => DrawSaturationColourField(g, saturation));

        public static void DrawBrightnessColourField(GDIGraphics g, int brightness)
        {

            var alpha = (byte)(255 - Math.Round(brightness * 2.55));

            g.DrawImage(GradientBitmap, 0, 0);

            using (SolidBrush sb = new SolidBrush(Colour.FromArgb(alpha, 0, 0, 0)))
            {
                g.FillRectangle(sb, 0, 0, 256, 256);
            }

        }

        public static Bitmap GetBrightnessColourField(int brightness)
            => DrawBitmap(g => DrawBrightnessColourField(g, brightness));

        public static Bitmap DrawBitmap(Action<GDIGraphics> drawAction)
        {
            Bitmap bmp = new Bitmap(256, 256);
            using (var g = GDIGraphics.FromImage(bmp))
            {
                drawAction(g);
            }
            return bmp;
        }

        private static void DrawMultiGradient(GDIGraphics gBmp)
        {
            int c = 255;

            for (int i = 0; i < 256; i++)
            {
                var rect = new Rectangle(0, i, 256, 1);

                using (var brBrush = new LinearGradientBrush(rect, Color.Blue, Color.Red, 0f, false))
                {
                    brBrush.InterpolationColors = new ColorBlend
                    {
                        Colors = (new Color[]
                        {
                                Color.FromArgb( c, i, i ), // red 
							    Color.FromArgb( c, c, i ), // yellow 
							    Color.FromArgb( i, c, i ), // green 
							    Color.FromArgb( i, c, c ), // cyan 
							    Color.FromArgb( i, i, c ), // blue 
							    Color.FromArgb( c, i, c ), // magneta
							    Color.FromArgb( c, i, i ) // red
                        }),
                        Positions = (new float[] { 0.0f, 0.1667f, 0.3372f, 0.502f, 0.6686f, 0.8313f, 1.0f })
                    };

                    gBmp.FillRectangle(brBrush, rect);

                }
            }


        }

    }

}