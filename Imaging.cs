﻿using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace BigTry64
{
    public static class Screen
    {
        public static string fullWorld(World world, SocketMessage message)
        {
            int Percent;
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
                    block.Dispose();
                    blocksDone++;
                    Console.WriteLine($"{blocksDone} Tiles out of {totalBlocks} rendered. {(int)(((float)x / (float)world.Blocks.GetLength(0)) * 100)}%                                                                                                                                                                                                                       ");
                    Console.WriteLine("                                                                                                                                                                                                                       ");
                    Console.WriteLine("                                                                                                                                                                                                                       ");
                    Console.WriteLine("                                                                                                                                                                                                                       ");
                    Percent = (int)(((float)x / (float)world.Blocks.GetLength(0)) * 10);
                }
            }
            Console.Clear();
            Console.CursorVisible = true;
            string output = @"images/output.png";
            finalImage.Save(output);
            graphics.Dispose();
            viewFrame.Dispose();
            finalImage.Dispose();
            return output;
        }
        public static string display(World world, int _x, int _y, List<Player> players, List<Mob> mobs, Player player, bool Inv)
        {
            Bitmap viewFrame;
            Bitmap block = null;
            List<BaseMob> TempMobs = new List<BaseMob>();
            foreach (var item in players)
            {
                if (item.World == world.Name && item.X > _x-10 && item.X < _x+10 && item.Y > _y-7 && item.Y < _y+8)
                {
                    TempMobs.Add(item);
                }
            }
            foreach (var item in mobs)
            {
                if (item.World == world.Name && item.X > _x - 10 && item.X < _x + 10 && item.Y > _y - 8 && item.Y < _y + 7)
                {
                    TempMobs.Add(item);
                }
            }
            viewFrame = (Bitmap)Image.FromFile(@"images/BT_bgsky.png");


            var finalImage = new Bitmap(viewFrame.Width, viewFrame.Height);
            var graphics = Graphics.FromImage(finalImage);

            graphics.DrawImage(viewFrame, 0, 0);
            while (_x -10 < 0)
            {
                _x++;
            }
            while (_x + 10 > world.Blocks.GetLength(0))
            {
                _x--;
            }
            while (_y - 8 < 0)
            {
                _y++;
            }
            while (_y + 7 > world.Blocks.GetLength(1))
            {
                _y--;
            }
            int x2 = 0;
            for (int x = _x-10; x < _x+10; x++)
            {
                int y2 = 0;
                for (int y = _y-8; y < _y+7; y++)
                {
                    block = (Bitmap)Image.FromFile(world.Blocks[x, y].FilePath);
                    graphics.DrawImage(block, x2 * 32, y2 * 32);
                    foreach (var item in TempMobs)
                    {
                        if (item.X == x && item.Y == y)
                        {
                            block = (Bitmap)Image.FromFile(item.FilePath);
                            graphics.DrawImage(block, x2 * 32, y2 * 32);
                            break;
                        }
                    }
                    y2++;
                }
                x2++;
            }
            if (Inv)
            {
                Bitmap INV = (Bitmap)Image.FromFile(@"images/BT_inventory.png");
                graphics.DrawImage(INV, 50, 87);
                for (int x = 0; x < player.Inventory.GetLength(0); x++)
                {
                    for (int y = 0; y < player.Inventory.GetLength(1); y++)
                    {
                        if (player.Inventory[x,y] != null)
                        {

                        }
                    }
                }
            }
            else
            {
                Bitmap INV = (Bitmap)Image.FromFile(@"images/BT_hotbar.png");
                graphics.DrawImage(INV, 100, 270);
                for (int x = 0; x < player.Inventory.GetLength(0); x++)
                {
                    for (int y = 0; y < player.Inventory.GetLength(1); y++)
                    {

                    }
                }
            }
            string output = @"images/output.png";
            finalImage.Save(output);
            return output;
        }
    }
}
