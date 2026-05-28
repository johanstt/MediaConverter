namespace MediaConverter
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        // Language
        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.ComboBox cmbLanguage;

        // Drop zone
        private System.Windows.Forms.Panel pnlDropZone;
        private System.Windows.Forms.Label lblDropIcon;
        private System.Windows.Forms.Label lblDropTitle;
        private System.Windows.Forms.Label lblDropSubtitle;
        private System.Windows.Forms.Button btnBrowseFile;

        // Input file
        private System.Windows.Forms.Label lblInputFile;
        private System.Windows.Forms.TextBox txtInputFile;
        private System.Windows.Forms.Button btnBrowseInput;

        // Output folder
        private System.Windows.Forms.Label lblOutputFolder;
        private System.Windows.Forms.TextBox txtOutputFolder;
        private System.Windows.Forms.Button btnBrowseOutput;
        private System.Windows.Forms.Button btnOpenOutputFolder;

        // Format pills
        private System.Windows.Forms.Label lblFormat;
        private System.Windows.Forms.Button btnFmtMP3;
        private System.Windows.Forms.Button btnFmtMP4;
        private System.Windows.Forms.Button btnFmtWAV;
        private System.Windows.Forms.Button btnFmtAAC;
        private System.Windows.Forms.Button btnFmtFLAC;

        // Profile
        private System.Windows.Forms.Label lblProfile;
        private System.Windows.Forms.ComboBox cmbProfile;
        private System.Windows.Forms.Button btnAdvSettings;

        // Convert
        private System.Windows.Forms.Button btnConvert;

        // Progress
        private System.Windows.Forms.Label lblProgressLabel;
        private RoundedProgressBar prgOverall;
        private System.Windows.Forms.Label lblProgressPct;
        private System.Windows.Forms.Button btnClear;

        // Log
        private System.Windows.Forms.CheckBox chkShowLog;
        private System.Windows.Forms.Panel pnlLog;
        private System.Windows.Forms.Panel pnlLogHeader;
        private System.Windows.Forms.Label lblLogsTitle;
        private System.Windows.Forms.Button btnToggleLog;
        private System.Windows.Forms.RichTextBox rtbLog;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblLanguage = new System.Windows.Forms.Label();
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.pnlDropZone = new System.Windows.Forms.Panel();
            this.lblDropIcon = new System.Windows.Forms.Label();
            this.lblDropTitle = new System.Windows.Forms.Label();
            this.lblDropSubtitle = new System.Windows.Forms.Label();
            this.btnBrowseFile = new System.Windows.Forms.Button();
            this.lblInputFile = new System.Windows.Forms.Label();
            this.txtInputFile = new System.Windows.Forms.TextBox();
            this.btnBrowseInput = new System.Windows.Forms.Button();
            this.lblOutputFolder = new System.Windows.Forms.Label();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.btnBrowseOutput = new System.Windows.Forms.Button();
            this.btnOpenOutputFolder = new System.Windows.Forms.Button();
            this.lblFormat = new System.Windows.Forms.Label();
            this.btnFmtMP3 = new System.Windows.Forms.Button();
            this.btnFmtMP4 = new System.Windows.Forms.Button();
            this.btnFmtWAV = new System.Windows.Forms.Button();
            this.btnFmtAAC = new System.Windows.Forms.Button();
            this.btnFmtFLAC = new System.Windows.Forms.Button();
            this.lblProfile = new System.Windows.Forms.Label();
            this.cmbProfile = new System.Windows.Forms.ComboBox();
            this.btnAdvSettings = new System.Windows.Forms.Button();
            this.btnConvert = new System.Windows.Forms.Button();
            this.lblProgressLabel = new System.Windows.Forms.Label();
            this.prgOverall = new RoundedProgressBar();
            this.lblProgressPct = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.chkShowLog = new System.Windows.Forms.CheckBox();
            this.pnlLog = new System.Windows.Forms.Panel();
            this.pnlLogHeader = new System.Windows.Forms.Panel();
            this.lblLogsTitle = new System.Windows.Forms.Label();
            this.btnToggleLog = new System.Windows.Forms.Button();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.pnlDropZone.SuspendLayout();
            this.pnlLog.SuspendLayout();
            this.pnlLogHeader.SuspendLayout();
            this.SuspendLayout();

            // ── Language ──────────────────────────────────────
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.ForeColor = System.Drawing.Color.FromArgb(140, 140, 200);
            this.lblLanguage.Location = new System.Drawing.Point(702, 17);
            this.lblLanguage.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblLanguage.Text = "Язык:";

            this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLanguage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbLanguage.Location = new System.Drawing.Point(750, 13);
            this.cmbLanguage.Size = new System.Drawing.Size(110, 26);

            // ── Drop zone ─────────────────────────────────────
            this.pnlDropZone.Anchor = System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right;
            this.pnlDropZone.Location = new System.Drawing.Point(20, 50);
            this.pnlDropZone.Size = new System.Drawing.Size(840, 152);
            this.pnlDropZone.Controls.Add(this.lblDropIcon);
            this.pnlDropZone.Controls.Add(this.lblDropTitle);
            this.pnlDropZone.Controls.Add(this.lblDropSubtitle);
            this.pnlDropZone.Controls.Add(this.btnBrowseFile);

            this.lblDropIcon.AutoSize = false;
            this.lblDropIcon.Font = new System.Drawing.Font("Segoe UI Symbol", 26F);
            this.lblDropIcon.ForeColor = System.Drawing.Color.FromArgb(108, 99, 255);
            this.lblDropIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDropIcon.Location = new System.Drawing.Point(0, 12);
            this.lblDropIcon.Size = new System.Drawing.Size(840, 42);
            this.lblDropIcon.Anchor = System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right;
            this.lblDropIcon.Text = "⬇";

            this.lblDropTitle.AutoSize = false;
            this.lblDropTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.lblDropTitle.ForeColor = System.Drawing.Color.FromArgb(225, 225, 255);
            this.lblDropTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDropTitle.Location = new System.Drawing.Point(0, 56);
            this.lblDropTitle.Size = new System.Drawing.Size(840, 26);
            this.lblDropTitle.Anchor = System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right;
            this.lblDropTitle.Text = "Перетащите файл сюда";

            this.lblDropSubtitle.AutoSize = false;
            this.lblDropSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDropSubtitle.ForeColor = System.Drawing.Color.FromArgb(130, 125, 190);
            this.lblDropSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDropSubtitle.Location = new System.Drawing.Point(0, 82);
            this.lblDropSubtitle.Size = new System.Drawing.Size(840, 20);
            this.lblDropSubtitle.Anchor = System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right;
            this.lblDropSubtitle.Text = "Поддерживаются видео и аудио форматы";

            this.btnBrowseFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseFile.Location = new System.Drawing.Point(340, 108);
            this.btnBrowseFile.Size = new System.Drawing.Size(160, 32);
            this.btnBrowseFile.Text = "⊞  Выбрать файл";
            this.btnBrowseFile.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnBrowseFile.Click += new System.EventHandler(this.btnBrowseFile_Click);

            // ── Input file row ────────────────────────────────
            this.lblInputFile.AutoSize = false;
            this.lblInputFile.ForeColor = System.Drawing.Color.FromArgb(155, 155, 215);
            this.lblInputFile.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblInputFile.Location = new System.Drawing.Point(20, 220);
            this.lblInputFile.Size = new System.Drawing.Size(120, 34);
            this.lblInputFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblInputFile.Text = "Входной файл";

            this.txtInputFile.Location = new System.Drawing.Point(148, 220);
            this.txtInputFile.Size = new System.Drawing.Size(692, 34);
            this.txtInputFile.Anchor = System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right;
            this.txtInputFile.ReadOnly = true;

            this.btnBrowseInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseInput.Location = new System.Drawing.Point(844, 220);
            this.btnBrowseInput.Size = new System.Drawing.Size(36, 34);
            this.btnBrowseInput.Text = "···";
            this.btnBrowseInput.Anchor = System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Right;
            this.btnBrowseInput.Click += new System.EventHandler(this.btnBrowseInput_Click);

            // ── Output folder row ─────────────────────────────
            this.lblOutputFolder.AutoSize = false;
            this.lblOutputFolder.ForeColor = System.Drawing.Color.FromArgb(155, 155, 215);
            this.lblOutputFolder.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblOutputFolder.Location = new System.Drawing.Point(20, 266);
            this.lblOutputFolder.Size = new System.Drawing.Size(120, 34);
            this.lblOutputFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblOutputFolder.Text = "Папка сохранения";

            this.txtOutputFolder.Location = new System.Drawing.Point(148, 266);
            this.txtOutputFolder.Size = new System.Drawing.Size(528, 34);
            this.txtOutputFolder.Anchor = System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right;

            this.btnBrowseOutput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseOutput.Location = new System.Drawing.Point(680, 266);
            this.btnBrowseOutput.Size = new System.Drawing.Size(36, 34);
            this.btnBrowseOutput.Text = "···";
            this.btnBrowseOutput.Anchor = System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Right;
            this.btnBrowseOutput.Click += new System.EventHandler(this.btnBrowseOutput_Click);

            this.btnOpenOutputFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenOutputFolder.Location = new System.Drawing.Point(720, 266);
            this.btnOpenOutputFolder.Size = new System.Drawing.Size(160, 34);
            this.btnOpenOutputFolder.Text = "⊡  Открыть папку";
            this.btnOpenOutputFolder.Anchor = System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Right;
            this.btnOpenOutputFolder.Click += new System.EventHandler(this.btnOpenOutputFolder_Click);

            // ── Format row ────────────────────────────────────
            this.lblFormat.AutoSize = false;
            this.lblFormat.ForeColor = System.Drawing.Color.FromArgb(155, 155, 215);
            this.lblFormat.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblFormat.Location = new System.Drawing.Point(20, 314);
            this.lblFormat.Size = new System.Drawing.Size(120, 38);
            this.lblFormat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblFormat.Text = "Формат";

            this.btnFmtMP3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFmtMP3.Location = new System.Drawing.Point(148, 314);
            this.btnFmtMP3.Size = new System.Drawing.Size(92, 38);
            this.btnFmtMP3.Text = "♫  MP3";

            this.btnFmtMP4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFmtMP4.Location = new System.Drawing.Point(248, 314);
            this.btnFmtMP4.Size = new System.Drawing.Size(92, 38);
            this.btnFmtMP4.Text = "▶  MP4";

            this.btnFmtWAV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFmtWAV.Location = new System.Drawing.Point(348, 314);
            this.btnFmtWAV.Size = new System.Drawing.Size(92, 38);
            this.btnFmtWAV.Text = "≋  WAV";

            this.btnFmtAAC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFmtAAC.Location = new System.Drawing.Point(448, 314);
            this.btnFmtAAC.Size = new System.Drawing.Size(92, 38);
            this.btnFmtAAC.Text = "♪  AAC";

            this.btnFmtFLAC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFmtFLAC.Location = new System.Drawing.Point(548, 314);
            this.btnFmtFLAC.Size = new System.Drawing.Size(92, 38);
            this.btnFmtFLAC.Text = "◈  FLAC";

            // ── Profile row ───────────────────────────────────
            this.lblProfile.AutoSize = false;
            this.lblProfile.ForeColor = System.Drawing.Color.FromArgb(155, 155, 215);
            this.lblProfile.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblProfile.Location = new System.Drawing.Point(20, 364);
            this.lblProfile.Size = new System.Drawing.Size(120, 34);
            this.lblProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblProfile.Text = "Профиль";

            this.cmbProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbProfile.Location = new System.Drawing.Point(148, 364);
            this.cmbProfile.Size = new System.Drawing.Size(482, 34);
            this.cmbProfile.Anchor = System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right;

            this.btnAdvSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdvSettings.Location = new System.Drawing.Point(638, 364);
            this.btnAdvSettings.Size = new System.Drawing.Size(242, 34);
            this.btnAdvSettings.Text = "⚙  Доп. настройки";
            this.btnAdvSettings.Anchor = System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Right;
            this.btnAdvSettings.Click += new System.EventHandler(this.btnAdvSettings_Click);

            // ── Convert button ────────────────────────────────
            this.btnConvert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConvert.Location = new System.Drawing.Point(20, 418);
            this.btnConvert.Size = new System.Drawing.Size(840, 54);
            this.btnConvert.Anchor = System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right;
            this.btnConvert.Text = "⟳  Конвертировать";
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);

            // ── Progress row ──────────────────────────────────
            this.lblProgressLabel.AutoSize = false;
            this.lblProgressLabel.ForeColor = System.Drawing.Color.FromArgb(155, 155, 215);
            this.lblProgressLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblProgressLabel.Location = new System.Drawing.Point(20, 490);
            this.lblProgressLabel.Size = new System.Drawing.Size(80, 28);
            this.lblProgressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblProgressLabel.Text = "Прогресс";

            this.prgOverall.Location = new System.Drawing.Point(106, 496);
            this.prgOverall.Size = new System.Drawing.Size(644, 16);
            this.prgOverall.Anchor = System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right;
            this.prgOverall.Minimum = 0;
            this.prgOverall.Maximum = 100;
            this.prgOverall.Value = 0;

            this.lblProgressPct.AutoSize = false;
            this.lblProgressPct.ForeColor = System.Drawing.Color.FromArgb(200, 200, 248);
            this.lblProgressPct.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblProgressPct.Location = new System.Drawing.Point(754, 490);
            this.lblProgressPct.Size = new System.Drawing.Size(50, 28);
            this.lblProgressPct.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblProgressPct.Anchor = System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Right;
            this.lblProgressPct.Text = "0%";

            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Location = new System.Drawing.Point(808, 486);
            this.btnClear.Size = new System.Drawing.Size(52, 36);
            this.btnClear.Text = "✕";
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Right;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);

            // ── Log section ───────────────────────────────────
            this.chkShowLog.AutoSize = true;
            this.chkShowLog.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkShowLog.Location = new System.Drawing.Point(20, 540);
            this.chkShowLog.Text = "Показать лог";
            this.chkShowLog.CheckedChanged += new System.EventHandler(this.chkShowLog_CheckedChanged);

            this.pnlLog.Anchor = System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Left
                | System.Windows.Forms.AnchorStyles.Right
                | System.Windows.Forms.AnchorStyles.Bottom;
            this.pnlLog.Location = new System.Drawing.Point(20, 568);
            this.pnlLog.Size = new System.Drawing.Size(840, 172);
            this.pnlLog.Controls.Add(this.pnlLogHeader);
            this.pnlLog.Controls.Add(this.rtbLog);

            this.pnlLogHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLogHeader.Height = 36;
            this.pnlLogHeader.BackColor = System.Drawing.Color.FromArgb(20, 20, 44);
            this.pnlLogHeader.Controls.Add(this.lblLogsTitle);
            this.pnlLogHeader.Controls.Add(this.btnToggleLog);

            this.lblLogsTitle.AutoSize = false;
            this.lblLogsTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblLogsTitle.ForeColor = System.Drawing.Color.FromArgb(175, 170, 235);
            this.lblLogsTitle.Location = new System.Drawing.Point(14, 0);
            this.lblLogsTitle.Size = new System.Drawing.Size(80, 36);
            this.lblLogsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLogsTitle.Text = "  Логи";

            this.btnToggleLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleLog.Anchor = System.Windows.Forms.AnchorStyles.Top
                | System.Windows.Forms.AnchorStyles.Right;
            this.btnToggleLog.Location = new System.Drawing.Point(804, 4);
            this.btnToggleLog.Size = new System.Drawing.Size(32, 28);
            this.btnToggleLog.Text = "∧";
            this.btnToggleLog.Click += new System.EventHandler(this.btnToggleLog_Click);

            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;

            // ── Form ──────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(10, 10, 22);
            this.ClientSize = new System.Drawing.Size(880, 760);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MinimumSize = new System.Drawing.Size(780, 680);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Media Converter";

            this.Controls.Add(this.lblLanguage);
            this.Controls.Add(this.cmbLanguage);
            this.Controls.Add(this.pnlDropZone);
            this.Controls.Add(this.lblInputFile);
            this.Controls.Add(this.txtInputFile);
            this.Controls.Add(this.btnBrowseInput);
            this.Controls.Add(this.lblOutputFolder);
            this.Controls.Add(this.txtOutputFolder);
            this.Controls.Add(this.btnBrowseOutput);
            this.Controls.Add(this.btnOpenOutputFolder);
            this.Controls.Add(this.lblFormat);
            this.Controls.Add(this.btnFmtMP3);
            this.Controls.Add(this.btnFmtMP4);
            this.Controls.Add(this.btnFmtWAV);
            this.Controls.Add(this.btnFmtAAC);
            this.Controls.Add(this.btnFmtFLAC);
            this.Controls.Add(this.lblProfile);
            this.Controls.Add(this.cmbProfile);
            this.Controls.Add(this.btnAdvSettings);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.lblProgressLabel);
            this.Controls.Add(this.prgOverall);
            this.Controls.Add(this.lblProgressPct);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.chkShowLog);
            this.Controls.Add(this.pnlLog);

            this.pnlDropZone.ResumeLayout(false);
            this.pnlLogHeader.ResumeLayout(false);
            this.pnlLog.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
