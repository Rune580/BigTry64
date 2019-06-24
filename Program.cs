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






            #endregion

            async Task MessageReceived(SocketMessage message)
            {

            }


            Client.MessageReceived += MessageReceived;

            do
            {

            } while (true);
        }
    }
}
