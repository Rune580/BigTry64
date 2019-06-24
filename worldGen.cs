using System;
using System.Collections.Generic;
using System.Text;

namespace BigTry64
{
    public class World
    {
        public string Name;
        public Block[,] Blocks;

        public Block[,] genWorld()
        {
            Random rand = new Random();
            int previousGrass = 0;
            Block[,] map = new Block[20, 15];
            for (int x = 0; x < map.GetLength(0); x++)
            {
                int grassTile;

                if (x == 0)
                {

                    grassTile = rand.Next(6, 10);
                    previousGrass = grassTile;
                }
                else
                {
                    grassTile = rand.Next(previousGrass-1, previousGrass+2);
                    while (grassTile > 10)
                    {
                        grassTile--;
                    }
                    while (grassTile < 5)
                    {
                        grassTile++;
                    }
                }
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (y < grassTile)
                    {

                    }
                }
            }
            return map;
        }
    }
}
