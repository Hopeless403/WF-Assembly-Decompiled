#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class NavigationStateMuncher : INavigationState
{
	public readonly List<UINavigationItem> disabled = new List<UINavigationItem>();

	public readonly EventRoutineMuncher muncher;

	public NavigationStateMuncher(EventRoutineMuncher muncher)
	{
		this.muncher = muncher;
	}

	public void Begin()
	{
		Button backButton = muncher.backButton;
		if ((object)backButton != null)
		{
			Disable(backButton.GetComponent<UINavigationItem>());
		}

		foreach (Entity item in muncher.cardContainer)
		{
			Disable(item.uINavigationItem);
		}

		UINavigationDefaultSystem.SetStartingItem();
	}

	public void End()
	{
		foreach (UINavigationItem item in disabled.Where((UINavigationItem a) => a != null))
		{
			item.enabled = true;
		}

		disabled.Clear();
	}

	public void Disable(UINavigationItem item)
	{
		if (!(item == null))
		{
			item.enabled = false;
			disabled.Add(item);
		}
	}
}
