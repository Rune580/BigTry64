using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BigTry64
{
    [Serializable]
    public class Controller
    {
        public List<Player> Players;
        public List<Mob> Mobs;
        public List<World> Worlds;
        public Controller()
        {
            Players = new List<Player>();
            Mobs = new List<Mob>();
            Worlds = new List<World>();
        }
        public void SpawnPlayer(ulong ID, SocketMessage message)
        {
            foreach (var item in Worlds)
            {
                if (item.Name == "TestWorld")
                {
                    for (int i = 0; i < item.Blocks.GetLength(1); i++)
                    {
                        if (item.Blocks[item.Blocks.GetLength(0) / 2, i].Name != "air" && item.Blocks[item.Blocks.GetLength(0) / 2, i].Solid == true)
                        {
                            Players.Add(new Player(item.Blocks.GetLength(0) / 2,i-1, ID, item.Name, @"images/crying.png"));
                            break;
                        }
                    }
                }
            }
        }
        public async Task Display(ulong ID, SocketMessage message, bool Inventory = false)
        {
            if (!IsIn(ID))
            {
                SpawnPlayer(ID, message);
            }
            foreach (var item in Players)
            {
                if (item.UserID == ID)
                {
                    foreach (var world in Worlds)
                    {
                        if (world.Name == item.World)
                        {
                            await message.Channel.SendFileAsync(Screen.display(world, item.X, item.Y, Players, Mobs, item, Inventory));
                            break;
                        }
                    }
                    break;
                }
            }
        }
        public bool IsIn(ulong ID)
        {
            foreach (var item in Players)
            {
                if (item.UserID == ID)
                {
                    return true;
                }
            }
            return false;
        }
        public async Task MovePlayer(ulong ID, string Direction, int Count, SocketMessage message)
        {
            if (!IsIn(ID))
            {
                SpawnPlayer(ID, message);
            }
            Direction = Direction.ToUpper();
            foreach (var player in Players)
            {
                if (player.UserID == ID)
                {
                    int _X = 0;
                    int _Y = 0;
                    if (Direction == "UP")
                    {
                        _Y = -1;
                    }
                    else if (Direction == "DOWN")
                    {
                        _Y = 1;
                    }
                    else if (Direction == "LEFT")
                    {
                        _X = -1;
                    }
                    else if (Direction == "RIGHT")
                    {
                        _X = 1;
                    }
                    foreach (var item in Worlds)
                    {
                        if (item.Name == player.World)
                        {
                            while (Count > 0)
                            {
                                Count--;
                                int TempX = player.X + _X;
                                int TempY = player.Y + _Y;
                                while (TempX < 0)
                                {
                                    TempX++;
                                }
                                while (TempX >= item.Blocks.GetLength(0))
                                {
                                    TempX--;
                                }
                                while (TempY < 0)
                                {
                                    TempY++;
                                }
                                while (TempY >= item.Blocks.GetLength(1))
                                {
                                    TempY--;
                                }


                                if (item.Blocks[TempX,TempY].Solid && (Direction == "LEFT" || Direction == "RIGHT"))
                                {
                                    if (!item.Blocks[TempX,TempY-1].Solid)
                                    {
                                        TempY--;
                                    }
                                    else
                                    {
                                        TempX -= _X;
                                        Count = 0;
                                    }
                                }
                                else if (item.Blocks[TempX, TempY].Solid && Direction == "DOWN")
                                {
                                    TempY--;
                                }
                                while (!item.Blocks[TempX, TempY+1].Solid)
                                {
                                    TempY++;
                                }
                                player.X = TempX;
                                player.Y = TempY;
                            }
                            await Display(message.Author.Id, message);
                            break;
                        }
                    }
                    break;
                }
            }
        }
        public async Task BreakBlock(ulong ID, string Direction1, string Direction2, SocketMessage message)
        {
            Direction1 = Direction1.ToUpper();
            Direction2 = Direction2.ToUpper();
            foreach (var player in Players)
            {
                if (player.UserID == ID)
                {
                    int _X = 0;
                    int _Y = 0;
                    if (Direction1 != Direction2)
                    {
                        if (Direction1 == "UP")
                        {
                            _Y += -1;
                        }
                        else if (Direction1 == "DOWN")
                        {
                            _Y += 1;
                        }
                        else if (Direction1 == "LEFT")
                        {
                            _X += -1;
                        }
                        else if (Direction1 == "RIGHT")
                        {
                            _X += 1;
                        }
                        if (Direction2 == "UP")
                        {
                            _Y += -1;
                        }
                        else if (Direction2 == "DOWN")
                        {
                            _Y += 1;
                        }
                        else if (Direction2 == "LEFT")
                        {
                            _X += -1;
                        }
                        else if (Direction2 == "RIGHT")
                        {
                            _X += 1;
                        }
                    }
                    foreach (var world in Worlds)
                    {
                        if (world.Name == player.World)
                        {
                            if (world.Blocks[player.X + _X, player.Y + _Y].Name != "air")
                            {
                                bool FoundItem = false;
                                for (int y = 0; y < player.Inventory.GetLength(1); y++)
                                {
                                    for (int x = 0; x < player.Inventory.GetLength(0); x++)
                                    {
                                        if (player.Inventory[x, y] != null)
                                        {
                                            if (player.Inventory[x, y].Name == world.Blocks[player.X + _X, player.Y + _Y].Name && player.Inventory[x, y].Count != 50)
                                            {
                                                player.Inventory[x, y].Count++;
                                                FoundItem = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (FoundItem)
                                    {
                                        break;
                                    }
                                }
                                if (!FoundItem)
                                {
                                    for (int y = 0; y < player.Inventory.GetLength(1); y++)
                                    {
                                        for (int x = 0; x < player.Inventory.GetLength(0); x++)
                                        {
                                            if (player.Inventory[x, y] == null)
                                            {
                                                player.Inventory[x, y] = new Item(world.Blocks[player.X + _X, player.Y + _Y], "block");
                                                FoundItem = true;
                                                break;
                                            }
                                        }
                                        if (FoundItem)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            Console.WriteLine(world.Blocks[player.X + _X, player.Y + _Y].isTree);
                            if (world.Blocks[player.X + _X, player.Y + _Y].isTree)
                            {
                                int treeX = player.X + _X;
                                int treeY = player.Y + _Y;
                                Console.WriteLine($"{treeX}, {treeY}: {world.Blocks[treeX, treeY].Name}");
                                while (world.Blocks[treeX, treeY].Name == "oak")
                                {
                                    world.Blocks[treeX, treeY] = new Block(@"images/BT_air.png", "air", false, 60);
                                    treeY--;
                                    Console.WriteLine($"{treeX}, {treeY}: {world.Blocks[treeX, treeY].Name}");
                                }
                                world.refreshLeaves();
                            }
                            else
                            {
                                world.Blocks[player.X + _X, player.Y + _Y] = new Block(@"images/BT_air.png", "air", false, 60);
                            }
                            foreach (var player2 in Players)
                            {
                                if (player2.World == world.Name)
                                {
                                    if (player2.World == world.Name)
                                    {
                                        while (!world.Blocks[player2.X, player2.Y + 1].Solid)
                                        {
                                            player2.Y++;
                                        }
                                    }
                                }
                                await Display(ID, message);
                                break;
                            }
                        }
                    }
                    break;
                }
            }
        }
    }


    [Serializable]
    public class BaseObject
    {
        //Image to use when representing here
        public string Name;
        public string FilePath;
    }
    [Serializable]
    public class BaseMob : BaseObject
    {
        public int X, Y;
        public int Health;
        public Item[,] Inventory = new Item[9,4];
        public string World;
    }
    [Serializable]
    public class Mob : BaseMob
    {
        public void AutoMove()
        {

        }
    }
    [Serializable]
    public class Player : BaseMob
    {
        public Player(int _X, int _Y, ulong _UserID, string _World, string _FilePath)
        {
            X = _X;
            Y = _Y;
            UserID = _UserID;
            World = _World;
            FilePath = _FilePath;
        }
        public ulong UserID;
    }

    [Serializable]
    public class Item : BaseObject
    {
        public Item(Block _Block, string _Type, int _Count = 1)
        {
            Block = _Block;
            Count = _Count;
            Type = _Type;
        }
        public Block Block;
        public int Durability;
        public string Type;
        public int Count;
    }

    [Serializable]
    public class Recipe
    {
        public Recipe(List<string> _inputItems, List<string> _outputItems, string _craftingHandler)
        {
            inputItems = _inputItems;
            outputItems = _outputItems;
            craftingHandler = _craftingHandler;
        }
        public List<string> inputItems;
        public List<string> outputItems;
        public string craftingHandler;
    }

    [Serializable]
    public class Block : BaseObject
    {
        public Block(string _FilePath, string _Name, bool _Solid,int _Chance = 0, Block _backgroundBlock = null, bool _isTree = false, bool _placedByPlayer = false, bool _Breakable = true, string _Text = "")
        {
            FilePath = _FilePath;
            Name = _Name;
            Solid = _Solid;
            Text = _Text;
            Chance = _Chance;
            backgroundBlock = _backgroundBlock;
            isTree = _isTree;
            placedByPlayer = _placedByPlayer;
            Breakable = _Breakable;
        }
        public bool ReadsItsText;
        public string Text;
        public bool Solid;
        public int Chance;
        public Block backgroundBlock;
        public bool isTree;
        public bool placedByPlayer;
        public bool Breakable;
    }
}
