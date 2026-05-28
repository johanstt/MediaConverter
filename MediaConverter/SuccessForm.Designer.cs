namespace MediaConverter
{
    partial class SuccessForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Label lblElapsed;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.Button btnClose;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblSummary = new System.Windows.Forms.Label();
            this.lblElapsed = new System.Windows.Forms.Label();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(18, 28, 50);
            this.ClientSize = new System.Drawing.Size(480, 210);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.Text = "Media Converter";

            this.lblMessage.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.lblMessage.ForeColor = System.Drawing.Color.FromArgb(131, 255, 181);
            this.lblMessage.Location = new System.Drawing.Point(24, 26);
            this.lblMessage.Size = new System.Drawing.Size(432, 30);
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.lblSummary.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSummary.ForeColor = System.Drawing.Color.FromArgb(208, 222, 242);
            this.lblSummary.Location = new System.Drawing.Point(24, 68);
            this.lblSummary.Size = new System.Drawing.Size(432, 22);

            this.lblElapsed.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblElapsed.ForeColor = System.Drawing.Color.FromArgb(158, 179, 210);
            this.lblElapsed.Location = new System.Drawing.Point(24, 96);
            this.lblElapsed.Size = new System.Drawing.Size(432, 20);

            this.btnOpenFolder.Location = new System.Drawing.Point(24, 152);
            this.btnOpenFolder.Size = new System.Drawing.Size(160, 38);
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);

            this.btnClose.Location = new System.Drawing.Point(366, 152);
            this.btnClose.Size = new System.Drawing.Size(90, 38);
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblSummary);
            this.Controls.Add(this.lblElapsed);
            this.Controls.Add(this.btnOpenFolder);
            this.Controls.Add(this.btnClose);

            this.ResumeLayout(false);
        }
    }
}
