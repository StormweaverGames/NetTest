using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetTest.Common;
using Microsoft.Xna.Framework.Input;

namespace NetTest.Client
{
    public class PlayerInput
    {
        private const float _SPEED = 1.3F;
        private const float _ROTSPEED = 2.0F;

        float _speedChange;
        public float CurrentReqSpeedChange
        {
            get { return _speedChange; }
        }

        float _directionChange;
        public float CurrentReqDirectionChange
        {
            get { return _directionChange; }
        }

        Dictionary<string, KeyInput> _keys = new Dictionary<string,KeyInput>();

        public PlayerInput()
        {
            _keys.Add("forward", Keys.W);
            _keys["forward"].KeyDown += OnForwardDown;
            _keys.Add("backward", Keys.S);
            _keys["backward"].KeyDown += OnBackwardDown;

            _keys.Add("left", Keys.A);
            _keys["left"].KeyDown += OnLeftDown;
            _keys.Add("right", Keys.D);
            _keys["right"].KeyDown += OnRightDown;
        }

        public void Update()
        {
            _speedChange = 0;
            _directionChange = 0;

            foreach (KeyInput k in _keys.Values)
                k.Update();
        }

        private void OnForwardDown(object sender, EventArgs e)
        {
            _speedChange += _SPEED;
        }

        private void OnBackwardDown(object sender, EventArgs e)
        {
            _speedChange -= _SPEED;
        }

        private void OnLeftDown(object sender, EventArgs e)
        {
            _directionChange -= _ROTSPEED;
        }

        private void OnRightDown(object sender, EventArgs e)
        {
            _directionChange += _ROTSPEED;
        }
    }
}
