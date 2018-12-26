namespace Piksel.Graphics
{
    public struct ColourPreset
    {
        public string Name { get; }

        public Colour Colour { get; }

        public ColourPreset(Colour colour, string name)
        {
            Colour = colour;
            Name = name;
        }
    }
}
