using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Lidgren.Network;
using System.Threading;
using NetTest.Common;

namespace NetTest.Server
{
    public class GameServer
    {
        NetServer server;
        List<Player> Players = new List<Player>();

        public void Run(string[] args)
        {
            Console.WriteLine("Opening server");
            NetPeerConfiguration config = new NetPeerConfiguration("nettest");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.Port = 14245;
            Console.WriteLine("Net configuration complete");

            // create and start server
            server = new NetServer(config);
            server.Start();
            Console.WriteLine("Net server started");

            // schedule initial sending of position updates
            double nextSendUpdates = NetTime.Now;

            // run until escape is pressed
            while (!Console.KeyAvailable || Console.ReadKey().Key != ConsoleKey.Escape)
            {
                NetIncomingMessage msg;
                while ((msg = server.ReadMessage()) != null)
                {
                    switch (msg.MessageType)
                    {
                        case NetIncomingMessageType.DiscoveryRequest:
                            //
                            // Server received a discovery request from a client; send a discovery response (with no extra data attached)
                            //
                            server.SendDiscoveryResponse(null, msg.SenderEndPoint);
                            Console.WriteLine("Pinged discovery request");
                            break;
                        case NetIncomingMessageType.VerboseDebugMessage:
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.WarningMessage:
                        case NetIncomingMessageType.ErrorMessage:
                            //
                            // Just print diagnostic messages to console
                            //
                            Console.WriteLine(msg.ReadString());
                            break;
                            
                        case NetIncomingMessageType.Data:
                            byte PacketID = msg.ReadByte();

                            switch (PacketID)
                            {
                                case 0: // a player wants to join
                                    if (Players.Count < 255)
                                    {
                                        HandlePlayerJoinRequest(msg);
                                    }
                                    break;

                                case 1: // a player has left
                                    HandlePlayerLeft(msg);
                                    break;
                            }
                            break;
                    }

                    //
                    // send position updates 30 times per second
                    //
                    double now = NetTime.Now;
                    if (now > nextSendUpdates)
                    {
                        // Yes, it's time to send position updates

                        // for each player...
                        foreach (Player player in Players)
                        {
                            // ... send information about every other player (actually including self)
                            foreach (Player otherPlayer in Players)
                            {
                                // send position update about 'otherPlayer' to 'player'
                                NetOutgoingMessage om = server.CreateMessage();

                                //write packet ID
                                om.Write((byte)3);

                                // write who this position is for
                                om.Write(otherPlayer.PlayerID);

                                //write data
                                om.Write(otherPlayer.Position.X);
                                om.Write(otherPlayer.Position.Y);
                                om.Write(otherPlayer.Speed);
                                om.Write(otherPlayer.Direction);

                                // send message
                                server.SendMessage(om, player.Connection, NetDeliveryMethod.Unreliable);
                            }
                        }

                        // schedule next update
                        nextSendUpdates += (1.0 / 30.0);
                    }
                }

                // sleep to allow other processes to run smoothly
                Thread.Sleep(1);
            }

            server.Shutdown("app exiting");
        }

        private byte GetFirstOpenID()
        {
            for (byte i = 0; i <= 255; i++)
            {
                if (Players.Find(x => x.PlayerID == i) == null)
                    return i;
            }
            throw new ArgumentOutOfRangeException("Cannot fit any more players");
        }

        /// <summary>
        /// Called when a player request to join
        /// 
        /// IN:
        ///  - Player info
        ///  
        /// OUT:
        ///  - Player's ID
        ///  - Player's position
        ///  - Player count
        ///  - All other player's data
        /// </summary>
        /// <param name="m"></param>
        private void HandlePlayerJoinRequest(NetIncomingMessage m)
        {
            PlayerInfo pInfo = PlayerInfo.ReadFromPacket(m); //read player data

            byte pID = GetFirstOpenID(); //get first available ID

            //create new player object
            Player p = new Player(pID,
                new Vector2(new Random().Next(10, 790), new Random().Next(10, 470)),
                pInfo, m.SenderConnection); 

            NetOutgoingMessage o = server.CreateMessage();
            o.Write((byte)0);
            o.Write(pID);
            o.Write(p.Position.X);
            o.Write(p.Position.Y);
            o.Write((byte)Players.Count);

            foreach (Player plr in Players)
            {
                o.Write(plr.PlayerID);
                o.Write(plr.Position.X);
                o.Write(plr.Position.Y);
                plr.Info.WriteToPacket(o);
            }

            //send message to new player
            server.SendMessage(o, p.Connection, NetDeliveryMethod.ReliableOrdered);
            
            Console.WriteLine(
                "{0} has joined the game",
                pInfo.Username);

            foreach (Player plr in Players) //loop through existing players
            {
                NetOutgoingMessage outM = server.CreateMessage();
                outM.Write((byte)1);
                outM.Write(pID);
                outM.Write(p.Position.X);
                outM.Write(p.Position.Y);
                p.Info.WriteToPacket(outM);
                //send them packet notifying of new player
                server.SendMessage(outM, plr.Connection, NetDeliveryMethod.ReliableOrdered);
            }

            //finally add a player
            Players.Add(p);
        }

        /// <summary>
        /// Called when a player leaves
        /// 
        /// IN:
        ///  - Player ID
        ///  - Reason
        /// </summary>
        /// <param name="m"></param>
        private void HandlePlayerLeft(NetIncomingMessage m)
        {
            byte pID = m.ReadByte();
            string reason = m.ReadString();

            Player removed = Players.Find(x => x.PlayerID == pID); //find the player

            Console.WriteLine(
                "{0} left with the following message: \n {1}",
                removed.Info.Username,
                reason);

            foreach (Player p in Players) //loop through other players
            {
                NetOutgoingMessage o = server.CreateMessage();
                o.Write((byte)2);
                o.Write(pID);
                //notify them of play leaving
                server.SendMessage(o, p.Connection, NetDeliveryMethod.ReliableOrdered);
            }

            //finally, remove the player
            Players.Remove(removed);
        }
    }
}