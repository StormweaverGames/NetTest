using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetTest.Common;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace NetTest.Server
{
    public class Player
    {
        byte _playerID;
        PlayerInfo _info;
        NetConnection _connection;

        Vector2 _position;
        float _speed;
        float _direction;

        public byte PlayerID { get { return _playerID; } }
        public PlayerInfo Info { get { return _info; } }
        public NetConnection Connection { get { return _connection; } }

        public Vector2 Position { get { return _position; } }
        public float Speed { get { return _speed; } }
        public float Direction { get { return _direction; } }

        public Player(byte ID, Vector2 pos, PlayerInfo info, NetConnection connection)
        {
            _playerID = ID;
            _position = pos;
            _info = info;
            _connection = connection;
        }
    }
}
