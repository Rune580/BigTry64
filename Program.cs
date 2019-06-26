using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
            Client.SetGameAsync($"image time");
            //string Token = System.IO.File.ReadAllText("token.txt");
            string Token;
            using (StreamReader sr = new StreamReader("token.txt"))
            {
                Token = sr.ReadToEnd();
            }


            Client.LoginAsync(TokenType.Bot, Token);
            Client.StartAsync();


            IFormatter formatter = new BinaryFormatter();
            Controller game = null;
            game = new Controller();
            game.Worlds.Add(new World("TestWorld", 200, 180));
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
                else if ((TheMessage.StartsWith("UP") || TheMessage.StartsWith("DOWN") || TheMessage.StartsWith("LEFT") || TheMessage.StartsWith("RIGHT")))
                {
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
                            await game.MovePlayer(AuthorID, Direction, int.Parse(TheMessage), message);
                        }
                        catch
                        {
                            await game.MovePlayer(AuthorID, Direction, 1, message);
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
                else if (TheMessage.StartsWith("SAVEWORLD"))
                {
                    BinarySave();
                }
            }


            Client.MessageReceived += MessageReceived;
            void BinarySave()
            {
                using (Stream s = new FileStream(@"Save.bin", FileMode.Create))
                {
                    formatter.Serialize(s, game);
                }
            }
            do
            {

            } while (true);
        }
    }
}
