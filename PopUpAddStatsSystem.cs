#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class PopUpAddStatsSystem : GameSystem
{
	[SerializeField]
	public UnityEngine.Localization.LocalizedString counterRef;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString counterThisTurnRef;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString counterFrozenRef;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString reactionRef;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString reactionJoinRef;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString reactionFrozenRef;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString reactionSilencedRef;

	public Entity hover;

	public bool hoverEntityIsSnowed;

	public bool hoverEntitySilenced;

	public const string buffedColour = "#5F5";

	public const string debuffedColour = "#e8a0a0";

	public void OnEnable()
	{
		Events.OnEntityHover += HoverChanged;
		Events.OnPopupCreated += PopupCreated;
	}

	public void OnDisable()
	{
		Events.OnEntityHover -= HoverChanged;
		Events.OnPopupCreated -= PopupCreated;
	}

	public void HoverChanged(Entity entity)
	{
		hover = entity;
		hoverEntityIsSnowed = entity.IsSnowed;
		hoverEntitySilenced = entity.silenced;
	}

	public void PopupCreated(KeywordData keyword, CardPopUpPanel panel)
	{
		if ((bool)hover)
		{
			switch (keyword.name)
			{
				case "Health":
				{
					string text3 = "<color=white>";
					text3 = ((hover.hp.current < hover.hp.max) ? (text3 + string.Format("<color={0}>{1}</color>", "#e8a0a0", hover.hp.current)) : ((hover.hp.current <= hover.hp.max) ? (text3 + $"{hover.hp.current}") : (text3 + string.Format("<color={0}>{1}</color>", "#5F5", hover.hp.current))));
					text3 += $"/{hover.hp.max}</color>";
					panel.AddToTitle(text3);
					panel.BuildTextElement();
					break;
				}
				case "Attack":
				{
					string text2 = "<color=white>";
					int num = hover.damage.current + hover.tempDamage.Value;
					text2 = ((num < hover.damage.max) ? (text2 + string.Format("<color={0}>{1}</color>", "#e8a0a0", num)) : ((num <= hover.damage.max) ? (text2 + $"{num}") : (text2 + string.Format("<color={0}>{1}</color>", "#5F5", num))));
					text2 += $"/{hover.damage.max}</color>";
					panel.AddToTitle(text2);
					panel.BuildTextElement();
					break;
				}
				case "Counter":
				{
					string text = "<color=white>";
					text = ((hover.counter.current <= hover.counter.max) ? (text + $"{hover.counter.current}") : (text + string.Format("<color={0}>{1}</color>", "#e8a0a0", hover.counter.current)));
					text += $"/{hover.counter.max}</color>";
					panel.AddToTitle(text);
					panel.AddToBody(BuildCounterBodyText(hoverEntityIsSnowed, hover.counter.current));
					panel.BuildTextElement();
					break;
				}
				case "Reaction":
					panel.AddToBody(BuildReactionBodyText(hoverEntityIsSnowed, hoverEntitySilenced));
					panel.BuildTextElement();
					break;
			}
		}
	}

	public string BuildCounterBodyText(bool frozen, int turnsTilTrigger)
	{
		if (!frozen)
		{
			if (turnsTilTrigger <= 1)
			{
				return counterThisTurnRef.GetLocalizedString();
			}

			return counterRef.GetLocalizedString().Format(turnsTilTrigger);
		}

		return counterFrozenRef.GetLocalizedString();
	}

	public string BuildReactionBodyText(bool frozen, bool silenced)
	{
		if (silenced)
		{
			return reactionSilencedRef.GetLocalizedString();
		}

		if (frozen)
		{
			return reactionFrozenRef.GetLocalizedString();
		}

		List<string> list = new List<string>();
		foreach (StatusEffectData statusEffect in hover.statusEffects)
		{
			if (statusEffect.isReaction && !statusEffect.textKey.IsEmpty)
			{
				list.Add(FormatReactionText(statusEffect));
			}
		}

		foreach (Entity.TraitStacks trait in hover.traits)
		{
			if (trait.data.isReaction)
			{
				list.Add("<" + FirstCharToLowerCase(trait.data.keyword.body) + ">");
			}
		}

		return StringExt.Format(reactionRef.GetLocalizedString(), string.Join(reactionJoinRef.GetLocalizedString(), list));
	}

	public static string FormatReactionText(StatusEffectData effect)
	{
		string text = Text.GetEffectText(effect.textKey, effect.textInsert, effect.count);
		for (int i = 0; i < text.Length; i++)
		{
			if (text[i] != '<')
			{
				continue;
			}

			int num = text.IndexOf('>', i);
			string text2 = text.Substring(i + 1, num - i - 1);
			string text3 = text2;
			if (text2.Contains('='))
			{
				string[] array = text2.Split('=');
				string text4 = array[0].Trim();
				if (!(text4 == "keyword"))
				{
					if (text4 == "card")
					{
						string assetName = array[1].Trim();
						text3 = AddressableLoader.Get<CardData>("CardData", assetName).title;
					}
				}
				else
				{
					KeywordData keywordData = Text.ToKeyword(array[1]);
					text3 = ((!keywordData.iconName.IsNullOrWhitespace()) ? ("><sprite name=" + keywordData.iconName + "><") : keywordData.title);
				}
			}

			text = text.Replace(text.Substring(i, num - i + 1), text3);
			i += text3.Length;
		}

		return ("<" + FirstCharToLowerCase(text) + ">").Replace("<>", "");
	}

	public static string FirstCharToLowerCase(string str)
	{
		if (char.IsUpper(str[0]))
		{
			if (str.Length != 1)
			{
				string text = char.ToLower(str[0]).ToString();
				int length = str.Length;
				int num = 1;
				int length2 = length - num;
				return text + str.Substring(num, length2);
			}

			return str.ToLower();
		}

		return str;
	}
}
