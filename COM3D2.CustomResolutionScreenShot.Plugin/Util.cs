using System;
using System.IO;

namespace COM3D2.CustomResolutionScreenShot.Plugin
{
    internal static class Util
    {
        private static string _ScreenshotBasePath = null;

        static Util()
        {
            var basePath = string.Concat(Path.GetFullPath(".\\"), "ScreenShot\\");
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            _ScreenshotBasePath = basePath;
        }

        public static string GetTimeFileName(string extension = ".png")
        {
            var basePath = _ScreenshotBasePath;
            return string.Concat(basePath, "\\img", DateTime.Now.ToString("yyyyMMddHHmmss"), extension);
        }
    }
}
