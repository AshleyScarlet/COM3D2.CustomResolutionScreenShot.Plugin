using System;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace COM3D2.CustomResolutionScreenShot.Plugin
{
    internal static class Configuration
    {
        public static readonly string ConfigFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Config\CustomResolutionScreenShot.xml";

        static Configuration()
        {
            if (!File.Exists(ConfigFilePath))
                return;

            try
            {
                XDocument xml = XDocument.Load(ConfigFilePath);
                var configElement = xml.Element("Config");
                var presetElement = configElement.Element("Presets");
                var selectedPresetName = presetElement.Attribute("Target").Value;
                var presets = presetElement.Elements();
                foreach (var x in presets)
                {
                    if (x.Name.LocalName == "Preset" && x.Attribute("Name").Value == selectedPresetName)
                    {
                        if (int.TryParse(x.Element("Width").Value, out var width) && int.TryParse(x.Element("Height").Value, out var height))
                            Preset = new ResolutionPreset() { Width = width, Height = height };
                        break;
                    }
                }
            }
            catch { }
        }

        public static ResolutionPreset Preset { get; set; } = new ResolutionPreset() { Width = 3840, Height = 2160 };
    }
}
