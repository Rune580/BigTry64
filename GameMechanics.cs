using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
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
                        if (item.Blocks[item.Blocks.GetLength(0) / 2, i].Name != "air" && item.Blocks[item.Blocks.GetLength(0) / 2, i].Solid == true && item.Blocks[item.Blocks.GetLength(0) / 2, i].Name != "unbreakable")
                        {
                            using (WebClient webClient = new WebClient())
                            {
                                webClient.DownloadFile(message.Author.GetAvatarUrl(size: 32), $@"images/{message.Author.Id}.png");
                            }
                            Players.Add(new Player(item.Blocks.GetLength(0) / 2,i-1, ID, item.Name, $@"images/{message.Author.Id}.png"));
                            break;
                        }
                    }
                }
            }
        }
        public async Task Display(ulong ID, SocketMessage message, bool Inventory = false, bool crafting = false)
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
                            await message.Channel.SendFileAsync(Screen.display(world, item.X, item.Y, Players, Mobs, item, Inventory, crafting));
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
        public async Task MovePlayer(ulong ID, string Direction, int Count, SocketMessage message, bool Jump = false)
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
                        if (Jump)
                        {
                            _Y = -1;
                        }
                        _X = -1;
                    }
                    else if (Direction == "RIGHT")
                    {
                        if (Jump)
                        {
                            _Y = -1;
                        }
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


                                if (item.Blocks[TempX,TempY].Solid && (Direction == "LEFT" || Direction == "RIGHT") && !Jump)
                                {
                                    if (!item.Blocks[TempX, TempY - 1].Solid && item.Blocks[TempX, TempY].Solid)
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
                                if (Jump)
                                {
                                    Count = 0;
                                    if (!item.Blocks[TempX, TempY-1].Solid && item.Blocks[TempX, TempY].Solid)
                                    {
                                        TempY -= 1;
                                    }
                                    else if (item.Blocks[TempX + _X, TempY + 1].Solid && !item.Blocks[TempX + _X, TempY].Solid)
                                    {
                                        TempX += _X;
                                    }
                                    else if (!item.Blocks[TempX + _X, TempY + 1].Solid)
                                    {
                                        TempX += _X;
                                        TempY++;
                                    }
                                    else
                                    {
                                        TempX = player.X;
                                        TempY = player.Y;
                                    }
                                }
                                while (!item.Blocks[TempX, TempY+1].Solid && !item.Blocks[TempX, TempY + 1].Ladder && !item.Blocks[TempX, TempY].Ladder)
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
                            if (world.Blocks[player.X + _X, player.Y + _Y].Name != "air" && world.Blocks[player.X + _X, player.Y + _Y].Name != "darkstone" && world.Blocks[player.X + _X, player.Y + _Y].Name != "unbreakable" && world.Blocks[player.X + _X, player.Y + _Y].Breakable != false)
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
                                    for (int y = player.Inventory.GetLength(1)-1; y >= 0; y--)
                                    {
                                        for (int x = 0; x < player.Inventory.GetLength(0); x++)
                                        {
                                            if (player.Inventory[x, y] == null)
                                            {
                                                player.Inventory[x, y] = new Item(world.Blocks[player.X + _X, player.Y + _Y], "block");
                                                player.Inventory[x, y].Name = world.Blocks[player.X + _X, player.Y + _Y].Name;
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
                            //Console.WriteLine(world.Blocks[player.X + _X, player.Y + _Y].isTree);
                            if (world.Blocks[player.X + _X, player.Y + _Y].isTree)
                            {
                                int treeX = player.X + _X;
                                int treeY = player.Y + _Y;
                                //Console.WriteLine($"{treeX}, {treeY}: {world.Blocks[treeX, treeY].Name}");
                                while (world.Blocks[treeX, treeY].Name == "oak")
                                {
                                    #region dont open this
                                    bool FoundItem = false;
                                    for (int y = 0; y < player.Inventory.GetLength(1); y++)
                                    {
                                        for (int x = 0; x < player.Inventory.GetLength(0); x++)
                                        {
                                            if (player.Inventory[x, y] != null)
                                            {
                                                if (player.Inventory[x, y].Name == world.Blocks[treeX, treeY].Name && player.Inventory[x, y].Count != 50)
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
                                        for (int y = player.Inventory.GetLength(1) - 1; y >= 0; y--)
                                        {
                                            for (int x = 0; x < player.Inventory.GetLength(0); x++)
                                            {
                                                if (player.Inventory[x, y] == null)
                                                {
                                                    player.Inventory[x, y] = new Item(world.Blocks[treeX, treeY], "block");
                                                    player.Inventory[x, y].Name = world.Blocks[treeX, treeY].Name;
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
                                    #endregion
                                    world.Blocks[treeX, treeY] = new Block(@"images/BT_air.png", "air", false, 60);
                                    treeY--;
                                    //Console.WriteLine($"{treeX}, {treeY}: {world.Blocks[treeX, treeY].Name}");
                                }
                                world.LeafCycle();
                                //world.refreshLeaves();
                            }
                            else if (world.Blocks[player.X + _X, player.Y + _Y].backgroundBlock != null)
                            {
                                world.Blocks[player.X + _X, player.Y + _Y] = world.Blocks[player.X + _X, player.Y + _Y].backgroundBlock;
                            }
                            else if (world.Blocks[player.X + _X, player.Y + _Y].Name != "darkstone")
                            {
                                world.Blocks[player.X + _X, player.Y + _Y] = new Block(@"images/BT_air.png", "air", false, 60);
                            }
                            Gravity();
                            await Display(ID, message);
                        }
                    }
                    break;
                }
            }
        }
        public async Task PlaceBlock(ulong ID, string Direction1, string Direction2, SocketMessage message)
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
                            bool PlayerCollide = false;
                            foreach (var player2 in Players)
                            {
                                if (player2.UserID != player.UserID && player2.X == player.X + _X && player2.Y == player.Y + _Y)
                                {
                                    PlayerCollide = true;
                                }
                            }
                            if (!world.Blocks[player.X + _X, player.Y].Solid && !PlayerCollide)
                            {
                                try
                                {
                                    if (_X == 0 && _Y == 0 && !world.Blocks[player.X, player.Y - 1].Solid && !world.Blocks[player.X, player.Y - 1].isTree && player.Inventory[player.HotBar, 3].Count > 0)
                                    {
                                        world.Blocks[player.X + _X, player.Y + _Y] = player.Inventory[player.HotBar, 3].Block;
                                        if (world.Blocks[player.X + _X, player.Y + _Y].isTree)
                                        {
                                            world.Blocks[player.X + _X, player.Y + _Y].isTree = false;
                                            world.Blocks[player.X + _X, player.Y + _Y].Solid = true;
                                        }
                                        player.Inventory[player.HotBar, 3].Count--;
                                        player.Y--;
                                    }
                                    else if (_X != 0 || _Y != 0)
                                    {
                                        world.Blocks[player.X + _X, player.Y + _Y] = player.Inventory[player.HotBar, 3].Block;
                                        if (world.Blocks[player.X + _X, player.Y + _Y].isTree)
                                        {
                                            world.Blocks[player.X + _X, player.Y + _Y].isTree = false;
                                            world.Blocks[player.X + _X, player.Y + _Y].Solid = true;
                                        }
                                        player.Inventory[player.HotBar, 3].Count--;
                                    }
                                    if (player.Inventory[player.HotBar, 3].Count == 0)
                                    {
                                        player.Inventory[player.HotBar, 3] = null;
                                    }
                                }
                                catch
                                {
                                }
                            }
                            await Display(ID, message);
                        }
                    }
                    break;
                }
            }
        }
        public void Gravity()
        {            foreach (var player2 in Players)
            {                foreach (var world in Worlds)
                {
                    if (player2.World == world.Name)
                    {
                        while (!world.Blocks[player2.X, player2.Y + 1].Solid && !world.Blocks[player2.X, player2.Y + 1].Ladder && !world.Blocks[player2.X, player2.Y].Ladder)
                        {
                            player2.Y++;
                        }
                    }
                }
            }
        }
        public async Task CraftItems(string craftItem, int amount, ulong ID, SocketMessage message)
        {
            craftItem = craftItem.ToLower();
            Item givetoPlayer = null;
            foreach (var player in Players)
            {
                foreach (var world in Worlds)
                {
                    if (player.World == world.Name)
                    {
                        if (player.UserID == ID)
                        {
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
                                                if (player.Inventory[x, y].Block.Name == world.Recipes[i].inputItems[_i].Block.Name || player.Inventory[x, y].Block.Name == world.Recipes[i].inputItems[_i].Name)
                                                {
                                                    while (player.Inventory[x, y].Count < (world.Recipes[i].inputItems[_i].Count * amount))
                                                    {
                                                        amount--;
                                                    }
                                                    if (craftItem == world.Recipes[i].outputItems[0].Block.Name)
                                                    {
                                                        givetoPlayer = world.Recipes[i].outputItems[0];
                                                        givetoPlayer.Name = world.Recipes[i].outputItems[0].Name;
                                                        player.Inventory[x, y].Count -= (world.Recipes[i].inputItems[_i].Count * amount);
                                                        if (player.Inventory[x, y].Count == 0)
                                                        {
                                                            player.Inventory[x, y] = null;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            
                            bool itemExists = false;
                            while(givetoPlayer != null)
                            {
                                Console.WriteLine($"{givetoPlayer.Name} othername: {givetoPlayer.Block.Name} yes: {givetoPlayer}");
                                for (int x = 0; x < player.Inventory.GetLength(0); x++)
                                {
                                    for (int y = 0; y < player.Inventory.GetLength(1); y++)
                                    {
                                        if (player.Inventory[x,y] != null)
                                        {
                                            if (player.Inventory[x, y].Block.Name == givetoPlayer.Block.Name)
                                            {
                                                itemExists = true;
                                            }
                                        }
                                    }
                                }
                                Console.WriteLine(itemExists);
                                int LIMIT = givetoPlayer.Count;
                                for (int x = 0; x < player.Inventory.GetLength(0); x++)
                                {
                                    for (int y = 0; y < player.Inventory.GetLength(1); y++)
                                    {
                                        if (itemExists)
                                        {
                                            if (player.Inventory[x, y] != null)
                                            {
                                                if (player.Inventory[x, y].Block.Name == givetoPlayer.Block.Name)
                                                {
                                                    for (int i = 0; i < amount; i++)
                                                    {
                                                        for (int remaining = 0; remaining < LIMIT; remaining++)
                                                        {
                                                            player.Inventory[x, y].Count++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else if (givetoPlayer != null)
                                        {
                                            if (player.Inventory[x, y] == null)
                                            {
                                                player.Inventory[x, y] = new Item(givetoPlayer.Block, "block", 0);
                                                Console.WriteLine(player.Inventory[x,y].Block.Name);
                                            }
                                            else
                                            {
                                                break;
                                            }
                                            for (int i = 0; i < amount; i++)
                                            {
                                                for (int remaining = 0; remaining < LIMIT; remaining++)
                                                {
                                                    if (player.Inventory[x, y] == null)
                                                    {
                                                        player.Inventory[x, y] = new Item(givetoPlayer.Block, "block", givetoPlayer.Count);
                                                        player.Inventory[x, y].Name = craftItem;
                                                        Console.WriteLine($"{player.Inventory[x,y].Block.Name}");
                                                    }
                                                    else
                                                    {
                                                        player.Inventory[x, y].Count++;
                                                    }
                                                }
                                            }
                                            givetoPlayer = null;
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
            await Display(message.Author.Id, message, true);
        }
        public async Task SwapItems(char item1, char item2, ulong ID, SocketMessage message)
        {
            foreach (var player in Players)
            {
                if (player.UserID == ID)
                {
                    int[] Item1 = LetterParsing.ParseString(item1);
                    int[] Item2 = LetterParsing.ParseString(item2);
                    player.SwapItem(Item1[0], Item1[1], Item2[0], Item2[1]);
                    await Display(message.Author.Id, message, true);
                }
            }
        }
        public async Task HotBarChange(int Num, ulong ID, SocketMessage message)
        {
            foreach (var player in Players)
            {
                if (player.UserID == ID)
                {
                    if (Num-1 >= 0 && Num-1 <= 8)
                    {
                        player.HotBar = Num - 1;
                        await Display(ID, message, false);
                    }
                }
            }
        }
    }


    [Serializable]
    public class BaseObject
    {
        //Image to use when representing here
        public int X, Y;
        public string Name;
        public string FilePath;
    }
    [Serializable]
    public class BaseMob : BaseObject
    {
        public int Health;
        public Item[,] Inventory = new Item[9,4];
        public string World;
        public void SwapItem(int X1, int Y1, int X2, int Y2)
        {
            Item Temp = Inventory[X1, Y1];
            Inventory[X1, Y1] = Inventory[X2, Y2];
            Inventory[X2, Y2] = Temp;
        }
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
        public Player(int _X, int _Y, ulong _UserID, string _World, string _FilePath, bool _craftingstation = false, bool _furnacestation = false)
        {
            X = _X;
            Y = _Y;
            UserID = _UserID;
            World = _World;
            FilePath = _FilePath;
            HotBar = 0;
        }
        public int HotBar;
        public ulong UserID;
        public bool craftingstation;
        public bool furnacestation;
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
        public Recipe(Item[] _inputItems, Item[] _outputItems, string _craftingHandler)
        {
            inputItems = _inputItems;
            outputItems = _outputItems;
            craftingHandler = _craftingHandler;
        }
        public Item[] inputItems;
        public Item[] outputItems;
        public string craftingHandler;
    }

    [Serializable]
    public class Block : BaseObject
    {
        public Block(string _FilePath, string _Name, bool _Solid, int _Chance = 0, Block _backgroundBlock = null, bool _isTree = false, bool _placedByPlayer = false, bool _Ladder = false, bool _Breakable = true, string _Text = "")
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
            Ladder = _Ladder;
        }
        public void SetPos(int _X, int _Y)
        {
            X = _X;
            Y = _Y;
        }
        public void LeafDecay(ref Block[,] Blocks, int _Distance = 99999)
        {
            if (_Distance != 99999)
            {
                Distance = _Distance;
            }
            while (X-1 < 0)
            {
                X++;
            }
            while (X+1 >= Blocks.GetLength(0))
            {
                X--;
            }
            while (Y - 1 < 0)
            {
                Y++;
            }
            while (Y + 1 >= Blocks.GetLength(1))
            {
                Y--;
            }
            if (Blocks[X - 1, Y].Name == "oak" || Blocks[X + 1, Y].Name == "oak" || Blocks[X, Y - 1].Name == "oak" || Blocks[X, Y + 1].Name == "oak")
            {
                _Distance = 1;
                Distance = _Distance;
            }
            if (_Distance < 3)
            {
                if (Blocks[X - 1, Y].Name == "leaves" && Blocks[X - 1, Y].Distance > _Distance + 1)
                {
                    Blocks[X - 1, Y].LeafDecay(ref Blocks, _Distance + 1);
                }
                if (Blocks[X + 1, Y].Name == "leaves" && Blocks[X + 1, Y].Distance > _Distance + 1)
                {
                    Blocks[X + 1, Y].LeafDecay(ref Blocks, _Distance + 1);
                }
                if (Blocks[X, Y - 1].Name == "leaves" && Blocks[X, Y - 1].Distance > _Distance + 1)
                {
                    Blocks[X, Y - 1].LeafDecay(ref Blocks, _Distance + 1);
                }
                if (Blocks[X, Y + 1].Name == "leaves" && Blocks[X, Y + 1].Distance > _Distance + 1)
                {
                    Blocks[X, Y + 1].LeafDecay(ref Blocks, _Distance + 1);
                }
            }
        }
        public bool ReadsItsText;
        public string Text;
        public bool Solid;
        public int Chance;
        public Block backgroundBlock;
        public bool isTree;
        public bool placedByPlayer;
        public bool Breakable;
        public bool Ladder;
        public int Distance = 99999;
        ~Block()
        {
            
        }
    }
}
