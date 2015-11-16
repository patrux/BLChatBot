using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace BLChatBot
{
    class PacketReader
    {
        GameInterface gameInterface;
        Settings settings;

        /// The capture socket.
        Socket captureSocket;

        /// Data to allocate per packet.
        byte[] byteData = new byte[4096];

        /// Flag to check whether to capture data or not.
        bool doCapture = false;

        /// Stores the supplied blood gate ip.
        string bloodgateIP = "";

        public PacketReader(GameInterface _gameInterface, Settings _settings)
        {
            gameInterface = _gameInterface;
            settings = _settings;

            BeginPacketReading();
        }

        void BeginPacketReading()
        {
            try
            {
                if (!doCapture)
                {
                    SetCapture(true);

                    // Get bloodgate ip
                    bloodgateIP = settings.GetBloodGateIP();

                    // Get local ip
                    string localIP = settings.GetLocalIP();

                    // Check if we need to auto-detect it
                    if (String.IsNullOrEmpty(localIP))
                    {
                        IPHostEntry hostEntry = Dns.GetHostEntry((Dns.GetHostName()));
                        if (hostEntry.AddressList.Length > 0)
                        {
                            foreach (IPAddress ip in hostEntry.AddressList)
                            {
                                if (ip.ToString().Substring(0, 7) == "192.168")
                                {
                                    localIP = ip.ToString();
                                }
                            }
                        }
                    }

                    // Create a new socket which will we read from
                    captureSocket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);

                    // Bind the socket to our local ip
                    captureSocket.Bind(new IPEndPoint(IPAddress.Parse(localIP), 0));

                    // Set socket options
                    captureSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);

                    byte[] byTrue = new byte[4] { 1, 0, 0, 0 };
                    byte[] byOut = new byte[4] { 1, 0, 0, 0 };

                    captureSocket.IOControl(IOControlCode.ReceiveAll, byTrue, byOut);

                    // Begin capture
                    captureSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
                }
                else
                {
                    SetCapture(false);
                }
            }
            catch (Exception ex)
            {
                AbortCapture();
                Write("Capture aborted. Ex: " + ex);
            }
        }

        /// Called when a packet was received.
        void OnReceive(IAsyncResult ar)
        {
            try
            {
                int length = captureSocket.EndReceive(ar);

                // Parse the data
                gameInterface.HandleRawData(byteData, length);

                // Another call to BeginReceive so that we continue to receive the incoming packets
                if (doCapture)
                {
                    byteData = new byte[4096];
                    captureSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
                }
            }
            catch (Exception ex)
            {
                AbortCapture();
                Write("OnReceive aborted. Ex: " + ex);
            }
        }

        void SetCapture(bool isCapturing)
        {
            doCapture = isCapturing;

            if (isCapturing)
            {
                //playerList.Clear();
                Write("Capturing...");
            }
            else
            {
                Write("Not capturing...");
            }
        }

        void AbortCapture()
        {
            doCapture = false;
        }

        void Write(string _line)
        {
            Tools.WriteLine(_line);
        }
    }
}
