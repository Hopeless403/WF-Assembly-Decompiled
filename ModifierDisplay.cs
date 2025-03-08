#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public class ModifierDisplay : MonoBehaviour
{
	[SerializeField]
	public ModifierIcon iconPrefab;

	[SerializeField]
	public ModifierIconMultiple stormLevelIconPrefab;

	[SerializeField]
	public Vector2 popUpOffset;

	public readonly List<ModifierIcon> icons = new List<ModifierIcon>();

	public ModifierIconMultiple stormLevelIcon;

	public int modifierCount { get; set; }

	public ModifierIcon CreateIcon(GameModifierData data, bool combineStormBells = true)
	{
		if (combineStormBells && (bool)data.linkedStormBell && (bool)stormLevelIconPrefab)
		{
			return AddToStormLevelIcon(data);
		}

		modifierCount++;
		if (!data.visible)
		{
			return null;
		}

		ModifierIcon modifierIcon = Object.Instantiate(iconPrefab, base.transform);
		modifierIcon.Set(data, popUpOffset);
		icons.Add(modifierIcon);
		return modifierIcon;
	}

	public void ClearIcons()
	{
		for (int num = icons.Count - 1; num >= 0; num--)
		{
			ModifierIcon modifierIcon = icons[num];
			if ((bool)modifierIcon)
			{
				modifierIcon.gameObject.Destroy();
			}
		}

		modifierCount = 0;
		stormLevelIcon = null;
	}

	public ModifierIcon AddToStormLevelIcon(GameModifierData modifier)
	{
		if (!stormLevelIcon)
		{
			CreateStormLevelIcon(modifier);
		}
		else
		{
			modifierCount++;
			stormLevelIcon.Add(modifier);
		}

		return stormLevelIcon;
	}

	public ModifierIcon CreateStormLevelIcon(GameModifierData firstModifier)
	{
		modifierCount++;
		stormLevelIcon = Object.Instantiate(stormLevelIconPrefab, base.transform);
		stormLevelIcon.Set(firstModifier, popUpOffset);
		icons.Add(stormLevelIcon);
		return stormLevelIcon;
	}
}
