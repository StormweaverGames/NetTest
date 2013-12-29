using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Lidgren.Network;

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

            NetPeerConfiguration config = new NetPeerConfiguration("xnaapp");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

            client = new NetClient(config);
            client.Start();
        }

        protected override void Initialize()
        {
            client.DiscoverLocalPeers(14242);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Content.Load<Texture2D>("dot");
        }

        protected override void Update(GameTime gameTime)
        {
            //
            // Collect input
            //
            float xinput = 0;
            float yinput = 0;

            float speed = 1;

            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                speed = 2.5F;

            KeyboardState keyState = Keyboard.GetState();

            // exit game if escape or Back is pressed
            if (keyState.IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // use arrows or dpad to move avatar
            if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed || keyState.IsKeyDown(Keys.Left))
                xinput += -speed;
            if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed || keyState.IsKeyDown(Keys.Right))
                xinput += speed;
            if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed || keyState.IsKeyDown(Keys.Up))
                yinput += -speed;
            if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed || keyState.IsKeyDown(Keys.Down))
                yinput += speed;

            if (xinput != 0 || yinput != 0)
            {
                //
                // If there's input; send it to server
                //
                NetOutgoingMessage om = client.CreateMessage();
                om.Write(xinput); // very inefficient to send a full Int32 (4 bytes) but we'll use this for simplicity
                om.Write(yinput);
                client.SendMessage(om, NetDeliveryMethod.Unreliable);
            }

            // read messages
            NetIncomingMessage msg;
            while ((msg = client.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryResponse:
                        // just connect to first server discovered
                        client.Connect(msg.SenderEndPoint);
                        break;
                    case NetIncomingMessageType.Data:
                        // server sent a position update
                        long who = msg.ReadInt64();
                        float x = msg.ReadSingle();
                        float y = msg.ReadSingle();
                        positions[who] = new Vector2(x, y);
                        break;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);

            // draw all players
            foreach (var kvp in positions)
            {
                // use player unique identifier to choose an image
                int num = Math.Abs((int)kvp.Key) % Colors.Length;

                // draw player
                spriteBatch.Draw(texture, kvp.Value, Colors[num]);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            client.Shutdown("bye");

            base.OnExiting(sender, args);
        }
    }
}
