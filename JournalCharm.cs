#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalCharm : MonoBehaviourRect
{
	public Vector2 popUpOffset = new Vector2(1f, 0f);

	[SerializeField]
	public Image image;

	[SerializeField]
	public Material discoveredMaterial;

	public CardUpgradeData upgradeData;

	public bool discovered;

	public bool hover;

	public void OnDisable()
	{
		UnHover();
	}

	public void Assign(CardUpgradeData upgradeData)
	{
		this.upgradeData = upgradeData;
		image.sprite = this.upgradeData.image;
	}

	public void CheckDiscovered(List<string> discoveredCharms)
	{
		if (!discovered && discoveredCharms.Contains(upgradeData.name))
		{
			SetDiscovered();
		}
	}

	public void SetDiscovered()
	{
		discovered = true;
		image.material = discoveredMaterial;
		image.color = Color.white;
	}

	public void Hover()
	{
		if (discovered)
		{
			hover = true;
			CardPopUp.AssignTo(base.rectTransform, popUpOffset.x, popUpOffset.y);
			CardPopUp.AddPanel(upgradeData.name, upgradeData.title, upgradeData.text);
		}
	}

	public void UnHover()
	{
		if (discovered && hover)
		{
			hover = false;
			CardPopUp.RemovePanel(upgradeData.name);
		}
	}
}
