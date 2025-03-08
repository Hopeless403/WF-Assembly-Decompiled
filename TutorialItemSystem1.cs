#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class TutorialItemSystem1 : GameSystem
{
	public string[] items;

	public void OnEnable()
	{
		Events.OnEventStart += EventStart;
	}

	public void OnDisable()
	{
		Events.OnEventStart -= EventStart;
	}

	public void SetItems(string[] items)
	{
		this.items = items;
	}

	public void EventStart(CampaignNode node, EventRoutine @event)
	{
		if (@event is ItemEventRoutine)
		{
			node.data["cards"] = new SaveCollection<string>(items);
		}

		Object.Destroy(this);
	}
}
