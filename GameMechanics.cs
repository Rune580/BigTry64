using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace BigTry64
{
    public class Controller
    {
        public List<Mob> Players;
        public List<Mob> Mobs;
        public List<World> Worlds;
        public Controller()
        {
            Players = new List<Mob>();
            Mobs = new List<Mob>();
            Worlds = new List<World>();
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
        public Inventory Items;
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
        public Player(int _X, int _Y, ulong _UserID, string _World)
        {
            X = _X;
            Y = _Y;
            UserID = _UserID;
            World = _World;
        }
        public ulong UserID;
        public void Move(string _Direction, int _Count, ulong _ID, SocketMessage message = null)
        {

        }
    }
    public class Inventory
    {

    }

    public class Item : BaseObject
    {

    }
    public class Block : BaseObject
    {
        public Block(string _FilePath, string _Name, bool _Solid, string _Text = "")
        {
            FilePath = _FilePath;
            Name = _Name;
            Solid = _Solid;
            Text = _Text;
        }
        public bool ReadsItsText;
        public string Text;
        public bool Solid;
    }
}
