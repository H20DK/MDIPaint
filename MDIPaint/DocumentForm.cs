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
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.LinkLabel;

namespace MDIPaint
{
    public partial class DocumentForm : Form
    {
        private Point? middleButtonDownPos = null;  // экранные координаты нажатия средней кнопки
        private PointF originalViewOffsetAtMiddleDown;  // Значение сдвига картинки  в момент нажатия средней кнопки
        private PointF? lastEraserPos = null;
        // Для стабилизации карандаша
        private PointF lastCommittedPoint;
        private PointF currentSmoothedPosition;
        private bool isStabilizing = false;
        private const float STABILIZATION_FACTOR = 0.18f;  // скорость сглаживания
        private const float MIN_MOVE_DISTANCE = 1.2f; // Минимальное расстояние, на которое должна сдвинуться сглаженная точка, чтобы нарисовать новый отрезок


        private PointF? startImage = null;
        private PointF currentImage = new PointF();
        private string pendingText = null; // Текст, который пользователь ввёл, но ещё не нарисован на холсте
        private Cursor pencilCursor;
        private Cursor eraserCursor;
        private Cursor bucketCursor;
        private Cursor textCursor;
        private Bitmap bitmap;
        private bool isDrawing = false; // зажата ли сейчас левая кнопка мыши и идёт ли рисование

        private float zoom = 1.0f;
        private PointF viewOffset = new PointF(0, 0);
        private const float ZOOM_STEP = 0.25f;
        private const float MIN_ZOOM = 0.25f;
        private const float MAX_ZOOM = 8.0f;

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

        private void FloodFill(Point pt, Color newColor)
        {
            if (bitmap == null) return;

            Color targetColor = bitmap.GetPixel(pt.X, pt.Y);
            if (targetColor == newColor) return;

            Stack<Point> pixels = new Stack<Point>();
            pixels.Push(pt);

            while (pixels.Count > 0)
            {
                Point p = pixels.Pop();

                if (p.X < 0 || p.X >= bitmap.Width || p.Y < 0 || p.Y >= bitmap.Height)
                    continue;

                if (bitmap.GetPixel(p.X, p.Y) != targetColor)
                    continue;

                bitmap.SetPixel(p.X, p.Y, newColor);

                pixels.Push(new Point(p.X + 1, p.Y));
                pixels.Push(new Point(p.X - 1, p.Y));
                pixels.Push(new Point(p.X, p.Y + 1));
                pixels.Push(new Point(p.X, p.Y - 1));
            }

            IsDirty = true;
            Invalidate();
        }

        public void ZoomIn(Point? mouseLocation = null)
        {
            if (bitmap == null) return;

            float oldZoom = zoom;
            zoom = Math.Min(MAX_ZOOM, zoom + ZOOM_STEP);

            if (mouseLocation.HasValue)
            {
                // Зум относительно точки под курсором
                Point pt = mouseLocation.Value;
                float dx = pt.X - viewOffset.X;
                float dy = pt.Y - viewOffset.Y;

                viewOffset.X = pt.X - dx * (zoom / oldZoom);
                viewOffset.Y = pt.Y - dy * (zoom / oldZoom);
            }
            else
            {
                // Зум относительно центра окна
                viewOffset.X -= (ClientSize.Width / 2f) * (zoom - oldZoom) / zoom;
                viewOffset.Y -= (ClientSize.Height / 2f) * (zoom - oldZoom) / zoom;
            }

            UpdateScrollbars();
            Invalidate();
        }

        public void ZoomOut()
        {
            if (bitmap == null) return;

            float oldZoom = zoom;
            zoom = Math.Max(MIN_ZOOM, zoom - ZOOM_STEP);

            viewOffset.X -= (ClientSize.Width / 2f) * (zoom - oldZoom) / zoom;
            viewOffset.Y -= (ClientSize.Height / 2f) * (zoom - oldZoom) / zoom;

            UpdateScrollbars();
            Invalidate();
        }

