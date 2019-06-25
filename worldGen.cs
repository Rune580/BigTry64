using System;
using System.Collections.Generic;
using System.Text;

namespace BigTry64
{
    public class World
    {
        public string Name;
        public Block[,] Blocks = new Block[200, 150];

        public void genWorld()
        {
            Random rand = new Random();
            int previousGrass = 0;
            for (int x = 0; x < Blocks.GetLength(0); x++)
            {
                int grassTile;

                if (x == 0)
                {

                    grassTile = rand.Next(6, 10);
                    previousGrass = grassTile;
                }
                else
                {
                    grassTile = rand.Next(previousGrass-1, previousGrass+1);
                    while (grassTile > 10)
                    {
                        grassTile--;
                    }
                    while (grassTile < 5)
                    {
                        grassTile++;
                    }
                }
                for (int y = 0; y < Blocks.GetLength(1); y++)
                {
                    int dirtTile = rand.Next(grassTile+1, grassTile+4);
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
        }
    }
}
