using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NetTest.Client
{
    public class NetworkHandler
    {
        NetClient _client;
        int _serverPort = 14242;
        List<Player> _players = new List<Player>();


        Texture2D _playerTex;        
        PlayerInfo _playerInfo;
        byte _playerID;

        public NetworkHandler(Texture2D playerTex, PlayerInfo info)
        {
            NetPeerConfiguration config = new NetPeerConfiguration("xnaapp");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

            _client = new NetClient(config);
            _client.Start();

            _client.DiscoverLocalPeers(_serverPort);

            _playerInfo = info;
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

        private void RequestJoin()
        {
            NetOutgoingMessage m = _client.CreateMessage();
            m.Write(0);
            _playerInfo.WriteToPacket(m);
            _client.SendMessage(m, NetDeliveryMethod.ReliableOrdered);
        }

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

        private void PlayerJoined(NetIncomingMessage m)
        {
            byte pID = m.ReadByte();
            Vector2 pPos = new Vector2(m.ReadSingle(), m.ReadSingle());
            PlayerInfo pInfo = PlayerInfo.ReadFromPacket(m);

            _players.Add(new Player(pID, pPos, pInfo, _playerTex));
            Console.WriteLine("{0} has left the game.", pInfo.Username);
        }

        private void PlayerLeft(NetIncomingMessage m)
        {
            byte pID = m.ReadByte();
            Player p = _players.Find(x => x.PlayerID == pID);
            _players.Remove(p);

            Console.WriteLine("{0} has left the game.",p.Info.Username);
        }
    }
}
