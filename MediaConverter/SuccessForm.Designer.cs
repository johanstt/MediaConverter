namespace MediaConverter
{
    partial class SuccessForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblMessage;
        private Label lblSubMessage;
        private Button btnOk;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblSubMessage = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SuccessForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(25, 25, 25);
            this.ClientSize = new System.Drawing.Size(400, 210);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Готово!";
            this.ShowInTaskbar = false;

            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.lblMessage.ForeColor = Color.White;
            this.lblMessage.Text = "Конвертация прошла успешно!";
            this.lblMessage.TextAlign = ContentAlignment.MiddleCenter;
            this.lblMessage.Location = new Point((this.ClientSize.Width - 330) / 2, 55);
            this.lblMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // 
            // lblSubMessage
            // 
            this.lblSubMessage.AutoSize = true;
            this.lblSubMessage.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.lblSubMessage.ForeColor = Color.Silver;
            this.lblSubMessage.Text = "Файл готов к использованию 🎧";
            this.lblSubMessage.TextAlign = ContentAlignment.MiddleCenter;
            this.lblSubMessage.Location = new Point((this.ClientSize.Width - 260) / 2, 90);
            this.lblSubMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // 
            // btnOk
            // 
            this.btnOk.Text = "Понял 😎";
            this.btnOk.Size = new Size(110, 35);
            this.btnOk.Location = new Point((this.ClientSize.Width - this.btnOk.Width) / 2, 140);
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);

            // 
            // SuccessForm Controls
            // 
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblSubMessage);
            this.Controls.Add(this.btnOk);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
    }
}
