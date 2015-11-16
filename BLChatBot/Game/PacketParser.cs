using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLChatBot
{
    class PacketParser
    {
        GameInterface gameInterface;
        Settings settings;
        PacketAnalyzer packetAnalyzer;

        bool[] globalJoinPattern = new bool[] { true, true };

        public PacketParser(GameInterface _gameInterface, Settings _settings)
        {
            gameInterface = _gameInterface;
            settings = _settings;
            packetAnalyzer = new PacketAnalyzer();
        }

        public GameInterface.PacketType GetPacketType(Packet _packet)
        {
            return packetAnalyzer.AnalyzePacketPattern(_packet);
        }

        public void ParseGlobalChannelJoin(Packet _packet, ref List<Player> playerList)
        {
            int index = 17; // Start at the 17th byte

            while (index < _packet.data.Length)
            {
                Player p = new Player();
                index = ParseName(index, _packet, p);
                playerList.Add(p);

                // Exit loop if three consecutive zeros appear
                if (_packet.data.Length >= index + 2)
                {
                    if (_packet.data[index] == 0 &&
                        _packet.data[index + 1] == 0 &&
                        _packet.data[index + 2] == 0)
                        break;
                }
            }
        }

        /// Parses a player name.
        int ParseName(int index, Packet _packet, Player player)
        {
            string s = "";
            int i = 0;

            for (i = index; i < _packet.data.Length; i++)
            {
                if (_packet.data[i] == 0)
                {
                    if (_packet.data[i + 1] == 0) // player has no clan
                    {
                        i += 8; // advance index by 8 bytes
                        break;
                    }
                }
                else if (_packet.data[i] == 2 || _packet.data[i] == 3) // clan tag found (tag length is revealed 2 bytes before the tag (either a 2 or 3))
                {
                    if (_packet.data[i + 1] == 0)
                    {
                        i = ParseClan(i, _packet, player); // parse clan name and return index for the next player
                        break;
                    }
                }
                else // continue building name
                {
                    char c = Convert.ToChar(_packet.data[i]);
                    s += c;
                }
            }

            player.SetName(s);
            return i;
        }

        /// Parses a clan for a player
        int ParseClan(int index, Packet _packet, Player player)
        {
            string s = "";
            int i = 0;
            int len = Convert.ToInt32(_packet.data[index]); // clan tag len

            for (i = index + 2; i < index + 2 + len; i++)
            {
                char c = Convert.ToChar(_packet.data[i]);
                s += c;
            }

            player.SetClan(s);
            i += 6; // advance index by 6 bytes
            return i;
        }

        void Write(string _line)
        {
            Tools.WriteLine(_line);
        }
    }
}
