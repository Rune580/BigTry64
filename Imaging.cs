using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace BigTry64
{
    public static class Screen
    {
        public static string fullWorld(World world)
        {
            Bitmap viewFrame;
            Bitmap block;

            viewFrame = new Bitmap(world.Blocks.GetLength(0)*32, world.Blocks.GetLength(1)*32);


            var finalImage = new Bitmap(viewFrame.Width, viewFrame.Height);
            var graphics = Graphics.FromImage(finalImage);

            graphics.DrawImage(viewFrame, 0, 0);
            for (int x = 0; x < world.Blocks.GetLength(0); x++)
            {
                for (int y = 0; y < world.Blocks.GetLength(1); y++)
                {
                    block = (Bitmap)Image.FromFile(world.Blocks[x, y].FilePath);
                    graphics.DrawImage(block, x*32, y*32);
                }
            }
            string output = @"images/output.png";
            finalImage.Save(output);
            return output;
        }
        public static string display(World world)
        {
            Bitmap viewFrame;
            Bitmap block;

            viewFrame = (Bitmap)Image.FromFile(@"images/BT_bgsky.png");


            var finalImage = new Bitmap(viewFrame.Width, viewFrame.Height);
            var graphics = Graphics.FromImage(finalImage);

            graphics.DrawImage(viewFrame, 0, 0);
            for (int x = 0; x < world.Blocks.GetLength(0); x++)
            {
                for (int y = 0; y < world.Blocks.GetLength(1); y++)
                {
                    block = (Bitmap)Image.FromFile(world.Blocks[x, y].FilePath);
                    graphics.DrawImage(block, x * 32, y * 32);
                }
            }
            string output = @"images/output.png";
            finalImage.Save(output);
            return output;
        }
    }
}
