using Discord.WebSocket;
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
        public static string display(World world, int _x, int _y, List<Player> players, List<Mob> mobs, Player player, bool Inv, bool crafting)
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
            player.craftingstation = false;
            player.furnacestation = false;
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
                            if (item.Name == "craftingstation")
                            {
                                player.craftingstation = true;
                            }
                            if (item.Name == "furnacestation")
                            {
                                player.furnacestation = true;
                            }

                            block = (Bitmap)Image.FromFile(item.FilePath);
                            graphics.DrawImage(block, x2 * 32, y2 * 32);
                            block.Dispose();
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
                int StartX = (viewFrame.Width / 2) - ((INV.Width * 2) / 2);
                int StartY = (int)((viewFrame.Height / 1.2f) - (INV.Height * 1.5f));

                graphics.DrawImage(INV, StartX, StartY, INV.Width * 2, INV.Height * 2);
                for (int x = 0; x < player.Inventory.GetLength(0); x++)
                {
                    for (int y = 0; y < player.Inventory.GetLength(1); y++)
                    {
                        if (player.Inventory[x, y] != null)
                        {
                            if (player.Inventory[x, y].Count != 0)
                            {
                                if (y == 3)
                                {
                                    INV = (Bitmap)Image.FromFile(player.Inventory[x, y].Block.FilePath);
                                    int X = StartX + (16 + (64 * x));
                                    int Y = StartY + (16 + (64 * y)) + 32;
                                    graphics.DrawImage(INV, X, Y);

                                    if (player.Inventory[x, y].Count != 1)
                                    {
                                        Font f = new Font("Comic Sans MS", 16);
                                        SolidBrush b = new SolidBrush(Color.White);
                                        graphics.DrawString($"{player.Inventory[x, y].Count}", f, b, X + 18, Y + 15);
                                    }
                                }
                                else
                                {
                                    INV = (Bitmap)Image.FromFile(player.Inventory[x, y].Block.FilePath);
                                    int X = StartX + (16 + (64 * x));
                                    int Y = StartY + (16 + (64 * y));
                                    graphics.DrawImage(INV, X, Y);

                                    if (player.Inventory[x, y].Count != 1)
                                    {
                                        Font f = new Font("Comic Sans MS", 16);
                                        SolidBrush b = new SolidBrush(Color.White);
                                        graphics.DrawString($"{player.Inventory[x, y].Count}", f, b, X + 18, Y + 15);
                                    }
                                }
                            }
                        }
                    }
                }
                INV = (Bitmap)Image.FromFile(@"images/BT_inventoryletters.png");
                graphics.DrawImage(INV, StartX, StartY, INV.Width * 2, INV.Height * 2);
            }
            else if (crafting)
            {
                Bitmap outputimg;
                Bitmap INV = (Bitmap)Image.FromFile(@"images/BT_invslot.png");
                int StartX = 2; //(viewFrame.Width / 2) - ((INV.Width * 2) / 2);
                int StartY = 2; //(int)((viewFrame.Height / 1.2f) - (INV.Height * 1.5f));

                Font f = new Font("Comic Sans MS", 16);
                SolidBrush b = new SolidBrush(Color.White);

                for (int i = 0; i < world.Recipes.Length; i++)
                {
                    if (world.Recipes[i].craftingHandler == "craftingStation" && !player.craftingstation)
                    {
                        break;
                    }
                    if (world.Recipes[i].craftingHandler == "furnaceStation" && !player.furnacestation)
                    {
                        break;
                    }
                    for (int _i = 0; _i < world.Recipes[i].inputItems.Length; _i++)
                    {
                        for (int x = 0; x < player.Inventory.GetLength(0); x++)
                        {
                            for (int y = 0; y < player.Inventory.GetLength(1); y++)
                            {
                                if (player.Inventory[x, y] != null)
                                {
                                    if (player.Inventory[x, y].Name == world.Recipes[i].inputItems[_i].Name || player.Inventory[x, y].Name == world.Recipes[i].inputItems[_i].Block.Name)
                                    {
                                        if (player.Inventory[x, y].Count >= world.Recipes[i].inputItems[_i].Count)
                                        {
                                            //Console.WriteLine($"{world.Recipes[i].outputItems[0].Block.Name} is craftable");
                                            Console.WriteLine($"{world.Recipes[i].outputItems[0].Block.FilePath} othername:  {world.Recipes[i].outputItems[0].FilePath}");
                                            Console.WriteLine($"currently checking recipe {world.Recipes[i].outputItems[0].Block.Name}");
                                            try
                                            {
                                                outputimg = (Bitmap)Image.FromFile(world.Recipes[i].outputItems[0].Block.FilePath);
                                            }
                                            catch
                                            {
                                                try
                                                {
                                                    outputimg = (Bitmap)Image.FromFile(world.Recipes[i].outputItems[0].FilePath);
                                                }
                                                catch
                                                {
                                                    outputimg = (Bitmap)Image.FromFile(@"images/error.png");
                                                }
                                            }

                                            int X = StartX + (16 + (64 * 2));
                                            int Y = StartY + (16 + (64 * 2)* i/2);

                                            graphics.DrawImage(INV, X, Y, 64, 64);
                                            graphics.DrawImage(outputimg, X + 16, Y + 8, 32, 32);
                                            if (world.Recipes[i].outputItems[0].Count > 1)
                                            {
                                                graphics.DrawString(world.Recipes[i].outputItems[0].Block.Name, f, b, (X + 8) - world.Recipes[i].outputItems[0].Block.Name.Length, Y + 32);
                                                graphics.DrawString($"{world.Recipes[i].outputItems[0].Count}", f, b, (X + 40) - world.Recipes[i].outputItems[0].Block.Name.Length, Y + 20);
                                            }
                                            else
                                            {
                                                graphics.DrawString(world.Recipes[i].outputItems[0].Block.Name, f, b, (X + 8) - world.Recipes[i].outputItems[0].Block.Name.Length, Y + 32);
                                            }
                                        }
                                    }
                                    //Console.WriteLine($"Player has {player.Inventory[x,y].Count} of {player.Inventory[x,y].Block.Name}");
                                    //Console.WriteLine($"{world.Recipes[i].inputItems[_i].Count} of {world.Recipes[i].inputItems[_i].Block.Name} required for {world.Recipes[i].outputItems[0].Block.Name}");
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Bitmap INV = (Bitmap)Image.FromFile(@"images/BT_hotbar.png");
                int StartX = (viewFrame.Width / 2) - ((INV.Width * 2) / 2);
                int StartY = (int)((viewFrame.Height / 1.2f) - ((INV.Height - 37) * 1.5f));

                graphics.DrawImage(INV, StartX, StartY, INV.Width * 2, INV.Height * 2);
                for (int x = 0; x < player.Inventory.GetLength(0); x++)
                {
                    for (int y = 0; y < player.Inventory.GetLength(1); y++)
                    {
                        if (y == 3 && player.HotBar == x)
                        {
                            INV = (Bitmap)Image.FromFile(@"images/BT_invslot.png");
                            graphics.DrawImage(INV,32 + (64 * (player.HotBar)), StartY, INV.Width * 2, INV.Height * 2);
                        }
                        if (player.Inventory[x, y] != null && y == 3)
                        {
                            if (player.Inventory[x, y].Count != 0)
                            {
                                INV = (Bitmap)Image.FromFile(player.Inventory[x, y].Block.FilePath);
                                int X = StartX + (16 + (64 * x));
                                int Y = StartY + 16;
                                graphics.DrawImage(INV, X, Y);

                                if (player.Inventory[x, y].Count != 1)
                                {
                                    Font f = new Font("Comic Sans MS", 16);
                                    SolidBrush b = new SolidBrush(Color.White);
                                    graphics.DrawString($"{player.Inventory[x, y].Count}", f, b, X + 18, Y + 15);
                                }
                            }
                        }
                    }
                }
                INV = (Bitmap)Image.FromFile(@"images/BT_hotbarletters.png");
                graphics.DrawImage(INV, StartX, StartY, INV.Width * 2, INV.Height * 2);
            }
            string output = @"images/output.png";
            finalImage.Save(output);
            graphics.Dispose();
            viewFrame.Dispose();
            finalImage.Dispose();
            return output;
        }
    }
}
