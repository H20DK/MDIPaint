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

        public void Transform(Bitmap image)
        {
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color pixel = image.GetPixel(x, y);
                    int gray = (int)(pixel.R * 0.3 + pixel.G * 0.59 + pixel.B * 0.11);
                    Color grayColor = Color.FromArgb(pixel.A, gray, gray, gray);
                    image.SetPixel(x, y, grayColor);
                }
            }
        }
    }
}

