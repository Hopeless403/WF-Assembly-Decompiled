#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class CardTooltip : Tooltip
{
	[Header("Card Elements")]
	[SerializeField]
	public TMP_Text titleElement;

	[SerializeField]
	public TMP_Text descElement;

	[SerializeField]
	public Image cardArt;

	[SerializeField]
	public Image textBox;

	[SerializeField]
	public Image nameTag;

	[SerializeField]
	public Transform healthIconGroup;

	[SerializeField]
	public Transform damageIconGroup;

	[SerializeField]
	public Transform counterIconGroup;

	[SerializeField]
	public Transform crownIconGroup;

	public readonly Dictionary<string, Transform> iconGroups = new Dictionary<string, Transform>();

	public HashSet<KeywordData> keywords;

	public void Awake()
	{
		if (healthIconGroup != null)
		{
			iconGroups["health"] = healthIconGroup;
		}

		if (damageIconGroup != null)
		{
			iconGroups["damage"] = damageIconGroup;
		}

		if (counterIconGroup != null)
		{
			iconGroups["counter"] = counterIconGroup;
		}

		if (crownIconGroup != null)
		{
			iconGroups["crown"] = crownIconGroup;
		}
	}

	public void Set(CardData data)
	{
		healthIconGroup.DestroyAllChildren();
		damageIconGroup.DestroyAllChildren();
		counterIconGroup.DestroyAllChildren();
		crownIconGroup.DestroyAllChildren();
		cardArt.sprite = data.mainSprite;
		textBox.sprite = data.cardType.textBoxSprite;
		nameTag.sprite = data.cardType.nameTagSprite;
		titleElement.text = data.title;
		string description = Card.GetDescription(data);
		keywords = Text.GetKeywords(description);
		string text = Text.Process(description, 0, 1f, data.cardType.descriptionColours);
		if (text.IsNullOrWhitespace())
		{
			UnityEngine.Localization.LocalizedString flavourKey = data.flavourKey;
			if (flavourKey != null && !flavourKey.IsEmpty)
			{
				string localizedString = data.flavourKey.GetLocalizedString();
				text = "<i><color=#" + data.cardType.descriptionColours.flavourColour + ">" + localizedString;
			}
		}

		descElement.text = text;
		if (data.hasHealth)
		{
			CreateIcon("health", "health", data.hp);
		}

		if (data.hasAttack)
		{
			CreateIcon("damage", "damage", data.damage);
		}

		if (data.counter > 0)
		{
			CreateIcon("counter", "counter", data.counter);
		}

		CardData.StatusEffectStacks[] startWithEffects = data.startWithEffects;
		foreach (CardData.StatusEffectStacks statusEffectStacks in startWithEffects)
		{
			if (statusEffectStacks.data.visible && !statusEffectStacks.data.iconGroupName.IsNullOrWhitespace())
			{
				CreateIcon(statusEffectStacks.data.type, statusEffectStacks.data.iconGroupName, statusEffectStacks.count);
			}
		}

		Ping();
	}

	public void CreateIcon(string type, string iconGroupName, int value)
	{
		CardManager.NewStatusIcon(type, iconGroups[iconGroupName]).SetValue(new Stat(value), doPing: false);
	}
}
