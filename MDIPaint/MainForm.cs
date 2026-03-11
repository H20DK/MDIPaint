using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PluginInterface;

namespace MDIPaint
{
    public partial class MainForm : Form
    {

        public static Color Color { get; set; }
        public static new int Width { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Tools Tool { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool FilledShapes { get; set; } = false;
        Dictionary<string, IPlugin> plugins = new Dictionary<string, IPlugin>();

        public MainForm()
        {
            InitializeComponent();
            FindPlugins();
            CreatePluginsMenu();
            Color = Color.Black;
            Width = 3;
            Tool = Tools.Pencil;
            brushSizeTextBox.Text = Width.ToString();
        }

        public void UpdateStatus(int x, int y, int w, int h, Tools tool, bool dirty)
        {
            if (sslCursorPos != null)
                sslCursorPos.Text = $"X: {x}, Y: {y}";

            if (sslImageSize != null)
                sslImageSize.Text = $"Размер: {w} × {h}";

            if (sslTool != null)
            {
                string toolName;

                switch (tool)
                {
                    case Tools.Pencil:
                        toolName = "Карандаш";
                        break;
                    case Tools.Line:
                        toolName = "Линия";
                        break;
                    case Tools.Ellipse:
                        toolName = "Эллипс";
                        break;
                    case Tools.Eraser:
                        toolName = "Ластик";
                        break;
                    case Tools.Fill:
                        toolName = "Заливка";
                        break;
                    case Tools.Text:
                        toolName = "Текст";
                        break;
                    case Tools.Arrow:
                        toolName = "Стрелка";
                        break;
                    default:
                        toolName = tool.ToString();
                        break;
                }

                sslTool.Text = $"Инструмент: {toolName}";
            }
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
            SetTool(Tool);
        }

        private void lineBtn_Click(object sender, EventArgs e)
        {
            Tool = Tools.Line;
            SetTool(Tool);
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                FilledShapes = true;
            }
            else
            {
                FilledShapes = false;
            }
        }

        private void ellipseBtn_Click(object sender, EventArgs e)
        {
            Tool = Tools.Ellipse;
            SetTool(Tool);
        }

        private void eraserBtn_Click(object sender, EventArgs e)
        {
            Tool = Tools.Eraser;
            SetTool(Tool);
        }

        private void textBtn_Click(object sender, EventArgs e)
        {
            Tool = Tools.Text;
            SetTool(Tool);
        }

        private void bucketBtn_Click(object sender, EventArgs e)
        {
            Tool = Tools.Fill;
            SetTool(Tool);
        }

        private void SetTool(Tools tool)
        {
            Tool = tool;

            // Обновляем курсор во всех открытых документах
            foreach (Form child in this.MdiChildren)
            {
                if (child is DocumentForm doc)
                {
                    doc.UpdateCursor(tool);
                }
            }
        }

        private void MainForm_MdiChildActivate(object sender, EventArgs e)
        {
            if (ActiveMdiChild is DocumentForm doc)
            {
                UpdateStatus(0, 0, doc.Image.Width, doc.Image.Height, Tool, doc.IsDirty);
            }
        }

        private void zoomInBtn_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is DocumentForm doc)
                doc.ZoomIn();
        }

        private void zoomOutBtn_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is DocumentForm doc)
                doc.ZoomOut();
        }

        private void resetViewBtn_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild is DocumentForm doc)
            {
                doc.ResetView();
            }
        }

        private void arrowBtn_Click(object sender, EventArgs e)
        {
            Tool = Tools.Arrow;
            SetTool(Tool);
        }

        private void sslImageSize_Click(object sender, EventArgs e)
        {

        }

        private void фильтрыToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        void FindPlugins()
        {
            // папка с плагинами
            string folder = System.AppDomain.CurrentDomain.BaseDirectory;

            // dll-файлы в этой папке
            string[] files = Directory.GetFiles(folder, "*.dll");

            foreach (string file in files)
                try
                {
                    Assembly assembly = Assembly.LoadFile(file);

                    foreach (Type type in assembly.GetTypes())
                    {
                        Type iface = type.GetInterface(nameof(PluginInterface.IPlugin));

                        if (iface != null)
                        {
                            IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                            plugins.Add(plugin.Name, plugin);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки плагина\n" + ex.Message);
                }
        }

        private void CreatePluginsMenu()
        {
            foreach (var p in plugins)
            {
                var item = фильтрыToolStripMenuItem.DropDownItems.Add(p.Value.Name);
                item.Click += OnPluginClick;
            }
        }

        private void OnPluginClick(object sender, EventArgs args)
        {
            IPlugin plugin = plugins[((ToolStripMenuItem)sender).Text];
            if (ActiveMdiChild is DocumentForm activeDoc)
            {
                plugin.Transform((Bitmap)activeDoc.Image);
                activeDoc.Refresh();
            }
        }



    }
}
