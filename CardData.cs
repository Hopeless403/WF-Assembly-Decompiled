#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CardData", menuName = "Card Data")]
public class CardData : DataFile, ISaveable<CardSaveData>
{
	public enum PlayPosition
	{
		None,
		Friendly,
		Enemy,
		Hand,
		FriendlyRow,
		EnemyRow,
		Field,
		FriendlySlot,
		EnemySlot
	}

	[Serializable]
	public class StatusEffectStacks : ISaveable<StatusEffectSaveData>
	{
		public StatusEffectData data;

		public int count;

		public StatusEffectStacks()
		{
		}

		public StatusEffectStacks(StatusEffectData data, int count)
		{
			this.data = data;
			this.count = count;
		}

		public StatusEffectSaveData Save()
		{
			return new StatusEffectSaveData
			{
				name = data.name,
				count = count
			};
		}

		public static StatusEffectStacks[] Stack(IEnumerable<StatusEffectStacks> currentEffects, IEnumerable<StatusEffectStacks> newEffects)
		{
			List<StatusEffectStacks> list = new List<StatusEffectStacks>(currentEffects);
			foreach (StatusEffectStacks e in newEffects)
			{
				StatusEffectStacks statusEffectStacks = list.FirstOrDefault((StatusEffectStacks a) => a.data == e.data);
				if (statusEffectStacks != null)
				{
					statusEffectStacks.count += e.count;
				}
				else
				{
					list.Add(new StatusEffectStacks(e.data, e.count));
				}
			}

			return list.ToArray();
		}

		public override string ToString()
		{
			return $"{count} {data.name}";
		}

		public StatusEffectStacks Clone()
		{
			return new StatusEffectStacks(data, count);
		}
	}

	[Serializable]
	public class TraitStacks : ISaveable<TraitSaveData>
	{
		public TraitData data;

		public int count;

		public TraitStacks(TraitData data, int count)
		{
			this.data = data;
			this.count = count;
		}

		public TraitStacks()
		{
		}

		public TraitSaveData Save()
		{
			return new TraitSaveData
			{
				name = data.name,
				count = count
			};
		}

		public static void Stack(ref List<TraitStacks> traits, IEnumerable<TraitStacks> newTraits)
		{
			foreach (TraitStacks newTrait in newTraits)
			{
				bool flag = false;
				foreach (TraitStacks trait in traits)
				{
					if (trait.data.Equals(newTrait.data))
					{
						trait.count += newTrait.count;
						flag = true;
						break;
					}
				}

				if (!flag)
				{
					traits.Add(new TraitStacks
					{
						data = newTrait.data,
						count = newTrait.count
					});
				}
			}
		}

		public override string ToString()
		{
			return $"{data.name} {count}";
		}
	}

	public static ulong idCurrent;

	[FormerlySerializedAs("title")]
	public string titleFallback;

	public string forceTitle;

	[TextArea]
	public string desc;

	public UnityEngine.Localization.LocalizedString titleKey;

	public UnityEngine.Localization.LocalizedString textKey;

	[SerializeField]
	public string textInsert;

	public string flavour;

	public UnityEngine.Localization.LocalizedString flavourKey;

	public int value;

	public CardAnimationProfile idleAnimationProfile;

	public string[] greetMessages;

	public BloodProfile bloodProfile;

	[Header("Stats")]
	public bool hasAttack;

	public int damage;

	public bool hasHealth;

	public int hp;

	public int counter;

	[Header("Attacking")]
	public bool canBeHit;

	[Required(null)]
	public TargetMode targetMode;

	public StatusEffectStacks[] attackEffects;

	[Header("Assets")]
	[ShowAssetPreview(64, 64)]
	public Sprite mainSprite;

	[ShowAssetPreview(64, 64)]
	public Sprite backgroundSprite;

	[Header("Details")]
	public CardType cardType;

	[ShowIf("IsClunker")]
	public bool isEnemyClunker;

	public Card.PlayType playType;

	public bool needsTarget = true;

	public bool canPlayOnBoard = true;

	public bool canPlayOnHand = true;

	public bool canPlayOnFriendly = true;

	public bool canPlayOnEnemy = true;

	public bool playOnSlot;

	[ShowIf("DoesShove")]
	public bool canShoveToOtherRow = true;

	public PlayPosition defaultPlayPosition;

	public int uses;

	public StatusEffectStacks[] startWithEffects;

	public List<TraitStacks> traits;

	public TargetConstraint[] targetConstraints;

	[Space]
	[ReadOnly]
	public int effectBonus;

	[ReadOnly]
	public float effectFactor = 1f;

	public List<StatusEffectStacks> injuries;

	[ReadOnly]
	public List<CardUpgradeData> upgrades;

	[ReadOnly]
	public Vector3 random3;

	[ReadOnly]
	public int charmSlots = 3;

