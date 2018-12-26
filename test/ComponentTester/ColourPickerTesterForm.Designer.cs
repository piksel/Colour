namespace ComponentTester
{
    partial class ColourPickerTesterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColourPickerTesterForm));
            this.colourSwatchPanel1 = new Piksel.Graphics.ColourPicker.Controls.ColourSwatchPanel();
            this.colourPanelControl2 = new Piksel.Graphics.ColourPicker.Controls.ColourPanelControl();
            this.SuspendLayout();
            // 
            // colourSwatchPanel1
            // 
            this.colourSwatchPanel1.AllowDrop = true;
            this.colourSwatchPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.colourSwatchPanel1.Location = new System.Drawing.Point(12, 290);
            this.colourSwatchPanel1.Name = "colourSwatchPanel1";
            this.colourSwatchPanel1.Size = new System.Drawing.Size(445, 138);
            this.colourSwatchPanel1.SwatchSize = 16;
            this.colourSwatchPanel1.TabIndex = 1;
            this.colourSwatchPanel1.ColourSwatchSelected += new Piksel.Graphics.ColourPicker.Controls.ColourSwatchSelectedHandler(this.colourSwatchPanel1_ColourSwatchSelected);
            // 
            // colourPanelControl2
            // 
            this.colourPanelControl2.AllowDrop = true;
            this.colourPanelControl2.Location = new System.Drawing.Point(12, 12);
            this.colourPanelControl2.Name = "colourPanelControl2";
            this.colourPanelControl2.Size = new System.Drawing.Size(460, 272);
            this.colourPanelControl2.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 440);
            this.Controls.Add(this.colourSwatchPanel1);
            this.Controls.Add(this.colourPanelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(484, 9999);
            this.MinimumSize = new System.Drawing.Size(484, 380);
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Piksel.Graphics.Colour - Colour Picker Example";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Piksel.Graphics.ColourPicker.Controls.ColourPanelControl colourPanelControl2;
        private Piksel.Graphics.ColourPicker.Controls.ColourSwatchPanel colourSwatchPanel1;
    }
}

