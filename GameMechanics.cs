using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BigTry64
{
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
        public async Task Display(ulong ID, SocketMessage message)
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
                            await message.Channel.SendFileAsync(Screen.display(world, item.X, item.Y, Players, Mobs));
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

    }



    public class BaseObject
    {
        //Image to use when representing here
        public string Name;
        public string FilePath;
    }
    public class BaseMob : BaseObject
    {
        public int X, Y;
        public int Health;
        public List<Item> Inventory = new List<Item>();
        public string World;
    }
    public class Mob : BaseMob
    {
        public void AutoMove()
        {

        }
    }
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
        public void Move(string _Direction, int _Count, ulong _ID, SocketMessage message = null)
        {

        }
    }

    public class Item : BaseObject
    {

    }
    public class Block : BaseObject
    {
        public Block(string _FilePath, string _Name, bool _Solid,int _Chance = 0, Block _backgroundBlock = null, string _Text = "")
        {
            FilePath = _FilePath;
            Name = _Name;
            Solid = _Solid;
            Text = _Text;
            Chance = _Chance;
            backgroundBlock = _backgroundBlock;
        }
        public bool ReadsItsText;
        public string Text;
        public bool Solid;
        public int Chance;
        public Block backgroundBlock;
    }
}
