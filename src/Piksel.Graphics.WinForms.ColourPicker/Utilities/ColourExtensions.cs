using System.Windows.Forms;

namespace Piksel.Graphics.ColourPicker.Utilities
{
    public static class ColourExtensions
    {
        public static DataObject ToDataObject(this Colour colour)
        {
            var data = new DataObject();
            var hex = colour.ToHex(HexPrefix.Hash, HexFormatAlpha.Auto);
            var rtf = @"{\rtf1\ansi\deff0 {\colortbl ;"
                + $"\\red{colour.Red}\\green{colour.Green}\\blue{colour.Blue}"
                + @";}\cf1" + hex + @"}";

            data.SetData(DataFormats.Text, true, hex);
            data.SetData(DataFormats.Html, true, $"<font color=\"{hex}\">{hex}</font>");
            data.SetData(DataFormats.Rtf, true, rtf);
            data.SetData(colour);

            return data;
        }
    }
}
