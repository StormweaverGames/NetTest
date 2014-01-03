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

        PlayerInput _pInput;

        Vector2 RoomSize;

        public Vector2 CameraPos
        {
            get
            {
                return _players.Find(x => x.PlayerID == _playerID) != null ?
                    _players.Find(x => x.PlayerID == _playerID).Position : Vector2.Zero;
            }
        }

        public NetworkHandler(Texture2D playerTex, PlayerInfo info, string IP = null)
        {
            NetPeerConfiguration config = new NetPeerConfiguration("nettest");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

            _client = new NetClient(config);
            _client.Start();

            if (IP == null)
                _client.DiscoverLocalPeers(_serverPort);
            else
                _client.Connect(IP, _serverPort);

            _playerInfo = info;
            _playerTex = playerTex;

            _pInput = new PlayerInput();
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
            _pInput.Update();

            if (_pInput.CurrentReqDirectionChange != 0 || _pInput.CurrentReqSpeedChange != 0)
            {
                SendInput();
            }

            foreach (Player p in _players)
            {
                p.UpdatePositions(RoomSize);
            }

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
                        int packetID = msg.ReadByte(8); //get the packet ID

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

                            case 3: //another player has left the game
                                HandlePlayerUpdated(msg);
                                break;

                            case 4: //another player's HP has changed
                                HandlePlayerHPChanged(msg);
                                break;
                        }
                        break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(CommonResources.MapTex, new Rectangle(0, 0, 1600 * 4, 960 * 4), Color.Olive);
            foreach (Player p in _players)
                p.Draw(spriteBatch);
        }

        private void SendInput()
        {
            NetOutgoingMessage m = _client.CreateMessage();
            m.Write((byte)2, 8);
            m.Write(_playerID);
            m.Write(_pInput.CurrentReqSpeedChange);
            m.Write(_pInput.CurrentReqDirectionChange);
            _client.SendMessage(m, NetDeliveryMethod.Unreliable);
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

            if (p != null)
            {
                p.X = X;
                p.Y = Y;
                p.Speed = speed;
                p.Direction = direction;
            }
        }

        private void HandlePlayerHPChanged(NetIncomingMessage m)
        {
            try
            {
                byte pID = m.ReadByte(8);
                float newHP = m.ReadSingle();

                _players.Find(x => x.PlayerID == pID).Health = newHP;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Called when this client should request to join the server
        /// </summary>
        private void RequestJoin()
        {
            NetOutgoingMessage m = _client.CreateMessage();
            m.Write(0, 8);
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

            _players.Add(new Player(_playerID, pos, _playerInfo, null));

            RoomSize = new Vector2(m.ReadSingle(), m.ReadSingle());

            byte playerCount = m.ReadByte();

            for (int i = 0; i < playerCount; i++)
            {
                byte pID = m.ReadByte();
                Vector2 pPos = new Vector2(m.ReadSingle(), m.ReadSingle());
                PlayerInfo pInfo = PlayerInfo.ReadFromPacket(m);

                _players.Add(new Player(pID, pPos, pInfo, null));
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

            _players.Add(new Player(pID, pPos, pInfo, null));
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
