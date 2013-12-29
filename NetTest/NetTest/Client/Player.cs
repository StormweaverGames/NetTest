using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Lidgren.Network;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using NetTest.Common;

namespace NetTest.Client
{
    public class Player
    {
        static float ToRad = 180.0F / MathHelper.Pi;

        byte _playerID;

        float _direction;
        float _velocity;

        float _prevDirection;
        float _prevVelocity;

        Vector2 _position;

        PlayerInfo _info;

        Texture2D _texture;
        Vector2 _orgin;

        /// <summary>
        /// Gets this player's player ID
        /// </summary>
        public byte PlayerID { get { return _playerID; } }
        /// <summary>
        /// Gets this player's info
        /// </summary>
        public PlayerInfo Info { get { return _info; } }

        public float X 
        { 
            get { return _position.X; } 
            set { _position.X = value; } 
        }
        public float Y
        {
            get { return _position.Y; }
            set { _position.Y = value; }
        }
        public float Speed 
        { 
            get { return _velocity; } 
            set { _velocity = value; } 
        }
        public float Direction 
        { 
            get { return _direction; }
            set { _direction = value; } 
        }

        Dictionary<string, KeyInput> Keys = new Dictionary<string, KeyInput>();

        public Player(byte ID, Vector2 position, PlayerInfo info, Texture2D texture)
        {
            this._playerID = ID;
            this._position = position;
            this._info = info;

            this._texture = texture;
            this._orgin = new Vector2(texture.Width / 2.0F, texture.Height / 2.0F);
        }

        public void Update(GameTime gameTime, NetworkHandler handler)
        {
            for (int i = 0; i < Keys.Values.Count; i++)
            {
                Keys.Values.ElementAt(i).Update();
            }

            if (_direction != _prevDirection || _velocity != _prevDirection)
            {
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, _info.Color, _direction * ToRad, _orgin, 1.0F, SpriteEffects.None, 0);
        }
        
        public void SendPlayerInfo(NetworkHandler handler)
        {
            NetOutgoingMessage m = handler.BeginPacket();
            m.Write((byte)1);
            m.Write(_playerID);
            m.Write(_info.Color.R);
            m.Write(_info.Color.G);
            m.Write(_info.Color.B);
            m.Write(_info.Color.A);
            handler.EndPacket(m);
        }

        public void HandleUpdateInfo(NetIncomingMessage message)
        {
            _info.Color.R = message.ReadByte();
            _info.Color.G = message.ReadByte();
            _info.Color.B = message.ReadByte();
            _info.Color.A = message.ReadByte();
        }

        public void SendPlayerUpdate(NetworkHandler handler)
        {
            NetOutgoingMessage m = handler.BeginPacket();
            m.Write((byte)2);
            m.Write(_playerID);
            m.Write(_direction);
            m.Write(_velocity);
            handler.EndPacket(m);
        }

        public void HandlePlayerUpdate(NetIncomingMessage message)
        {
            _position.X = message.ReadSingle();
            _position.Y = message.ReadSingle();
            _direction = message.ReadSingle();
        }
    }
}
