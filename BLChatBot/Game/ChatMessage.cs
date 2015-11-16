using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace BLChatBot
{
    class ChatMessage
    {
        string name = "";
        string message = "";

        public ChatMessage() { }

        public ChatMessage(string _name, string _message)
        {
            name = _name;
            message = _message;
        }

        public string GetFullMessage()
        {
            if (String.IsNullOrEmpty(name) && String.IsNullOrEmpty(message))
                return "EMPTY";
            else
                return "<" + name + "> " + message + "";
        }

        public string GetName()
        {
            return name;
        }

        public void SetName(string _name)
        {
            name = _name;
        }

        public string GetMessage()
        {
            return message;
        }

        public void SetMessage(string _message)
        {
            message = _message;
        }

        string hex = "";

        public void ParseChatMessage(Packet _packet)
        {
            Write("data: " + Tools.ByteArrayToString(_packet.data));
            Write("hex: " + hex);
            string[] split = hex.Split(new string[] { "00" }, StringSplitOptions.None);
            Write("split.Length[" + split.Length + "] data.Length[" + _packet.data.Length + "] _length[" + _packet.length + "]");
            //foreach (string s in split)
            //{
            //    Write("split: " + s);
            //}

            // Get Name
            string name = split[split.Length - 2];

            if (name.Length - 2 < 0)
                name = name.Substring(0, name.Length);
            else
                name = name.Substring(0, name.Length - 2);

            // Get Message
            string message = split[split.Length - 1];

            if (message.Length - 2 < 0)
                message = message.Substring(0, message.Length);
            else
                message = message.Substring(0, message.Length - 2);

            // Translate from Hex to String
            this.name = Tools.HexStringToString(name);
            this.message = Tools.HexStringToString(message);

            Write("Message[" + GetFullMessage() + "]");
            //Upload(GetFullMessage());
        }

        private string postSecretKey = "k5DMeo3mJK39cl1DgG4basTm5km1b";

        void Upload(string postData)
        {
            string webpageContent = string.Empty;
            try
            {
                string hash = Encrypt(this.name + this.message + postSecretKey);
                string postUrl = "http://www.furyzone.com/blc/global_add.php";

                string postParams = "&name=" + this.name + "&message=" + this.message + "&hash=" + hash;
                byte[] byteArray = Encoding.UTF8.GetBytes(postParams);

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(postUrl);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = byteArray.Length;

                using (Stream webpageStream = webRequest.GetRequestStream())
                {
                    webpageStream.Write(byteArray, 0, byteArray.Length);
                }

                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                    {
                        webpageContent = reader.ReadToEnd();
                    }
                }
                bool success = webpageContent == "1" ? true : false;

                if (success)
                    Write("Posted successfully! Data: " + postParams);
                else
                    Write("Post failed! Data: " + postParams);
            }
            catch (WebException)
            {
                Write("Post error!");
            }
        }

        public string Encrypt(string _string)
        {
            System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
            byte[] bytes = ue.GetBytes(_string);

            // Encrypt bytes
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);

            // Convert the encrypted bytes back to a string (base 16)
            string hashString = "";

            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }

            return hashString.PadLeft(32, '0');
        }

        void Write(string _line)
        {
            Tools.WriteLine(_line);
        }
    }
}
