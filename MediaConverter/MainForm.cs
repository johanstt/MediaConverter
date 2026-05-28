using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace MediaConverter
{
    public partial class MainForm : Form
    {
        // ── State ─────────────────────────────────────────────
        private readonly ConverterService _converter = new();
        private CancellationTokenSource? _cts;
        private bool _isConverting;
        private bool _logExpanded = true;
        private bool _openFolderOnComplete;
        private string _inputFile = string.Empty;
        private string _selectedFormat = ".mp4";
        private AppLanguage _currentLanguage = AppLanguage.Russian;
        private Button[] _formatButtons = Array.Empty<Button>();
        private int _logCollapsedHeight;

        private bool UseRussian => _currentLanguage == AppLanguage.Russian;
        private string T(string ru, string en) => UseRussian ? ru : en;

        private static readonly (string Label, string Ext)[] Formats =
        {
            ("MP3",  ".mp3"),
            ("MP4",  ".mp4"),
            ("WAV",  ".wav"),
            ("AAC",  ".aac"),
            ("FLAC", ".flac"),
        };

        // ── Construction ──────────────────────────────────────
        public MainForm()
        {
            InitializeComponent();
            InitializeUi();
            Icon = CreateAppIcon();
        }

        private void InitializeUi()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            // Background
            BackColor = Color.FromArgb(10, 10, 22);

            // Drop zone paint + drag-drop
            AttachDropZonePaint();
            AllowDrop = true;
            DragEnter += Form_DragEnter;
            DragDrop  += Form_DragDrop;
            pnlDropZone.AllowDrop = true;
            pnlDropZone.DragEnter += Form_DragEnter;
            pnlDropZone.DragDrop  += Form_DragDrop;
            foreach (Control c in pnlDropZone.Controls)
            {
                c.AllowDrop = true;
                c.DragEnter += Form_DragEnter;
                c.DragDrop  += Form_DragDrop;
            }

            // Language combo
            StyleDarkComboBox(cmbLanguage);
            cmbLanguage.Items.AddRange(new object[] { "Русский", "English" });
            cmbLanguage.SelectedIndexChanged += cmbLanguage_SelectedIndexChanged;
            cmbLanguage.SelectedIndex = 0;

            // Profile combo
            StyleDarkComboBox(cmbProfile);

            // Format pills
            _formatButtons = new[] { btnFmtMP3, btnFmtMP4, btnFmtWAV, btnFmtAAC, btnFmtFLAC };
            for (int i = 0; i < _formatButtons.Length; i++)
            {
                var ext = Formats[i].Ext;
                _formatButtons[i].Click += (_, _) => SelectFormat(ext);
            }

            // Misc button styles
            StyleSecondaryButton(btnBrowseInput);
            StyleSecondaryButton(btnBrowseOutput);
            StyleSecondaryButton(btnOpenOutputFolder);
            StyleSecondaryButton(btnAdvSettings);
            StyleSecondaryButton(btnClear);
            StyleSecondaryButton(btnBrowseFile);
            StyleSecondaryButton(btnToggleLog);

            // Convert button
            StyleConvertButton(btnConvert);

            // Text boxes
            StyleTextBox(txtInputFile);
            StyleTextBox(txtOutputFolder);

            // Log
            rtbLog.BackColor = Color.FromArgb(11, 11, 24);
            rtbLog.ForeColor = Color.FromArgb(170, 170, 215);
            rtbLog.BorderStyle = BorderStyle.None;
            rtbLog.ReadOnly = true;
            rtbLog.Font = new Font("Consolas", 9F);

            // Log panel border
            pnlLog.BackColor = Color.FromArgb(14, 14, 30);
            pnlLog.Paint += (_, e) =>
            {
                using var pen = new Pen(Color.FromArgb(40, 40, 80), 1f);
                e.Graphics.DrawRectangle(pen, 0, 0, pnlLog.Width - 1, pnlLog.Height - 1);
            };

            // Show log checkbox
            chkShowLog.ForeColor = Color.FromArgb(150, 150, 210);
            chkShowLog.BackColor = Color.Transparent;
            chkShowLog.Checked = true;

            // Progress labels
            lblProgressLabel.ForeColor = Color.FromArgb(150, 150, 210);
            lblProgressPct.ForeColor = Color.FromArgb(200, 200, 248);

            // Transparent backgrounds for all labels on the form
            foreach (Control c in Controls)
                if (c is Label lbl) lbl.BackColor = Color.Transparent;
            foreach (Control c in pnlDropZone.Controls)
                c.BackColor = Color.Transparent;

            // Init state
            SelectFormat(".mp4");
            txtOutputFolder.Text = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "Converted");
            _logCollapsedHeight = pnlLogHeader.Height + 2;

            ApplyLanguage();
        }

        // ── Background ────────────────────────────────────────
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (Width < 2 || Height < 2) { base.OnPaintBackground(e); return; }
            var g = e.Graphics;
            g.CompositingQuality = CompositingQuality.HighQuality;
            using var grad = new LinearGradientBrush(ClientRectangle,
                Color.FromArgb(10, 10, 22), Color.FromArgb(6, 6, 14), 135F);
            g.FillRectangle(grad, ClientRectangle);
            int glowH = Math.Max(2, Height / 2);
            using var glow = new LinearGradientBrush(
                new Rectangle(0, 0, Math.Max(2, Width), glowH),
                Color.FromArgb(22, 14, 70), Color.FromArgb(0, 22, 14, 70), 90F);
            g.FillRectangle(glow, 0, 0, Width, glowH);
        }

        // ── Drop zone paint ───────────────────────────────────
        private void AttachDropZonePaint()
        {
            pnlDropZone.BackColor = Color.Transparent;
            pnlDropZone.Paint += (_, e) =>
            {
                if (pnlDropZone.Width < 8 || pnlDropZone.Height < 8) return;
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = new Rectangle(1, 1, pnlDropZone.Width - 2, pnlDropZone.Height - 2);
                using var path = RoundedRect(rect, 12);
                using var fill = new SolidBrush(Color.FromArgb(16, 16, 36));
                g.FillPath(fill, path);
                using var pen = new Pen(Color.FromArgb(72, 65, 160), 1.5f)
                {
                    DashStyle = DashStyle.Custom,
                    DashPattern = new float[] { 7, 4 }
                };
                g.DrawPath(pen, path);
            };
            pnlDropZone.Resize += (_, _) => pnlDropZone.Invalidate();
        }

        // ── Styling helpers ───────────────────────────────────
        private static void StyleConvertButton(Button b)
        {
            b.BackColor = Color.FromArgb(108, 99, 255);
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);
            b.Cursor = Cursors.Hand;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(128, 120, 255);
            b.FlatAppearance.MouseDownBackColor = Color.FromArgb(86, 78, 215);
        }

        private static void StyleSecondaryButton(Button b)
        {
            b.BackColor = Color.FromArgb(26, 26, 54);
            b.ForeColor = Color.FromArgb(195, 195, 238);
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderColor = Color.FromArgb(68, 65, 130);
            b.FlatAppearance.BorderSize = 1;
            b.Cursor = Cursors.Hand;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(36, 36, 72);
            b.FlatAppearance.MouseDownBackColor = Color.FromArgb(18, 18, 44);
        }

        private static void StyleFormatButton(Button b, bool selected)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.Cursor = Cursors.Hand;
            b.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            if (selected)
            {
                b.BackColor = Color.FromArgb(108, 99, 255);
                b.ForeColor = Color.White;
                b.FlatAppearance.BorderColor = Color.FromArgb(140, 132, 255);
                b.FlatAppearance.BorderSize = 1;
                b.FlatAppearance.MouseOverBackColor = Color.FromArgb(120, 112, 255);
                b.FlatAppearance.MouseDownBackColor = Color.FromArgb(88, 80, 220);
            }
            else
            {
                b.BackColor = Color.FromArgb(22, 22, 48);
                b.ForeColor = Color.FromArgb(180, 178, 235);
                b.FlatAppearance.BorderColor = Color.FromArgb(58, 56, 115);
                b.FlatAppearance.BorderSize = 1;
                b.FlatAppearance.MouseOverBackColor = Color.FromArgb(34, 32, 68);
                b.FlatAppearance.MouseDownBackColor = Color.FromArgb(16, 16, 38);
            }
        }

        private static void StyleDarkComboBox(ComboBox combo)
        {
            combo.DrawMode = DrawMode.OwnerDrawFixed;
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.FlatStyle = FlatStyle.Flat;
            combo.BackColor = Color.FromArgb(22, 22, 48);
            combo.ForeColor = Color.FromArgb(210, 210, 248);
            combo.IntegralHeight = false;
            combo.ItemHeight = 24;
            combo.DrawItem += ComboBox_DrawItem;
            combo.HandleCreated += (_, _) => SetWindowTheme(combo.Handle, string.Empty, string.Empty);
            if (combo.IsHandleCreated)
                SetWindowTheme(combo.Handle, string.Empty, string.Empty);
        }

        private static void ComboBox_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (sender is not ComboBox combo || e.Index < -1) return;
            bool sel = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            using var bg = new SolidBrush(sel ? Color.FromArgb(70, 60, 148) : Color.FromArgb(22, 22, 48));
            e.Graphics.FillRectangle(bg, e.Bounds);
            var text = e.Index >= 0 ? combo.Items[e.Index]?.ToString() ?? "" : combo.Text;
            TextRenderer.DrawText(e.Graphics, text, combo.Font,
                Rectangle.Inflate(e.Bounds, -6, 0),
                Color.FromArgb(210, 210, 248),
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        private static void StyleTextBox(TextBox t)
        {
            t.BackColor = Color.FromArgb(18, 18, 40);
            t.ForeColor = Color.FromArgb(210, 210, 250);
            t.BorderStyle = BorderStyle.FixedSingle;
            t.Font = new Font("Segoe UI", 10F);
        }

        // ── Format selection ──────────────────────────────────
        private void SelectFormat(string ext)
        {
            _selectedFormat = ext;
            for (int i = 0; i < Formats.Length; i++)
                StyleFormatButton(_formatButtons[i], Formats[i].Ext == ext);
            UpdateProfilesForFormat(ext);
        }

        private void UpdateProfilesForFormat(string ext)
        {
            bool audio = ext is ".mp3" or ".wav" or ".aac" or ".flac";
            cmbProfile.Items.Clear();
            if (audio)
            {
                cmbProfile.Items.Add(T("Как источник (рекомендуется)", "As source (recommended)"));
                cmbProfile.Items.Add(T("AAC — высокое качество", "AAC — high quality"));
                cmbProfile.Items.Add(T("MP3 — стандарт", "MP3 — standard"));
            }
            else
            {
                cmbProfile.Items.Add(T("Как источник (рекомендуется)", "As source (recommended)"));
                cmbProfile.Items.Add(T("H.264 / AAC — универсальный", "H.264 / AAC — universal"));
                cmbProfile.Items.Add(T("H.265 / AAC — компактный", "H.265 / AAC — compact"));
            }
            cmbProfile.SelectedIndex = 0;
        }

        private (string video, string audio) GetCodecs()
        {
            bool audio = _selectedFormat is ".mp3" or ".wav" or ".aac" or ".flac";
            int idx = Math.Max(0, cmbProfile.SelectedIndex);
            if (audio)
                return ("", idx == 2 ? "mp3" : idx == 1 ? "aac" : "copy");
            return idx switch
            {
                1 => ("h264", "aac"),
                2 => ("h265", "aac"),
                _ => ("copy", "copy")
            };
        }

        // ── Language ──────────────────────────────────────────
        private void cmbLanguage_SelectedIndexChanged(object? sender, EventArgs e)
        {
            _currentLanguage = cmbLanguage.SelectedIndex == 1
                ? AppLanguage.English : AppLanguage.Russian;
            ApplyLanguage();
        }

        private void ApplyLanguage()
        {
            Text = "Media Converter";
            lblDropTitle.Text    = T("Перетащите файл сюда", "Drop a file here");
            lblDropSubtitle.Text = T("Поддерживаются видео и аудио форматы", "Video and audio formats supported");
            btnBrowseFile.Text   = T("⊞  Выбрать файл", "⊞  Choose file");
            lblInputFile.Text    = T("Входной файл", "Input file");
            lblOutputFolder.Text = T("Папка сохранения", "Output folder");
            btnBrowseOutput.Text = "···";
            btnBrowseInput.Text  = "···";
            btnOpenOutputFolder.Text = T("⊡  Открыть папку", "⊡  Open folder");
            lblFormat.Text    = T("Формат", "Format");
            lblProfile.Text   = T("Профиль", "Profile");
            btnAdvSettings.Text = T("⚙  Доп. настройки", "⚙  Settings");
            lblProgressLabel.Text = T("Прогресс", "Progress");
            btnClear.Text     = "✕";
            chkShowLog.Text   = T("Показать лог", "Show log");
            lblLogsTitle.Text = T("  Логи", "  Log");
            lblLanguage.Text  = T("Язык:", "Lang:");
            lblLogsTitle.Text = T("  Логи", "  Log");

            if (!_isConverting)
                btnConvert.Text = T("⟳  Конвертировать", "⟳  Convert");

            UpdateProfilesForFormat(_selectedFormat);
        }

        // ── Convert ───────────────────────────────────────────
        private async void btnConvert_Click(object? sender, EventArgs e)
        {
            if (_isConverting)
            {
                _cts?.Cancel();
                return;
            }

            if (string.IsNullOrWhiteSpace(_inputFile) || !File.Exists(_inputFile))
            {
                MessageBox.Show(
                    T("Сначала выберите входной файл.", "Please select an input file first."),
                    "Media Converter", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var outFolder = txtOutputFolder.Text.Trim();
            if (string.IsNullOrWhiteSpace(outFolder))
            {
                MessageBox.Show(
                    T("Укажите папку для сохранения.", "Please specify an output folder."),
                    "Media Converter", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Directory.CreateDirectory(outFolder);

            var outPath = Path.Combine(outFolder,
                Path.GetFileNameWithoutExtension(_inputFile) + "_converted" + _selectedFormat);

            var (videoCodec, audioCodec) = GetCodecs();

            BeginConvertState();

            var batchWatch = Stopwatch.StartNew();
            bool success = false;

            try
            {
                var prog = new Progress<ConversionProgress>(p =>
                {
                    if (p.Percent.HasValue)
                    {
                        int pct = Math.Clamp((int)Math.Round(p.Percent.Value), 0, 100);
                        prgOverall.Value = pct;
                        lblProgressPct.Text = $"{pct}%";
                    }
                    if (!string.IsNullOrWhiteSpace(p.Message))
                        AppendLog(p.Message, LogLevel.Info);
                });

                AppendLog(T($"Начало конвертации: {Path.GetFileName(_inputFile)}",
                             $"Starting: {Path.GetFileName(_inputFile)}"), LogLevel.Info);

                var result = await _converter.ConvertAsync(
                    _inputFile, outPath, _selectedFormat,
                    videoCodec, audioCodec, UseRussian,
                    prog, msg => AppendLog(msg, LogLevel.Info),
                    _cts!.Token);

                prgOverall.Value = 100;
                lblProgressPct.Text = "100%";
                AppendLog(T($"Готово: {Path.GetFileName(result.OutputFile)}  ({result.Elapsed:mm\\:ss})",
                             $"Done: {Path.GetFileName(result.OutputFile)}  ({result.Elapsed:mm\\:ss})"),
                          LogLevel.Success);
                success = true;

                batchWatch.Stop();
                EndConvertState();

                if (_openFolderOnComplete && Directory.Exists(outFolder))
                {
                    Process.Start(new ProcessStartInfo { FileName = outFolder, UseShellExecute = true });
                }
                else
                {
                    using var sf = new SuccessForm(1, 0, batchWatch.Elapsed, outFolder, UseRussian);
                    sf.ShowDialog(this);
                }
            }
            catch (OperationCanceledException)
            {
                AppendLog(T("Конвертация отменена.", "Conversion canceled."), LogLevel.Warning);
                batchWatch.Stop();
                EndConvertState();
            }
            catch (Exception ex)
            {
                AppendLog(T($"Ошибка: {ex.Message}", $"Error: {ex.Message}"), LogLevel.Error);
                batchWatch.Stop();
                EndConvertState();
                if (!success)
                {
                    using var sf = new SuccessForm(0, 1, batchWatch.Elapsed, outFolder, UseRussian);
                    sf.ShowDialog(this);
                }
            }
        }

        private void BeginConvertState()
        {
            _isConverting = true;
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            btnConvert.Text = T("✕  Отмена", "✕  Cancel");
            btnConvert.BackColor = Color.FromArgb(170, 45, 65);
            btnConvert.FlatAppearance.MouseOverBackColor = Color.FromArgb(195, 58, 80);
            btnConvert.FlatAppearance.MouseDownBackColor = Color.FromArgb(145, 35, 55);
            prgOverall.Value = 0;
            lblProgressPct.Text = "0%";
            btnBrowseFile.Enabled = false;
            btnBrowseInput.Enabled = false;
            foreach (var b in _formatButtons) b.Enabled = false;
        }

        private void EndConvertState()
        {
            if (InvokeRequired) { BeginInvoke(EndConvertState); return; }
            _isConverting = false;
            _cts?.Dispose();
            _cts = null;
            btnConvert.Text = T("⟳  Конвертировать", "⟳  Convert");
            StyleConvertButton(btnConvert);
            btnBrowseFile.Enabled = true;
            btnBrowseInput.Enabled = true;
            foreach (var b in _formatButtons) b.Enabled = true;
        }

        // ── File selection ────────────────────────────────────
        private void btnBrowseFile_Click(object? sender, EventArgs e) => OpenFilePicker();
        private void btnBrowseInput_Click(object? sender, EventArgs e) => OpenFilePicker();

        private void OpenFilePicker()
        {
            using var dlg = new OpenFileDialog
            {
                Filter = T("Медиафайлы|*.mp3;*.mp4;*.wav;*.aac;*.flac;*.avi;*.mkv;*.mov;*.m4a|Все файлы|*.*",
                            "Media files|*.mp3;*.mp4;*.wav;*.aac;*.flac;*.avi;*.mkv;*.mov;*.m4a|All files|*.*"),
                Title = T("Выберите медиафайл", "Select a media file")
            };
            if (dlg.ShowDialog() == DialogResult.OK)
                SetInputFile(dlg.FileName);
        }

        private void btnBrowseOutput_Click(object? sender, EventArgs e)
        {
            using var dlg = new FolderBrowserDialog
            {
                Description = T("Папка для сохранения файлов", "Output folder"),
                SelectedPath = txtOutputFolder.Text,
                ShowNewFolderButton = true
            };
            if (dlg.ShowDialog() == DialogResult.OK)
                txtOutputFolder.Text = dlg.SelectedPath;
        }

        private void btnOpenOutputFolder_Click(object? sender, EventArgs e)
        {
            var folder = txtOutputFolder.Text.Trim();
            if (Directory.Exists(folder))
                Process.Start(new ProcessStartInfo { FileName = folder, UseShellExecute = true });
        }

        private void SetInputFile(string path)
        {
            _inputFile = path;
            txtInputFile.Text = path;
            AppendLog(T($"Файл выбран: {path}", $"File selected: {path}"), LogLevel.Info);

            // Auto-suggest format based on extension
            var ext = Path.GetExtension(path).ToLowerInvariant();
            foreach (var f in Formats)
            {
                if (f.Ext == ext) { SelectFormat(ext); break; }
            }
        }

        // ── Drag & drop ───────────────────────────────────────
        private void Form_DragEnter(object? sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
                e.Effect = DragDropEffects.Copy;
        }

        private void Form_DragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data?.GetData(DataFormats.FileDrop) is string[] files && files.Length > 0)
                SetInputFile(files[0]);
        }

        // ── Log ───────────────────────────────────────────────
        private enum LogLevel { Info, Success, Warning, Error }

        private void AppendLog(string message, LogLevel level = LogLevel.Info)
        {
            if (InvokeRequired) { BeginInvoke(new Action(() => AppendLog(message, level))); return; }

            var color = level switch
            {
                LogLevel.Success => Color.FromArgb(52, 211, 153),
                LogLevel.Warning => Color.FromArgb(251, 191, 36),
                LogLevel.Error   => Color.FromArgb(248, 113, 113),
                _                => Color.FromArgb(129, 183, 255)
            };

            rtbLog.SuspendLayout();
            rtbLog.SelectionStart = rtbLog.TextLength;
            rtbLog.SelectionLength = 0;
            rtbLog.SelectionColor = Color.FromArgb(90, 90, 140);
            rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}]  ");
            rtbLog.SelectionColor = color;
            rtbLog.AppendText(message + "\n");
            rtbLog.ScrollToCaret();
            rtbLog.ResumeLayout();
        }

        private void btnClear_Click(object? sender, EventArgs e)
        {
            rtbLog.Clear();
            prgOverall.Value = 0;
            lblProgressPct.Text = "0%";
        }

        private void chkShowLog_CheckedChanged(object? sender, EventArgs e)
        {
            pnlLog.Visible = chkShowLog.Checked;
        }

        private void btnToggleLog_Click(object? sender, EventArgs e)
        {
            _logExpanded = !_logExpanded;
            rtbLog.Visible = _logExpanded;
            pnlLog.Height = _logExpanded ? 172 : _logCollapsedHeight;
            btnToggleLog.Text = _logExpanded ? "∧" : "∨";
        }

        // ── Settings ──────────────────────────────────────────
        private void btnAdvSettings_Click(object? sender, EventArgs e)
        {
            using var form = new Form
            {
                Text = T("Настройки", "Settings"),
                BackColor = Color.FromArgb(16, 16, 32),
                ForeColor = Color.FromArgb(218, 218, 248),
                ClientSize = new Size(400, 148),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false, MinimizeBox = false,
                StartPosition = FormStartPosition.CenterParent,
                Font = new Font("Segoe UI", 10F),
                ShowInTaskbar = false
            };

            var chk = new CheckBox
            {
                Text = T("Открыть папку вывода после конвертации", "Open output folder when done"),
                Checked = _openFolderOnComplete,
                ForeColor = Color.FromArgb(200, 200, 240),
                BackColor = Color.Transparent,
                Location = new Point(20, 32),
                AutoSize = true
            };

            var btnApply = new Button
            {
                Text = T("Применить", "Apply"),
                DialogResult = DialogResult.OK,
                Location = new Point(276, 98),
                Size = new Size(104, 34)
            };
            btnApply.BackColor = Color.FromArgb(108, 99, 255);
            btnApply.ForeColor = Color.White;
            btnApply.FlatStyle = FlatStyle.Flat;
            btnApply.FlatAppearance.BorderSize = 0;
            btnApply.Cursor = Cursors.Hand;

            form.Controls.Add(chk);
            form.Controls.Add(btnApply);
            form.AcceptButton = btnApply;

            if (form.ShowDialog(this) == DialogResult.OK)
                _openFolderOnComplete = chk.Checked;
        }

        // ── App icon ──────────────────────────────────────────
        private static Icon CreateAppIcon()
        {
            try
            {
                using var bmp = new Bitmap(64, 64, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.Clear(Color.Transparent);

                    var rect = new Rectangle(2, 2, 60, 60);
                    using var bgPath = RoundedRect(rect, 14);
                    using var bg = new LinearGradientBrush(
                        new Rectangle(2, 2, 60, 60),
                        Color.FromArgb(108, 99, 255), Color.FromArgb(70, 150, 255), 45F);
                    g.FillPath(bg, bgPath);

                    using var pen = new Pen(Color.White, 4.5f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
                    g.DrawArc(pen, 15, 15, 34, 34, -140, 200);
                    g.DrawArc(pen, 15, 15, 34, 34, 40, 200);

                    using var ap = new Pen(Color.White, 3f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
                    g.DrawLine(ap, 43f, 18f, 49f, 15f);
                    g.DrawLine(ap, 49f, 15f, 50f, 22f);
                    g.DrawLine(ap, 21f, 46f, 15f, 49f);
                    g.DrawLine(ap, 15f, 49f, 14f, 42f);
                }
                return Icon.FromHandle(bmp.GetHicon());
            }
            catch
            {
                return SystemIcons.Application;
            }
        }

        // ── GDI helpers ───────────────────────────────────────
        private static GraphicsPath RoundedRect(Rectangle rect, int radius)
        {
            int safe = Math.Max(1, Math.Min(radius, Math.Min(rect.Width, rect.Height) / 2));
            int d = safe * 2;
            var path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hWnd, string? app, string? id);

        // ── Nested classes ────────────────────────────────────
        private sealed class RoundedProgressBar : ProgressBar
        {
            public RoundedProgressBar()
            {
                SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint
                    | ControlStyles.OptimizedDoubleBuffer, true);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = new Rectangle(0, 0, Width - 1, Height - 1);
                const int r = 4;

                using var track = new SolidBrush(Color.FromArgb(22, 22, 44));
                using var trackPath = Rounded(rect, r);
                g.FillPath(track, trackPath);

                if (Maximum > Minimum && Value > Minimum)
                {
                    int fw = Math.Max(r * 2 + 2,
                        (int)Math.Round((double)(Value - Minimum) / (Maximum - Minimum) * Width));
                    fw = Math.Min(fw, Width - 1);
                    var fr = new Rectangle(0, 0, fw, Height - 1);
                    using var fillPath = Rounded(fr, r);
                    using var grad = new LinearGradientBrush(
                        new Rectangle(0, 0, Math.Max(1, fw), Math.Max(1, Height)),
                        Color.FromArgb(108, 99, 255), Color.FromArgb(80, 175, 255),
                        LinearGradientMode.Horizontal);
                    g.FillPath(grad, fillPath);
                }

                using var border = new Pen(Color.FromArgb(46, 46, 90), 1f);
                g.DrawPath(border, trackPath);
            }

            private static GraphicsPath Rounded(Rectangle r, int rad)
            {
                int safe = Math.Max(1, Math.Min(rad, Math.Min(r.Width, r.Height) / 2));
                int d = safe * 2;
                var p = new GraphicsPath();
                p.AddArc(r.X, r.Y, d, d, 180, 90);
                p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                p.CloseFigure();
                return p;
            }
        }

        private enum AppLanguage { Russian, English }
    }
}
