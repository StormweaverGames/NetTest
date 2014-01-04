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
        const int _DEFAULTPORT = 14245;
        const string _DEFAULTNAME = "Unnamed Server";
        NetServer _server;
        List<Player> _players = new List<Player>();
        Vector2 _roomSize;
        string _serverName;

        public void Run(string[] args)
        {
            int port = _DEFAULTPORT;
            _serverName = _DEFAULTNAME;

            if (args.Length >= 1)
                _serverName = args[0];
            if (args.Length >= 2)
                port = int.Parse(args[1]);


            Console.WriteLine("Opening server");
            Console.WriteLine("Server name set to \"{0}\"", _serverName);
            NetPeerConfiguration config = new NetPeerConfiguration("nettest");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.Port = port;
            Console.WriteLine("Net configuration complete");

            try
            {
                Console.WriteLine("Attemting to bind to port {0}", port);

                _server = new NetServer(config);
                _server.Start();

                Console.WriteLine("Server started at {0}:{1}", _server.Configuration.BroadcastAddress, _server.Port);
            }
            catch
            {
                Console.WriteLine("Failed to bind to port {0}\n", port);
                Console.WriteLine("Aborting");
                return;
            }

            _roomSize = new Vector2(1600 * 4, 960 * 4);

            // schedule initial sending of position updates
            double nextSendUpdates = NetTime.Now;

            // run until escape is pressed
            while (!Console.KeyAvailable || Console.ReadKey().Key != ConsoleKey.Escape)
            {
                NetIncomingMessage msg;
                while ((msg = _server.ReadMessage()) != null)
                {
                    switch (msg.MessageType)
                    {
                        case NetIncomingMessageType.DiscoveryRequest:
                            NetOutgoingMessage om = _server.CreateMessage();
                            om.Write(_serverName);
                            _server.SendDiscoveryResponse(om, msg.SenderEndPoint);
                            Console.WriteLine("Pinged discovery request");
                            break;
                        case NetIncomingMessageType.StatusChanged: //a client's status has changed
                            NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
                            if (status == NetConnectionStatus.Disconnected)
                                HandlePlayerLeft(_players.Find(x => x.Connection == msg.SenderConnection));
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
                            byte PacketID = msg.ReadByte(8);

                            switch (PacketID)
                            {
                                case 0: // a player wants to join
                                    if (_players.Count < 255)
                                    {
                                        HandlePlayerJoinRequest(msg);
                                    }
                                    break;

                                case 2: //player wants to move
                                    HandlePlayerReqMove(msg);
                                    break;
                            }
                            break;
                    }
                    _server.Recycle(msg);
                }

                //
                // send position updates 30 times per second
                //
                double now = NetTime.Now;
                if (now > nextSendUpdates)
                {
                    // Yes, it's time to send position updates

                    // for each player...
                    foreach (Player player in _players)
                    {
                        player.UpdatePositions(_roomSize);

                        if (player.NeedsUpdate)
                            SendPlayerMoved(player);
                    }

                    // schedule next update
                    nextSendUpdates += (1.0 / 30.0);
                }

                // sleep to allow other processes to run smoothly
                Thread.Sleep(1);
            }

            _server.Shutdown("app exiting");
        }

        private byte GetFirstOpenID()
        {
            for (byte i = 0; i <= 255; i++)
            {
                if (_players.Find(x => x.PlayerID == i) == null)
                    return i;
            }
            throw new ArgumentOutOfRangeException("Cannot fit any more players");
        }

        private void SendPlayerHPChanged(Player player)
        {
            NetOutgoingMessage m = _server.CreateMessage();
            m.Write((byte)4, 8);
            m.Write(player.PlayerID, 8);
            m.Write(player.Health);
            _server.SendToAll(m, NetDeliveryMethod.ReliableUnordered);
        }

        private void SendPlayerMoved(Player player)
        {
            // send position update about this player to all other players
            NetOutgoingMessage om = _server.CreateMessage();

            //write packet ID
            om.Write((byte)3, 8);

            // write who this position is for
            om.Write(player.PlayerID);

            //write data
            om.Write(player.Position.X);
            om.Write(player.Position.Y);
            om.Write(player.Speed);
            om.Write(player.Direction);

            //Send the packet
            _server.SendToAll(om, NetDeliveryMethod.Unreliable);
        }

        /// <summary>
        /// Called when a player request to join
        /// 
        /// IN:
        ///  - Player ID
        ///  - SpeedChange
        ///  - DirectionChange
        /// </summary>
        /// <param name="m"></param>
        private void HandlePlayerReqMove(NetIncomingMessage m)
        {
            byte PlayerID = m.ReadByte(8);

            float speedChange = m.ReadSingle();
            float directionChange = m.ReadSingle();

            Player p = _players.Find(x => x.PlayerID == PlayerID);
            p.Speed += speedChange;
            p.Direction += directionChange;
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

            NetOutgoingMessage o = _server.CreateMessage();
            o.Write((byte)0, 8);
            o.Write(pID);
            o.Write(p.Position.X);
            o.Write(p.Position.Y);
            o.Write(_roomSize.X);
            o.Write(_roomSize.Y);
            o.Write((byte)_players.Count);

            foreach (Player plr in _players)
            {
                o.Write(plr.PlayerID);
                o.Write(plr.Position.X);
                o.Write(plr.Position.Y);
                plr.Info.WriteToPacket(o);
            }

            //send message to new player
            _server.SendMessage(o, p.Connection, NetDeliveryMethod.ReliableUnordered);

            Console.WriteLine(
                "{0} has joined the game",
                pInfo.Username);

            foreach (Player plr in _players) //loop through existing players
            {
                NetOutgoingMessage outM = _server.CreateMessage();
                outM.Write((byte)1, 8);
                outM.Write(pID);
                outM.Write(p.Position.X);
                outM.Write(p.Position.Y);
                p.Info.WriteToPacket(outM);
                //send them packet notifying of new player
                _server.SendMessage(outM, plr.Connection, NetDeliveryMethod.ReliableUnordered);
            }

            //finally add a player
            _players.Add(p);
        }

        /// <summary>
        /// Called when a player leaves
        /// 
        /// IN:
        ///  - Player ID
        ///  - Reason
        /// </summary>
        /// <param name="m"></param>
        private void HandlePlayerLeft(Player removed)
        {
            Console.WriteLine(
                "{0} left with the following message: \n {1}",
                removed.Info.Username,
                "unknown");

            foreach (Player p in _players) //loop through other players
            {
                NetOutgoingMessage o = _server.CreateMessage();
                o.Write((byte)2, 8);
                o.Write(removed.PlayerID);
                //notify them of play leaving
                _server.SendMessage(o, p.Connection, NetDeliveryMethod.ReliableUnordered);
            }

            //finally, remove the player
            _players.Remove(removed);
        }
    }
}
