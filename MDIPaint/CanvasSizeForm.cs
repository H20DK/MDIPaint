using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDIPaint
{
    public partial class CanvasSizeForm : Form
    {
        public int NewWidth { get; private set; }
        public int NewHeight { get; private set; }
        private const int MAX_CANVAS_SIZE = 5000;
        private const int MIN_CANVAS_SIZE = 1;

        public CanvasSizeForm(int currentWidth, int currentHeight)
        {
            InitializeComponent();
            txtWidth.Text = currentWidth.ToString();
            txtHeight.Text = currentHeight.ToString();
        }

        private void txtWidth_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем только цифры и управляющие клавиши
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            // Запрещаем ввод нуля, если:
            // 1. Текст пустой (будет ведущий ноль)
            // 2. Текст уже содержит только "0" (не даем добавить второй ноль)
            if (e.KeyChar == '0' &&
                (txtWidth.Text.Length == 0 || txtWidth.Text == "0"))
            {
                e.Handled = true;
            }
        }

        private void txtWidth_TextChanged(object sender, EventArgs e)
        {
            // Дополнительная страховка для случаев вставки из буфера обмена
            if (txtWidth.Text.StartsWith("0") && txtWidth.Text.Length > 1)
            {
                txtWidth.Text = txtWidth.Text.TrimStart('0');
                txtWidth.SelectionStart = txtWidth.Text.Length;
            }
        }

        private void txtHeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем только цифры и управляющие клавиши
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            // Запрещаем ввод нуля, если:
            // 1. Текст пустой (будет ведущий ноль)
            // 2. Текст уже содержит только "0" (не даем добавить второй ноль)
            if (e.KeyChar == '0' &&
                (txtHeight.Text.Length == 0 || txtHeight.Text == "0"))
            {
                e.Handled = true;
            }
        }

        private void txtHeight_TextChanged(object sender, EventArgs e)
        {
            // Дополнительная страховка для случаев вставки из буфера обмена
            if (txtHeight.Text.StartsWith("0") && txtHeight.Text.Length > 1)
            {
                txtHeight.Text = txtHeight.Text.TrimStart('0');
                txtHeight.SelectionStart = txtHeight.Text.Length;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtWidth.Text, out int w) || w < MIN_CANVAS_SIZE || w > MAX_CANVAS_SIZE)
            {
                MessageBox.Show($"Ширина должна быть целым числом от {MIN_CANVAS_SIZE} до {MAX_CANVAS_SIZE}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtWidth.Focus();
                return;
            }

            if (!int.TryParse(txtHeight.Text, out int h) || h < MIN_CANVAS_SIZE || h > MAX_CANVAS_SIZE)
            {
                MessageBox.Show($"Высота должна быть целым числом от {MIN_CANVAS_SIZE} до {MAX_CANVAS_SIZE}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHeight.Focus();
                return;
            }

            NewWidth = w;
            NewHeight = h;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
