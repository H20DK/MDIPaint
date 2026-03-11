using PluginInterface;
using System.Drawing;

namespace Transforms
{
    [Version(1, 0)]
    public class DateTimeGeoTransform: IPlugin
    {
        public string Name
        {
            get
            {
                return "Дата и геолокация";
            }
        }

        public string Author
        {
            get
            {
                return "Александр";
            }
        }

        public void Transform(Bitmap image)
        {
            using (Graphics g = Graphics.FromImage(image))
            {
                // Настройки шрифта
                Font font = new Font("Arial", 12, FontStyle.Bold);
                Brush brush = new SolidBrush(Color.White);
                Pen pen = new Pen(Color.Black, 2);

                // Формируем текст
                string dateText = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
                string geoText = GetGeoLocation();
                string fullText = dateText + (geoText != null ? "\n" + geoText : "");

                // Измеряем размер текста
                SizeF textSize = g.MeasureString(fullText, font);

                // Позиция в правом нижнем углу с отступом 10 пикселей
                float x = image.Width - textSize.Width - 10;
                float y = image.Height - textSize.Height - 10;

                // Рисуем фон для текста
                RectangleF rect = new RectangleF(x - 2, y - 2, textSize.Width + 4, textSize.Height + 4);
                g.FillRectangle(new SolidBrush(Color.FromArgb(128, 0, 0, 0)), rect);
                g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);

                // Рисуем текст
                g.DrawString(fullText, font, brush, x, y);
            }
        }

        private string GetGeoLocation()
        {
            // Здесь можно добавить реальное получение геолокации
            // Для примера возвращаем тестовые координаты
            return "55.7558° N, 37.6176° E";
        }
    }
}

