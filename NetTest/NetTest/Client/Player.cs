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
        const float _TORAD =  MathHelper.Pi / 180.0F;

        private const float _FRICTION = 1.5F;
        private const float _SPEEDELIPSON = 0.2F;

        const int _HEALTHBARWIDTH = 32;
        const int _HEALTHBARHEIGHT = 5;

        byte _playerID;

        float _direction;
        float _speed;
        
        Vector2 _position;

        PlayerInfo _info;
        Texture2D _texture;
        Vector2 _orgin;

        float _health = 1.0F;

        float _halfNameWidth = 0;
        float _nameHeight = 0;

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
            get { return _speed; } 
            set { _speed = value; } 
        }
        public float Direction 
        { 
            get { return _direction; }
            set { _direction = Wrap(value, 0, 360); } 
        }

        public float Health
        {
            get { return _health; }
            set { _health = value; }
        }
        
        public Player(byte ID, Vector2 position, PlayerInfo info, Texture2D texture)
        {
            this._playerID = ID;
            this._position = position;
            this._info = info;

            this._texture = texture;
            this._orgin = new Vector2(texture.Width / 2.0F, texture.Height / 2.0F);

            _halfNameWidth = CommonResources.StandardFont.MeasureString(_info.Username).X / 2.0F;
            _nameHeight = CommonResources.StandardFont.MeasureString(_info.Username).Y;
        }

        public void DeadReckoning()
        {
            _position.X += (float)Math.Cos(_direction * _TORAD) * _TORAD;
            _position.Y += (float)Math.Sin(_direction * _TORAD) * _TORAD;

            _speed /= _FRICTION;

            _speed = Math.Abs(_speed) < 0.1F ? 0F : _speed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, _info.Color, _direction * _TORAD, _orgin, 1.0F, SpriteEffects.None, 0);
            spriteBatch.DrawString(CommonResources.StandardFont, Info.Username, new Vector2(X - _halfNameWidth, Y - 20 - _nameHeight), Color.Black);
            spriteBatch.Draw(CommonResources.BlankTex, new Rectangle((int)(X - _HEALTHBARWIDTH / 2), (int)(Y + 20), _HEALTHBARWIDTH, _HEALTHBARHEIGHT), Color.Lerp(Color.Red, Color.Green, _health));
        }

        private static float Wrap(float val, float min, float max)
        {
            float r = max - min;

            while (val < min)
                val += r;
            while (val > max)
                val -= r;

            return val;
        }
    }
}
