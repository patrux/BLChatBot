using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BLChatBot
{
    class Program
    {
        public const string programVersion = "v1.0";
        public static bool isRunning = true;

        static void Main(string[] args)
        {
            Tools.WriteSeparator();
            BLChatBot program = new BLChatBot();
        }
    }
}
