using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using NetTest.Common;

namespace NetTest.Client
{
    public class NetworkHandler
    {
        NetClient _client;
        int _serverPort = 14245;
        List<Player> _players = new List<Player>();


        Texture2D _playerTex;        
        PlayerInfo _playerInfo;
        byte _playerID;

        public NetworkHandler(Texture2D playerTex, PlayerInfo info)
        {
            NetPeerConfiguration config = new NetPeerConfiguration("nettest");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

            _client = new NetClient(config);
            _client.Start();

            _client.DiscoverLocalPeers(_serverPort);

            _playerInfo = info;
            _playerTex = playerTex;
        }

        /// <summary>
        /// Gets a new NetOutgoingMessage to write a packet of data to
        /// </summary>
        /// <returns>A new NetOutgoingMessage</returns>
        public NetOutgoingMessage BeginPacket()
        {
            return _client.CreateMessage();
        }

        /// <summary>
        /// Finishes the packet and sends the data
        /// </summary>
        /// <param name="packet">The packet to send</param>
        /// <param name="method">The NetDeliveryMethod to use. Default <b>NetDeliveryMethod.Unreliable</b></param>
        public void EndPacket(NetOutgoingMessage packet, NetDeliveryMethod method = NetDeliveryMethod.Unreliable)
        {
            _client.SendMessage(packet, method);
        }

        public void Update()
        {
            // read messages
            NetIncomingMessage msg;
            while ((msg = _client.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryResponse:
                        _client.Connect(msg.SenderEndPoint);
                        Console.WriteLine("found a server, connecting...");
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        Console.WriteLine("Connected to server, joining game");
                        RequestJoin();
                        break;

                    case NetIncomingMessageType.Data: //data was received
                        int packetID = msg.ReadByte(); //get the packet ID

                        switch (packetID) //toggle based on packet state
                        {
                            case 0: //server has accepted join
                                HandleJoin(msg);
                                break;

                            case 1: //another player has joined
                                PlayerJoined(msg);
                                break;

                            case 2: //another player has left the game
                                PlayerLeft(msg);
                                break;
                        }
                        break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Player p in _players)
                p.Draw(spriteBatch);
        }

        /// <summary>
        /// Called when a player's state has been updated by the server
        /// </summary>
        /// <param name="m"></param>
        private void HandlePlayerUpdated(NetIncomingMessage m)
        {
            byte pID = m.ReadByte();

            float X = m.ReadSingle();
            float Y = m.ReadSingle();
            float speed = m.ReadSingle();
            float direction = m.ReadSingle();

            Player p = _players.Find(x => x.PlayerID == pID);

            p.X = X;
            p.Y = Y;
            p.Speed = speed;
            p.Direction = direction;
        }

        /// <summary>
        /// Called when this client should request to join the server
        /// </summary>
        private void RequestJoin()
        {
            NetOutgoingMessage m = _client.CreateMessage();
            m.Write(0);
            _playerInfo.WriteToPacket(m);
            _client.SendMessage(m, NetDeliveryMethod.ReliableOrdered);
            Console.WriteLine("Sent join request");
        }

        /// <summary>
        /// Called when we should leave the game
        /// </summary>
        /// <param name="reason"></param>
        public void ExitGame(string reason)
        {
            NetOutgoingMessage m= _client.CreateMessage();

            m.Write((byte)1);
            m.Write(_playerID);
            m.Write(reason);

            _client.SendMessage(m, NetDeliveryMethod.ReliableOrdered);
            _client.WaitMessage(1000);
            _client.Shutdown("Goodbye");
        }

        /// <summary>
        /// Called when the server accepts a clients join attempt
        /// </summary>
        /// <param name="m"></param>
        private void HandleJoin(NetIncomingMessage m)
        {
            _playerID = m.ReadByte();
            Vector2 pos = new Vector2(m.ReadSingle(), m.ReadSingle());

            _players.Add(new Player(_playerID, pos, _playerInfo, _playerTex));

            byte playerCount = m.ReadByte();

            for (int i = 0; i < playerCount; i++)
            {
                byte pID = m.ReadByte();
                Vector2 pPos = new Vector2(m.ReadSingle(), m.ReadSingle());
                PlayerInfo pInfo = PlayerInfo.ReadFromPacket(m);

                _players.Add(new Player(pID, pPos, pInfo, _playerTex));
            }

            Console.WriteLine("Joined a game with {0} players as '{1}'", playerCount, _playerInfo.Username);
        }

        /// <summary>
        /// Called when another player joins the game
        /// </summary>
        /// <param name="m"></param>
        private void PlayerJoined(NetIncomingMessage m)
        {
            byte pID = m.ReadByte();
            Vector2 pPos = new Vector2(m.ReadSingle(), m.ReadSingle());
            PlayerInfo pInfo = PlayerInfo.ReadFromPacket(m);

            _players.Add(new Player(pID, pPos, pInfo, _playerTex));
            Console.WriteLine("{0} has left the game.", pInfo.Username);
        }

        /// <summary>
        /// Called when a player leaves the game
        /// </summary>
        /// <param name="m"></param>
        private void PlayerLeft(NetIncomingMessage m)
        {
            byte pID = m.ReadByte();
            Player p = _players.Find(x => x.PlayerID == pID);
            _players.Remove(p);

            Console.WriteLine("{0} has left the game.",p.Info.Username);
        }
    }
}
