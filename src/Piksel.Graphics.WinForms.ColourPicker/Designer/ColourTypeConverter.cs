using System;
using System.ComponentModel;
using System.Globalization;

namespace Piksel.Graphics.ColourPicker.Designer
{
    public class ColourTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            => value is string s ? Colour.FromString(s) : base.ConvertFrom(context, culture, value);

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            => destinationType == typeof(string) && value is Colour c
                ? c.ToHex(HexPrefix.Hash, HexFormatAlpha.Auto)
                : base.ConvertTo(context, culture, value, destinationType);
    }
}
