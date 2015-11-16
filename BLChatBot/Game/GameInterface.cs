using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLChatBot
{
    class GameInterface
    {
        PacketReader packetReader;
        PacketParser packetParser;
        Settings settings;

        List<Player> playerList = new List<Player>();

        public GameInterface(Settings _settings)
        {
            settings = _settings;

            if (!BLChatBot.debug) // dont read packets if debug
                packetReader = new PacketReader(this, settings);

            packetParser = new PacketParser(this, settings);
            Test();
         //   DebugIncomingPacket(BLChatBot.debug);
        }

        void Test()
        {
            string hex = "24 00 00 00 | 12 00 00 00 02 00 0c 00 | 53 74 75 6e 74 6d 61 6e 4d 69 6b 65 | 0d 00 | 6d 6f 72 6f 6e 20 69 73 20 68 65 72 65 | 02".Replace(" ", "").Replace("|", "");

            string[] split = hex.Split(new string[] { "00" }, StringSplitOptions.None);

            byte[][] byteSplit = new byte[split.Length][];

            for (int i = 0; i < split.Length; i++)
            {
                byteSplit[i] = Tools.HexToByteArray(split[i]);
            }

            string name = Encoding.ASCII.GetString(byteSplit[byteSplit.Length - 2]);
            name = name.Substring(0, name.Length - 1);

            string message = Encoding.ASCII.GetString(byteSplit[byteSplit.Length-1]);
            message = message.Substring(0, message.Length - 1);
        }

        void DebugIncomingPacket(bool _runDebug)
        {
            if (!_runDebug)
                return;

            // The Hex test string
            string hex = "45 00 00 42 86 56 00 00 2E 06 FD 7E 36 98 10 9E C0 A8 01 03 27 DC C1 A7 13 81 05 2D 01 39 F9 9A 50 18 02 BD 33 0E 00 00 16 00 00 00 12 00 00 00 02 00 06 00 68616D616461 0500 617364663F02".Replace(" ", "");
            //string hex = "45000041B13800002E06D29D3698109EC0A8010327DCC1A71380FBB60139F8E9501802BD7C39000015000000120000000200060068616D61646104006173646602";
            //string hex = "ed0000000b0000000202000101000d04004a4b474c0000020200020800736c73636872697300000500000208004b52304d654c4c300000010000020c005370697269746c65617665730000030100020500785261696e03004b335905000002080056316e746f76617200000202000209006861726d6f6e796f6d03006b6d6a020200020b004b69636b6572536f6c6f5803003031410202000209004f73744a756e696f7203004f7572010000020500747572756b0000020200020a005374726f6e674265657203004f7572010000020c00626a6172676872696e677572000002020002060068616d616461000005000002";
            // Convert Hex to raw byte[] data
            int length = hex.Length;
            byte[] data = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
                data[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);

            // Create packet
            Packet packet = CreatePacket(data, data.Length);

            HandlePacket(packet);

            //ChatMessage cm = new ChatMessage();
            //cm.ParseChatMessage(packet);
            Tools.WriteSeparator();
        }

        public void HandleRawData(byte[] _data, int _length)
        {
            // Create IP Header
            IPHeader ipHeader = new IPHeader(_data, _length);
            string packetSourceIP = ipHeader.SourceAddress.ToString();

            // Check the IPHeader if the packet is from BloodGate, else ignore
            if (packetSourceIP != settings.GetBloodGateIP())
                return;

            // Create TCP Header
            TCPHeader tcpHeader = new TCPHeader(ipHeader.Data, ipHeader.MessageLength);

            // Create packet
            Packet packet = CreatePacket(_data, _length);

            HandlePacket(packet);

            // extract relevant info from packet
            // display info
        }

        void HandlePacket(Packet packet)
        {
            switch (packet.packetType)
            {
                default:
                    Write("Unknown packet.");
                    break;
                case PacketType.Ignore:
                    //Write("Ignored packet.");
                    break;
                case PacketType.JoinedGlobal:
                    playerList.Clear();
                    packetParser.ParseGlobalChannelJoin(packet, ref playerList);
                    Write("Global has " + playerList.Count + " players.");
                    break;
                case PacketType.ChatMessage:
                    ChatMessage cm = new ChatMessage();
                    cm.ParseChatMessage(packet);
                    break;
            }
        }

        Packet CreatePacket(byte[] _data, int _length)
        {
            Packet packet = new Packet();
            packet.data = _data;
            Tools.TrimTrailingBytes(ref packet.data, 0);

            packet.hex = BitConverter.ToString(packet.data).Replace("-", String.Empty);
            packet.length = _length;

            packet.packetType = packetParser.GetPacketType(packet);

            packet.Log();

            return packet;
        }

        public enum PacketType
        {
            JoinedGlobal,
            LeftChannel,
            JoinedChannel,
            ChatMessage,
            Ignore,
            Undefined
        }

        void Write(string _line)
        {
            Tools.WriteLine(_line);
        }
    }
}
