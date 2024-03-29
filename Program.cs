﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BigTry64
{
    class Program
    {
        static void Main(string[] args)
        {
            #region startup
            DiscordSocketClient Client;
            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info
            });

            CommandService Commands;
            Commands = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = true,
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Debug
            });
            Client.Log += Log;
            Task Log(LogMessage message)
            {
                Console.WriteLine(message.ToString());
                return Task.CompletedTask;
            }
            Client.SetGameAsync($"Type: BigHelp");
            //string Token = System.IO.File.ReadAllText("token.txt");
            string Token;
            using (StreamReader sr = new StreamReader("token.txt"))
            {
                Token = sr.ReadToEnd();
            }


            Client.LoginAsync(TokenType.Bot, Token);
            Client.StartAsync();


            IFormatter formatter = new BinaryFormatter();
            Controller game = new Controller();
            if (File.Exists(@"Save.bin"))
            {
                using (Stream s = new FileStream(@"Save.bin", FileMode.Open, FileAccess.Read))
                {
                    game = (Controller)formatter.Deserialize(s);
                }
            }
            else
            {
                game.Worlds.Add(new World("TestWorld", 200, 180));
            }
            int CurrentHour = DateTime.Now.Hour;
            #endregion

            async Task MessageReceived(SocketMessage message)
            {
                string TheMessage = message.Content.ToUpper();
                ulong AuthorID = message.Author.Id;



                if (TheMessage.StartsWith("SHOWWORLD"))
                {
                    await message.Channel.SendFileAsync(Screen.fullWorld(game.Worlds[0], message));
                }
                else if (TheMessage.StartsWith("SHOWSCREEN"))
                {
                    await game.Display(message.Author.Id, message);
                }
                else if (TheMessage.StartsWith("SPAWNME"))
                {
                    game.SpawnPlayer(AuthorID, message);
                }
                else if ((TheMessage.StartsWith("UP") || TheMessage.StartsWith("DOWN") || TheMessage.StartsWith("LEFT") || TheMessage.StartsWith("RIGHT") || TheMessage.StartsWith("JUMP")))
                {
                    bool Jump = false;
                    if (TheMessage.StartsWith("JUMP"))
                    {
                        Jump = true;
                        TheMessage = TheMessage.Remove(0,"JUMP".Length);
                        while (TheMessage[0] == ' ')
                        {
                            TheMessage = TheMessage.Remove(0, 1);
                        }
                    }
                    string Direction = "";
                    if (TheMessage.StartsWith("UP"))
                    {
                        Direction = "UP";
                        TheMessage = TheMessage.Remove(0, "UP".Length);
                    }
                    else if (TheMessage.StartsWith("DOWN"))
                    {
                        Direction = "DOWN";
                        TheMessage = TheMessage.Remove(0, "DOWN".Length);
                    }
                    else if (TheMessage.StartsWith("LEFT"))
                    {
                        Direction = "LEFT";
                        TheMessage = TheMessage.Remove(0, "LEFT".Length);
                    }
                    else if (TheMessage.StartsWith("RIGHT"))
                    {
                        Direction = "RIGHT";
                        TheMessage = TheMessage.Remove(0, "RIGHT".Length);
                    }
                    if (TheMessage.Length < 6)
                    {
                        try
                        {
                            await game.MovePlayer(AuthorID, Direction, int.Parse(TheMessage), message, Jump);
                        }
                        catch
                        {
                            try
                            {
                                await game.MovePlayer(AuthorID, Direction, 1, message, Jump);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                else if ((TheMessage.StartsWith("BUP") || TheMessage.StartsWith("BDOWN") || TheMessage.StartsWith("BLEFT") || TheMessage.StartsWith("BRIGHT") || TheMessage.StartsWith("BHERE")))
                {
                    string Direction1 = "";
                    string Direction2 = "";
                    if (TheMessage.StartsWith("BUP"))
                    {
                        Direction1 = "UP";
                        TheMessage = TheMessage.Remove(0, "BUP".Length);
                    }
                    else if (TheMessage.StartsWith("BDOWN"))
                    {
                        Direction1 = "DOWN";
                        TheMessage = TheMessage.Remove(0, "BDOWN".Length);
                    }
                    else if (TheMessage.StartsWith("BLEFT"))
                    {
                        Direction1 = "LEFT";
                        TheMessage = TheMessage.Remove(0, "BLEFT".Length);
                    }
                    else if (TheMessage.StartsWith("BRIGHT"))
                    {
                        Direction1 = "RIGHT";
                        TheMessage = TheMessage.Remove(0, "BRIGHT".Length);
                    }
                    else if (TheMessage.StartsWith("BHERE"))
                    {
                        Direction1 = "HERE";
                        TheMessage = TheMessage.Remove(0, "BHERE".Length);
                    }

                    try
                    {
                        while (TheMessage[0] == ' ')
                        {
                            TheMessage = TheMessage.Remove(0, 1);
                        }


                        if (TheMessage.StartsWith("BUP"))
                        {
                            Direction2 = "UP";
                            TheMessage = TheMessage.Remove(0, "BUP".Length);
                        }
                        else if (TheMessage.StartsWith("BDOWN"))
                        {
                            Direction2 = "DOWN";
                            TheMessage = TheMessage.Remove(0, "BDOWN".Length);
                        }
                        else if (TheMessage.StartsWith("BLEFT"))
                        {
                            Direction2 = "LEFT";
                            TheMessage = TheMessage.Remove(0, "BLEFT".Length);
                        }
                        else if (TheMessage.StartsWith("BRIGHT"))
                        {
                            Direction2 = "RIGHT";
                            TheMessage = TheMessage.Remove(0, "BRIGHT".Length);
                        }
                        else if (TheMessage.StartsWith("BHERE"))
                        {
                            Direction2 = "HERE";
                            TheMessage = TheMessage.Remove(0, "BHERE".Length);
                        }
                    }
                    catch
                    {
                    }
                    await game.BreakBlock(AuthorID, Direction1, Direction2, message);
                }
                else if ((TheMessage.StartsWith("PUP") || TheMessage.StartsWith("PDOWN") || TheMessage.StartsWith("PLEFT") || TheMessage.StartsWith("PRIGHT") || TheMessage.StartsWith("PHERE")))
                {
                    string Direction1 = "";
                    string Direction2 = "";
                    if (TheMessage.StartsWith("PUP"))
                    {
                        Direction1 = "UP";
                        TheMessage = TheMessage.Remove(0, "PUP".Length);
                    }
                    else if (TheMessage.StartsWith("PDOWN"))
                    {
                        Direction1 = "DOWN";
                        TheMessage = TheMessage.Remove(0, "PDOWN".Length);
                    }
                    else if (TheMessage.StartsWith("PLEFT"))
                    {
                        Direction1 = "LEFT";
                        TheMessage = TheMessage.Remove(0, "PLEFT".Length);
                    }
                    else if (TheMessage.StartsWith("PRIGHT"))
                    {
                        Direction1 = "RIGHT";
                        TheMessage = TheMessage.Remove(0, "PRIGHT".Length);
                    }
                    else if (TheMessage.StartsWith("PHERE"))
                    {
                        Direction1 = "HERE";
                        TheMessage = TheMessage.Remove(0, "PHERE".Length);
                    }

                    try
                    {
                        while (TheMessage[0] == ' ')
                        {
                            TheMessage = TheMessage.Remove(0, 1);
                        }


                        if (TheMessage.StartsWith("PUP"))
                        {
                            Direction2 = "UP";
                            TheMessage = TheMessage.Remove(0, "PUP".Length);
                        }
                        else if (TheMessage.StartsWith("PDOWN"))
                        {
                            Direction2 = "DOWN";
                            TheMessage = TheMessage.Remove(0, "PDOWN".Length);
                        }
                        else if (TheMessage.StartsWith("PLEFT"))
                        {
                            Direction2 = "LEFT";
                            TheMessage = TheMessage.Remove(0, "PLEFT".Length);
                        }
                        else if (TheMessage.StartsWith("PRIGHT"))
                        {
                            Direction2 = "RIGHT";
                            TheMessage = TheMessage.Remove(0, "PRIGHT".Length);
                        }
                        else if (TheMessage.StartsWith("PHERE"))
                        {
                            Direction2 = "HERE";
                            TheMessage = TheMessage.Remove(0, "PHERE".Length);
                        }
                    }
                    catch
                    {
                    }
                    await game.PlaceBlock(AuthorID, Direction1, Direction2, message);
                }
                else if (TheMessage.StartsWith("SAVEWORLD"))
                {
                    BinarySave();
                }
                else if (TheMessage.StartsWith("SHOWINV"))
                {
                    await game.Display(message.Author.Id, message, true);
                }
                else if (TheMessage.StartsWith("SWAP"))
                {
                    try
                    {
                        TheMessage = TheMessage.Remove(0, "SWAP".Length);
                        while (TheMessage[0] == ' ')
                        {
                            TheMessage = TheMessage.Remove(0, 1);
                        }
                        string[] Temp = TheMessage.Split(' ');
                        await game.SwapItems(Temp[0][0], Temp[1][0], AuthorID, message);
                    }
                    catch
                    {
                        await message.Channel.SendMessageAsync("Something went wrong, you might have input an invalid range or in the incorrect format. Correct format (swap 1 1 1 4)");
                    }
                }
                else if (TheMessage.StartsWith("BIGHELP"))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("```");
                    sb.Append("SpawnMe: Spawn yourself into the world!\n\n");
                    sb.Append("Up/Down/Left/Right: Move in a direction!\n\n");
                    sb.Append("Jump followed by Left/Right: Jump either left or right to cross small gaps or gain a bit more height!\n\n");
                    sb.Append("Bup/Bdown/Bleft/Bright: break a block in the direction specified. Combine two of them to break diagonaly!\n\n");
                    sb.Append("Pup/Pdown/Pleft/Pright: Place your currently selected block in the direction specified!\n\n");
                    sb.Append("ShowInv: Show your entire inventory!\n\n");
                    sb.Append("Swap: Swap items in your inventory! (EX: Swap Q 4)\n\n");
                    sb.Append("Hotbar: Change your selected hotbar slot! (EX: Hotbar 3)\n\n");
                    sb.Append("ShowScreen: Show your screen without moving!\n\n");
                    sb.Append("ShowWorld: Show the entire world! (Not recommended to spam this, it takes a long time)\n\n");
                    sb.Append("SaveWorld: The world saves automatically on the hour, but if you want to manually save, this is how!\n\n");
                    sb.Append("BigHelp: The command you just used!\n\n");
                    sb.Append("```");
                    await message.Channel.SendMessageAsync(sb.ToString());
                }
                #region rune gay
                //else if (TheMessage.StartsWith("SHOWCRAFTING"))
                //{
                //    await game.Display(message.Author.Id, message, false, true);
                //    await message.Channel.SendMessageAsync("to craft an item, Type: craft 'itemname' 'amount'");
                //}
                //else if (TheMessage.StartsWith("CRAFT"))
                //{
                //    TheMessage = TheMessage.Remove(0, "CRAFT".Length);
                //    try
                //    {
                //        while (TheMessage[0] == ' ')
                //        {
                //            TheMessage = TheMessage.Remove(0, 1);
                //        }
                //    }
                //    catch
                //    {
                //    }
                //    try
                //    {
                //        string[] _message = TheMessage.Split(' ');
                //        await game.CraftItems(_message[0], Int32.Parse(_message[1]), AuthorID, message);
                //    }
                //    catch (System.IndexOutOfRangeException e)
                //    {
                //        await game.CraftItems(TheMessage, 1, AuthorID, message);
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine(e);
                //        await message.Channel.SendMessageAsync("Something went wrong with the input, the expect input should look similar to ex: craft plank 10 (craft 'itemname' 'amount')");
                //    }
                //}
                #endregion
                else if (TheMessage.StartsWith("HOTBAR"))
                {
                    TheMessage = TheMessage.Remove(0, "HOTBAR".Length);
                    while (TheMessage[0] == ' ')
                    {
                        TheMessage = TheMessage.Remove(0, 1);
                    }
                    try
                    {
                        await game.HotBarChange(int.Parse(TheMessage), AuthorID, message);
                    }
                    catch
                    {
                    }
                }
            }


            Client.MessageReceived += MessageReceived;
            void BinarySave()
            {
                using (Stream s = new FileStream(@"Save.bin", FileMode.Create, FileAccess.Write))
                {
                    formatter.Serialize(s, game);
                }
            }
            do
            {
                if (DateTime.Now.Hour != CurrentHour)
                {
                    CurrentHour = DateTime.Now.Hour;
                    BinarySave();
                }
            } while (true);
        }
    }
}
