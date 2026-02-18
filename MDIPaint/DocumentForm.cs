using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace MDIPaint
{
    public partial class DocumentForm : Form
    {
        public string FilePath { get; private set; } = null;   // null = новый документ
        public bool IsDirty { get; private set; } = false;     // был ли изменён рисунок
        private Point? startPoint = null;          // начало линии (null = не рисуем)
        private Point currentPos = Point.Empty;    // текущая позиция мыши во время перетаскивания
        private int x, y;
        private Bitmap bitmap;
        public Bitmap Image => bitmap;
        public DocumentForm()
        {
            InitializeComponent();
            bitmap = new Bitmap(300, 200);
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint, true);
            this.UpdateStyles();

            bitmap = new Bitmap(300, 200);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
            }
        }

        public void ResizeCanvas(int newWidth, int newHeight)
        {
            if (newWidth <= 0 || newHeight <= 0)
                return;

            Bitmap newBitmap = new Bitmap(newWidth, newHeight);

            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                g.Clear(Color.White);

                // Копируем старое изображение в левый верхний угол
                // (если новое изображение меньше — обрежется, если больше — добавится пустое пространство)
                g.DrawImage(bitmap, 0, 0, Math.Min(bitmap.Width, newWidth), Math.Min(bitmap.Height, newHeight));
            }

            // Заменяем старый bitmap на новый
            bitmap?.Dispose();          // освобождаем старую память
            bitmap = newBitmap;

            // Обновляем форму
            this.AutoScrollMinSize = new Size(bitmap.Width, bitmap.Height);
            this.Invalidate();          // перерисовываем
        }

        // Если хочешь центрировать старое изображение при увеличении размера:
        // вместо g.DrawImage(bitmap, 0, 0, ...) можно сделать:
        // int dx = (newWidth  - bitmap.Width)  / 2;
        // int dy = (newHeight - bitmap.Height) / 2;
        // g.DrawImage(bitmap, dx, dy);


        public bool Save(bool askForName = false)
        {
            string pathToSave = FilePath;

            if (string.IsNullOrEmpty(pathToSave) || askForName)
            {
                using (var dlg = new SaveFileDialog())
                {
                    dlg.Filter = "PNG (*.png)|*.png|JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|BMP (*.bmp)|*.bmp";
                    dlg.FilterIndex = 1;
                    dlg.AddExtension = true;
                    dlg.DefaultExt = "png";

                    if (dlg.ShowDialog(this) != DialogResult.OK)
                        return false;

                    pathToSave = dlg.FileName;
                }
            }

            ImageFormat format;
            string ext = Path.GetExtension(pathToSave).ToLower();

            if (ext == ".jpg" || ext == ".jpeg") format = ImageFormat.Jpeg;
            else if (ext == ".bmp") format = ImageFormat.Bmp;
            else format = ImageFormat.Png;   // по умолчанию png

            try
            {
                bitmap.Save(pathToSave, format);
                FilePath = pathToSave;
                IsDirty = false;
                Text = Path.GetFileName(pathToSave);   // красивое имя окна
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void LoadFile(string fileName)
        {
            try
            {
                using (var img = System.Drawing.Image.FromFile(fileName))
                {
                    bitmap?.Dispose();
                    bitmap = new Bitmap(img);
                }
                FilePath = fileName;
                IsDirty = false;
                Text = Path.GetFileName(fileName);
                this.AutoScrollMinSize = new Size(bitmap.Width, bitmap.Height);
                Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть файл:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DocumentForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var main = MdiParent as MainForm;
            if (main == null) return;

            if (main.Tool == Tools.Line)
            {
                startPoint = e.Location;
                currentPos = e.Location;
            }
            else if (main.Tool == Tools.Pencil)
            {
                x = e.X;
                y = e.Y;
            }

            IsDirty = true;
        }

        private void DocumentForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var main = MdiParent as MainForm;
            if (main == null || !startPoint.HasValue) return;

            using (var g = Graphics.FromImage(bitmap))
            {
                if (main.Tool == Tools.Line)
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.DrawLine(new Pen(MainForm.Color, MainForm.Width),
                               startPoint.Value, currentPos);
                    IsDirty = true;
                }
                // Pencil уже нарисован в MouseMove
            }

            Invalidate();
            startPoint = null;
        }

        private void DocumentForm_MouseMove(object sender, MouseEventArgs e)
        {
            var main = MdiParent as MainForm;
            if (main == null) return;

            currentPos = e.Location;   // всегда запоминаем, куда указывает мышь

            if (e.Button != MouseButtons.Left) return;
            switch (main.Tool)
            {
                case Tools.Pencil:
                    DrawPencil(e);
                    break;
                case Tools.Line:
                    Invalidate();
                    break;
                default : 
                    throw new NotImplementedException();
            }
            
        }

        private void DrawPencil(MouseEventArgs e)
        {
            if (startPoint.HasValue)   // startPoint используем как предыдущую точку
            {
                Graphics g = Graphics.FromImage(bitmap);
                g.DrawLine(new Pen(MainForm.Color, MainForm.Width), startPoint.Value, e.Location);
            }
            startPoint = e.Location;
            IsDirty = true;
            Invalidate();
        }

        private void DrawLine(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Refresh();
                Graphics g = CreateGraphics();
                g.DrawLine(new Pen(MainForm.Color, MainForm.Width), x, y, e.X, e.Y);
            }

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Сначала рисуем постоянное изображение
            if (bitmap != null)
                e.Graphics.DrawImage(bitmap, 0, 0);

            // Если тянем линию — рисуем временную "резинку"
            var main = MdiParent as MainForm;
            if (main != null &&
                main.Tool == Tools.Line &&
                startPoint.HasValue &&
                (MouseButtons & MouseButtons.Left) == MouseButtons.Left)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var pen = new Pen(MainForm.Color, MainForm.Width))
                {
                    // pen.DashStyle = DashStyle.Dash;   // ← можно сделать пунктирной для красоты
                    e.Graphics.DrawLine(pen, startPoint.Value, currentPos);
                }
            }
        }



    }
}
