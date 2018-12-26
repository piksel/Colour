
using System;

namespace Piksel.Graphics.ColourPicker.Controls
{

    public class ColourSelectedEventArgs : EventArgs
    {

        public Colour Colour { get; }

        public ColourSelectedEventArgs(Colour colour)
        {
            Colour = colour;
        }

    }

}