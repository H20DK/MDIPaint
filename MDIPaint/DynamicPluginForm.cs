using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using PluginInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace MDIPaint
{
    public partial class DynamicPluginForm : Form
    {
        public string PluginCode { get; private set; }
        public string PluginName { get; private set; }

        public DynamicPluginForm()
        {
            InitializeComponent();
            LoadDefaultCode();
        }

        private void LoadDefaultCode()
        {
            txtCode.Text = @"using PluginInterface;
using System;
using System.Drawing;
using System.Threading;

namespace DynamicPlugins
{
    [Version(1, 0)]
    public class DynamicFilter : IPlugin
    {
        public string Name => ""Динамический фильтр (негатив)"";
        public string Author => ""Пользователь"";

        public void Transform(
            PluginContext context,
            IProgress<int> progress = null,
            IProgress<string> status = null,
            CancellationToken cancellationToken = default)
        {
            Bitmap image = context.Image;
            int total = image.Width * image.Height;
            int processed = 0;
            
            // Пример: инверсия цветов (негатив)
            for (int y = 0; y < image.Height; y++)
            {
                // Проверка отмены
                if (cancellationToken.IsCancellationRequested)
                {
                    status?.Report(""Операция отменена"");
                    return;
                }
                
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixel = image.GetPixel(x, y);
                    Color inverted = Color.FromArgb(
                        pixel.A,
                        255 - pixel.R,
                        255 - pixel.G,
                        255 - pixel.B
                    );
                    image.SetPixel(x, y, inverted);
                    processed++;
                }
                
                // Обновление прогресса по строкам
                int percent = (int)((double)(y + 1) / image.Height * 100);
                progress?.Report(percent);
                status?.Report($""Обработано: {y + 1} / {image.Height} строк"");
            }
            
            progress?.Report(100);
            status?.Report(""Готово!"");
        }
    }
}";
        }

        private void btnCompile_Click(object sender, EventArgs e)
        {
            PluginName = txtPluginName.Text.Trim();
            if (string.IsNullOrEmpty(PluginName))
            {
                MessageBox.Show("Введите название плагина", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PluginCode = txtCode.Text;

            // Компиляция
            var (success, error, plugin) = CompilePlugin(PluginCode, PluginName);

            if (success && plugin != null)
            {
                DialogResult = DialogResult.OK;

                // Запускаем плагин сразу после успешной компиляции
                RunCompiledPlugin(plugin);

                Close();
            }
            else
            {
                MessageBox.Show($"Ошибка компиляции:\n{error}", "Компиляция не удалась",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private (bool Success, string Error, IPlugin Plugin) CompilePlugin(string code, string pluginName)
        {
            try
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(code);

                var references = new List<MetadataReference>
        {
            // Основные системные сборки
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(string).Assembly.Location),
            
            // System.Threading для CancellationToken
            MetadataReference.CreateFromFile(typeof(CancellationToken).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(IProgress<>).Assembly.Location),
            
            // PluginInterface
            MetadataReference.CreateFromFile(typeof(IPlugin).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(PluginContext).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(VersionAttribute).Assembly.Location),
            
            // System.Drawing
            MetadataReference.CreateFromFile(typeof(Color).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Bitmap).Assembly.Location),
            
            // Атрибуты
            MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location),
        };

                // Добавляем все сборки из текущего домена
                try
                {
                    foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        if (!asm.IsDynamic && !string.IsNullOrEmpty(asm.Location))
                        {
                            try
                            {
                                references.Add(MetadataReference.CreateFromFile(asm.Location));
                            }
                            catch { /* игнорируем ошибки */ }
                        }
                    }
                }
                catch { /* игнорируем */ }

                var compilation = CSharpCompilation.Create(
                    $"DynamicPlugin_{Guid.NewGuid():N}",
                    new[] { syntaxTree },
                    references,
                    new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

                using var ms = new MemoryStream();
                var result = compilation.Emit(ms);

                if (result.Success)
                {
                    ms.Position = 0;
                    var assembly = Assembly.Load(ms.ToArray());

                    var pluginType = assembly.GetTypes()
                        .FirstOrDefault(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsAbstract);

                    if (pluginType != null)
                    {
                        var plugin = (IPlugin)Activator.CreateInstance(pluginType);
                        return (true, null, plugin);
                    }
                    return (false, "Не найден класс, реализующий IPlugin", null);
                }
                else
                {
                    var errors = string.Join("\n", result.Diagnostics
                        .Where(d => d.Severity == DiagnosticSeverity.Error)
                        .Select(d => d.ToString()));
                    return (false, errors, null);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

        private async void RunCompiledPlugin(IPlugin plugin)
        {
            if (FindForm() is MainForm mainForm && mainForm.ActiveMdiChild is DocumentForm activeDoc)
            {
                var context = new PluginContext
                {
                    Image = (Bitmap)activeDoc.Image.Clone(),
                    FilePath = activeDoc.FilePath
                };

                using var progressForm = new ProgressForm($"Динамический: {plugin.Name}");
                var progressPercent = new Progress<int>(p => progressForm.ReportProgress(p));
                var progressStatus = new Progress<string>(m => progressForm.ReportStatus(m));
                var cancellationToken = progressForm.CancellationToken;

                try
                {
                    progressForm.Show(mainForm);

                    await Task.Run(() =>
                    {
                        plugin.Transform(context, progressPercent, progressStatus, cancellationToken);
                    });

                    if (cancellationToken.IsCancellationRequested)
                    {
                        context.Image?.Dispose();
                        progressForm.Close();
                        MessageBox.Show("Операция отменена.", "Информация",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        activeDoc.Image?.Dispose();
                        activeDoc.Image = (Bitmap)context.Image.Clone();
                        activeDoc.IsDirty = true;
                        activeDoc.Refresh();
                        progressForm.CloseWithDelayAsync();
                    }
                }
                catch (Exception ex)
                {
                    context.Image?.Dispose();
                    progressForm.Close();
                    MessageBox.Show($"Ошибка выполнения плагина:\n{ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
