using System.Windows.Forms;

namespace Piksel.Graphics.ColourPicker.Controls
{

    // Used as a modal dialog that collects the Colour description from the 
    // user.

    internal class AddNewColourSwatchForm : Form
    {

        // controls
        private TextBox txtColourDescription;
        private Button btnOk;
        private PictureBox picColour;
        private Button btnCancel;
        private Label lblDescription;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="ColourToAdd">The Colour that the picture box's BackColour
        /// property is to be set to.</param>

        internal AddNewColourSwatchForm(Colour ColourToAdd) : base()
        {

            InitializeComponent();
            picColour.BackColor = ColourToAdd;

        }

        /// <summary>
        /// Gets the Colour description.
        /// </summary>

        internal string ColourDescription
        {
            get { return txtColourDescription.Text; }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>

        private void InitializeComponent()
        {
            this.picColour = new System.Windows.Forms.PictureBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtColourDescription = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // picColour
            // 
            this.picColour.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picColour.Location = new System.Drawing.Point(12, 8);
            this.picColour.Name = "picColour";
            this.picColour.Size = new System.Drawing.Size(50, 50);
            this.picColour.TabIndex = 0;
            this.picColour.TabStop = false;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(79, 14);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(184, 23);
            this.lblDescription.TabIndex = 3;
            this.lblDescription.Text = "Enter Colour description:";
            // 
            // txtColourDescription
            // 
            this.txtColourDescription.Location = new System.Drawing.Point(79, 33);
            this.txtColourDescription.MaxLength = 100;
            this.txtColourDescription.Name = "txtColourDescription";
            this.txtColourDescription.Size = new System.Drawing.Size(208, 20);
            this.txtColourDescription.TabIndex = 0;
            this.txtColourDescription.Text = "";
            this.txtColourDescription.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtColourDescription_KeyUp);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(304, 35);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(304, 8);
            this.btnOk.Name = "btnOk";
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // AddNewColourSwatchForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(394, 66);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtColourDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.picColour);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddNewColourSwatchForm";
            this.Text = "Add Colour";
            this.ResumeLayout(false);

        }

        #endregion

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void txtColourDescription_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyData == Keys.Enter)
            {

                this.DialogResult = DialogResult.OK;
                this.Close();

            }
            else if (e.KeyData == Keys.Escape)
            {

                this.DialogResult = DialogResult.Cancel;
                this.Close();

            }


        }

    } // AddNewColourSwatchForm

}