
using System;

namespace Piksel.Graphics.ColourSpaces
{

	public class RGB : IColourSpace {
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }

        public RGB(byte red, byte green, byte blue )
        {
			Red = red;
			Green = green;
			Blue = blue;
		}

        public override string ToString() 
            => $"<RGBColour(Red: {Red}; Green: {Green}; Blue: {Blue})>";

        public override bool Equals( object obj )
            => obj is RGB rgb
                ? Red == rgb.Red
                    && Green == rgb.Green
                    && Blue == rgb.Blue
                : base.Equals(obj);


        public override int GetHashCode() {
			return base.GetHashCode ();
		}


	} // RGB

} // Sano.PersonalProjects.ColorPicker.Controls