        private void UpdateScrollbars()
        {
            if (bitmap == null) return;

            int scaledWidth = (int)(bitmap.Width * zoom);
            int scaledHeight = (int)(bitmap.Height * zoom);

            AutoScrollMinSize = new Size(scaledWidth + 40, scaledHeight + 40);
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
                if (!Save())
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
            string basePath = Path.Combine(System.Windows.Forms.Application.StartupPath, "Resources");

            pencilCursor = LoadCursorFromFile(Path.Combine(basePath, "pencil.cur"), Cursors.Cross);
            eraserCursor = LoadCursorFromFile(Path.Combine(basePath, "eraser.cur"), Cursors.Cross);
            bucketCursor = LoadCursorFromFile(Path.Combine(basePath, "bucket.cur"), Cursors.Hand);
            textCursor = LoadCursorFromFile(Path.Combine(basePath, "text.cur"), Cursors.IBeam);
        }

        private Cursor LoadCursorFromFile(string filePath, Cursor defaultCursor)
        {
            if (System.IO.File.Exists(filePath))
            {
                return new Cursor(filePath);
            }
            return defaultCursor;
        }

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

            bitmap?.Dispose();
            bitmap = newBitmap;

            this.AutoScrollMinSize = new Size(bitmap.Width, bitmap.Height);
            this.Invalidate();
        }

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
            else format = ImageFormat.Png;

            try
            {
                bitmap.Save(pathToSave, format);
                FilePath = pathToSave;
                IsDirty = false;
                Text = Path.GetFileName(pathToSave);
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
            if (e.Button == MouseButtons.Middle)
            {
                middleButtonDownPos = e.Location;
                originalViewOffsetAtMiddleDown = viewOffset;
                this.Cursor = Cursors.SizeAll;
            }

            if (e.Button != MouseButtons.Left) return;

            var main = MdiParent as MainForm;
            if (main == null) return;

            PointF imgPt = ScreenToImage(e.Location);

            // Проверяем, попали ли в изображение
            if (imgPt.X < 0 || imgPt.Y < 0 || imgPt.X >= bitmap.Width || imgPt.Y >= bitmap.Height)
                return;

            startImage = imgPt;
            currentImage = imgPt;
            isDrawing = true;

            switch (main.Tool)
            {
                case Tools.Pencil:
                    if (imgPt.X < 0 || imgPt.Y < 0 || imgPt.X >= bitmap.Width || imgPt.Y >= bitmap.Height)
                        break;

                    lastCommittedPoint = imgPt;
                    currentSmoothedPosition = imgPt;
                    isStabilizing = true;

                    using (var g = Graphics.FromImage(bitmap))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        float w = Math.Max(1f, MainForm.Width);
                        using (var brush = new SolidBrush(MainForm.Color))
                        {
                            g.FillEllipse(brush, imgPt.X - w / 2, imgPt.Y - w / 2, w, w);
                        }
                    }
                    IsDirty = true;
                    Invalidate();
                    break;
                case Tools.Line:
                    break;
                case Tools.Ellipse:
                    break;
                case Tools.Eraser:
                    if (e.Button != MouseButtons.Left) break;

                    // Проверяем, что попали в изображение
                    if (imgPt.X < 0 || imgPt.Y < 0 ||
                        imgPt.X >= bitmap.Width || imgPt.Y >= bitmap.Height)
                        break;

                    float diameter = MainForm.Width;
                    float radius = diameter / 2f;
                    radius = Math.Max(2f, radius);

                    using (var g = Graphics.FromImage(bitmap))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;

                        using (var brush = new SolidBrush(Color.White))
                        {
                            g.FillEllipse(brush,
                                imgPt.X - radius,
                                imgPt.Y - radius,
                                diameter,
                                diameter);
                        }
                    }

                    lastEraserPos = imgPt;
                    IsDirty = true;
                    Invalidate();

