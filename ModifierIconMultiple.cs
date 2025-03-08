#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ModifierIconMultiple : ModifierIcon
{
	[SerializeField]
	public TMP_Text stormLevelText;

	[SerializeField]
	public Transform juiceLevel;

	[SerializeField]
	public Vector2 juiceLevelRange = new Vector2(-0.9f, 0f);

	public readonly List<GameModifierData> modifiers = new List<GameModifierData>();

	public int _stormLevel;

	public int stormLevel
	{
		get
		{
			return _stormLevel;
		}
		set
		{
			_stormLevel = value;
			if ((bool)stormLevelText)
			{
				stormLevelText.text = _stormLevel.ToString();
			}

			if ((bool)juiceLevel)
			{
				float t = Mathf.Clamp((float)_stormLevel / 10f, 0f, 1f);
				float value2 = Mathf.Lerp(juiceLevelRange.x, juiceLevelRange.y, t);
				juiceLevel.transform.localPosition = juiceLevel.transform.localPosition.WithY(value2);
			}
		}
	}

	public override void Set(GameModifierData modifier, Vector2 popUpOffset)
	{
		base.Set(modifier, popUpOffset);
		modifiers.Add(modifier);
		if ((bool)modifier.linkedStormBell)
		{
			stormLevel = modifier.linkedStormBell.stormPoints;
		}
		else
		{
			stormLevel = 0;
		}
	}

	public void Add(GameModifierData modifier)
	{
		modifiers.Add(modifier);
		if ((bool)modifier.linkedStormBell)
		{
			stormLevel += modifier.linkedStormBell.stormPoints;
		}
	}

	public void Clear()
	{
		stormLevel = 0;
		modifiers.Clear();
	}

	public override void Pop()
	{
		if (poppedUp)
		{
			return;
		}

		CardPopUp.AssignTo(base.rectTransform, popUpOffset.x, popUpOffset.y);
		foreach (GameModifierData modifier in modifiers)
		{
			CardPopUp.AddPanel(modifier.name, modifier.titleKey.GetLocalizedString(), modifier.descriptionKey.GetLocalizedString());
		}

		poppedUp = true;
	}

	public override void UnPop()
	{
		if (!poppedUp)
		{
			return;
		}

		foreach (GameModifierData modifier in modifiers)
		{
			CardPopUp.RemovePanel(modifier.name);
		}

		poppedUp = false;
	}
}
