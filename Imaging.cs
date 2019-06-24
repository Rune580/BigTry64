using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace BigTry64
{
    public class Screen
    {
        public string display()
        {
            Bitmap backgroundImage;
            Bitmap dirt;

            backgroundImage = (Bitmap)Image.FromFile(@"images/background.png");

            dirt = (Bitmap)Image.FromFile(@"images/shitdirt.png");

            var finalImage = new Bitmap(backgroundImage.Width, backgroundImage.Height);
            var graphics = Graphics.FromImage(finalImage);

            graphics.DrawImage(backgroundImage, 0, 0);
            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 15; y++)
                {
                    graphics.DrawImage(dirt, x*32, y*32);
                }
            }
            string output = @"images/output.png";
            finalImage.Save(output);
            return output;
        }
    }
}
