#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class Card : EntityDisplay
{
	public enum PlayType
	{
		None,
		Play,
		Place
	}

	[SerializeField]
	public Vector2 baseSize = new Vector2(740f, 1100f);

	public int frameLevel;

	[Required(null)]
	public Canvas canvas;

	[Required(null)]
	public CanvasGroup canvasGroup;

	[Required(null)]
	[BoxGroup("Text Elements")]
	public TextMeshProUGUI titleText;

	[Required(null)]
	[BoxGroup("Text Elements")]
	public TextMeshProUGUI descText;

	[Required(null)]
	[BoxGroup("Images")]
	public Image mainImage;

	[Required(null)]
	[BoxGroup("Images")]
	public Image backImage;

	[Required(null)]
	[BoxGroup("Images")]
	public Image backgroundImage;

	[Required(null)]
	[BoxGroup("Images")]
	public Image frameImage;

	[Required(null)]
	[BoxGroup("Images")]
	public CardFrameSetter frameSetter;

	[Required(null)]
	[SerializeField]
	public UINavigationItem uINavigationItem;

	[Required(null)]
	[SerializeField]
	public GameObject frontGroup;

	[Required(null)]
	[SerializeField]
	public GameObject backGroup;

	public UpgradeHolder charmHolder;

	public UpgradeHolder tokenHolder;

	public UpgradeHolder crownHolder;

	public ItemHolderPetCreator itemHolderPet;

	public LargeUIScaleUpdater[] scalers;

	[BoxGroup("Idle Animation")]
	public CardIdleAnimation imageIdleAnimator;

	[BoxGroup("Idle Animation")]
	public CardIdleAnimation backgroundIdleAnimator;

	public int currentEffectBonus;

	public float currentEffectFactor;

	public bool currentSilenced;

	public HashSet<KeywordData> keywords = new HashSet<KeywordData>();

	public HashSet<CardData> mentionedCards;

	public bool hasScriptableImage;

	[SerializeField]
	public ScriptableCardImage scriptableImage;

	public void FlipUp()
	{
		backGroup.SetActive(value: false);
		frontGroup.SetActive(value: true);
	}

	public void FlipDown()
	{
		backGroup.SetActive(value: true);
		frontGroup.SetActive(value: false);
	}

	public override Canvas GetCanvas()
	{
		return canvas;
	}

	public override IEnumerator UpdateData(bool doPing = false)
	{
		base.name = entity.data.name;
		Debug.Log("Updating Data for [" + base.name + "]");
		entity.damage.current = entity.data.damage;
		entity.damage.max = entity.data.damage;
		entity.hp.current = entity.data.hp;
		entity.hp.max = entity.data.hp;
		entity.counter.current = entity.data.counter;
		entity.counter.max = entity.data.counter;
		entity.uses.current = entity.data.uses;
		entity.uses.max = entity.data.uses;
		entity.effectBonus = entity.data.effectBonus;
		entity.effectFactor = entity.data.effectFactor;
		backgroundImage.sprite = entity.data.backgroundSprite;
		Vector2 v = Vector2.one;
		if ((bool)entity.data.mainSprite)
		{
			mainImage.sprite = entity.data.mainSprite;
			v = mainImage.sprite.rect.size / baseSize;
		}

		float num = v.Max();
		Transform transform = mainImage.transform;
		transform.localScale = new Vector2(num, num);
		transform.localPosition = new Vector2(0f, v.y - 1f);
		int num2 = ((!entity.owner || entity.owner.team == 1) ? 1 : (-1));
		transform.SetScaleX((float)num2 * transform.localScale.x);
		backgroundImage.transform.SetScaleX((float)num2 * backgroundImage.transform.localScale.x);
		if (hasScriptableImage)
		{
			foreach (Transform item in transform.parent)
			{
				if (item.gameObject != mainImage.gameObject)
				{
					item.gameObject.Destroy();
				}
			}

			if (!entity.data.scriptableImagePrefab)
			{
				mainImage.gameObject.SetActive(value: true);
			}
		}

		if ((bool)entity.data.scriptableImagePrefab)
		{
			hasScriptableImage = true;
			scriptableImage = Object.Instantiate(entity.data.scriptableImagePrefab, mainImage.transform.parent);
			scriptableImage.Assign(entity);
			Transform obj = scriptableImage.transform;
			obj.localScale *= new Vector2(num * (float)num2, num);
			scriptableImage.transform.localPosition += new Vector3(0f, v.y - 1f, 0f);
			mainImage.gameObject.SetActive(value: false);
		}

		SetName(entity.data.title);
		currentEffectBonus = entity.data.effectBonus;
		currentEffectFactor = entity.data.effectFactor;
		currentSilenced = entity.silenced;
		if (!entity.startingEffectsApplied)
		{
			entity.attackEffects = entity.data.attackEffects.Select((CardData.StatusEffectStacks a) => a.Clone()).ToList();
			entity.traits.Clear();
			foreach (CardData.TraitStacks trait in entity.data.traits)
			{
				entity.traits.Add(new Entity.TraitStacks(trait.data, trait.count));
			}

			CardData.StatusEffectStacks[] startWithEffects = entity.data.startWithEffects;
			foreach (CardData.StatusEffectStacks statusEffectStacks in startWithEffects)
			{
				yield return StatusEffectSystem.Apply(entity, null, statusEffectStacks.data, statusEffectStacks.count, temporary: false, onEffectApplied: null, fireEvents: true, applyEvenIfZero: true);
			}

			yield return entity.UpdateTraits();
			if (entity.data.injuries != null)
			{
				foreach (CardData.StatusEffectStacks injury in entity.data.injuries)
				{
					yield return StatusEffectSystem.Apply(entity, null, injury.data, injury.count);
				}
			}

			entity.startingEffectsApplied = true;
		}

		SetDescription();
		if ((bool)entity.data.idleAnimationProfile)
		{
			if ((bool)imageIdleAnimator)
			{
				imageIdleAnimator.entity = entity;
			}

			if ((bool)backgroundIdleAnimator)
			{
				backgroundIdleAnimator.entity = entity;
			}
		}

		charmHolder?.Clear();
		tokenHolder?.Clear();
		crownHolder?.Clear();
		List<CardUpgradeData> upgrades = entity.data.upgrades;
		if (upgrades != null && upgrades.Count > 0)
		{
			foreach (CardUpgradeData upgrade in entity.data.upgrades)
			{
				upgrade.Display(entity);
			}
		}

		yield return base.UpdateData(doPing);
	}

	public override IEnumerator UpdateDisplay(bool doPing = true)
	{
		yield return base.UpdateDisplay(doPing);
		bool flag = entity.effectBonus != currentEffectBonus || entity.effectFactor != currentEffectFactor || entity.silenced != currentSilenced;
		if (flag || promptUpdateDescription)
		{
			SetDescription();
			promptUpdateDescription = false;
			if (flag)
			{
				currentEffectBonus = entity.effectBonus;
				currentEffectFactor = entity.effectFactor;
				currentSilenced = entity.silenced;
				yield return StatusEffectSystem.EffectBonusChangedEvent(entity);
			}
		}

		if (hasScriptableImage)
		{
			scriptableImage.UpdateEvent();
		}
	}

	public void SetName()
	{
		SetName(entity.data.title);
	}

	public void SetName(string name)
	{
		string text = name;
		if (entity.data.injuries.Count > 0)
		{
			text = "<color=red>" + text + "</color>";
		}

		titleText.text = text;
	}

	public void SetDescription()
	{
		string description = GetDescription(entity);
		keywords = Text.GetKeywords(description);
		mentionedCards = Text.GetMentionedCards(description);
		string text = Text.Process(description, entity.effectBonus, entity.effectFactor, entity.data.cardType.descriptionColours);
		AddInjuryText(ref text, entity.data);
		if (text.IsNullOrWhitespace())
		{
			UnityEngine.Localization.LocalizedString flavourKey = entity.data.flavourKey;
			if (flavourKey != null && !flavourKey.IsEmpty)
			{
				string localizedString = entity.data.flavourKey.GetLocalizedString();
				text = "<i><color=#" + entity.data.cardType.descriptionColours.flavourColour + ">" + localizedString;
			}
		}

		if (text.IsNullOrWhitespace())
		{
			descText.text = "";
		}
		else
		{
			descText.text = "<color=#" + entity.data.cardType.descriptionColours.textColour + ">" + text;
		}
	}

	public static string GetDescription(CardData data, bool silenced = false)
	{
		string text = "";
		AddAttackEffectText(ref text, data.attackEffects, silenced);
		AddCustomCardText(ref text, data, silenced);
		AddPassiveEffectText(ref text, data.startWithEffects, silenced);
		AddUpgradeText(ref text, data, silenced);
		AddTraitText(ref text, data, silenced);
		return text;
	}

	public static string GetDescription(Entity entity)
	{
		string text = "";
		AddAttackEffectText(ref text, entity.attackEffects, entity.silenced);
		AddCustomCardText(ref text, entity.data, entity.silenced);
		AddPassiveEffectText(ref text, entity.statusEffects, entity.silenced);
		AddUpgradeText(ref text, entity.data, entity.silenced);
		AddTraitText(ref text, entity);
		return text;
	}

	public static void AddAttackEffectText(ref string text, ICollection<CardData.StatusEffectStacks> attackEffects, bool silenced = false)
	{
		if (attackEffects.Count <= 0)
		{
			return;
		}

		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (CardData.StatusEffectStacks attackEffect in attackEffects)
		{
			string applyFormat = attackEffect.data.GetApplyFormat();
			if (!applyFormat.IsNullOrWhitespace() && !attackEffect.data.keyword.IsNullOrWhitespace())
			{
				if (dictionary.ContainsKey(applyFormat))
				{
					dictionary[applyFormat] += $", <{attackEffect.count}><keyword={attackEffect.data.keyword}>";
				}
				else
				{
					dictionary[applyFormat] = $"<{attackEffect.count}><keyword={attackEffect.data.keyword}>";
				}
			}
			else if (!attackEffect.data.textKey.IsEmpty)
			{
				dictionary.Add(attackEffect.data.GetDesc(attackEffect.count), "");
			}
		}

		foreach (KeyValuePair<string, string> item in dictionary)
		{
			if (!text.IsNullOrWhitespace())
			{
				text += "\n";
			}

			string text2 = item.Key.Replace("{0}", item.Value);
			text += (silenced ? ("<s>" + text2 + "</s>") : text2);
		}
	}

	public static void AddCustomCardText(ref string text, CardData data, bool silenced = false)
	{
		if (data.HasCustomText)
		{
			if (!text.IsNullOrWhitespace())
			{
				text += "\n";
			}

			text += data.GetCustomText(silenced);
		}
	}

	public static void AddPassiveEffectText(ref string text, ICollection<CardData.StatusEffectStacks> passiveEffects, bool silenced = false)
	{
		if (passiveEffects.Count <= 0)
		{
			return;
		}

		foreach (CardData.StatusEffectStacks passiveEffect in passiveEffects)
		{
			if (passiveEffect.data.keyword.IsNullOrWhitespace() && passiveEffect.data.HasDesc)
			{
				if (!text.IsNullOrWhitespace())
				{
					text += "\n";
				}

				text += passiveEffect.data.GetDesc(passiveEffect.count, silenced);
			}
		}
	}

	public static void AddPassiveEffectText(ref string text, ICollection<StatusEffectData> passiveEffects, bool silenced = false)
	{
		if (passiveEffects.Count <= 0)
		{
			return;
		}

		foreach (StatusEffectData item in passiveEffects.OrderBy((StatusEffectData a) => a.textOrder))
		{
			if (item.keyword.IsNullOrWhitespace() && item.HasDesc)
			{
				if (!text.IsNullOrWhitespace())
				{
					text += "\n";
				}

				text += item.GetDesc(item.count, silenced);
			}
		}
	}

	public static void AddUpgradeText(ref string text, CardData data, bool silenced = false)
	{
	}

	public static void AddTraitText(ref string text, CardData data, bool silenced = false)
	{
		if (data.traits == null || data.traits.Count <= 0)
		{
			return;
		}

		int count = data.traits.Count;
		string traitSeparator = GetTraitSeparator(count);
		string text2 = "";
		for (int i = 0; i < count; i++)
		{
			CardData.TraitStacks traitStacks = data.traits[i];
			text2 += GetTraitText(traitStacks.data, traitStacks.count, silenced);
			if (i < count - 1)
			{
				text2 += traitSeparator;
			}
		}

		if (!text2.IsNullOrWhitespace())
		{
			text = text + "\n" + text2;
		}
	}

	public static void AddTraitText(ref string text, Entity entity)
	{
		if (entity.traits == null || entity.traits.Count <= 0)
		{
			return;
		}

		int count = entity.traits.Count;
		string traitSeparator = GetTraitSeparator(count);
		string text2 = "";
		for (int i = 0; i < count; i++)
		{
			Entity.TraitStacks traitStacks = entity.traits[i];
			text2 += GetTraitText(traitStacks.data, traitStacks.count, entity.silenced || traitStacks.silenced);
			if (i < count - 1)
			{
				text2 += traitSeparator;
			}
		}

		if (!text2.IsNullOrWhitespace())
		{
			text = text + "\n" + text2;
		}
	}

	public static string GetTraitSeparator(int traitCount)
	{
		if (traitCount <= 2)
		{
			return "\n";
		}

		return ", ";
	}

	public static string GetTraitText(TraitData trait, int count, bool silenced = false)
	{
		string arg = trait.keyword.name.ToLower();
		if (!silenced)
		{
			return $"<keyword={arg} {count}>";
		}

		return $"<keyword={arg} {count} silenced>";
	}

	public static void AddInjuryText(ref string text, CardData data)
	{
		int count = data.injuries.Count;
		if (count > 0)
		{
			if (!text.IsNullOrWhitespace())
			{
				text += "\n";
			}

			text = text + "<color=red>" + MonoBehaviourSingleton<StringReference>.instance.injured.GetLocalizedString();
			if (count > 1)
			{
				text += $" {count}";
			}

			text += "</color>";
		}
	}

	public override void OnGetFromPool()
	{
		base.OnGetFromPool();
		imageIdleAnimator.OnGetFromPool();
		backgroundIdleAnimator.OnGetFromPool();
		LargeUIScaleUpdater[] array = scalers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].PromptUpdate();
		}
	}

	public override void OnReturnToPool()
	{
		base.OnReturnToPool();
		FlipUp();
		imageIdleAnimator.OnReturnToPool();
		backgroundIdleAnimator.OnReturnToPool();
		if ((bool)crownHolder)
		{
			crownHolder.Clear();
		}

		if ((bool)charmHolder)
		{
			charmHolder.Clear();
		}

		if ((bool)tokenHolder)
		{
			tokenHolder.Clear();
		}

		itemHolderPet.DestroyCurrent();
		canvasGroup.alpha = 1f;
		currentEffectBonus = 0;
		currentEffectFactor = 1f;
		currentSilenced = false;
		canvas.overrideSorting = false;
		if (!hasScriptableImage)
		{
			return;
		}

		foreach (Transform item in mainImage.transform.parent)
		{
			if (item.gameObject != mainImage.gameObject)
			{
				item.gameObject.Destroy();
			}
		}

		mainImage.gameObject.SetActive(value: true);
		hasScriptableImage = false;
	}

	public void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;
		if (!entity.data.TryGetCustomData("splitOriginalId", out var value, 0uL))
		{
			return;
		}

		foreach (Entity card in References.Battle.cards)
		{
			if (card.data.id == value)
			{
				Vector3 position = base.transform.position;
				Vector3 position2 = card.transform.position;
				Gizmos.DrawLine(position, position2);
				Gizmos.DrawCube(position, Vector3.one * 0.5f);
			}
		}
	}
}
