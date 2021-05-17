using System;
using System.IO;
using UnityEngine;

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

        public static int GetPixel(int value)
        {
            float num = 1f + (Screen.width / 1280f - 1f) * 0.6f;
            return (int)(num * value);
        }

        public static bool IsMouseOnRect(Rect rect)
        {
            return rect.Contains(new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y));
        }

        public static bool GetAnyMouseDown()
        {
            return Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2);
        }

        public static bool GetMouseWheelUse()
        {
            return Input.GetAxis("Mouse ScrollWheel") != 0f;
        }
        
        public static int CountDigits(uint value)
        {
            int digits = 1;
            if (value >= 100000)
            {
                value /= 100000;
                digits += 5;
            }

            if (value < 10)
            {
                // no-op
            }
            else if (value < 100)
            {
                digits++;
            }
            else if (value < 1000)
            {
                digits += 2;
            }
            else if (value < 10000)
            {
                digits += 3;
            }
            else
            {
                Debug.Assert(value < 100000);
                digits += 4;
            }

            return digits;
        }


        public static RenderTexture WithAntiAliasing(this RenderTexture renderTexture)
        {
            var value = GameMain.Instance.CMSystem.Antialias;
            if (value == CMSystem.AntiAliasType.None)
                renderTexture.antiAliasing = 0;
            else if (value == CMSystem.AntiAliasType.X2)
                renderTexture.antiAliasing = 2;
            else if (value == CMSystem.AntiAliasType.X4)
                renderTexture.antiAliasing = 4;
            else
                renderTexture.antiAliasing = 8;
            return renderTexture;
        }

    }
}
