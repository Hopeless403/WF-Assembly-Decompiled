#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviourSingleton<CustomCursor>
{
	[Serializable]
	public class Style
	{
		public string name;

		public Texture2D sprite;

		public Vector2 hotSpot;
	}

	public CursorMode cursorMode;

	public Style[] styles;

	public string currentStyle = "default";

	public Dictionary<string, Style> styleLookup;

	public static bool visible = true;

	public override void Awake()
	{
		base.Awake();
		SetStyle(currentStyle);
	}

	public Style Get(string styleName)
	{
		Style result = null;
		if (styleLookup == null)
		{
			styleLookup = new Dictionary<string, Style>();
			Style[] array = styles;
			foreach (Style style in array)
			{
				styleLookup[style.name] = style;
				if (style.name == styleName)
				{
					result = style;
				}
			}
		}
		else
		{
			result = styleLookup[styleName];
		}

		return result;
	}

	public void Set(Style style)
	{
		MonoBehaviourSingleton<CustomCursor>.instance.currentStyle = style.name;
		Cursor.SetCursor(style.sprite, style.hotSpot, MonoBehaviourSingleton<CustomCursor>.instance.cursorMode);
	}

	public static void SetStyle(string styleName)
	{
		Style style = MonoBehaviourSingleton<CustomCursor>.instance.Get(styleName);
		if (style != null)
		{
			MonoBehaviourSingleton<CustomCursor>.instance.Set(style);
		}
	}

	public void OnApplicationFocus(bool focus)
	{
		if (focus)
		{
			UpdateState();
		}
		else
		{
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}
	}

	public static void UpdateState()
	{
		if (visible && (MonoBehaviourSingleton<Cursor3d>.instance == null || MonoBehaviourSingleton<Cursor3d>.instance.usingMouse))
		{
			Cursor.visible = true;
			SetStyle(MonoBehaviourSingleton<CustomCursor>.instance.currentStyle);
		}
		else
		{
			Cursor.visible = false;
		}
	}
}
