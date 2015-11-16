using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLChatBot
{
    class Player
    {
        string name = "";
        string clan = "";

        public Player() {}

        public Player(string _name, string _clan)
        {
            name = _name;
            clan = _clan;
        }

        public string GetFullName()
        {
            if (String.IsNullOrEmpty(clan))
                return name;
            else
                return "[" + clan + "] " + name;
        }

        public string GetName()
        {
            return name;
        }

        public void SetName(string _name)
        {
            name = _name;
        }

        public string GetClan()
        {
            return clan;
        }

        public void SetClan(string _clan)
        {
            clan = _clan;
        }
    }
}
