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
            p.Write((byte)Color.R, 8);
            p.Write((byte)Color.G, 8);
            p.Write((byte)Color.B, 8);
            p.Write((byte)Color.A, 8);

            p.Write((string)Username);
        }

        public static PlayerInfo ReadFromPacket(NetIncomingMessage p)
        {
            PlayerInfo r = new PlayerInfo();

            byte R = p.ReadByte(8);
            byte G = p.ReadByte(8);
            byte B = p.ReadByte(8);
            byte A = p.ReadByte(8);

            p.ReadString(out r.Username);

            r.Color = new Color(R,G,B,A);
            return r;
        }
    }
}
