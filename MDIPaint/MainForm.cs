using PluginInterface;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


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
        private List<PluginInfo> pluginList = new List<PluginInfo>();
        private Dictionary<string, IPlugin> activePlugins = new Dictionary<string, IPlugin>();

        public MainForm()
        {
            InitializeComponent();
            FindPlugins();
            CreatePluginsMenu();
            //CreateDynamicPluginMenuItem();
            Color = Color.Black;
            Width = 3;
            Tool = Tools.Pencil;
            brushSizeTextBox.Text = Width.ToString();
        }
        public class PluginInfo
        {
            public string Name { get; set; }
            public string Author { get; set; }
            public string Version { get; set; }     // "1.0" например
            public bool Enabled { get; set; }
            public IPlugin Instance { get; set; }   // ссылка на объект плагина
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

        private void FindPlugins()
        {
            string folder = AppDomain.CurrentDomain.BaseDirectory;
            string configPath = Path.Combine(folder, "plugins.cfg");

            // Собираем ВСЕ возможные плагины
            var allPlugins = new List<PluginInfo>();

            string[] files = Directory.GetFiles(folder, "*.dll", SearchOption.TopDirectoryOnly);

            foreach (string file in files)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFile(file);
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (type.IsClass && !type.IsAbstract &&
                            typeof(IPlugin).IsAssignableFrom(type))
                        {
                            IPlugin plugin = (IPlugin)Activator.CreateInstance(type);

                            // Читаем версию из атрибута
                            string version = "—";
                            var attr = type.GetCustomAttribute<VersionAttribute>();
                            if (attr != null)
                                version = $"{attr.Major}.{attr.Minor}";

                            allPlugins.Add(new PluginInfo
                            {
                                Name = plugin.Name,
                                Author = plugin.Author,
                                Version = version,
                                Enabled = true,           // по умолчанию
                                Instance = plugin
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Можно логировать, но не показывать пользователю каждый раз
                    System.Diagnostics.Debug.WriteLine($"Ошибка загрузки {file}: {ex.Message}");
                }
            }

            // Читаем или создаём конфиг
            Dictionary<string, bool> config = LoadConfig(configPath);

            if (config.Count == 0)
            {
                // Первый запуск — все включены
                foreach (var p in allPlugins)
                    config[p.Name] = true;
                SaveConfig(configPath, config);
            }

            // Применяем настройки конфига
            foreach (var pluginInfo in allPlugins)
            {
                if (config.TryGetValue(pluginInfo.Name, out bool enabled))
                    pluginInfo.Enabled = enabled;
                else
                    pluginInfo.Enabled = true; // новые плагины по умолчанию включены
            }

            pluginList = allPlugins;

            // Заполняем активные плагины (только включённые)
            activePlugins.Clear();
            foreach (var info in pluginList.Where(p => p.Enabled))
            {
                activePlugins[info.Name] = info.Instance;
            }
        }

        private Dictionary<string, bool> LoadConfig(string path)
        {
            var dict = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

            if (!File.Exists(path)) return dict;

            try
            {
                foreach (string line in File.ReadAllLines(path))
                {
                    if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#"))
                        continue;

                    var parts = line.Split('=', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        string name = parts[0].Trim();
                        bool enabled = parts[1].Trim().Equals("true", StringComparison.OrdinalIgnoreCase);
                        dict[name] = enabled;
                    }
                }
            }
            catch { /* silent fail */ }

            return dict;
        }

        private void SaveConfig(string path, Dictionary<string, bool> config)
        {
            try
            {
                var lines = config.Select(kv => $"{kv.Key}={kv.Value.ToString().ToLower()}");
                File.WriteAllLines(path, lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось сохранить конфигурацию плагинов:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ManagePlugins_Click(object sender, EventArgs e)
        {
            using (var form = new PluginManagerForm(pluginList))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    // Пересоздаём меню и активные плагины
                    string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins.cfg");

                    var newConfig = new Dictionary<string, bool>();
                    foreach (var p in pluginList)
                        newConfig[p.Name] = p.Enabled;

                    SaveConfig(configPath, newConfig);

                    // Обновляем активные плагины
                    activePlugins.Clear();
                    foreach (var info in pluginList.Where(p => p.Enabled))
                        activePlugins[info.Name] = info.Instance;

                    // Перестраиваем меню
                    CreatePluginsMenu();
                }
            }
        }

        private void CreatePluginsMenu()
        {
            фильтрыToolStripMenuItem.DropDownItems.Clear();

            foreach (var kv in activePlugins)
            {
                var item = фильтрыToolStripMenuItem.DropDownItems.Add(kv.Key);
                item.Click += OnPluginClick;
            }
            var manageItem = фильтрыToolStripMenuItem.DropDownItems.Add("Управление плагинами...");
            manageItem.Click += ManagePlugins_Click;

            var userFilter = фильтрыToolStripMenuItem.DropDownItems.Add("Пользовательский фильтр...");
            userFilter.Click += (s, e) =>
            {
                var form = new UserFilterForm();
                form.Owner = this;
                form.Show();
            };
        }
        //private void CreateDynamicPluginMenuItem()
        //{
        //    // Добавляем разделитель
        //    фильтрыToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());

        //    // Пункт "Создать динамический плагин..."
        //    var dynamicItem = фильтрыToolStripMenuItem.DropDownItems.Add("✏️ Создать плагин (код)...");
        //    dynamicItem.Click += DynamicPluginMenuItem_Click;
        //}

        //private void DynamicPluginMenuItem_Click(object sender, EventArgs e)
        //{
        //    using (var form = new DynamicPluginForm())
        //    {
        //        if (form.ShowDialog(this) == DialogResult.OK)
        //        {
        //            // Плагин уже выполнен в DynamicPluginForm.RunCompiledPlugin()
        //            // Здесь можно добавить логирование или обновление списка
        //        }
        //    }
        //}

        private async void OnPluginClick(object sender, EventArgs args)
        {
            string pluginName = ((ToolStripMenuItem)sender).Text;
            if (!activePlugins.TryGetValue(pluginName, out IPlugin plugin)) return;
            if (ActiveMdiChild is not DocumentForm activeDoc) return;

            // Создаем контекст с копией изображения
            var context = new PluginContext
            {
                Image = (Bitmap)activeDoc.Image.Clone(),
                FilePath = activeDoc.FilePath
            };

            var progressForm = new ProgressForm($"Фильтр: {pluginName}");
            var progressPercent = new Progress<int>(p => progressForm.ReportProgress(p));
            var progressStatus = new Progress<string>(m => progressForm.ReportStatus(m));

            // Сохраняем ссылку на токен для проверки после завершения
            var cancellationToken = progressForm.CancellationToken;

            try
            {
                progressForm.Show(this);
                await Task.Run(() =>
                {
                    plugin.Transform(context, progressPercent, progressStatus, cancellationToken);
                });

                if (cancellationToken.IsCancellationRequested)
                {
                    progressForm.CloseWithDelayAsync();
                    context.Image?.Dispose();
                    MessageBox.Show("Операция отменена пользователем.", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    activeDoc.Image?.Dispose();
                    activeDoc.Image = (Bitmap)context.Image.Clone(); // Применяем результат
                    activeDoc.IsDirty = true;
                    activeDoc.Refresh();
                    progressForm.CloseWithDelayAsync();
                }
            }
            catch (Exception ex)
            {
                progressForm.CloseWithDelayAsync();
                context.Image?.Dispose();
                MessageBox.Show($"Ошибка при применении фильтра:\n{ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
