using PluginInterface;
using System.Drawing;

namespace Transforms
{
    [Version(1, 0)]
    public class GrayscaleTransform : IPlugin
    {
        public string Name
        {
            get
            {
                return "Оттенки серого";
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
            Bitmap image = context.Image;
            for (int y = 0; y < image.Height; y++)                
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    status?.Report("Операция отменена пользователем");
                    return; // Просто выходим из метода, без исключения
                }
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixel = image.GetPixel(x, y);
                    int gray = (int)(pixel.R * 0.3 + pixel.G * 0.59 + pixel.B * 0.11);
                    Color grayColor = Color.FromArgb(pixel.A, gray, gray, gray);
                    image.SetPixel(x, y, grayColor);
                }
                int percent = (int)((double)(y + 1) / image.Height * 100);
                progress?.Report(percent);
                status?.Report($"Обработано строк: {y + 1} / {image.Height}");
            }
            progress?.Report(100);
        }
    }
}

