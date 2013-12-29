using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace NetTest
{
    public class KeyInput
    {
        public EventHandler KeyDown = default(EventHandler);
        public EventHandler KeyPressed = default(EventHandler);
        public EventHandler KeyReleased = default(EventHandler);

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
    }
}
