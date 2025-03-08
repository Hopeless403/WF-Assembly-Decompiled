#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public class ModifierDisplayCurrent : ModifierDisplay
{
	[SerializeField]
	public GameObject navController;

	public void OnEnable()
	{
		CreateIcons();
	}

	public void CreateIcons()
	{
		if (!this)
		{
			return;
		}

		ClearIcons();
		bool mainGameMode = Campaign.Data.GameMode.mainGameMode;
		CampaignData data = Campaign.Data;
		if (data != null)
		{
			List<GameModifierData> modifiers = data.Modifiers;
			if (modifiers != null)
			{
				foreach (GameModifierData item in modifiers)
				{
					CreateIcon(item, mainGameMode);
				}
			}
		}

		if ((bool)navController)
		{
			navController.SetActive(base.modifierCount > 0);
		}
	}

	public void Update()
	{
		CampaignData data = Campaign.Data;
		int num;
		if (data != null)
		{
			List<GameModifierData> modifiers = data.Modifiers;
			if (modifiers != null)
			{
				num = modifiers.Count;
				goto IL_001c;
			}
		}

		num = 0;
		goto IL_001c;
		IL_001c:
		if (num != base.modifierCount)
		{
			CreateIcons();
		}
	}
}
