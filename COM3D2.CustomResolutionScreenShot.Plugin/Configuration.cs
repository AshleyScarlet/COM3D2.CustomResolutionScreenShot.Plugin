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
                    if (x.Name.LocalName == "Preset" && x.Attribute("Name")?.Value == selectedPresetName)
                    {
                        if (int.TryParse(x.Element("Width")?.Value, out var width) && int.TryParse(x.Element("Height")?.Value, out var height))
                        {
                            if (int.TryParse(x.Element("DepthBuffer")?.Value, out var depthBuffer))
                                Preset = new ResolutionPreset(width, height, depthBuffer);
                            else
                                Preset = new ResolutionPreset(width, height);
                        }
                        return;
                    }
                }
                var tmp = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[CRSS] : プリセット\"{0}\"が見つかりませんでした。デフォルト設定(3840x2160)を使用します。", selectedPresetName);
                Console.ForegroundColor = tmp;
            }
            catch(Exception e)
            {
                var tmp = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[CRSS] : {0}", e.Message);
                Console.ForegroundColor = tmp;
            }
        }

        public static ResolutionPreset Preset { get; set; } = new ResolutionPreset(3840,2160);
    }
}
