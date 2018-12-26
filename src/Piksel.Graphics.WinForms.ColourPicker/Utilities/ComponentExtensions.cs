using System.ComponentModel;
using System.Windows.Forms;

namespace Piksel.Graphics.ColourPicker.Utilities
{
    public static class ComponentExtensions
    {
        public static bool IsInDesignMode(this IComponent component)
            => component?.Site is ISite site ? site.DesignMode
            : (component is Control control) ? control.Parent.IsInDesignMode()
            : false;
    }
}
