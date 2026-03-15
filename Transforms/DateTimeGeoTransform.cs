using PluginInterface;
using System.Drawing;
using System.Linq;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;

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

        public void Transform(
            PluginContext context,
            IProgress<int>? progress = null,
            IProgress<string>? status = null,
            CancellationToken cancellationToken = default
        )
        {
            progress?.Report(0);
            status?.Report("Чтение метаданных...");
            if (cancellationToken.IsCancellationRequested)
            {
                status?.Report("Операция отменена пользователем");
                return; // Просто выходим из метода, без исключения
            }
            Bitmap image = context.Image;
            string geoText = "Геолокация не найдена";
            string location = null;
            if (context.FilePath!= null)
            {
                 location = GetGeoLocation(context.FilePath);
            }
            
            if (location != null)
                geoText = location;

            status?.Report("Нанесение текста...");

            // Рисуем дату и геолокацию
            using (Graphics g = Graphics.FromImage(image))
            {
                string dateText = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
                string fullText = dateText + "\n" + geoText;

                Font font = new Font("Arial", 12, FontStyle.Bold);
                Brush brush = new SolidBrush(Color.White);
                Pen pen = new Pen(Color.Black, 2);

                SizeF textSize = g.MeasureString(fullText, font);
                float x = image.Width - textSize.Width - 10;
                float y = image.Height - textSize.Height - 10;

                RectangleF rect = new RectangleF(x - 2, y - 2, textSize.Width + 4, textSize.Height + 4);
                g.FillRectangle(new SolidBrush(Color.FromArgb(128, 0, 0, 0)), rect);
                g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
                g.DrawString(fullText, font, brush, x, y);
            }
            progress?.Report(100);
            status?.Report("Готово");
        }
        private string GetGeoLocation(string filePath)
        {
            try
            {
                var directories = ImageMetadataReader.ReadMetadata(filePath);
                var gpsDir = directories.OfType<GpsDirectory>().FirstOrDefault();

                if (gpsDir == null) return null;

                // Ручное извлечение координат
                var latRef = gpsDir.GetString(GpsDirectory.TagLatitudeRef);
                var lonRef = gpsDir.GetString(GpsDirectory.TagLongitudeRef);

                // Получаем массивы рациональных чисел (градусы/минуты/секунды)
                var latitudeArray = gpsDir.GetRationalArray(GpsDirectory.TagLatitude);
                var longitudeArray = gpsDir.GetRationalArray(GpsDirectory.TagLongitude);

                if (latitudeArray == null || longitudeArray == null ||
                    latitudeArray.Length < 3 || longitudeArray.Length < 3)
                    return null;

                // Конвертируем в десятичные градусы
                double latitude = ConvertToDegrees(latitudeArray);
                double longitude = ConvertToDegrees(longitudeArray);

                // Учитываем направление (N/S, E/W)
                if (latRef == "S") latitude = -latitude;
                if (lonRef == "W") longitude = -longitude;

                return $"{latitude:F4}° N, {longitude:F4}° E";
            }
            catch
            {
                return null;
            }
        }

        private double ConvertToDegrees(Rational[] values)
        {
            // values[0] = градусы, values[1] = минуты, values[2] = секунды
            double degrees = values[0].ToDouble();
            double minutes = values[1].ToDouble();
            double seconds = values[2].ToDouble();

            return degrees + (minutes / 60.0) + (seconds / 3600.0);
        }
    }
}

