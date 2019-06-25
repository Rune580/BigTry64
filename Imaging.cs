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
            int totalBlocks = world.Blocks.GetLength(0) * world.Blocks.GetLength(1);
            int blocksDone = 0;
            Bitmap viewFrame;
            Bitmap block;

            viewFrame = new Bitmap(world.Blocks.GetLength(0)*32, world.Blocks.GetLength(1)*32);


            var finalImage = new Bitmap(viewFrame.Width, viewFrame.Height);
            var graphics = Graphics.FromImage(finalImage);

            graphics.DrawImage(viewFrame, 0, 0);

            Console.Clear();
            for (int x = 0; x < world.Blocks.GetLength(0); x++)
            {
                for (int y = 0; y < world.Blocks.GetLength(1); y++)
                {
                    Console.CursorVisible = false;
                    Console.SetCursorPosition(0, 0);
                    block = (Bitmap)Image.FromFile(world.Blocks[x, y].FilePath);
                    graphics.DrawImage(block, x*32, y*32);
                    blocksDone++;
                    Console.WriteLine($"{blocksDone} Tiles out of {totalBlocks} rendered. {(int)(((float)x / (float)world.Blocks.GetLength(0)) * 100)}%");
                }
            }
            Console.Clear();
            Console.CursorVisible = true;
            string output = @"images/output.png";
            finalImage.Save(output);
            return output;
        }
        public static string display(World world, int _x, int _y)
        {
            Bitmap viewFrame;
            Bitmap block;

            viewFrame = (Bitmap)Image.FromFile(@"images/BT_bgsky.png");


            var finalImage = new Bitmap(viewFrame.Width, viewFrame.Height);
            var graphics = Graphics.FromImage(finalImage);

            graphics.DrawImage(viewFrame, 0, 0);
            int x2 = 0;
            for (int x = _x-10; x < _x+10; x++)
            {
                int y2 = 0;
                for (int y = _y-7; y < _y+8; y++)
                {
                    block = (Bitmap)Image.FromFile(world.Blocks[x, y].FilePath);
                    graphics.DrawImage(block, x2 * 32, y2 * 32);
                    y2++;
                }
                x2++;
            }
            string output = @"images/output.png";
            finalImage.Save(output);
            return output;
        }
    }
}
