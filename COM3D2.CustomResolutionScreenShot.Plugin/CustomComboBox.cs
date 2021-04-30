using System;
using UnityEngine;

namespace COM3D2.CustomResolutionScreenShot.Plugin
{
    internal class CustomComboBox
	{
		public Rect Rect { get; set; }
		public bool IsVisible { get; set; }

		private Rect _RectItem;
		private string[] _Items;
		private Action<int> _Callback;
		private bool _CanScroll;
		private Vector2 _ScrollPosition;

		private GUIStyle _SelectionGrid;

		public readonly int WindowID;


		public CustomComboBox(int wIndowID)
		{
			WindowID = wIndowID;

			var blackStyle = new GUIStyleState();
			blackStyle.textColor = Color.white;
			blackStyle.background = Texture2D.blackTexture;

			var whiteStyle = new GUIStyleState();
			whiteStyle.textColor = Color.black;
			whiteStyle.background = Texture2D.whiteTexture;

			var selectionGrid = new GUIStyle();
			selectionGrid.fontSize = 12;
			selectionGrid.normal = blackStyle;
			selectionGrid.hover = whiteStyle;

			_SelectionGrid = selectionGrid;
		}

		public void Set(Rect rect, string[] items, int fontSize, Action<int> callback)
		{
			if (rect.height > Screen.height * 0.5f)
			{
				this.Rect = new Rect(rect.x, rect.y, rect.width, Screen.height * 0.5f);
				_CanScroll = true;
			}
			else
			{
				_CanScroll = false;
				this.Rect = rect;
			}
			_Items = items;
			_SelectionGrid.fontSize = fontSize;
			_RectItem = new Rect(0f, 0f, rect.width, rect.height);
			_Callback = callback;
			IsVisible = true;
		}

		public void GuiFunc(int windowID)
		{
			if (_CanScroll)
			{
				_ScrollPosition = GUI.BeginScrollView(new Rect(0f, 0f, Rect.width, Rect.height), _ScrollPosition, _RectItem);
			}
			int num = GUI.SelectionGrid(_RectItem, -1, _Items, 1, _SelectionGrid);
			if (_CanScroll)
			{
				GUI.EndScrollView();
			}
			if (num >= 0)
			{
				_Callback(num);
				IsVisible = false;
			}
			if (GetAnyMouseButtonDown())
			{
				Vector2 point = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
				if (!Rect.Contains(point))
				{
					IsVisible = false;
				}
			}
		}

		private bool GetAnyMouseButtonDown()
		{
			return Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2);
		}


	}
}
