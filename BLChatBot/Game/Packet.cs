using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLChatBot
{
    class Packet
    {
        public byte[] data;
        public string hex;
        public int length;
        public GameInterface.PacketType packetType;

        public void Print()
        {
            Tools.WriteSeparator();
            Tools.WriteLine("Packet ("+length+")");
            Tools.WriteSeparator();
            Tools.WriteLine("Type: " + packetType);
            Tools.WriteLine("Hex ("+hex.Length+"): " + hex);
            Tools.WriteLine("Data (" + data.Length + "): " + Tools.ByteArrayToString(data));
            Tools.WriteSeparator();
        }

        public void Log()
        {
            if (length < 60)
                return;

            Tools.LogLine("---------------------------------------");
            Tools.LogLine("Packet (" + length + ")");
            Tools.LogLine("---------------------------------------");
            Tools.LogLine("Type: " + packetType);
            Tools.LogLine("Hex (" + hex.Length + "): " + hex);
            Tools.LogLine("Data (" + data.Length + "): " + Tools.ByteArrayToString(data));
            Tools.LogLine("---------------------------------------");
        }
    }
}
