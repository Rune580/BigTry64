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
            game.Worlds.Add(new World());
            game.Worlds[0].genWorld();
            #endregion

            async Task MessageReceived(SocketMessage message)
            {
                if (message.Content.StartsWith("showWorld"))
                {
                   await message.Channel.SendFileAsync(Screen.fullWorld(game.Worlds[0]));
                }
                else if (message.Content.StartsWith("showScreen"))
                {
                   await message.Channel.SendFileAsync(Screen.display(game.Worlds[0], 10, 30));
                }
                else if (message.Content.StartsWith("LigmashowScreen"))
                {
                    await message.Channel.SendFileAsync(Screen.display(game.Worlds[0], 10, 30));
                }
            }


            Client.MessageReceived += MessageReceived;

            do
            {

            } while (true);
        }
    }
}
