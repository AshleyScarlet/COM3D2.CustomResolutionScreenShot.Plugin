using System;
using System.Collections.Generic;
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
                var dict = new Dictionary<string,ResolutionPreset>();
                var defaultPreset = CurrentPreset;
                dict.Add(defaultPreset.Name, defaultPreset);
                foreach (var x in presets)
                {
                    if (x.Name.LocalName == "Preset")
                    {
                        var name = x.Attribute("Name").Value;
                        if (int.TryParse(x.Element("Width")?.Value, out var width) && int.TryParse(x.Element("Height")?.Value, out var height))
                        {
                            ResolutionPreset preset;
                            if (int.TryParse(x.Element("DepthBuffer")?.Value, out var depthBuffer))
                                preset = new ResolutionPreset(width, height, depthBuffer, name);
                            else
                                preset = new ResolutionPreset(width, height, name);

                            if (!dict.ContainsKey(name))
                                dict.Add(name, preset);
                        }
                    }
                }
                if (dict.TryGetValue(selectedPresetName, out var selectedPreset))
                {
                    CurrentPreset = selectedPreset;
                }
                else
                {
                    var tmp = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[CRSS] : プリセット\"{0}\"が見つかりませんでした。デフォルト設定(3840x2160)を使用します。", selectedPresetName);
                    Console.ForegroundColor = tmp;
                    CurrentPreset = default;
                }

                Presets = dict;
            }
            catch(Exception e)
            {
                var tmp = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[CRSS] : {0}", e.Message);
                Console.ForegroundColor = tmp;
            }
        }

        public static bool IsHighQualityTransparentMode { get; set; } = true;
        public static ResolutionPreset CurrentPreset { get; set; } = new ResolutionPreset(3840, 2160, "Default");

        public static Dictionary<string, ResolutionPreset> Presets { get; }
    }
}
