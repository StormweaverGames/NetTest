using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Lidgren.Network;
using NetTest.Common;

namespace NetTest.Client
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D texture;
        Color[] Colors = new Color[] { Color.Red, Color.Orange, Color.Blue, Color.Magenta };
        Dictionary<long, Vector2> positions = new Dictionary<long, Vector2>();

        NetGame _netHandler;
        PlayerInfo pInfo;

        BasicEffect effect;
        string _ip;

        public Game(PlayerInfo clientInfo, string IP = null)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            pInfo = clientInfo;
            _ip = IP;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            CommonResources.Initialize(Content, GraphicsDevice);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = CommonResources.PlayerTex;

            effect = new BasicEffect(GraphicsDevice);
            
            _netHandler = new NetGame(texture, pInfo, _ip);
        }

        protected override void Update(GameTime gameTime)
        {
            _netHandler.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _netHandler.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            _netHandler.ExitGame("I'm Closing down!!");

            base.OnExiting(sender, args);
        }
    }
}
