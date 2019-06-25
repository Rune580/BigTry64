using System;
using System.Collections.Generic;
using System.Text;

namespace BigTry64
{
    public class World
    {
        public string Name;
        public Block[,] Blocks = new Block[500, 150];

        public void genWorld()
        {
            Random rand = new Random();
            int grassTile = 0;
            int previousGrass = 0;
            int ppGrass = 0;
            for (int x = 0; x < Blocks.GetLength(0); x++)
            {
                if (x == 0)
                {
                    grassTile = rand.Next(20, 36);
                    previousGrass = grassTile;
                }
                else if(x == 1)
                {
                    ppGrass = previousGrass;
                    grassTile = rand.Next(previousGrass - 1, previousGrass + 2);
                    while (grassTile > 35)
                    {
                        grassTile--;
                    }
                    while (grassTile < 20)
                    {
                        grassTile++;
                    }
                    previousGrass = grassTile;

                }
                else
                {
                    int Difference;
                    int Odds = rand.Next(0,100);
                    Difference = ppGrass - previousGrass;
                    ppGrass = previousGrass;
                    if (Difference == -1)
                    {
                        if (Odds > 40)
                        {
                            grassTile = previousGrass + 1;
                        }
                        else if (Odds > 10)
                        {
                            grassTile = previousGrass;
                        }
                        else
                        {
                            grassTile = previousGrass - 1;
                        }
                    }
                    else if (Difference == 0)
                    {
                        if (Odds > 40)
                        {
                            grassTile = previousGrass;
                        }
                        else if (Odds > 20)
                        {
                            grassTile = previousGrass + 1;
                        }
                        else
                        {
                            grassTile = previousGrass - 1;
                        }
                    }
                    else
                    {
                        if (Odds > 40)
                        {
                            grassTile = previousGrass - 1;
                        }
                        else if (Odds > 10)
                        {
                            grassTile = previousGrass;
                        }
                        else
                        {
                            grassTile = previousGrass + 1;
                        }
                    }
                    previousGrass = grassTile;
                }
                for (int y = 0; y < Blocks.GetLength(1); y++)
                {
                    int dirtTile = rand.Next(grassTile+1, grassTile+12);
                    if (y < grassTile)
                    {
                        Blocks[x, y] = new Block(@"images/BT_air.png", "air", false);
                    }
                    else if (y == grassTile)
                    {
                        Blocks[x, y] = new Block(@"images/BT_grass.png", "grass", true);
                    }
                    else if (y <= dirtTile)
                    {
                        Blocks[x, y] = new Block(@"images/BT_dirt.png", "dirt", true);
                    }
                    else
                    {
                        Blocks[x, y] = new Block(@"images/BT_stone.png", "stone", true);
                    }
                }
            }
            for (int x = 0; x < Blocks.GetLength(0); x++)
            {
                for (int y = 0; y < Blocks.GetLength(1); y++)
                {
                    int oreCoalTile = rand.Next(20, 120);
                    if (Blocks[x, y].Name == "stone");
                    {
                        //Blocks[x, y] = new Block(@"images/BT_orecoal.png", "coal", true);
                    }
                }
            }
            Console.WriteLine("WorldGen Complete");
        }
    }
}
