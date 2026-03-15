using PluginInterface;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace ReverseTransform
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

        public void Transform(PluginContext context)
        {
            Bitmap image = context.Image;
            for (int i = 0; i < image.Width; ++i)
                for (int j = 0; j < image.Height / 2; ++j)
                {
                    Color color = image.GetPixel(i, j);
                    image.SetPixel(i, j, image.GetPixel(i, image.Height - j - 1));
                    image.SetPixel(i, image.Height - j - 1, color);
                }
        }
    }
}

