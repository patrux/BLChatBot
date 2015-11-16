using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BLChatBot
{
    class Settings
    {
        string path;

        SettingsValue bloodGateIP = new SettingsValue("bloodgateip", "54.152.16.158", "54.152.16.158");
        SettingsValue remoteWebpage = new SettingsValue("remotewebpage", "http://www.furyzone.com/blc/chat.php", "http://www.furyzone.com/blc/chat.php");
        SettingsValue localIP = new SettingsValue("localip", "", "");

        public Settings()
        {
            try
            {
                path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/settings.xml";
                LoadSettings();

                Tools.WriteLine("BloodGate IP: " + bloodGateIP.GetValue());
                Tools.WriteLine("Remote Webpage: " + remoteWebpage.GetValue());

                if (localIP.GetValue() != localIP.GetDefaultValue())
                    Tools.WriteLine("Local IP (override): " + localIP.GetValue());
                else
                    Tools.WriteLine("Local IP: auto detect");

                Tools.WriteSeparator();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BLCB::Exception> " + ex.ToString() + "\n");
                Tools.WriteLine("Error loading settings.xml");
                Tools.WriteLine("Restart application...");
            }
        }

        void DefaultValues()
        {
            bloodGateIP.SetDefaultValue();
            remoteWebpage.SetDefaultValue();
            localIP.SetDefaultValue();
        }

        public void LoadSettings()
        {
            XDocument xdoc = XDocument.Load(path);

            bloodGateIP.LoadValue(xdoc);
            remoteWebpage.LoadValue(xdoc);
            localIP.LoadValue(xdoc);
        }

        void SaveSettings()
        {
            XDocument xdoc = XDocument.Load(path);

            bloodGateIP.SaveValue(xdoc);
            remoteWebpage.SaveValue(xdoc);
            localIP.SaveValue(xdoc);

            xdoc.Save(path);
        }

        class SettingsValue
        {
            string field; // The value name to look-up
            string value; // The current value
            string defaultValue; // The default value

            public SettingsValue(string _field, string _value, string _defaultValue) { field = _field; value = _value; defaultValue = _defaultValue; }

            public string GetField() { return field; }
            public string GetValue() { return value; }
            public string GetDefaultValue() { return defaultValue; }

            public void LoadValue(XDocument _xdoc)
            {
                XElement xe = _xdoc.Root.Element(field);

                if (xe != null)
                {
                    string v = xe.Value;

                    if (v != null)
                        SetValue(v);
                    else
                        SetDefaultValue();
                }
                else
                    SetDefaultValue();
            }

            public void SaveValue(XDocument _xdoc)
            {
                _xdoc.Root.Element(field).SetValue(value);
            }

            public void SetValue(string _value) { value = _value; }
            public void SetDefaultValue() { value = defaultValue; }
        }

        public string GetBloodGateIP()
        {
            return bloodGateIP.GetValue();
        }

        public string GetRemoteWebpage()
        {
            return remoteWebpage.GetValue();
        }

        public string GetLocalIP()
        {
            return localIP.GetValue();
        }
    }
}