                    startImage = imgPt;
                    isDrawing = true;
                    break;
                case Tools.Fill:
                    if (e.Button == MouseButtons.Left)
                    {
                        Point origPt = new Point((int)imgPt.X, (int)imgPt.Y);
                        if (origPt.X >= 0 && origPt.X < bitmap.Width &&
                            origPt.Y >= 0 && origPt.Y < bitmap.Height)
                        {
                            FloodFill(origPt, MainForm.Color);   // твой старый метод
                        }
                    }
                    break;
                case Tools.Text:
                    if (e.Button == MouseButtons.Left)
                    {

                        pendingText = null;
                        startImage = ScreenToImage(e.Location);

                        using (var form = new Form
                        {
                            Text = "Введите текст",
                            Size = new Size(320, 140),
                            StartPosition = FormStartPosition.CenterParent,
                            FormBorderStyle = FormBorderStyle.FixedDialog,
                            MaximizeBox = false,
                            MinimizeBox = false
                        })
                        {
                            var tb = new TextBox
                            {
                                Location = new Point(12, 12),
                                Width = 280,
                                Multiline = true,
                                Height = 60,
                                AcceptsReturn = true,
                                AcceptsTab = true
                            };

                            var btnOk = new Button
                            {
                                Text = "Вставить",
                                Location = new Point(120, 85),
                                DialogResult = DialogResult.OK,
                                Width = 80
                            };

                            form.Controls.Add(tb);
                            form.Controls.Add(btnOk);
                            form.AcceptButton = btnOk;

                            if (form.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(tb.Text))
                            {
                                pendingText = tb.Text;
                                Invalidate();
                                using (var g = Graphics.FromImage(bitmap))
                                {
                                    using (var font = new System.Drawing.Font("Arial", MainForm.Width))
                                    using (var brush = new SolidBrush(MainForm.Color))
                                    {
                                        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                                        g.DrawString(tb.Text, font, brush, startImage.Value.X, startImage.Value.Y);
                                    }
                                }
                                IsDirty = true;
                                Invalidate();
                            }
                        }
                    }
                    break;
                case Tools.Arrow:
                    PointF pt = ScreenToImage(e.Location);
                    if (pt.X < 0 || pt.Y < 0 || pt.X >= bitmap.Width || pt.Y >= bitmap.Height)
                        break;

