using System;
using System.Drawing;
using System.Windows.Forms;

namespace MediaConverter
{
    public partial class SuccessForm : Form
    {
        public SuccessForm()
        {
            InitializeComponent();
            StyleButton(btnOk);
        }

        // Стиль кнопки — одинаковый с "Конвертировать"
        private void StyleButton(Button btn)
        {
            btn.BackColor = Color.FromArgb(91, 169, 233); // нежно-голубой
            btn.ForeColor = Color.FromArgb(20, 20, 20);   // тёмный текст
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(110, 185, 245);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(80, 160, 220);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
