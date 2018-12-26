using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Piksel.Graphics.ColourPicker.Controls
{

    /// <summary>
    /// Abstract base class for the Colour spaces used in the application.
    /// </summary>

    public abstract class ColourSpaceControl<TColourSpace> : UserControl, IColourSpaceControl
    {

        // events
        public event ColourSpaceEventHandler ComponentValueChanged;
        public event ColourSpaceEventHandler SelectedComponentChanged;

        private bool displayRadioButtons;
        private bool readOnly;

        /// <summary>
        /// Gets an ArrayList containing the components contained by this Colour
        /// space.
        /// </summary>

        public List<ColourSpaceComponent> ColourSpaceComponents { get; }

        /// <summary>
        /// Gets the currently selected Colour space component.
        /// </summary>

        public ColourSpaceComponent SelectedComponent { get; private set; }

        /// <summary>
        /// Gets or sets a value that indicates whether or not the radio 
        /// buttons in the Colour space components are displayed.
        /// </summary>

        public bool DisplayRadioButtons
        {
            get { return displayRadioButtons; }
            set
            {

                displayRadioButtons = value;

                foreach (var csc in ColourSpaceComponents)
                {
                    csc.RadioButtonVisible = displayRadioButtons;
                }

            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether or not the contents of
        /// the ColourSpaceComponent can be changed.
        /// </summary>

        public bool ReadOnly
        {
            get { return readOnly; }
            set
            {

                readOnly = value;

                foreach (var csc in ColourSpaceComponents)
                {
                    csc.ReadOnly = readOnly;
                }

            }

        }

        /// <summary>
        /// Constructor.
        /// </summary>

        protected ColourSpaceControl() : base()
        {

            ColourSpaceComponents = new List<ColourSpaceComponent>();
            displayRadioButtons = true;

        }

        /// <summary>
        /// Handles the ComponentSelected event that the ColourSpaceComponent
        /// raises.
        /// </summary>
        /// <param name="sender">The ColourSpaceComponent that raised the event.</param>
        /// <param name="e">An EventArgs containing the event data.</param>

        protected void ComponentSelected(ColourSpaceComponent sender, EventArgs e)
        {
            ChangeCurrentlySelectedComponent(sender);
        }

        /// <summary>
        /// Changes the currently selected Colour space component.
        /// </summary>
        /// <param name="component">A ColourSpaceComponent representing the 
        /// component that is to be set as the selected component.</param>

        public void ChangeCurrentlySelectedComponent(ColourSpaceComponent component)
        {

            // make sure none of the ColourSpaceComponents are checked.
            ResetComponents();

            component.Selected = true;
            SelectedComponent = component;

            OnSelectedComponentChanged(EventArgs.Empty);

        }

        /// <summary>
        /// Handles the ComponentTextKeyUp event that the ColourSpaceComponent
        /// raises.
        /// </summary>
        /// <param name="sender">The ColourSpaceComponent that raised the event.</param>
        /// <param name="e">An EventArgs containing the event data.</param>

        protected void ComponentTextKeyUp(ColourSpaceComponent sender, EventArgs e)
        {
            OnComponentValueChanged(e);
        }

        /// <summary>
        /// Sets the selected property of the Colour space components to false.
        /// </summary>

        internal void ResetComponents()
        {

            foreach (ColourSpaceComponent csc in ColourSpaceComponents)
            {
                csc.Selected = false;
            }

        }

        /// <summary>
        /// Raises the SelectedComponentChanged event.
        /// </summary>
        /// <param name="e">An EventArgs containing event data.</param>

        protected void OnSelectedComponentChanged(EventArgs e)
            => SelectedComponentChanged?.Invoke(this, e);



        /// <summary>
        /// Raises the ComponentValueChanged event.
        /// </summary>
        /// <param name="e">An EventArgs containing event data.</param>

        protected void OnComponentValueChanged(EventArgs e)
            => ComponentValueChanged?.Invoke(this, e);

        /// <summary>
        /// Sets the default selected component.
        /// </summary>

        public abstract void SetDefaultSelection();

        /// <summary>
        /// Returns a Colour object representing the Colour that the Colour 
        /// space's coordinates represent.
        /// </summary>

        public abstract Colour GetColour();

        /// <summary>
        /// Updates the Colour space coordinate values.
        /// </summary>
        /// <param name="csStructure">A IColourSpaceStructure object containing
        /// the coordinate values that the Colour space is to be updated with.</param>

        protected abstract void UpdateValues(TColourSpace csStructure);

        /// <summary>
        /// Gets or sets the IColourSpace based object that contains the 
        /// coordinate values of the components in the Colour space.
        /// </summary>

        internal abstract TColourSpace Structure { get; set; }

    } // ColourSpace

}
