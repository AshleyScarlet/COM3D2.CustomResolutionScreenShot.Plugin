using System;
using System.IO;
using UnityEngine;

namespace COM3D2.CustomResolutionScreenShot.Plugin
{
    internal static class ScreenShot
    {
        public static void Screenshot(this object _, GameObject obj)
        {
            var camera = Camera.main;
            string path = Util.GetTimeFileName();

            InternalScreenshot(camera, path, false);
        }

        public unsafe static void TransparentScreenshot(this object _, GameObject obj)
        {
            var camera = Camera.main;
            string path = Util.GetTimeFileName();

            var currentBGObj = GameMain.Instance.BgMgr.current_bg_object;

            var tmpCullingMask = camera.cullingMask;
            var tmpBackgroundColor = camera.backgroundColor;
            var tmpClearFlags = camera.clearFlags;
            var tmpBGActive = ReferenceEquals(currentBGObj, null) ? false : currentBGObj.activeSelf;

            try
            {
                camera.cullingMask = 1024;
                camera.backgroundColor = new Color(0, 0, 0, 0);
                camera.clearFlags = CameraClearFlags.SolidColor;
                if (!ReferenceEquals(currentBGObj, null))
                    currentBGObj.SetActive(false);

                if (Configuration.IsHighQualityTransparentMode)
                    HighQualityTransparentScreenshot(camera, path);
                else
                    InternalScreenshot(camera, path);
            }
            finally
            {
                if (!ReferenceEquals(currentBGObj, null))
                    GameMain.Instance.BgMgr.current_bg_object.SetActive(tmpBGActive);
                camera.cullingMask = tmpCullingMask;
                camera.backgroundColor = tmpBackgroundColor;
                camera.clearFlags = tmpClearFlags;
            }
        }

        private static void HighQualityTransparentScreenshot(Camera camera, string path)
        {
            var instance = CustomResolutionScreenShot.Instance;
            var tmpIsPreviewVisible = instance.IsPreviewVisible;
            instance.IsPreviewVisible = false;

            var preset = Configuration.CurrentPreset;
            var renderTexture = RenderTexture.GetTemporary(preset.Width, preset.Height, preset.DepthBuffer).WithAntiAliasing();
            var texture = new Texture2D(preset.Width, preset.Height, TextureFormat.ARGB32, false);

            var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            var tmpTargetTexture = camera.targetTexture;
            var tmpActiveTexture = RenderTexture.active;

            try
            {
                camera.backgroundColor = new Color(0, 0, 0, 1);
                camera.targetTexture = renderTexture;
                RenderTexture.active = renderTexture;
                camera.Render();
                texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                texture.Apply();
                var pixels1 = texture.GetPixels();

                camera.backgroundColor = new Color(1, 1, 1, 1);
                camera.Render();
                texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                texture.Apply();
                var pixels2 = texture.GetPixels();

                _ = pixels1.Length;
                _ = pixels2.Length;
                for (int i = 0; i < pixels1.Length; i++)
                {
                    pixels1[i] = new Color((pixels1[i].r + pixels2[i].r) / 2f, (pixels1[i].g + pixels2[i].g) / 2f, (pixels1[i].b + pixels2[i].b) / 2f, 1f - (pixels2[i].r - pixels1[i].r));
                }
                texture.SetPixels(pixels1);
                var data = texture.EncodeToPNG();
                fs.Write(data, 0, data.Length);
            }
            finally
            {
                fs.Dispose();
                camera.targetTexture = tmpTargetTexture;
                RenderTexture.active = tmpActiveTexture;
                RenderTexture.ReleaseTemporary(renderTexture);
                instance.IsPreviewVisible = tmpIsPreviewVisible;
                GameObject.Destroy(texture);
            }
        }

        private static void InternalScreenshot(Camera camera, string path, bool allowAlpha = true)
        {
            var instance = CustomResolutionScreenShot.Instance;
            var tmpIsPreviewVisible = instance.IsPreviewVisible;
            instance.IsPreviewVisible = false;

            var preset = Configuration.CurrentPreset;
            var renderTexture = RenderTexture.GetTemporary(preset.Width, preset.Height, preset.DepthBuffer, RenderTextureFormat.ARGB32).WithAntiAliasing();
            var texture = new Texture2D(preset.Width, preset.Height, allowAlpha ? TextureFormat.ARGB32 : TextureFormat.RGB24, false);

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
                instance.IsPreviewVisible = tmpIsPreviewVisible;
                GameObject.Destroy(texture);
            }
        }
    }
}
