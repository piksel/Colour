using System;
using System.Collections.Generic;
using System.Text;

namespace Piksel.Graphics
{
    public struct ColourPair
    {
        public Colour Primary { get; private set; }
        public Colour Secondary { get; private set; }

        public ColourPair(Colour primary, Colour secondary)
        {
            Primary = primary;
            Secondary = secondary;
        }
    }
}
