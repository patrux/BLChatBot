using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLChatBot
{
    public static class Tools
    {
        public static void WriteLine(string _line)
        {
            Console.WriteLine("BLCB> " + _line);
            LogLine(_line);
        }

        public static void LogLine(string _line)
        {
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/log.txt";
            File.AppendAllText(path, _line + Environment.NewLine);
        }

        public static void WriteSeparator()
        {
            Console.WriteLine("---------------------------------------");
            LogLine("---------------------------------------");
        }

        public static byte[] HexToByteArray(string _hex)
        {
            string hex = _hex.Replace(" ", "");

            int length = hex.Length;
            byte[] data = new byte[length / 2];

            for (int i = 0; i < length; i += 2)
                data[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);

            return data;
        }

        public static void TrimTrailingBytes(ref byte[] buffer, byte trimValue)
        {
            int i = buffer.Length;

            while (i > 0 && buffer[--i] == trimValue)
            {
                ; // no-op by design
            }

            Array.Resize(ref buffer, i + 1);

            return;
        }

        public static string ByteArrayToString(byte[] _data)
        {
            string s = "";

            foreach (byte b in _data)
                s += b + ":";

            return s.Substring(0, s.Length - 1);
        }

        public static string HexStringToString3(byte[] _data)
        {
            string s = "";

            for (int i = 0; i < _data.Length; i++)
            {
                char c = Convert.ToChar(_data[i]);

                if (!Regex.IsMatch(c.ToString(), @"[^\u001F-\u007E]"))
                    s += c;
            }

            return s;
        }

        public static string HexStringToString(string _hexString)
        {
            string stringValue = "";
            for (int i = 0; i < _hexString.Length / 2; i++)
            {
                string hexChar = _hexString.Substring(i * 2, 2);
                int hexValue = Convert.ToInt32(hexChar, 16);
                stringValue += @Char.ConvertFromUtf32(hexValue);
            }
            return stringValue;
        }

        public static byte[] HexStringToString2(string hex)
        {
            hex = hex.Replace("-", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }
    }
}
