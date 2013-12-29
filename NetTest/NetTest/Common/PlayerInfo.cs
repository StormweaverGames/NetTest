using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace NetTest.Common
{
    /// <summary>
    /// Represents basic player profile data
    /// </summary>
    public struct PlayerInfo
    {
        public Color Color;
        public string Username;

        public void WriteToPacket(NetOutgoingMessage p)
        {
            p.Write(Color.R);
            p.Write(Color.G);
            p.Write(Color.B);
            p.Write(Color.A);
            p.Write(Username);
        }

        public static PlayerInfo ReadFromPacket(NetIncomingMessage p)
        {
            PlayerInfo r = new PlayerInfo();
            r.Color = new Color(p.ReadByte(), p.ReadByte(), p.ReadByte(), p.ReadByte());
            r.Username = p.ReadString();
            return r;
        }
    }
}
