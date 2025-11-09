using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace MediaConverter
{
    public partial class MainForm : Form
    {
        private readonly ConverterService converter = new ConverterService();
        private bool isConverting = false;
        private readonly Timer progressTimer;

        public MainForm()
        {
            InitializeComponent();

            // Настройка таймера
            progressTimer = new Timer
            {
                Interval = 100
            };
            progressTimer.Tick += ProgressTimer_Tick;

            // Настройка выпадающего списка
            cmbFormats.Items.AddRange(new object[]
            {
                "mp3 (MP3 audio)",
                "mp4 (MPEG-4 video)",
                "wav (Waveform Audio)",
                "aac (Advanced Audio Codec)",
                "flac (Free Lossless Audio)"
            });
            cmbFormats.SelectedIndex = 0;

            // Применяем стиль кнопки
            StyleActionButton(btnConvert);
        }

        /// <summary>
        /// Универсальный стиль кнопок (нежно-голубой фон, тёмный текст)
        /// </summary>
        private void StyleActionButton(Button btn)
        {
            btn.BackColor = System.Drawing.Color.FromArgb(91, 169, 233);      // Нежно-голубой
            btn.ForeColor = System.Drawing.Color.FromArgb(20, 20, 20);        // Тёмный текст
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(110, 185, 245);
            btn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(80, 160, 220);
        }

        private void btnInput_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Медиафайлы|*.mp3;*.mp4;*.wav;*.aac;*.flac;*.avi;*.mkv|Все файлы|*.*";
                if (dlg.ShowDialog() == DialogResult.OK)
                    txtInput.Text = dlg.FileName;
            }
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "Медиафайлы|*.mp3;*.mp4;*.wav;*.aac;*.flac|Все файлы|*.*";
                if (dlg.ShowDialog() == DialogResult.OK)
                    txtOutput.Text = dlg.FileName;
            }
        }

        private async void btnConvert_Click(object sender, EventArgs e)
        {
            if (isConverting)
            {
                MessageBox.Show("⚠ Конвертация уже выполняется!", "Инфо", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string input = txtInput.Text;
            string output = txtOutput.Text;
            string format = cmbFormats.SelectedItem?.ToString()?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(output))
            {
                MessageBox.Show("❗ Укажите входной и выходной файл!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            txtLog.Clear();
            isConverting = true;
            btnConvert.Enabled = false;
            progressTimer.Start();

            try
            {
                await converter.ConvertAsync(input, output, format, AddLog);
                progressTimer.Stop();
                AddLog("✅ Конвертация завершена успешно!");

                // Показываем новое стильное окно
                var successForm = new SuccessForm();
                successForm.ShowDialog();
            }
            catch (Exception ex)
            {
                AddLog($"❌ Ошибка: {ex.Message}");
                MessageBox.Show($"🚫 Ошибка:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isConverting = false;
                btnConvert.Enabled = true;
                progressTimer.Stop();
            }
        }

        private void AddLog(string msg)
        {
            if (txtLog.InvokeRequired)
                txtLog.Invoke(new Action(() => AddLog(msg)));
            else
                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}{Environment.NewLine}");
        }

        private void ProgressTimer_Tick(object sender, EventArgs e)
        {
            if (!isConverting) return;
            AddLog("⏳ Конвертация выполняется...");
        }

        private void chkShowLog_CheckedChanged(object sender, EventArgs e)
        {
            txtLog.Visible = chkShowLog.Checked;
        }
    }
}
