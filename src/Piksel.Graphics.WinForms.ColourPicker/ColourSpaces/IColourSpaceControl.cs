using System.Collections.Generic;

namespace Piksel.Graphics.ColourPicker.Controls
{
    public interface IColourSpaceControl
    {
        List<ColourSpaceComponent> ColourSpaceComponents { get; }

        ColourSpaceComponent SelectedComponent { get; }
        //double Value { get; }

        void SetDefaultSelection();
        Colour GetColour();
    }
}