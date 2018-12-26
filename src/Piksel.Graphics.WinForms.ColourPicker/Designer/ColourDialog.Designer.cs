namespace Piksel.Graphics.ColourPicker.Designer
{
    partial class ColourDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColourDialog));
            this.colourPanelControl2 = new Piksel.Graphics.ColourPicker.Controls.ColourPanelControl();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // colourPanelControl2
            // 
            this.colourPanelControl2.AllowDrop = true;
            this.colourPanelControl2.Location = new System.Drawing.Point(12, 12);
            this.colourPanelControl2.Name = "colourPanelControl2";
            this.colourPanelControl2.Size = new System.Drawing.Size(460, 272);
            this.colourPanelControl2.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(358, 298);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // ColourEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 333);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.colourPanelControl2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColourEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select colour...";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ColourPanelControl colourPanelControl2;
        private System.Windows.Forms.Button button1;
    }
}