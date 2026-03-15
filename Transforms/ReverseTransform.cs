using PluginInterface;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace Transforms
{
    [Version(1, 0)]
    public class ReverseTransform : IPlugin
    {
        public string Name
        {
            get
            {
                return "Переворот изображения";
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
            int total = image.Width * image.Height;
            int processed = 0;

            for (int y = 0; y < image.Height / 2; ++y)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    status?.Report("Операция отменена пользователем");
                    return; // Просто выходим из метода, без исключения
                }
                for (int x = 0; x < image.Width; ++x)
                {
                    Color color = image.GetPixel(x, y);
                    image.SetPixel(x, y, image.GetPixel(x, image.Height - y - 1));
                    image.SetPixel(x, image.Height - y - 1, color);

                    processed++;
                    if (processed % (total / 100) == 0)
                    {
                        int percent = (int)((double)processed / total * 100);
                        progress?.Report(percent);
                    }
                }
                status?.Report($"Обработано строк: {(y + 1)*2} / {image.Height}");
            }
            progress?.Report(100);
        }
    }
}


