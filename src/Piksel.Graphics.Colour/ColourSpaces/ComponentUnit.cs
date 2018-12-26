
using System;

namespace Piksel.Graphics.ColourSpaces
{

	public enum ComponentUnitType {
		Degree,
		Percentage,
		Byte
	}

    public struct ComponentUnit
    {
        public static ComponentUnit Degree = new ComponentUnit(ComponentUnitType.Degree, 0, 359);
        public static ComponentUnit Percentage = new ComponentUnit(ComponentUnitType.Percentage, 0, 100);
        public static ComponentUnit Byte = new ComponentUnit(ComponentUnitType.Byte, 0, 255);

        private ComponentUnit(ComponentUnitType type, int minValue, int maxValue)
        {
            Type = type;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public ComponentUnitType Type { get; }
        public int MinValue { get; }
        public int MaxValue { get; }

        public string Suffix =>
            Type == ComponentUnitType.Percentage ? "%" :
            Type == ComponentUnitType.Degree ? "°" :
            Type == ComponentUnitType.Byte ? "" : 
            string.Empty;

    }

}