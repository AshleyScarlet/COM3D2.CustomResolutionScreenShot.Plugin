using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityInjector;
using UnityInjector.Attributes;

namespace COM3D2.CustomResolutionScreenShot.Plugin
{
    [PluginName("CustomResolutionScreenShot")]
    [PluginVersion("1.1.0")]
    public class CustomResolutionScreenShot : PluginBase
    {
		public CustomResolutionScreenShot()
        {
			_Instance = this;

			var presets = Configuration.Presets;
			var array = new string[presets.Count];
			_ = array.Length;
			int i = 0;
			foreach(var x in presets.Keys)
            {
				array[i++] = x;
            }
			_PresetNames = array;
			_SelectedPresetName = Configuration.CurrentPreset.Name;
			_CurrentPresetResolutionText = Configuration.CurrentPreset.GetResolutionText();
		}

		private void Start()
		{
			_ComboBox = new CustomComboBox(WindowID + 1);

			GearMenu.Buttons.Add("CustomResolutionScreenShot", "カスタム解像度撮影", Icon.Normal, default(object).Screenshot);
            GearMenu.Buttons.Add("CustomResolutionTransparentScreenShot", "透過カスタム解像度撮影", Icon.Transparent, default(object).TransparentScreenshot);
			GearMenu.Buttons.Add("CustomResolutionScreenShotSetting", "カスタム解像度撮影設定", Icon.Setting, _ => IsWindowVisible = !IsWindowVisible);

			SceneManager.sceneLoaded += OnSceneLoaded;
			
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
			IsPreviewVisible = false;
        }

        private void Update()
        {
            if (ReferenceEquals(GameObject.Find("CRSS-PreviewRenderer"), null))
            {
                var canvasObject = new GameObject("CRSS-Canvas");
                var canvas = canvasObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                _ = canvasObject.AddComponent<CanvasScaler>();

                var previewRenderer = new GameObject("CRSS-PreviewRenderer");
                previewRenderer.transform.parent = canvasObject.transform;
				var rawImage = previewRenderer.AddComponent<RawImage>();
				rawImage.color = new Color(0, 0, 0, 0);

				var preset = Configuration.CurrentPreset;
				ResizePreviewImage(preset);
			}
        }

		private void ResizePreviewImage(ResolutionPreset preset)
		{
			var rawImage = GameObject.Find("CRSS-PreviewRenderer").GetComponent<RawImage>();
			float scale = Math.Min((float)(Screen.width / 4f) / preset.Width, (float)(Screen.height / 2f) / preset.Height);
			var width = preset.Width * scale;
			var height = preset.Height * scale;

			rawImage.rectTransform.sizeDelta = new Vector2(width, height);
			rawImage.rectTransform.position = new Vector3(width / 2, height / 2, 0);
		}

