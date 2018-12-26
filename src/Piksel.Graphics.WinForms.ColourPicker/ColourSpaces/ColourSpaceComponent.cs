using Piksel.Graphics.ColourSpaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Piksel.Graphics.ColourPicker.Controls
{

    // TODO: move all textbox functionality into the ColourSpaceComponentTextBox
    // class.
    [DesignTimeVisible(false)]
    public class ColourSpaceComponent : UserControl
    {

        // controls		
        private ColourSpaceComponentTextBox txtComponentValue;
        private Label lblComponent;
        private Label lblComponentUnit;
        private RadioButton rdoComponent;

        // events
        public event ColourSpaceComponentEventHandler ComponentSelected;
        public event ColourSpaceComponentEventHandler ComponentTextKeyUp;

        // member data fields
        char displayCharacter;
        ComponentUnit unit = ComponentUnit.Byte;
        int value;
        int minimumValue;
        int maximumValue = 255;
        readonly string name = String.Empty;

        public bool Selected
        {
            get { return rdoComponent.Checked; }
            set
            {
                rdoComponent.Checked = value;
                BackColor = value ? SystemColors.Highlight : DefaultBackColor;
                ForeColor = value ? SystemColors.HighlightText : DefaultForeColor;
            }
        }

        public bool RadioButtonVisible
        {
            get { return rdoComponent.Visible; }
            set
            {
                rdoComponent.Visible = value;
                lblComponent.Visible = !value;
            }
        }

        public bool ReadOnly
        {
            get { return txtComponentValue.ReadOnly; }
            set
            {
                txtComponentValue.ReadOnly = value;
                txtComponentValue.BackColor = Color.White;
            }
        }

        public ComponentUnit Unit
        {

            get { return unit; }
            set
            {
                unit = value;
                lblComponentUnit.Text = value.Suffix;
            }

        }

        public byte ByteValue => (byte)(Math.Min(255, Math.Max(0, Value)));

        public int Value
        {

            get
            {

                int currentValue;

                if (txtComponentValue.Text.Length == 0)
                {
                    currentValue = 0;
                }
                else
                {
                    currentValue = Int32.Parse(this.txtComponentValue.Text);

                    if (currentValue > this.MaximumValue)
                    {
                        currentValue = Int32.Parse(this.txtComponentValue.Text.Substring(0, (this.txtComponentValue.Text.Length - 1)));
                    }
                }

                return currentValue;

            }

            set
            {

                this.value = value;
                txtComponentValue.Text = value.ToString();
                txtComponentValue.SelectionStart = txtComponentValue.Text.Length;

            }

        }

        [DefaultValue(0)]
        public int MinimumValue
        {
            get { return minimumValue; }
            set { minimumValue = value; }
        }

        [DefaultValue(255)]
        public int MaximumValue
        {
            get { return maximumValue; }
            set { maximumValue = value; }
        }

        public char DisplayCharacter
        {
            get { return displayCharacter; }

            set
            {

                displayCharacter = value;
                this.rdoComponent.Text = String.Format("{0}:", displayCharacter.ToString().ToUpper());
                this.lblComponent.Text = String.Format("{0}:", displayCharacter.ToString().ToUpper());

            }
        }

        public virtual double GetSliderValue()
            => (((double)Value - MinimumValue) / (MaximumValue - MinimumValue));

        public virtual int SetSliderValue(double value)
            => Value = MinimumValue + (int)(value * (MaximumValue - MinimumValue));



        #region Constructor

        internal ColourSpaceComponent() : base()
        {
            InitializeComponent();

            txtComponentValue.MouseWheel += TxtComponentValue_MouseWheel;
        }

        private void TxtComponentValue_MouseWheel(object sender, MouseEventArgs e)
        {
            //var delta = e.Delta / SystemInformation.MouseWheelScrollDelta;
            if (e.Delta != 0)
            {
                ChangeComponentValue(e.Delta > 0, ModifierKeys.HasFlag(Keys.Shift));
            }
        }

        #endregion Constructor

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.lblComponentUnit = new System.Windows.Forms.Label();
            this.rdoComponent = new System.Windows.Forms.RadioButton();
            this.lblComponent = new System.Windows.Forms.Label();
            this.SuspendLayout();
            this.txtComponentValue = new ColourSpaceComponentTextBox();

            // 
            // lblComponentUnit
            // 
            this.lblComponentUnit.Location = new System.Drawing.Point(80, 6);
            this.lblComponentUnit.Name = "lblComponentUnit";
            this.lblComponentUnit.Size = new System.Drawing.Size(16, 16);
            this.lblComponentUnit.TabIndex = 49;
            this.lblComponentUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtComponentValue
            // 
            this.txtComponentValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtComponentValue.Location = new System.Drawing.Point(40, 3);
            this.txtComponentValue.MaxLength = 3;
            this.txtComponentValue.Name = "txtComponentValue";
            this.txtComponentValue.Size = new System.Drawing.Size(40, 20);
            this.txtComponentValue.TabIndex = 0;
            this.txtComponentValue.Text = "";
            this.txtComponentValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtComponentValue_KeyDown);
            this.txtComponentValue.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtComponentValue_KeyUp);
            this.txtComponentValue.LostFocus += new System.EventHandler(this.txtComponentValue_LostFocus);
            // 
            // rdoComponent
            // 
            this.rdoComponent.Location = new System.Drawing.Point(4, 2);
            this.rdoComponent.Name = "rdoComponent";
            this.rdoComponent.Size = new System.Drawing.Size(36, 24);
            this.rdoComponent.TabIndex = 50;
            this.rdoComponent.Click += new System.EventHandler(this.rdoComponent_Click);
            // 
            // lblComponent
            // 
            this.lblComponent.Location = new System.Drawing.Point(18, 2);
            this.lblComponent.Name = "lblComponent";
            this.lblComponent.Size = new System.Drawing.Size(20, 24);
            this.lblComponent.TabIndex = 51;
            this.lblComponent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblComponent.Visible = false;
            // 
            // ColourSpaceComponent
            // 
            this.Controls.Add(this.lblComponent);
            this.Controls.Add(this.lblComponentUnit);
            this.Controls.Add(this.txtComponentValue);
            this.Controls.Add(this.rdoComponent);
            this.Name = "ColourSpaceComponent";
            this.Size = new System.Drawing.Size(112, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// Handles the Click event that the rdoComponent object raises.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs containing the event data.</param>

        private void rdoComponent_Click(object sender, EventArgs e)
        {

            RadioButton rb = (RadioButton)sender;

            if (rb.Checked)
            {
                OnComponentSelected(EventArgs.Empty);
            }

        }

        /// <summary>
        /// Handles the LostFocus event that the txtComponentValue object 
        /// raises.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs containing the event data.</param>

        private void txtComponentValue_LostFocus(object sender, EventArgs e)
        {

            ColourSpaceComponentTextBox textbox = (ColourSpaceComponentTextBox)sender;

            int componentValue;
            int resetValue;
            bool showError = true;

            if (textbox.Text != null && textbox.Text.Length != 0)
            {

                componentValue = Int32.Parse(textbox.Text);
                resetValue = componentValue;

                if ((componentValue > this.MaximumValue))
                {
                    resetValue = this.MaximumValue;
                }
                else if (componentValue < this.MinimumValue)
                {
                    resetValue = this.MinimumValue;
                }
                else
                {
                    showError = false;
                }

            }
            else
            {
                resetValue = this.MinimumValue;
            }

            if (showError)
            {
                MessageBox.Show(this, String.Format("An integer between {0} and {1} is required. Closest value inserted.", this.MinimumValue, this.MaximumValue), "Colour Picker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            textbox.Text = resetValue.ToString();

        }

        /// <summary>
        /// Raises the ComponentSelected event.
        /// </summary>
        /// <param name="e">An EventArgs containing the event data.</param>

        protected virtual void OnComponentSelected(EventArgs e) => ComponentSelected?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Raises the ComponentTextKeyUp event.
        /// </summary>
        /// <param name="e">An EventArgs containing the event data.</param>

        protected virtual void OnComponentTextKeyUp(EventArgs e) => ComponentTextKeyUp?.Invoke(this, EventArgs.Empty);

        private void txtComponentValue_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyData != Keys.Tab)
            {
                OnComponentTextKeyUp(EventArgs.Empty);
            }

        }

        private void txtComponentValue_KeyDown(object sender, KeyEventArgs e)
        {

            if (txtComponentValue.Text.Length > 0 &&
                ((e.KeyData | Keys.Shift) == (Keys.Up | Keys.Shift) ||
                (e.KeyData | Keys.Shift) == (Keys.Down | Keys.Shift)))
            {

                ChangeComponentValue(e.KeyData.HasFlag(Keys.Up), e.KeyData.HasFlag(Keys.Shift));

                // this is a hack to fix some of the problems with the key 
                // combination selecting part of the text (only happens
                // with shift+down).
                //
                // TODO: see if there is a better way to do this.

                if (e.KeyData == (Keys.Shift | Keys.Down))
                {
                    txtComponentValue.SelectionStart = txtComponentValue.Text.Length;
                }

            }

        }

        private void ChangeComponentValue(bool increase, bool x10)
        {
            int componentValue = Int16.Parse(txtComponentValue.Text);
            int incrementValue = x10 ? 10 : 1;

            if (increase)
            {
                componentValue = Math.Min(componentValue + incrementValue, MaximumValue);
            }
            else
            {
                componentValue = Math.Max(componentValue - incrementValue, MinimumValue);
            }

            txtComponentValue.Text = componentValue.ToString();
            ComponentTextKeyUp(this, EventArgs.Empty);
        }
    } // ColourSpaceComponent

}
