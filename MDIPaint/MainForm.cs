using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDIPaint
{
    public partial class MainForm : Form
    {
        public static Color Color { get; set; }
        public static new int Width { get; set; }
        public Tools Tool { get; set; }

        public MainForm()
        {
            InitializeComponent();
            Color = Color.Black;
            Width = 3;
            Tool = Tools.Pencil;
            brushSizeTextBox.Text = Width.ToString();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void новыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var documentForm = new DocumentForm();
            documentForm.MdiParent = this;
            documentForm.Show();

        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            сохранитьToolStripMenuItem.Enabled = ActiveMdiChild != null;
            сохранитьКакToolStripMenuItem.Enabled = ActiveMdiChild != null;
        }

        private void рисунокToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            размерХолстаToolStripMenuItem.Enabled = ActiveMdiChild != null;
        }

        private void размерХолстаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is DocumentForm activeDoc)
            {
                int currentWidth = activeDoc.Image.Width;
                int currentHeight = activeDoc.Image.Height;

                using (var dlg = new CanvasSizeForm(currentWidth, currentHeight))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        activeDoc.ResizeCanvas(dlg.NewWidth, dlg.NewHeight);
                    }
                }
            }
        }

        private void красныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Color = Color.Red;
        }

        private void синийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Color = Color.Blue;
        }

        private void зеленыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Color = Color.Green;
        }

        private void другойToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
                Color = cd.Color;

        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var activeDocumentForm = this.ActiveMdiChild as DocumentForm;
            if (activeDocumentForm != null)
            {
                activeDocumentForm.Save(askForName: false);
            }

        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var activeDocumentForm = this.ActiveMdiChild as DocumentForm;
            if (activeDocumentForm != null)
            {
                activeDocumentForm.Save(askForName: true);
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Все поддерживаемые файлы (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|" +
                             "PNG (*.png)|*.png|JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|BMP (*.bmp)|*.bmp";
                dlg.FilterIndex = 1;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var doc = new DocumentForm();
                    doc.MdiParent = this;
                    doc.LoadFile(dlg.FileName);
                    doc.Show();
                }
            }
        }


        private void каскадомToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void слеваНаправоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void сверхуВнизToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void упорядочитьЗначкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void окноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            каскадомToolStripMenuItem.Enabled = ActiveMdiChild != null;
            слеваНаправоToolStripMenuItem.Enabled = ActiveMdiChild != null;
            сверхуВнизToolStripMenuItem.Enabled = ActiveMdiChild != null;
            упорядочитьЗначкиToolStripMenuItem.Enabled = ActiveMdiChild != null;
        }

        private void pencilBtn_Click(object sender, EventArgs e)
        {
            Tool = Tools.Pencil;
        }

        private void lineBtn_Click(object sender, EventArgs e)
        {
            Tool = Tools.Line;
        }

        private void brushSizeTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            if (e.KeyChar == '0' &&
                (brushSizeTextBox.Text.Length == 0 || brushSizeTextBox.Text == "0"))
            {
                e.Handled = true;
            }
        }

        private void brushSizeTextBox_TextChanged(object sender, EventArgs e)
        {
            // Дополнительная страховка для случаев вставки из буфера обмена
            if (brushSizeTextBox.Text.StartsWith("0") && brushSizeTextBox.Text.Length > 1)
            {
                brushSizeTextBox.Text = brushSizeTextBox.Text.TrimStart('0');
                brushSizeTextBox.SelectionStart = brushSizeTextBox.Text.Length;
            }
            if (int.TryParse(brushSizeTextBox.Text, out int size) && size >= 1)
            {
                if (size > 100) 
                {
                    MainForm.Width = 100;
                    brushSizeTextBox.Text = Width.ToString();
                }
                else
                    MainForm.Width = size;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(brushSizeTextBox.Text))
                {
                    MainForm.Width = 1;
                    brushSizeTextBox.Text = Width.ToString();
                }
            }
        }
    }
}
