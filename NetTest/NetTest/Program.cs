using System;
using System.Threading;
using Lidgren.Network;
using System.IO;

using NetTest.Client;

using Game = NetTest.Client.Game;
using NetTest.Server;
using System.Windows.Forms;
using NetTest.Common;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace NetTest
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] arguments)
        {
            #region OldStartup

            string GameType = "none";
            string[] args = new string[0];
            PlayerInfo clientInfo = new PlayerInfo();
            string IP = null;
            int? port = null;

            if (arguments.Length >= 5 & arguments[0] == "client")
            {
                GameType = "client";
                clientInfo.Username = arguments[1];
                byte r = byte.Parse(arguments[2]);
                byte g = byte.Parse(arguments[3]);
                byte b = byte.Parse(arguments[4]);
                clientInfo.Color = new Color(r, g, b, 255);
                if (arguments.Length == 6)
                    IP = arguments[5];
                if (arguments.Length == 7)
                    port = int.Parse(arguments[6]);

            }
            else if (arguments[0] == "server")
            {
                GameType = "server";
                if (arguments.Length == 2)
                    args = new string[] { arguments[1] };
                if (arguments.Length > 2)
                    args = new string[] { arguments[1], arguments[2] };
            }
            
            switch (GameType.ToLower())
            {
                case "client":
                    using (Game game = new Game(clientInfo, IP, port))
                    {
                        game.Run();
                    }
                    break;

                case "server":
                    GameServer server = new GameServer();
                    server.Run(args);
                    break;
            }
            #endregion
        }
    }
#endif
}

