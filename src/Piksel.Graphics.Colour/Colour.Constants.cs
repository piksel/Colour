using System;
using System.Collections.Generic;
using System.Text;

namespace Piksel.Graphics
{
    public partial struct Colour
    {
        const uint Opaque = 0xff000000;
        const uint Transparent = 0x000000;

        const uint ValueMask = 0xffffffff;
        const uint RedMask = 0xff00ffff;
        const uint GreenMask = 0xffff00ff;
        const uint BlueMask = 0xffffff00;
        const uint AlphaMask = 0x00ffffff;

        const int AlphaShift = 24;
        const int RedShift = 16;
        const int GreenShift = 8;
        const int BlueShift = 0;
    }
}
