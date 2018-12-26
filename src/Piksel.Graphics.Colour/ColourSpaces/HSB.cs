namespace Piksel.Graphics.ColourSpaces
{

    public class HSB : IColourSpace
    {
        public int Hue { get; set; }
        public byte Saturation { get; set; }
        public byte Brightness { get; set; }

        public HSB(int hue, byte saturation, byte brightness)
        {
            Hue = hue;
            Saturation = saturation;
            Brightness = brightness;
        }

        public override string ToString()
            => $"<HSBColour(Hue: {Hue}; Saturation: {Saturation}; Brightness: {Brightness})>";

        public override bool Equals(object obj)
            => obj is HSB hsb
                ? Hue == hsb.Hue
                    && Saturation == hsb.Saturation
                    && Brightness == hsb.Brightness
                : base.Equals(obj);

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }

}
