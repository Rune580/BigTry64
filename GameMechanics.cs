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
        public void Move(string _Direction, int _Count = 0)
        {

        }
    }
    public class Mob : BaseMob
    {
        public void AutoMove()
        {

        }
    }
    public class Player : BaseMob
    {
        public ulong UserID;
    }
    public class Inventory
    {

    }

    public class Item : BaseObject
    {

    }
    public class Block : BaseObject
    {
        public bool ReadsItsText;
        public string Text;
        public bool Solid;
    }
}