        private void OnGUI()
		{
			if (!IsWindowVisible)
				return;

			var windowGUIStyle = _WindowGUIStyle;
			if (windowGUIStyle == null)
			{
				windowGUIStyle = new GUIStyle("box");
				windowGUIStyle.fontSize = Util.GetPixel(12);
				windowGUIStyle.alignment = TextAnchor.UpperRight;
				_WindowGUIStyle = windowGUIStyle;
			}
			float num = windowGUIStyle.fontSize * 19;

			var windowRect = _WindowRect;
			var windowHeight = _WindowHeight;
			var screenSize = _ScreenSize;
			var comboBox = _ComboBox;

			if (windowRect.width < 1f)
			{
				windowRect.Set(20f, 20f, num, windowHeight);
			}
			if (windowHeight != windowRect.height)
			{
				windowRect.Set(windowRect.x, windowRect.y, num, windowHeight);
			}
			if (screenSize != new Vector2(Screen.width, Screen.height))
			{
				windowRect.Set(windowRect.x, windowRect.y, num, windowHeight);
				screenSize = new Vector2(Screen.width, Screen.height);

				ResizePreviewImage(Configuration.CurrentPreset);
			}
			if (windowRect.x < 0f - windowRect.width * 0.9f)
			{
				windowRect.x = 0f;
			}
			else if (windowRect.x > screenSize.x - windowRect.width * 0.1f)
			{
				windowRect.x = screenSize.x - windowRect.width;
			}
			else if (windowRect.y < 0f - windowRect.height * 0.9f)
			{
				windowRect.y = 0f;
			}
			else if (windowRect.y > screenSize.y - windowRect.height * 0.1f)
			{
				windowRect.y = screenSize.y - windowRect.height;
			}

			_WindowRect = windowRect;
			_WindowHeight = windowHeight;
			_ScreenSize = screenSize;

			windowRect = GUI.Window(WindowID, windowRect, OnGUIWindow, "CustomResolutionScreenShot", windowGUIStyle);
			_WindowRect = windowRect;


			if (comboBox.IsVisible)
			{
				comboBox.Rect = GUI.Window(comboBox.WindowID, comboBox.Rect, comboBox.GuiFunc, string.Empty, windowGUIStyle);
				if (Util.IsMouseOnRect(comboBox.Rect))
				{
					if (Util.GetAnyMouseDown() || Util.GetMouseWheelUse())
					{
						Input.ResetInputAxes();
					}
				}
			}


		}
		private void OnGUIWindow(int windowID)
		{
			var windowRect = _WindowRect;
			var windowHeight = _WindowHeight;

			GUIStyle labelStyle = new GUIStyle("label");
			labelStyle.fontSize = Util.GetPixel(12);
			labelStyle.alignment = TextAnchor.UpperLeft;

			GUIStyle toggleButtonStyle = new GUIStyle("toggle");
			toggleButtonStyle.fontSize = Util.GetPixel(12);
			toggleButtonStyle.alignment = TextAnchor.MiddleLeft;

			GUIStyle buttonStyle = new GUIStyle("button");
			buttonStyle.fontSize = Util.GetPixel(12);
			buttonStyle.alignment = TextAnchor.MiddleCenter;

			GUIStyle guistyle5 = new GUIStyle("textfield");
			guistyle5.fontSize = Util.GetPixel(12);
			guistyle5.alignment = TextAnchor.UpperLeft;
			float num = labelStyle.fontSize;
			float num2 = labelStyle.fontSize * 1.5f;
			float num3 = labelStyle.fontSize * 0.5f;

			Rect rect = new Rect(num / 2f, labelStyle.fontSize + num3, windowRect.width - num, windowRect.height - num);
			Rect position = new Rect(rect.x, rect.y, rect.width, num2);
			if (GUI.Button(new Rect(0f, 0f, Util.GetPixel(8) * 2, Util.GetPixel(8) * 2), "×", buttonStyle))
			{
				IsWindowVisible = false;
			}
			position.Set(rect.x, position.y + 0.5f, rect.width / 2f, num2);
			GUI.Label(position, "プリセット", labelStyle);

			position.Set(rect.x + num3 * 11, position.y, rect.width - position.width , position.height);
			if (GUI.Button(position, _SelectedPresetName, buttonStyle))
			{
				_ComboBox.Set(new Rect(windowRect.x + position.x, windowRect.y + position.y + num2, position.width, num2 * _PresetNames.Length), _PresetNames, (int)num, x =>
				{
					var selectedPresetName = _PresetNames[x];
					if (_SelectedPresetName != selectedPresetName)
					{
						_SelectedPresetName = _PresetNames[x];
						var preset = Configuration.Presets[_SelectedPresetName];
						Configuration.CurrentPreset = preset;
						_CurrentPresetResolutionText = preset.GetResolutionText();

						ResizePreviewImage(preset);
					}
				});
			}

			position.Set(position.x, position.y + num3 * 2.5f, position.width, position.height);
			GUI.Label(position, _CurrentPresetResolutionText, labelStyle);

			position.Set(rect.x, position.y + num3 * 3, Util.GetPixel(8) * 14, Util.GetPixel(8) * 2);
			Configuration.IsHighQualityTransparentMode = GUI.Toggle(position, Configuration.IsHighQualityTransparentMode, "高品質モード", toggleButtonStyle);

			position.Set(rect.x, position.y + num3 * 3, rect.width, Util.GetPixel(8) * 2);
			if (GUI.Button(position, "プレビュー表示", buttonStyle))
			{
				IsPreviewVisible = !IsPreviewVisible;
				var rawImage = GameObject.Find("CRSS-PreviewRenderer").GetComponent<RawImage>();
				if (IsPreviewVisible)
                {
					rawImage.color = new Color(1, 1, 1, 1);
                }
                else
                {
					rawImage.color = new Color(0, 0, 0, 0);
				}
			}


			num3 += 20;

			if (IsPreviewVisible)
			{
				var rawImage = GameObject.Find("CRSS-PreviewRenderer").GetComponent<RawImage>();
				var size = rawImage.rectTransform.sizeDelta;
				var camera = Camera.main;
				var preset = Configuration.CurrentPreset;

				var width = size.x;
				var height = size.y;

				var renderTexrure = RenderTexture.GetTemporary((int)width, (int)height, preset.DepthBuffer);

				var tmpTargetTexture = camera.targetTexture;
				var tmpActiveTexture = RenderTexture.active;
				camera.targetTexture = renderTexrure;
				RenderTexture.active = renderTexrure;

				camera.Render();
				rawImage.texture = renderTexrure;

				camera.targetTexture = tmpTargetTexture;
				RenderTexture.active = tmpActiveTexture;
				RenderTexture.ReleaseTemporary(renderTexrure);
			}

			windowHeight = position.y + position.height + num3;

			_WindowRect = windowRect;
			_WindowHeight = windowHeight;
			GUI.DragWindow();
		}

		public static CustomResolutionScreenShot Instance => _Instance;
		private static CustomResolutionScreenShot _Instance;

		public bool IsWindowVisible { get; set; }
		public bool IsPreviewVisible { get; set; }

		private string _SelectedPresetName;
		private string _CurrentPresetResolutionText;
		private readonly string[] _PresetNames;

		private CustomComboBox _ComboBox;
        private GUIStyle _WindowGUIStyle;
        private Rect _WindowRect;
        private float _WindowHeight;
        private Vector2 _ScreenSize;

        private const int WindowID = 5568;
	}
}
