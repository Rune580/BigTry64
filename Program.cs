using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.IO;
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





            Controller game = new Controller();
            game.Worlds.Add(new World("TestWorld",580,180));
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
                            await game.MovePlayer(AuthorID, Direction, int.Parse(TheMessage),  message);
                        }
                        catch
                        {
                            await game.MovePlayer(AuthorID, Direction, 1,  message);
                        }
                    }
                }
            }


            Client.MessageReceived += MessageReceived;

            do
            {

            } while (true);
        }
    }
}
