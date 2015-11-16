using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace BLChatBot
{
    class BLChatBot
    {
        public static bool debug = true;

        public Settings settings;
        GameInterface gi;

        public BLChatBot()
        {
            // Set title
            SetTitle();
            titleStopWatch.Start();

            // Load modules
            settings = new Settings();
            gi = new GameInterface(settings);

            // Main loop
            while (Program.isRunning)
            {
                UpdateTitle();

                Thread.Sleep(1);
            }
        }

        #region ConsoleTitle
        Stopwatch titleStopWatch = new Stopwatch();

        string titleText = "Running";
        string titleDotCountString = "";

        int titleUpdateRate = 1200; // millisec
        int titleDotCount = 0;

        void UpdateTitle()
        {
            if (titleStopWatch.Elapsed.TotalMilliseconds >= titleUpdateRate)
            {
                TitleAdvanceDotCount();
                SetTitle();
                titleStopWatch.Restart();
            }
        }

        void TitleAdvanceDotCount()
        {
            titleDotCount++;
            if (titleDotCount > 3)
                titleDotCount = 0;

            switch (titleDotCount)
            {
                default:
                    titleDotCountString = "";
                    break;
                case 1:
                    titleDotCountString = ".";
                    break;
                case 2:
                    titleDotCountString = "..";
                    break;
                case 3:
                    titleDotCountString = "...";
                    break;
            }
        }

        void SetTitle()
        {
            Console.Title = "BLChatBot :: " + Program.programVersion + " :: " + titleText + titleDotCountString;
        }
        #endregion

        void Write(string _line)
        {
            Tools.WriteLine(_line);
        }
    }
}
