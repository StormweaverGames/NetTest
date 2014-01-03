using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace NetTest.Common
{
    public class KeyInput
    {
        public EventHandler KeyDown = BlankEventHandler;
        public EventHandler KeyPressed = BlankEventHandler;
        public EventHandler KeyReleased = BlankEventHandler;

        public readonly Keys Key;

        private bool _previouslyDown;

        public KeyInput(Keys Key)
        {
            this.Key = Key;            
        }

        public void Update()
        {
            KeyboardState key = Keyboard.GetState();

            if (key.IsKeyDown(Key))
            {
                if (!_previouslyDown)
                    KeyReleased.Invoke(this, default(EventArgs));
                else
                    KeyDown.Invoke(this, default(EventArgs));

                _previouslyDown = true;
            }
            else if (_previouslyDown)
            {
                _previouslyDown = false;
                KeyReleased.Invoke(this, default(EventArgs));
            }
        }

        private static void BlankEventHandler(object sender, EventArgs e)
        {

        }

        public static implicit operator KeyInput(Keys SourceKey)
        {
            return new KeyInput(SourceKey);
        }
    }
}
