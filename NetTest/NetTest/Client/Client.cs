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

        NetworkHandler _netHandler;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Content.Load<Texture2D>("dot");

            PlayerInfo pInfo = new PlayerInfo
            {
                Color = Color.Yellow,
                Username = "Stormweaver"
            };

            _netHandler = new NetworkHandler(texture, pInfo);
        }

        protected override void Update(GameTime gameTime)
        {
            _netHandler.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);

            _netHandler.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            _netHandler.ExitGame("I'm Closing down!!");

            base.OnExiting(sender, args);
        }
    }
}