                    startImage = pt;
                    currentImage = pt;
                    isDrawing = true;
                    Invalidate();
                    break;
            }
        }
            


        private void DocumentForm_MouseUp(object sender, MouseEventArgs e)
        {
            var main = MdiParent as MainForm;
            if (main == null) return;

            if (e.Button == MouseButtons.Middle)
            {
                middleButtonDownPos = null;
            }
            UpdateCursor(main.Tool);
            if (!isDrawing || !startImage.HasValue) return;

            

            switch (main.Tool)
            {
                case Tools.Pencil:
                    if (isStabilizing && startImage.HasValue)
                    {
                        if (currentSmoothedPosition != lastCommittedPoint)
                        {
                            using (var g = Graphics.FromImage(bitmap))
                            {
                                g.SmoothingMode = SmoothingMode.AntiAlias;
                                float w = Math.Max(1f, MainForm.Width);
                                using (var pen = new Pen(MainForm.Color, w))
                                {
                                    pen.StartCap = LineCap.Round;
                                    pen.EndCap = LineCap.Round;
                                    pen.LineJoin = LineJoin.Round;
                                    g.DrawLine(pen, lastCommittedPoint, currentSmoothedPosition);
                                }
                            }
                            IsDirty = true;
                            Invalidate();
                        }

                        isStabilizing = false;
                        startImage = null;
                    }
                    break;
                case Tools.Line:
                    using (var g = Graphics.FromImage(bitmap))
                    {
                        g.DrawLine(new Pen(MainForm.Color, MainForm.Width), startImage.Value, currentImage);
                    }
                    break;
                case Tools.Ellipse:
                    using (var g = Graphics.FromImage(bitmap))
                    {
                        var rect = GetRectFromPoints(startImage.Value, currentImage);
                        if (main.FilledShapes)
                            g.FillEllipse(new SolidBrush(MainForm.Color), rect);
                        else
                            g.DrawEllipse(new Pen(MainForm.Color, MainForm.Width), rect);
                    }
                    break;
                case Tools.Eraser:
                    lastEraserPos = null;
                    break;
                case Tools.Fill:
                    break;
                case Tools.Text:                        
                    break;
                case Tools.Arrow:
                    using (var g = Graphics.FromImage(bitmap))
                    {
                        float finalThickness = MainForm.Width * 1.5f;     // можно умножить, чтобы стрелка была толще
                                                                          // или просто MainForm.Width, если толщина уже достаточная

                        DrawThickArrow(g, startImage.Value, currentImage, finalThickness, MainForm.Color, main.FilledShapes);
                    }
                    break;
            }
            
            startImage = null;
            IsDirty = true;
            Invalidate();
            isDrawing = false;
        }

        private void DocumentForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (middleButtonDownPos.HasValue && e.Button == MouseButtons.Middle)
            {
                // Разница в экранных координатах
                int dx = e.X - middleButtonDownPos.Value.X;
                int dy = e.Y - middleButtonDownPos.Value.Y;

                // Сдвигаем viewOffset в противоположную сторону (как будто тянем бумагу)
                viewOffset = new PointF(
                    originalViewOffsetAtMiddleDown.X + dx,
                    originalViewOffsetAtMiddleDown.Y + dy
                );

                UpdateScrollbars();     // важно! обновляем AutoScrollMinSize
                Invalidate();           // перерисовываем
            }

            var main = MdiParent as MainForm;
            if (main == null) return;

            PointF imgPt = ScreenToImage(e.Location);
            currentImage = imgPt;

            if (main != null)
                main.UpdateStatus(e.X, e.Y, bitmap.Width, bitmap.Height, main.Tool, IsDirty);

            switch (main.Tool)
            {
                case Tools.Pencil:
                    if (!isStabilizing) break;

                    PointF target = imgPt;  // куда хочет попасть пользователь прямо сейчас

                    // Экспоненциальное сглаживание (очень простая и быстрая формула)
                    currentSmoothedPosition = new PointF(
                        currentSmoothedPosition.X + (target.X - currentSmoothedPosition.X) * STABILIZATION_FACTOR,
                        currentSmoothedPosition.Y + (target.Y - currentSmoothedPosition.Y) * STABILIZATION_FACTOR
                    );

                    // Рисуем только если сглаженная точка достаточно далеко от последней зафиксированной
                    float distance = (float)Math.Sqrt(
                        Math.Pow(currentSmoothedPosition.X - lastCommittedPoint.X, 2) +
                        Math.Pow(currentSmoothedPosition.Y - lastCommittedPoint.Y, 2));

                    if (distance >= MIN_MOVE_DISTANCE)
                    {
                        using (var g = Graphics.FromImage(bitmap))
                        {
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            g.CompositingQuality = CompositingQuality.HighQuality;

                            float penWidth = Math.Max(1f, MainForm.Width);

                            using (var pen = new Pen(MainForm.Color, penWidth))
                            {
                                pen.StartCap = LineCap.Round;
                                pen.EndCap = LineCap.Round;
                                pen.LineJoin = LineJoin.Round;
                                g.DrawLine(pen, lastCommittedPoint, currentSmoothedPosition);
                            }
                        }

                        lastCommittedPoint = currentSmoothedPosition;
                        IsDirty = true;
                        Invalidate();
                    }

                    // Можно дополнительно показывать "резиновую" линию до текущей цели (опционально)
                     currentImage = target;   // если хочешь показывать предпросмотр до мыши
                    break;
                case Tools.Line:
                    Invalidate();
                    break;
                case Tools.Ellipse:
                    Invalidate();
                    break;
                case Tools.Eraser:
                    if (e.Button != MouseButtons.Left) break;

                    float eraserSize = MainForm.Width;
                    float eraserRadius = eraserSize / 2f;
                    float effectiveRadius = Math.Max(2f, eraserRadius);

                    using (var g = Graphics.FromImage(bitmap))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.CompositingQuality = CompositingQuality.HighQuality;

                        using (var brush = new SolidBrush(Color.White))
                        {
                            // Рисуем круг в текущей позиции
                            g.FillEllipse(brush,
                                imgPt.X - effectiveRadius,
                                imgPt.Y - effectiveRadius,
                                effectiveRadius * 2,
                                effectiveRadius * 2);

                            if (lastEraserPos.HasValue)
                            {
                                using (var pen = new Pen(Color.White, effectiveRadius * 2))
                                {
                                    pen.StartCap = LineCap.Round;
                                    pen.EndCap = LineCap.Round;
                                    g.DrawLine(pen, lastEraserPos.Value, imgPt);
                                }
                            }
                        }
                    }

                    lastEraserPos = imgPt;
                    IsDirty = true;
                    Invalidate();
                    break;
                case Tools.Fill:
                case Tools.Text:
                case Tools.Arrow:
                    if(isDrawing)
                    {
                        Invalidate();
                    }
                    break;
            }
        }

        private RectangleF GetRectFromPoints(PointF p1, PointF p2)
        {
            return new RectangleF(
                Math.Min(p1.X, p2.X),
                Math.Min(p1.Y, p2.Y),
                Math.Abs(p1.X - p2.X),
                Math.Abs(p1.Y - p2.Y)
            );
        }

        private void DrawThickArrow(Graphics g, PointF start, PointF end, float thickness, Color color, bool filled)
        {
            if (start == end) return;
            // Направление стрелки
            float dx = end.X - start.X;
            float dy = end.Y - start.Y;
            float length = (float)Math.Sqrt(dx * dx + dy * dy);
            if (length < 1) return;

            // Единичный вектор направления
            float ux = dx / length;
            float uy = dy / length;

            // Перпендикулярный вектор (для ширины тела стрелки)
            float px = -uy;
            float py = ux;

            // Половина толщины
            float halfTh = thickness / 2f;

            // Точки тела стрелки (прямоугольник)
            PointF A = new PointF(start.X + px * halfTh, start.Y + py * halfTh);
            PointF B = new PointF(start.X - px * halfTh, start.Y - py * halfTh);
            PointF C = new PointF(end.X - px * halfTh, end.Y - py * halfTh);
            PointF D = new PointF(end.X + px * halfTh, end.Y + py * halfTh);

            // Точки наконечника
            float headLength = thickness * 2.5f;    // длина наконечника  2.5 толщины
            float headWidth = thickness * 2.2f;     // ширина основания наконечника 2.2 толщины

            PointF tip = new PointF(
                end.X + ux * headLength,
                end.Y + uy * headLength
            );

            PointF left = new PointF(
                end.X + px * headWidth / 2,
                end.Y + py * headWidth / 2
            );
            PointF right = new PointF(
                end.X - px * headWidth / 2,
                end.Y - py * headWidth / 2
            );

            // Массив точек для полигона
            PointF[] points = new PointF[]
            {
                A, B, C, right, tip, left, D,
            };

            using (var pen = new Pen(color, 1f))
            {
                if (filled)
                {
                    using (var brush = new SolidBrush(color))
                    {
                        g.FillPolygon(brush, points);
                    }
                }
                g.DrawPolygon(pen, points);
            }
        }


        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (Control.ModifierKeys == Keys.Control)
            {
                if (e.Delta > 0)
                    ZoomIn(e.Location);
                else if (e.Delta < 0)
                    ZoomOut();
            }
            else
            {
                // Обычное прокручивание колёсиком (AutoScroll работает сам)
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (bitmap == null) return;

            Graphics g = e.Graphics;
            g.Clear(this.BackColor);
            g.TranslateTransform(viewOffset.X, viewOffset.Y);
            g.ScaleTransform(zoom, zoom);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;


            g.DrawImage(bitmap, 0, 0);

            if (isDrawing && startImage.HasValue)
            {
                var main = MdiParent as MainForm;
                if (main == null) return;

                Pen p = new Pen(MainForm.Color, MainForm.Width )
                {
                    DashStyle = DashStyle.Solid
                };

                switch (main.Tool)
                {
                    case Tools.Pencil:
                        break;
                    case Tools.Line:
                        g.DrawLine(p, startImage.Value, currentImage);
                        break;
                    case Tools.Ellipse:
                        var r = GetRectFromPoints(startImage.Value, currentImage);
                        g.DrawEllipse(p, r);
                        break;
                    case Tools.Eraser:
                        break;
                    case Tools.Fill:
                        break;
                    case Tools.Text:
                        break;
                    case Tools.Arrow:
                        float previewThickness = MainForm.Width;
                        DrawThickArrow(g, startImage.Value, currentImage, previewThickness, MainForm.Color, main.FilledShapes);
                        break;
                }
                p.Dispose();
            }
        }

        private PointF ScreenToImage(Point screenPt)
        {
            return new PointF(
                (screenPt.X - viewOffset.X) / zoom,
                (screenPt.Y - viewOffset.Y) / zoom
            );
        }

        public void ResetView()
        {
            zoom = 1.0f;
            viewOffset = new PointF(0, 0);
            UpdateScrollbars();
            Invalidate();
        }


    }
}

