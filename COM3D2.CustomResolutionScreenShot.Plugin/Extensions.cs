using System;
using System.IO;
using UnityEngine;

namespace COM3D2.CustomResolutionScreenShot.Plugin
{
    internal static class Extensions
    {
        public static void Screenshot(this object _, GameObject obj)
        {
            var camera = Camera.main;
            string path = Util.GetTimeFileName();

            InternalScreenshot(camera, path);

            
        }

        public static void TransparentScreenshot(this object _, GameObject obj)
        {
            var camera = Camera.main;
            string path = Util.GetTimeFileName();

            var tmpCullingMask = camera.cullingMask;
            var tmpBackgroundColor = camera.backgroundColor;
            var tmpClearFlags = camera.clearFlags;

            try
            {
                camera.cullingMask = 1024;
                camera.backgroundColor = new Color(0, 0, 0, 0);
                camera.clearFlags = CameraClearFlags.SolidColor;

                InternalScreenshot(camera, path);
            }
            finally
            {
                camera.cullingMask = tmpCullingMask;
                camera.backgroundColor = tmpBackgroundColor;
                camera.clearFlags = tmpClearFlags;
            }
        }

        private static void InternalScreenshot(Camera camera, string path)
        {
            var preset = Configuration.Preset;
            var renderTexture = RenderTexture.GetTemporary(preset.Width, preset.Height);
            var texture = _TextureCache;
            if (ReferenceEquals(texture, null))
            {
                texture = new Texture2D(preset.Width, preset.Height, TextureFormat.ARGB32, false);
                _TextureCache = texture;
            }
            SetAntiAliasing(renderTexture);

            var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            var tmpTargetTexture = camera.targetTexture;
            var tmpActiveTexture = RenderTexture.active;

            try
            {
                camera.targetTexture = renderTexture;
                RenderTexture.active = renderTexture;
                camera.Render();
                texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                texture.Apply();
                var data = texture.EncodeToPNG();
                fs.Write(data, 0, data.Length);
            }
            finally
            {
                fs.Dispose();
                camera.targetTexture = tmpTargetTexture;
                RenderTexture.active = tmpActiveTexture;
                RenderTexture.ReleaseTemporary(renderTexture);
            }
        }

        private static void SetAntiAliasing(RenderTexture renderTexture)
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
        }

        private static Texture2D _TextureCache;
    }
}
