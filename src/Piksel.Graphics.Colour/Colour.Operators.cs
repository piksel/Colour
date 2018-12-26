using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Piksel.Graphics
{
    public partial struct Colour
    {
        public static implicit operator Color(Colour colour)
            => Color.FromArgb(unchecked((int)colour.value));

        public static implicit operator Colour(Color color)
            => Colour.FromArgb(unchecked((uint)color.ToArgb()));

        public static bool operator ==(Colour ca, Colour cb)
            => ca.value == cb.value;

        public static bool operator !=(Colour ca, Colour cb)
            => ca.value != cb.value;

        public static bool operator ==(Colour ca, Color cb)
            => ca.value == unchecked((uint)cb.ToArgb());

        public static bool operator !=(Colour ca, Color cb)
            => ca.value != unchecked((uint)cb.ToArgb());

        public static bool operator ==(Color ca, Colour cb)
            => unchecked((uint)ca.ToArgb()) == cb.value;

        public static bool operator !=(Color ca, Colour cb)
            => unchecked((uint)ca.ToArgb()) != cb.value;


        public override int GetHashCode()
            => unchecked((int)value);

        public override bool Equals(object obj)
        {
            if (obj is Colour colour)
            {
                return colour.value == value;
            }
            return base.Equals(obj);
        }
    }
}
