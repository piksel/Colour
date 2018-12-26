using System.Drawing;

namespace Piksel.Graphics.ColourPicker.ColourSwatches
{

    public struct ColourSwatch
    {
        private ColourPreset preset;

        public string Description => preset.Name;

        public Colour Colour => preset.Colour;

        public Point Location { get; set; }

        public Size Size { get; set; }

        public Rectangle Rectangle => new Rectangle(Location, Size);

        public ColourSwatch(ColourPreset preset) :
            this(preset, new Point(0, 0), new Size(10, 10))
        {

        }

        public ColourSwatch(ColourPreset preset, Point location, Size size)
        {

            this.preset = preset;
            Location = location;
            Size = size;

        }

        public override bool Equals(object obj)
        {

            ColourSwatch swatch = (ColourSwatch)obj;
            bool isEquals = false;

            if (Colour == swatch.Colour && Description.Equals(swatch.Description))
            {
                isEquals = true;
            }

            return isEquals;

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString() => $"Description: {Description}, Colour: {Colour}";

        public void SetBounds(Point location, Size size)
        {
            Location = location;
            Size = size;
        }
    }

}