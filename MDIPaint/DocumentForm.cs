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
        private int x, y;
        private Bitmap bitmap;
        private Point? startPoint = null;
        public Bitmap Image => bitmap;
        public DocumentForm()
        {
            InitializeComponent();
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
            x = e.X;
            y = e.Y;
        }

        private void DocumentForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Graphics g = Graphics.FromImage(bitmap);
            g.DrawLine(new Pen(MainForm.Color, MainForm.Width), x, y, e.X, e.Y);
            x = e.X;
            y = e.Y;
            Invalidate();
        }}

        private void DocumentForm_MouseMove(object sender, MouseEventArgs e)
        {
            var mainForm = this.ParentForm as MainForm;
            var currentTool = mainForm.Tool;
            switch (currentTool)
            {
                case Tools.Pencil:
                    DrawPencil(e);
                    break;
                case Tools.Line:
                    DrawLine(e);
                    break;
                default : 
                    throw new NotImplementedException();
            }
            
        }

        private void DrawPencil(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Graphics g = Graphics.FromImage(bitmap);
                g.DrawLine(new Pen(MainForm.Color, MainForm.Width), x, y, e.X, e.Y);
                Invalidate();
                x = e.X;
                y = e.Y;
            }
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
            e.Graphics.DrawImage(bitmap, 0, 0);
        }



    }
}
