namespace MediaConverter
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnInput;
        private System.Windows.Forms.Button btnOutput;
        private System.Windows.Forms.ComboBox cmbFormats;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.CheckBox chkShowLog;
        private System.Windows.Forms.Label lblTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtInput = new System.Windows.Forms.TextBox();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnInput = new System.Windows.Forms.Button();
            this.btnOutput = new System.Windows.Forms.Button();
            this.cmbFormats = new System.Windows.Forms.ComboBox();
            this.btnConvert = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.chkShowLog = new System.Windows.Forms.CheckBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // ======= Главное окно =======
            this.BackColor = System.Drawing.Color.FromArgb(20, 20, 25);
            this.ClientSize = new System.Drawing.Size(660, 440);
            this.Text = "🎧 Media Converter";
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ForeColor = System.Drawing.Color.Silver;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // ======= Заголовок =======
            this.lblTitle.Text = "🎶 Media Converter";
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(130, 180, 255);
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(25, 20);

            // ======= Поле ввода =======
            this.txtInput.Location = new System.Drawing.Point(30, 70);
            this.txtInput.Size = new System.Drawing.Size(510, 27);
            this.txtInput.BackColor = System.Drawing.Color.FromArgb(35, 35, 40);
            this.txtInput.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txtInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.btnInput.Text = "📂";
            this.btnInput.Location = new System.Drawing.Point(550, 70);
            this.btnInput.Size = new System.Drawing.Size(45, 27);
            this.btnInput.BackColor = System.Drawing.Color.FromArgb(40, 40, 50);
            this.btnInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInput.FlatAppearance.BorderSize = 0;
            this.btnInput.ForeColor = System.Drawing.Color.White;
            this.btnInput.Click += new System.EventHandler(this.btnInput_Click);

            // ======= Поле вывода =======
            this.txtOutput.Location = new System.Drawing.Point(30, 115);
            this.txtOutput.Size = new System.Drawing.Size(510, 27);
            this.txtOutput.BackColor = System.Drawing.Color.FromArgb(35, 35, 40);
            this.txtOutput.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txtOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.btnOutput.Text = "💾";
            this.btnOutput.Location = new System.Drawing.Point(550, 115);
            this.btnOutput.Size = new System.Drawing.Size(45, 27);
            this.btnOutput.BackColor = System.Drawing.Color.FromArgb(40, 40, 50);
            this.btnOutput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOutput.FlatAppearance.BorderSize = 0;
            this.btnOutput.ForeColor = System.Drawing.Color.White;
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);

            // ======= Форматы =======
            this.cmbFormats.Location = new System.Drawing.Point(30, 165);
            this.cmbFormats.Size = new System.Drawing.Size(220, 27);
            this.cmbFormats.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFormats.BackColor = System.Drawing.Color.FromArgb(35, 35, 40);
            this.cmbFormats.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.cmbFormats.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            // ======= Кнопка "Конвертировать" =======
            this.btnConvert.Text = "✨ Конвертировать";
            this.btnConvert.BackColor = System.Drawing.Color.FromArgb(90, 160, 255); // Мягкий голубой
            this.btnConvert.ForeColor = System.Drawing.Color.White;
            this.btnConvert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConvert.FlatAppearance.BorderSize = 0;
            this.btnConvert.Location = new System.Drawing.Point(270, 165);
            this.btnConvert.Size = new System.Drawing.Size(325, 38);
            this.btnConvert.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);

            // ======= Чекбокс логов =======
            this.chkShowLog.Text = "🪶 Показать логи";
            this.chkShowLog.ForeColor = System.Drawing.Color.LightGray;
            this.chkShowLog.AutoSize = true;
            this.chkShowLog.Location = new System.Drawing.Point(30, 220);
            this.chkShowLog.CheckedChanged += new System.EventHandler(this.chkShowLog_CheckedChanged);

            // ======= Поле логов =======
            this.txtLog.Location = new System.Drawing.Point(30, 250);
            this.txtLog.Size = new System.Drawing.Size(600, 150);
            this.txtLog.Multiline = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.ReadOnly = true;
            this.txtLog.BackColor = System.Drawing.Color.FromArgb(30, 30, 35);
            this.txtLog.ForeColor = System.Drawing.Color.Gainsboro;
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLog.Visible = false;

            // ======= Добавляем элементы =======
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.btnInput);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnOutput);
            this.Controls.Add(this.cmbFormats);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.chkShowLog);
            this.Controls.Add(this.txtLog);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
