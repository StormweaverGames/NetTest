using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NetTest.Common
{
    public static class CommonResources
    {
        public static Texture2D PlayerTex;
        public static Texture2D BlankTex;
        public static Texture2D MapTex;
        public static SpriteFont StandardFont;
        public static bool Useable = false;

        public static void Initialize(ContentManager Content, GraphicsDevice Graphics)
        {
            PlayerTex = Content.Load<Texture2D>("dot");
            MapTex = Content.Load<Texture2D>("map");
            StandardFont = Content.Load<SpriteFont>("StandardFont");

            BlankTex = new Texture2D(Graphics, 1, 1);
            BlankTex.SetData<Color>(new Color[] { Color.White });

            Useable = true;
        }
    }
}
