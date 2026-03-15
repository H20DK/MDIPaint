using PluginInterface;
using System.Drawing;

namespace Transforms
{
    [Version(1, 0)]
    public class MedianFilterTransform: IPlugin
    {
        public string Name
        {
            get
            {
                return "Медианный фильтр";
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
            CancellationToken cancellationToken = default)
        {
            Bitmap image = context.Image;
            Bitmap result = new Bitmap(image.Width, image.Height);
            int filterSize = 20;
            int radius = 10;            
            for (int y = 0; y < image.Height; y++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    status?.Report("Операция отменена пользователем");
                    return; // Просто выходим из метода, без исключения
                }
                for (int x = 0; x < image.Width; x++)
                {
                    List<int> rValues = new List<int>();
                    List<int> gValues = new List<int>();
                    List<int> bValues = new List<int>();
                    // Собираем значения пикселей в окне
                    for (int fx = -radius; fx <= radius; fx++)
                    {
                        for (int fy = -radius; fy <= radius; fy++)
                        {
                            int nx = Math.Clamp(x + fx, 0, image.Width - 1);
                            int ny = Math.Clamp(y + fy, 0, image.Height - 1);

                            Color pixel = image.GetPixel(nx, ny);
                            rValues.Add(pixel.R);
                            gValues.Add(pixel.G);
                            bValues.Add(pixel.B);
                        }
                    }

                    // Находим медиану
                    rValues.Sort();
                    gValues.Sort();
                    bValues.Sort();

                    int medianIndex = rValues.Count / 2;
                    Color medianColor = Color.FromArgb(
                        rValues[medianIndex],
                        gValues[medianIndex],
                        bValues[medianIndex]
                    );

                    result.SetPixel(x, y, medianColor);
                }
                int percent = (int)((double)(y + 1) / image.Height * 100);
                progress?.Report(percent);
                status?.Report($"Обработано строк: {y + 1} / {image.Height}");
            }

            // Копируем результат обратно в исходное изображение
            using (Graphics g = Graphics.FromImage(image))
            {
                g.DrawImage(result, 0, 0);
            }
            progress?.Report(100);
        }
    }
}

