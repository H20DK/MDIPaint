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
        private Cursor pencilCursor;
        private Cursor eraserCursor;
        private Cursor bucketCursor;
        private Cursor textCursor;

        private int x, y;
        private Bitmap bitmap;
        private Point? startPoint = null;          // начало линии (null = не рисуем)
        private Point currentPos = Point.Empty;    // текущая позиция мыши во время перетаскивания
        private Rectangle previewRect = Rectangle.Empty; // для эллипса/прямоугольника
        private bool isDrawing = false;

        public int EraserRadius { get; set; } = 8;
        public string FilePath { get; private set; } = null;   // null = новый документ
        public bool IsDirty { get; private set; } = false;     // был ли изменён рисунок        
        public Bitmap Image => bitmap;

        public DocumentForm()
        {
            InitializeComponent();
            bitmap = new Bitmap(300, 200);
            // Загружаем курсоры
            LoadCustomCursors();
            // Подписываемся на события
            this.Load += DocumentForm_Load;
            this.Activated += DocumentForm_Activated;
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (!IsDirty) return;

            var result = MessageBox.Show(
                $"Сохранить изменения в {Text ?? "новом документе"}?",
                "Сохранение",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
            else if (result == DialogResult.Yes)
            {
                if (!Save()) // твой метод Save
                    e.Cancel = true;
            }
        }

        private void DocumentForm_Load(object sender, EventArgs e)
        {
            // При загрузке формы устанавливаем курсор
            if (ParentForm is MainForm mainForm)
            {
                UpdateCursor(mainForm.Tool);
            }
        }

        private void DocumentForm_Activated(object sender, EventArgs e)
        {
            // При активации обновляем курсор
            if (ParentForm is MainForm mainForm)
            {
                UpdateCursor(mainForm.Tool);
            }
        }

        private void LoadCustomCursors()
        {
            try
            {
                // Ищем файлы в папке Resources рядом с exe
                string basePath = Path.Combine(System.Windows.Forms.Application.StartupPath, "Resources");

                pencilCursor = LoadCursorFromFile(Path.Combine(basePath, "pencil.cur"), Cursors.Cross);
                eraserCursor = LoadCursorFromFile(Path.Combine(basePath, "eraser.cur"), Cursors.Cross);
                bucketCursor = LoadCursorFromFile(Path.Combine(basePath, "bucket.cur"), Cursors.Hand);
                textCursor = LoadCursorFromFile(Path.Combine(basePath, "text.cur"), Cursors.IBeam);
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Ошибка загрузки курсоров: {ex.Message}");
                //// Запасные варианты
                //pencilCursor = Cursors.Cross;
                //eraserCursor = Cursors.Cross;
                //bucketCursor = Cursors.Hand;
                //textCursor = Cursors.IBeam;
            }
        }

        private Cursor LoadCursorFromFile(string filePath, Cursor defaultCursor)
        {
            if (File.Exists(filePath))
            {
                return new Cursor(filePath);
            }
            return defaultCursor;
        }

        // Использование в UpdateCursor
        public void UpdateCursor(Tools tool)
        {
            switch (tool)
            {
                case Tools.Pencil:
                    this.Cursor = pencilCursor ?? Cursors.Cross;
                    break;
                case Tools.Eraser:
                    this.Cursor = eraserCursor ?? Cursors.Cross;
                    break;
                case Tools.Fill:
                    this.Cursor = bucketCursor ?? Cursors.Hand;
                    break;
                case Tools.Text:
                    this.Cursor = textCursor ?? Cursors.IBeam;
                    break;
                default:
                    this.Cursor = Cursors.Cross;
                    break;
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

            startPoint = e.Location;
            currentPos = e.Location;
            isDrawing = true;

            if (main.Tool == Tools.Pencil)
            {
                // для карандаша можно сразу начать линию
            }
            if (main != null)
                main.UpdateStatus(e.X, e.Y, bitmap.Width, bitmap.Height, main.Tool, IsDirty);
        }

        private void DocumentForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var main = MdiParent as MainForm;
            if (main == null || !startPoint.HasValue) return;

            using (var g = Graphics.FromImage(bitmap))
            {
                switch (main.Tool)
                {
                    case Tools.Line:
                        g.DrawLine(new Pen(MainForm.Color, MainForm.Width), startPoint.Value, currentPos);
                        break;

                    case Tools.Ellipse:
                        var rect = GetNormalizedRect(startPoint.Value, currentPos);
                        if (main.FilledShapes)
                            g.FillEllipse(new SolidBrush(MainForm.Color), rect);
                        else
                            g.DrawEllipse(new Pen(MainForm.Color, MainForm.Width), rect);
                        break;
                }
            }

            IsDirty = true;
            Invalidate();
            startPoint = null;
            isDrawing = false;
        }

        private void DocumentForm_MouseMove(object sender, MouseEventArgs e)
        {
            var main = MdiParent as MainForm;
            if (main == null) return;

            currentPos = e.Location;

            if (!isDrawing) return;

            switch (main.Tool)
            {
                case Tools.Pencil:
                    DrawPencil(e);
                    break;

                case Tools.Line:
                case Tools.Ellipse:
                    Invalidate(); // только просим перерисовку → preview в OnPaint
                    break;

                case Tools.Eraser:
                    EraseAt(e.Location);
                    IsDirty = true;
                    Invalidate();
                    break;
            }
        }

        private Rectangle GetNormalizedRect(Point p1, Point p2)
        {
            return new Rectangle(
                Math.Min(p1.X, p2.X),
                Math.Min(p1.Y, p2.Y),
                Math.Abs(p1.X - p2.X),
                Math.Abs(p1.Y - p2.Y));
        }

        private void EraseAt(Point pt)
        {
            using (var g = Graphics.FromImage(bitmap))
            {
                using (var brush = new SolidBrush(Color.White)) // или другой фон
                {
                    g.FillEllipse(brush, pt.X - EraserRadius, pt.Y - EraserRadius,
                                       EraserRadius * 2, EraserRadius * 2);
                }
            }
        }

        private void DrawPencil(MouseEventArgs e)
        {
            if (startPoint.HasValue)
            {
                using (var g = Graphics.FromImage(bitmap))
                    g.DrawLine(new Pen(MainForm.Color, MainForm.Width), startPoint.Value, e.Location);
                startPoint = e.Location;
                IsDirty = true;
                Invalidate();
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (bitmap != null)
                e.Graphics.DrawImage(bitmap, 0, 0);

            var main = MdiParent as MainForm;
            if (main == null || !isDrawing || !startPoint.HasValue) return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            switch (main.Tool)
            {
                case Tools.Line:
                    e.Graphics.DrawLine(new Pen(MainForm.Color, MainForm.Width), startPoint.Value, currentPos);
                    break;

                case Tools.Ellipse:
                    var rect = GetNormalizedRect(startPoint.Value, currentPos);
                    if (rect.Width > 0 && rect.Height > 0)
                    {
                        if (main.FilledShapes)
                            e.Graphics.FillEllipse(new SolidBrush(MainForm.Color), rect);
                        else
                            e.Graphics.DrawEllipse(new Pen(MainForm.Color, MainForm.Width), rect);
                    }
                    break;
            }
        }

        


    }
}
