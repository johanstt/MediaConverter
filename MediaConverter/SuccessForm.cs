using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MediaConverter
{
    public sealed partial class SuccessForm : Form
    {
        private readonly string _outputFolder;

        public SuccessForm(int completed, int failed, TimeSpan elapsed, string outputFolder, bool useRussian)
        {
            InitializeComponent();

            _outputFolder = outputFolder;

            if (useRussian)
            {
                Text = "Конвертация завершена";
                lblMessage.Text = failed == 0
                    ? "Конвертация успешно завершена"
                    : "Конвертация завершена с ошибками";
                lblSummary.Text = $"Обработано: {completed + failed}  •  Успешно: {completed}  •  Ошибок: {failed}";
                lblElapsed.Text = $"Затраченное время: {elapsed:mm\\:ss}";
                btnOpenFolder.Text = "Открыть папку";
                btnClose.Text = "Закрыть";
            }
            else
            {
                Text = "Conversion Complete";
                lblMessage.Text = failed == 0
                    ? "Conversion completed successfully"
                    : "Conversion finished with errors";
                lblSummary.Text = $"Processed: {completed + failed}  •  Succeeded: {completed}  •  Failed: {failed}";
                lblElapsed.Text = $"Elapsed: {elapsed:mm\\:ss}";
                btnOpenFolder.Text = "Open Folder";
                btnClose.Text = "Close";
            }

            lblMessage.ForeColor = failed == 0
                ? Color.FromArgb(131, 255, 181)
                : Color.FromArgb(255, 209, 124);

            StylePrimaryButton(btnClose);
            StyleSecondaryButton(btnOpenFolder);

            btnOpenFolder.Enabled = Directory.Exists(outputFolder);
            AcceptButton = btnClose;
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(_outputFolder))
            {
                Process.Start(new ProcessStartInfo { FileName = _outputFolder, UseShellExecute = true });
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private static void StylePrimaryButton(Button button)
        {
            button.BackColor = Color.FromArgb(108, 99, 255);
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(130, 122, 255);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(84, 76, 210);
        }

        private static void StyleSecondaryButton(Button button)
        {
            button.BackColor = Color.FromArgb(28, 28, 58);
            button.ForeColor = Color.FromArgb(200, 200, 240);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.FromArgb(72, 72, 135);
            button.FlatAppearance.BorderSize = 1;
            button.Cursor = Cursors.Hand;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 40, 78);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(20, 20, 48);
        }
    }
}
