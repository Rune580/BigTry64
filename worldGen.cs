using System;
using System.Collections.Generic;
using System.Text;

namespace BigTry64
{
    public class World
    {
        public string Name;
        public Block[,] Blocks = new Block[1000, 180];
        Random rand = new Random();

        public void genWorld()
        {
            
            int grassTile = 0;
            int previousGrass = 0;
            int ppGrass = 0;
            for (int x = 0; x < Blocks.GetLength(0); x++)
            {
                if (x == 0)
                {
                    grassTile = rand.Next(40, 60);
                    previousGrass = grassTile;
                }
                else if(x == 1)
                {
                    ppGrass = previousGrass;
                    grassTile = rand.Next(previousGrass - 1, previousGrass + 2);
                    while (grassTile > 59)
                    {
                        grassTile--;
                    }
                    while (grassTile < 40)
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
                    while (grassTile > 59)
                    {
                        grassTile--;
                    }
                    while (grassTile < 40)
                    {
                        grassTile++;
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
                        Blocks[x, y] = new Block(@"images/BT_stone.png", "stone", true, 80);
                    }
                }
            }
            // First Pass Generation
            for (int x = 0; x < Blocks.GetLength(0); x++)
            {
                int treeChance = rand.Next(0,100);
                if (treeChance > 60)
                {
                    for (int y = 0; y < Blocks.GetLength(1); y++)
                    {
                        if (Blocks[x,y].Name == "grass")
                        {
                            int _y = y - 1;
                            int treeClamp = 20;
                            treeChance = rand.Next(0, 300);
                            while (treeChance > 1)
                            {
                                Blocks[x, _y] = new Block(@"images/BT_oaklog.png", "oak", false);
                                _y--;
                                treeChance = rand.Next(0, treeClamp*treeClamp);
                                treeClamp--;
                            }
                            if (Blocks.GetLength(0) != x)
                            {
                                x += 2;
                            }
                        }
                    }
                }
                for (int y = 0; y < Blocks.GetLength(1); y++)
                {
                    int oreCoalTile = rand.Next(0, 100);
                    int oreIronTile = rand.Next(0, 100);
                    int oreGoldTile = rand.Next(0, 100);
                    int oreDiamondTile = rand.Next(0, 200);
                    if (Blocks[x, y].Name == "stone" && oreCoalTile == 0)
                    {
                        Blocks[x, y] = new Block(@"images/BT_orecoal.png", "coal", true, 100);
                    }
                    else if (Blocks[x, y].Name == "stone" && oreIronTile == 0 && y > 60)
                    {
                        Blocks[x, y] = new Block(@"images/BT_oreiron.png", "iron", true, 90);
                    }
                    else if (Blocks[x, y].Name == "stone" && oreGoldTile == 0 && y > 100)
                    {
                        Blocks[x, y] = new Block(@"images/BT_oregold.png", "gold", true, 55);
                    }
                    else if (Blocks[x, y].Name == "stone" && oreDiamondTile == 0 && y > 120)
                    {
                        Blocks[x, y] = new Block(@"images/BT_orediamond.png", "diamond", true, 35);
                    }
                }
            }
            genOre(4, 22, @"images/BT_orecoal.png", "coal");
            genOre(3, 40, @"images/BT_oreiron.png", "iron");
            genOre(2, 20, @"images/BT_oregold.png", "gold");
            genOre(1, 25, @"images/BT_orediamond.png", "diamond");

            Console.WriteLine("WorldGen Complete");
        }
        public void genOre(int passes, int reduction, string orepng, string orename)
        {
            for (int i = 0; i < passes; i++)
            {
                for (int x = 0; x < Blocks.GetLength(0); x++)
                {
                    for (int y = 0; y < Blocks.GetLength(1); y++)
                    {
                        try
                        {
                            if (Blocks[x, y].Name == orename)
                            {

                                int chance = Blocks[x, y].Chance;
                                int expandChance;
                                expandChance = rand.Next(0, 100);
                                if (Blocks[x - 1, y].Chance < expandChance && Blocks[x - 1, y].Name == "stone")
                                {
                                    Blocks[x - 1, y] = new Block(orepng, orename, true, chance - reduction);
                                }
                                expandChance = rand.Next(0, 100);
                                if (Blocks[x + 1, y].Chance < expandChance && Blocks[x + 1, y].Name == "stone")
                                {
                                    Blocks[x + 1, y] = new Block(orepng, orename, true, chance - reduction);
                                }
                                expandChance = rand.Next(0, 100);
                                if (Blocks[x, y - 1].Chance < expandChance && Blocks[x, y - 1].Name == "stone")
                                {
                                    Blocks[x, y - 1] = new Block(orepng, orename, true, chance - reduction);
                                }
                                expandChance = rand.Next(0, 100);
                                if (Blocks[x, y + 1].Chance < expandChance && Blocks[x, y + 1].Name == "stone")
                                {
                                    Blocks[x, y + 1] = new Block(orepng, orename, true, chance - reduction);
                                }

                            }
                        }
                        catch
                        {
                        }

                    }
                }
            }
        }
    }
}
