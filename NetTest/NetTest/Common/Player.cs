using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetTest.Common;
using Microsoft.Xna.Framework;
using Lidgren.Network;
using Microsoft.Xna.Framework.Graphics;

namespace NetTest.Common
{
    public class Player
    {
        private const float _TORAD = MathHelper.Pi / 180.0F;
        private const float _FRICTION = 1.2F;
        private const float _SPEEDELIPSON = 0.5F;

        byte _playerID;
        PlayerInfo _info;
        NetConnection _connection;

        Vector2 _position;
        float _speed, _prevSpeed;
        float _direction, _prevDirection;

        float _health = 1.0F;
        
        const int _HEALTHBARWIDTH = 32;
        const int _HEALTHBARHEIGHT = 5;

        Texture2D _texture;
        Vector2 _orgin;

        float _halfNameWidth = 0;
        float _nameHeight = 0;

        public bool NeedsUpdate
        {
            get { return _needsUpdate; }
        }
        private bool _needsUpdate = true;

        public byte PlayerID { get { return _playerID; } }
        public PlayerInfo Info { get { return _info; } }
        public NetConnection Connection { get { return _connection; } }

        public Vector2 Position { get { return _position; } }
        public float Speed { get { return _speed; } set { _speed = value; } }
        public float Direction { get { return _direction; } set { _direction = value; } }

        public float Health { get { return _health; } set { _health = value; } }

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

        public Player(byte ID, Vector2 pos, PlayerInfo info, NetConnection connection)
        {
            _playerID = ID;
            _position = pos;
            _info = info;
            _connection = connection;

            if (CommonResources.Useable)
            {
                _texture = CommonResources.PlayerTex;
                _orgin = new Vector2(_texture.Width / 2.0F, _texture.Height / 2.0F);

                _halfNameWidth = CommonResources.StandardFont.MeasureString(_info.Username).X / 2.0F;
                _nameHeight = CommonResources.StandardFont.MeasureString(_info.Username).Y;
            }
        }

        /// <summary>
        /// Updates player's position and applies fricton
        /// </summary>
        public void UpdatePositions(Vector2 RoomSize)
        {
            _position.X += (float)Math.Cos(_direction * _TORAD) * _speed;
            _position.Y += (float)Math.Sin(_direction * _TORAD) * _speed;

            _speed /= _FRICTION;

            _position.X = _position.X > RoomSize.X ? RoomSize.X :
                _position.X < 0 ? 0 : _position.X;
            _position.Y = _position.Y > RoomSize.Y ? RoomSize.Y :
                _position.Y < 0 ? 0 : _position.Y;

            _speed = Math.Abs(_speed) < 0.1F ? 0F : _speed;

            if (_speed != _prevSpeed || _direction != _prevDirection)
                _needsUpdate = true;
            else
                _needsUpdate = false;

            _prevSpeed = _speed;
            _prevDirection = _direction;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, _info.Color, _direction * _TORAD, _orgin, 1.0F, SpriteEffects.None, 0);
            spriteBatch.DrawString(CommonResources.StandardFont, Info.Username, new Vector2(X - _halfNameWidth, Y - 20 - _nameHeight), Color.Black);
            spriteBatch.Draw(CommonResources.BlankTex, new Rectangle((int)(X - _HEALTHBARWIDTH / 2), (int)(Y + 20), (int)(_HEALTHBARWIDTH * _health), _HEALTHBARHEIGHT), Color.Lerp(Color.Red, Color.Green, _health));
        }
    }
}
