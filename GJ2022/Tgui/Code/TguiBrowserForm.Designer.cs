namespace GJ2022.Tgui.Code
{
    partial class TguiBrowserForm
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
            this.EmbeddedBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // EmbeddedBrowser
            // 
            this.EmbeddedBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EmbeddedBrowser.Location = new System.Drawing.Point(0, 0);
            this.EmbeddedBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.EmbeddedBrowser.Name = "EmbeddedBrowser";
            this.EmbeddedBrowser.Size = new System.Drawing.Size(800, 450);
            this.EmbeddedBrowser.TabIndex = 0;
            // 
            // TguiBrowserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ControlBox = false;
            this.Controls.Add(this.EmbeddedBrowser);
            this.MaximizeBox = false;
            this.Name = "TguiBrowserForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "TguiBrowserForm";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser EmbeddedBrowser;
    }
}