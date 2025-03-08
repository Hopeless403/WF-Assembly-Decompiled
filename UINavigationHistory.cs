#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;

public static class UINavigationHistory
{
	[Serializable]
	public class Layer
	{
		public UINavigationLayer navigationLayer;

		public readonly List<UINavigationItem> navigationItemHistory = new List<UINavigationItem>();

		public Layer(UINavigationLayer navigationLayer)
		{
			this.navigationLayer = navigationLayer;
		}
	}

	public static readonly List<Layer> layers = new List<Layer>();

	public static readonly List<UINavigationItem> items = new List<UINavigationItem>(10);

	public const int capacity = 10;

	public static void AddItem(UINavigationItem item)
	{
		items.Add(item);
		if (items.Count > 10)
		{
			items.RemoveAt(0);
		}
	}

	public static int GetItemIndex(UINavigationItem item)
	{
		if (!items.Contains(item))
		{
			return 0;
		}

		return items.IndexOf(item);
	}

	public static void AddLayer(UINavigationLayer navLayer)
	{
		layers.Add(new Layer(navLayer));
	}

	public static void Clear()
	{
		layers.Clear();
		items.Clear();
	}

	public static void GoBackALayer()
	{
		if (MonoBehaviourSingleton<Cursor3d>.instance.usingMouse)
		{
			return;
		}

		if (layers.Count > 0)
		{
			Layer layer = layers.FirstOrDefault((Layer a) => a.navigationLayer == UINavigationSystem.ActiveNavigationLayer);
			if (layer != null && layer.navigationItemHistory.Count > 0)
			{
				MonoBehaviourSingleton<UINavigationSystem>.instance.SetCurrentNavigationItem(layer.navigationItemHistory.Last());
				return;
			}
		}

		UINavigationDefaultSystem.SetStartingItem(useClosest: false);
	}
}