	[ReadOnly]
	public int tokenSlots = 1;

	[ReadOnly]
	public int crownSlots = 1;

	public ScriptableCardImage scriptableImagePrefab;

	[SerializeField]
	public CardScript[] createScripts;

	public Dictionary<string, object> customData;

	public ulong id { get; set; }

	public string title
	{
		get
		{
			if (!forceTitle.IsNullOrWhitespace())
			{
				return forceTitle;
			}

			UnityEngine.Localization.LocalizedString localizedString = titleKey;
			if (localizedString == null || localizedString.IsEmpty)
			{
				return titleFallback;
			}

			return titleKey.GetLocalizedString();
		}
	}

	public bool HasCustomText => !textKey.IsEmpty;

	public bool IsClunker => cardType.name == "Clunker";

	public bool IsItem => playType == Card.PlayType.Play;

	public bool DoesShove
	{
		get
		{
			if (playType != Card.PlayType.Place)
			{
				if (playType == Card.PlayType.Play)
				{
					return playOnSlot;
				}

				return false;
			}

			return true;
		}
	}

	public CardData original { get; set; }

	public bool HasCrown => GetCrown() != null;

	public void SetId(ulong value)
	{
		id = value;
		if (id > idCurrent)
		{
			idCurrent = id;
		}
	}

	public string GetCustomText(bool silenced = false)
	{
		int amount = ((attackEffects.Length != 0) ? attackEffects[0].count : ((startWithEffects.Length != 0) ? startWithEffects[0].count : 0));
		string text = Text.GetEffectText(textKey, textInsert, amount, silenced);
		for (int num = attackEffects.Length - 1; num >= 0; num--)
		{
			string oldValue = "{a" + num + "}";
			if (text.Contains(oldValue))
			{
				text = text.Replace(oldValue, attackEffects[num].count.ToString());
			}
		}

		for (int num2 = startWithEffects.Length - 1; num2 >= 0; num2--)
		{
			string oldValue2 = "{s" + num2 + "}";
			if (text.Contains(oldValue2))
			{
				text = text.Replace(oldValue2, startWithEffects[num2].count.ToString());
			}
		}

		return text;
	}

	public CardUpgradeData GetCrown()
	{
		return upgrades.Find((CardUpgradeData a) => a.type == CardUpgradeData.Type.Crown);
	}

	public void RemoveCrown()
	{
		GetCrown()?.UnAssign(this);
	}

	public CardData Clone(bool runCreateScripts = true)
	{
		Vector3 normalized = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
		return Clone(normalized, idCurrent + 1, runCreateScripts);
	}

	public CardData Clone(Vector3 random3, bool runCreateScripts = true)
	{
		return Clone(random3, idCurrent + 1, runCreateScripts);
	}

	public CardData Clone(Vector3 random3, ulong id, bool runCreateScripts = true)
	{
		CardData cardData = this.InstantiateKeepName();
		cardData.original = this;
		cardData.SetId(id);
		cardData.random3 = random3;
		cardData.customData = customData?.ToDictionary((KeyValuePair<string, object> entry) => entry.Key, (KeyValuePair<string, object> entry) => entry.Value);
		if (runCreateScripts)
		{
			cardData.RunCreateScripts();
			Events.InvokeCardDataCreated(cardData);
		}

		return cardData;
	}

	public void RunCreateScripts()
	{
		if (createScripts != null && createScripts.Length != 0)
		{
			UnityEngine.Random.State state = UnityEngine.Random.state;
			int num = Mathf.Abs(Mathf.RoundToInt(random3.x * 100000f));
			for (int i = 0; i < createScripts.Length; i++)
			{
				UnityEngine.Random.InitState(num + i);
				createScripts[i].Run(this);
			}

			UnityEngine.Random.state = state;
		}
	}

	public void SetCustomData(string key, object value)
	{
		if (customData == null)
		{
			customData = new Dictionary<string, object>();
		}

		customData[key] = value;
	}

	public bool TryGetCustomData<T>(string key, out T value, T defaultValue)
	{
		if (customData != null && customData.TryGetValue(key, out var obj) && obj is T val)
		{
			value = val;
			return true;
		}

		value = defaultValue;
		return false;
	}

	public bool IsOffensive()
	{
		bool flag = damage > 0 || ((bool)original && original.damage > 0);
		if (!flag && attackEffects.Any((StatusEffectStacks s) => s.data.offensive))
		{
			flag = true;
		}

		if (!flag && startWithEffects.Any((StatusEffectStacks s) => s.data.makesOffensive))
		{
			flag = true;
		}

		return flag;
	}

	public CardSaveData Save()
	{
		return new CardSaveData(this);
	}

	public override int GetHashCode()
	{
		return base.name.GetHashCode() ^ random3.GetHashCode() ^ id.GetHashCode();
	}
}
