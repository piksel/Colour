using System;
using System.Collections.Generic;
using System.Text;

namespace Piksel.Graphics.ColourSpaces
{
    public static class ColourFieldMap
    {
        public static FieldMapColourDelegate GetFieldMapColourDelegate(ColourField field, byte fieldValue)
        {
            switch (field)
            {
                case ColourField.Red: return (x, y) => new Colour(fieldValue, x, y);
                case ColourField.Green: return (x, y) => new Colour(y, fieldValue, x);
                case ColourField.Blue: return (x, y) => new Colour(x , y, fieldValue);
                case ColourField.Hue:
                case ColourField.Saturation:
                case ColourField.Brightness:
                // TODO: Add HSB 
                default:
                    return (x, y) => new Colour(0, 0, 0); 
            }
        }

        public delegate Colour FieldMapColourDelegate(byte x, byte y);
    }
}