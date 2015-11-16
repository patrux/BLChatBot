using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLChatBot
{
    class PacketAnalyzer
    {
        float accuracy = 0.9f;

        public GameInterface.PacketType AnalyzePacketPattern(Packet _packet)
        {
            //if (_length < 1400)
            //    return GameInterface.PacketType.Ignore;

            //string hex = BitConverter.ToString(_packet.data).Replace("-", string.Empty);

            //Write("----- NEW ANALYZE -----");
            //Tools.WriteSeparator();
            //Write("byte (first 32): " + Tools.ByteArrayToString(_packet.data).Substring(0, 32));
            //Write("hex (first 32): " + _packet.hex.Substring(0, 32));
            //Write("byte: " + Tools.ByteArrayToString(_packet.data));
            //Write("hex: " + _packet.hex);
            //Tools.WriteSeparator();

            if (_packet.hex.Substring(_packet.hex.Length - 2, 2) != "02")
                return GameInterface.PacketType.Ignore;

            bool isGlobalJoinPacket = IsPacketGlobalJoin(_packet);
            bool isGlobalChatPacket = IsPacketGlobalChat(_packet);
            //Tools.WriteSeparator();

            //Write("- Finished");

            if (isGlobalJoinPacket)
                return GameInterface.PacketType.JoinedGlobal;
            else if (isGlobalChatPacket)
                return GameInterface.PacketType.ChatMessage;
            else
                return GameInterface.PacketType.Ignore;
        }

        void CheckPosition(string _data, int _position, string _value, bool _isEqual, ref int checks, ref int matches)
        {
            string character = _data.Substring(_position, 1);

            if (_isEqual)
            {
                if (character == _value)
                    matches++;
            }
            else
            {
                if (character != _value)
                    matches++;
            }
            checks++;
        }

        bool IsPacketGlobalJoin(Packet _packet)
        {
            int highestIndex = 99;
            int checks = 0;
            int matches = 0;

            if (_packet.hex.Length < highestIndex)
                return false;

            try
            {
                CheckPosition(_packet.hex, 0, "" + 0, false, ref checks, ref matches);
                CheckPosition(_packet.hex, 1, "" + 0, false, ref checks, ref matches);

                CheckPosition(_packet.hex, 12, "" + 0, true, ref checks, ref matches);
                CheckPosition(_packet.hex, 13, "" + 0, true, ref checks, ref matches);
                CheckPosition(_packet.hex, 14, "" + 0, true, ref checks, ref matches);
                CheckPosition(_packet.hex, 15, "" + 0, true, ref checks, ref matches);

                CheckPosition(_packet.hex, 17, "" + 0, false, ref checks, ref matches);

                CheckPosition(_packet.hex, 25, "" + 0, false, ref checks, ref matches);

                CheckPosition(_packet.hex, 31, "" + 0, false, ref checks, ref matches);
            }
            catch (Exception ex)
            {
                Write("Exception " + ex);
                return false;
            }

            if ((float)matches / (float)checks >= accuracy)
                Write("GlobalJoined Scored: " + (float)((float)matches / (float)checks) + ". (Length=" + _packet.length + ") (Checks=" + checks + ") (Matches=" + matches + ")");

            return ((float)matches / (float)checks >= accuracy);
        }

        bool IsPacketGlobalChat(Packet _packet)
        {
            int highestIndex = 99;
            int checks = 0;
            int matches = 0;

            if (_packet.hex.Length < highestIndex)
                return false;

            try
            {
                CheckPosition(_packet.hex, 0, "" + 0, false, ref checks, ref matches);
                CheckPosition(_packet.hex, 1, "" + 0, false, ref checks, ref matches);

                CheckPosition(_packet.hex, 93, "" + 0, true, ref checks, ref matches);
                CheckPosition(_packet.hex, 94, "" + 0, true, ref checks, ref matches);
                CheckPosition(_packet.hex, 95, "" + 0, true, ref checks, ref matches);
                CheckPosition(_packet.hex, 96, "" + 0, true, ref checks, ref matches);
                CheckPosition(_packet.hex, 97, "" + 0, true, ref checks, ref matches);
                CheckPosition(_packet.hex, 98, "" + 0, true, ref checks, ref matches);

                CheckPosition(_packet.hex, 99, "" + 0, false, ref checks, ref matches);

            }
            catch (Exception ex)
            {
                Write("Exception " + ex);
                return false;
            }

            if ((float)matches / (float)checks >= accuracy)
                Write("GlobalChat Scored: " + (float)((float)matches / (float)checks) + ". (Length=" + _packet.length + ") (Checks=" + checks + ") (Matches=" + matches + ")");

            return ((float)matches / (float)checks >= accuracy);
        }

        void Write(string _line)
        {
            Tools.WriteLine(_line);
        }
    }
}



//CheckPosition(_packet.hex, 0, "" + 0, false, ref checks, ref matches);
//CheckPosition(_packet.hex, 1, "" + 0, false, ref checks, ref matches);
//CheckPosition(_packet.hex, 2, "" + 0, true, ref checks, ref matches);
//CheckPosition(_packet.hex, 3, "" + 0, false, ref checks, ref matches);

//CheckPosition(_packet.hex, 4, "" + 0, true, ref checks, ref matches);
//CheckPosition(_packet.hex, 5, "" + 0, true, ref checks, ref matches);
//CheckPosition(_packet.hex, 6, "" + 0, true, ref checks, ref matches);
//CheckPosition(_packet.hex, 7, "" + 0, true, ref checks, ref matches);

//CheckPosition(_packet.hex, 8, "" + 0, true, ref checks, ref matches);
//CheckPosition(_packet.hex, 9, "" + 0, false, ref checks, ref matches);
//CheckPosition(_packet.hex, 10, "" + 0, true, ref checks, ref matches);
//CheckPosition(_packet.hex, 11, "" + 0, true, ref checks, ref matches);

//CheckPosition(_packet.hex, 12, "" + 0, true, ref checks, ref matches);
//CheckPosition(_packet.hex, 13, "" + 0, true, ref checks, ref matches);
//CheckPosition(_packet.hex, 14, "" + 0, true, ref checks, ref matches);
//CheckPosition(_packet.hex, 15, "" + 0, true, ref checks, ref matches);

//CheckPosition(_packet.hex, 16, "" + 0, true, ref checks, ref matches);
//CheckPosition(_packet.hex, 17, "" + 2, true, ref checks, ref matches);
//CheckPosition(_packet.hex, 18, "" + 0, true, ref checks, ref matches);
//CheckPosition(_packet.hex, 19, "" + 0, false, ref checks, ref matches);

//CheckPosition(_packet.hex, 20, "" + 0, true, ref checks, ref matches);
//CheckPosition(_packet.hex, 21, "" + 0, true, ref checks, ref matches);
//CheckPosition(_packet.hex, 22, "" + 0, true, ref checks, ref matches);
//CheckPosition(_packet.hex, 23, "" + 0, false, ref checks, ref matches);

//CheckPosition(_packet.hex, 24, "" + 0, true, ref checks, ref matches);
//CheckPosition(_packet.hex, 25, "" + 0, false, ref checks, ref matches);
//CheckPosition(_packet.hex, 26, "" + 0, true, ref checks, ref matches);
//CheckPosition(_packet.hex, 27, "" + 0, true, ref checks, ref matches);

//CheckPosition(_packet.hex, _packet.hex.Length-2, "" + 0, true, ref checks, ref matches);
//CheckPosition(_packet.hex, _packet.hex.Length-1, "" + 2, true, ref checks, ref matches);
