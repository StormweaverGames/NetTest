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

            string[] targs = arguments[0].Split("/\\".ToCharArray());
            List<string> targs2 = new List<string>();
            for (int i = 0; i < targs.Length; i++)
                if (targs[i] != "")
                    targs2.Add(targs[i]);
            string[] args = targs2.ToArray();

            string GameType = "none";
            PlayerInfo clientInfo = new PlayerInfo();
            string IP = null;

            if (args.Length >= 5 & args[0] == "client")
            {
                GameType = "client";
                clientInfo.Username = args[1];
                byte r = byte.Parse(args[2]);
                byte g = byte.Parse(args[3]);
                byte b = byte.Parse(args[4]);
                clientInfo.Color = new Color(r, g, b, 255);
                if (args.Length == 6)
                    IP = args[5];
            }
            else if (args.Length == 1 & args[0] == "server")
            {
                GameType = "server";
            }
            
            switch (GameType.ToLower())
            {
                case "client":
                    using (Game game = new Game(clientInfo, IP))
                    {
                        game.Run();
                    }
                    break;

                case "server":
                    GameServer server = new GameServer();
                    server.Run(new string[] { });
                    #region Old Server
                    //{
                    //    Console.WriteLine("Opening server");
                    //    NetPeerConfiguration config = new NetPeerConfiguration("xnaapp");
                    //    config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
                    //    config.Port = 14242;
                    //    Console.WriteLine("Net configuration complete");

                    //    // create and start server
                    //    NetServer server = new NetServer(config);
                    //    server.Start();
                    //    Console.WriteLine("Net server started");

                    //    // schedule initial sending of position updates
                    //    double nextSendUpdates = NetTime.Now;

                    //    // run until escape is pressed
                    //    while (!Console.KeyAvailable || Console.ReadKey().Key != ConsoleKey.Escape)
                    //    {
                    //        NetIncomingMessage msg;
                    //        while ((msg = server.ReadMessage()) != null)
                    //        {
                    //            switch (msg.MessageType)
                    //            {
                    //                case NetIncomingMessageType.DiscoveryRequest:
                    //                    //
                    //                    // Server received a discovery request from a client; send a discovery response (with no extra data attached)
                    //                    //
                    //                    server.SendDiscoveryResponse(null, msg.SenderEndPoint);
                    //                    Console.WriteLine("Pinged discovery request");
                    //                    break;
                    //                case NetIncomingMessageType.VerboseDebugMessage:
                    //                case NetIncomingMessageType.DebugMessage:
                    //                case NetIncomingMessageType.WarningMessage:
                    //                case NetIncomingMessageType.ErrorMessage:
                    //                    //
                    //                    // Just print diagnostic messages to console
                    //                    //
                    //                    Console.WriteLine(msg.ReadString());
                    //                    break;
                    //                case NetIncomingMessageType.StatusChanged:
                    //                    NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
                    //                    if (status == NetConnectionStatus.Connected)
                    //                    {
                    //                        //
                    //                        // A new player just connected!
                    //                        //
                    //                        Console.WriteLine(NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier) + " connected!");

                    //                        // randomize his position and store in connection tag
                    //                        msg.SenderConnection.Tag = new float[] {
                    //                NetRandom.Instance.Next(10, 100),
                    //                NetRandom.Instance.Next(10, 100)
                    //            };
                    //                    }

                    //                    break;
                    //                case NetIncomingMessageType.Data:
                    //                    //
                    //                    // The client sent input to the server
                    //                    //
                    //                    float xinput = msg.ReadSingle();
                    //                    float yinput = msg.ReadSingle();

                    //                    float[] pos = msg.SenderConnection.Tag as float[];

                    //                    // fancy movement logic goes here; we just append input to position
                    //                    pos[0] += xinput;
                    //                    pos[1] += yinput;
                    //                    break;
                    //            }

                    //            //
                    //            // send position updates 30 times per second
                    //            //
                    //            double now = NetTime.Now;
                    //            if (now > nextSendUpdates)
                    //            {
                    //                // Yes, it's time to send position updates

                    //                // for each player...
                    //                foreach (NetConnection player in server.Connections)
                    //                {
                    //                    // ... send information about every other player (actually including self)
                    //                    foreach (NetConnection otherPlayer in server.Connections)
                    //                    {
                    //                        // send position update about 'otherPlayer' to 'player'
                    //                        NetOutgoingMessage om = server.CreateMessage();

                    //                        // write who this position is for
                    //                        om.Write(otherPlayer.RemoteUniqueIdentifier);

                    //                        if (otherPlayer.Tag == null)
                    //                            otherPlayer.Tag = new float[2];

                    //                        float[] pos = otherPlayer.Tag as float[];
                    //                        om.Write(pos[0]);
                    //                        om.Write(pos[1]);

                    //                        // send message
                    //                        server.SendMessage(om, player, NetDeliveryMethod.Unreliable);
                    //                    }
                    //                }

                    //                // schedule next update
                    //                nextSendUpdates += (1.0 / 30.0);
                    //            }
                    //        }

                    //        // sleep to allow other processes to run smoothly
                    //        Thread.Sleep(1);
                    //    }

                    //    server.Shutdown("app exiting");
                    //}
                    #endregion
                    break;
            }
            #endregion
        }
    }
#endif
}

