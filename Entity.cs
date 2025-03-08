#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dead;
using NaughtyAttributes;
using UnityEngine;

public class Entity : MonoBehaviourCacheTransform, IPoolable
{
	[Serializable]
	public class TraitStacks
	{
		public TraitData data;

		public int count;

		public int silenceCount;

		public int tempCount;

		public int init;

		public bool effectsDisabled;

		public readonly List<StatusEffectData> passiveEffects = new List<StatusEffectData>();

		public bool silenced => silenceCount > 0;

		public bool ReadyToInit
		{
			get
			{
				if (init < count)
				{
					return !silenced;
				}

				return false;
			}
		}

		public bool MustDisable
		{
			get
			{
				if (init == count && silenced)
				{
					return !effectsDisabled;
				}

				return false;
			}
		}

		public bool MustEnable
		{
			get
			{
				if (init == count && !silenced)
				{
					return effectsDisabled;
				}

				return false;
			}
		}

		public bool StacksRemoved => count < init;

		public TraitStacks(TraitData data, int count, bool temporary = false)
		{
			this.data = data;
			this.count = count;
			if (temporary)
			{
				tempCount = count;
			}
		}

		public IEnumerator DisableEffects()
		{
			foreach (StatusEffectData passiveEffect in passiveEffects)
			{
				yield return passiveEffect.Remove();
			}

			passiveEffects.Clear();
			effectsDisabled = true;
			init = 0;
		}

		public IEnumerator EnableEffects(Entity entity)
		{
			int stacks = count - init;
			yield return AddEffectStacks(entity, stacks);
		}

		public IEnumerator AddEffectStacks(Entity entity, int stacks)
		{
			StatusEffectData[] effects = data.effects;
			foreach (StatusEffectData effectData in effects)
			{
				yield return StatusEffectSystem.Apply(entity, null, effectData, stacks, temporary: true, delegate(StatusEffectData a)
				{
					passiveEffects.Add(a);
				});
			}

			effectsDisabled = false;
			init += stacks;
		}

		public IEnumerator RemoveEffectStacks(Entity entity, int removeStacks)
		{
			StatusEffectData[] effects = data.effects;
			foreach (StatusEffectData dataType in effects)
			{
				StatusEffectData statusEffectData = entity.FindStatus(dataType);
				if ((bool)statusEffectData)
				{
					yield return statusEffectData.RemoveStacks(removeStacks, removeTemporary: true);
				}
			}

			init -= removeStacks;
		}
	}

	public bool inPlay = true;

	public CardData _data;

	public EntityDisplay display;

	public int height = 1;

	public bool paused;

	[HorizontalLine(2f, EColor.Gray)]
	public Wobbler wobbler;

	public Flipper flipper;

	public UINavigationItem uINavigationItem;

	public CurveAnimator curveAnimator;

	public CardIdleAnimation imminentAnimation;

	[Required(null)]
	public Transform offset;

	[HorizontalLine(2f, EColor.Gray)]
	[ReadOnly]
	public bool dragging;

	[ReadOnly]
	public int blockRecall;

	[SerializeField]
	[ReadOnly]
	public List<CardContainer> _containers;

	public CardContainer[] _preContainers;

	public bool alive = true;

	public Character owner;

	public SplatterSurface splatterSurface;

	public bool inCardPool;

	public bool returnToPool = true;

	[HorizontalLine(2f, EColor.Gray)]
	public List<CardData.StatusEffectStacks> attackEffects;

	public List<StatusEffectData> statusEffects;

	public Stat damage;

	public SafeInt tempDamage;

	public Stat hp;

	public Stat counter;

	public Stat uses;

	public int effectBonus;

	public float effectFactor = 1f;

	[ReadOnly]
	public Hit lastHit;

	public bool promptUpdate;

	[ReadOnly]
	public Vector3 random3;

	public DeathType forceKill;

	public TargetMode targetMode;

	public int positionPriority = 1;

	[ReadOnly]
	public bool startingEffectsApplied;

	[HideInInspector]
	public Entity triggeredBy;

	public int cannotBeHitCount;

	public int silenceCount;

	public readonly List<TraitStacks> traits = new List<TraitStacks>();

	public Canvas canvas;

	public int traitUpdateRunning;

	public CardData data
	{
		get
		{
			return _data;
		}
		set
		{
			_data = value;
			if ((bool)_data)
			{
				if (!_data.canBeHit)
				{
					cannotBeHitCount++;
				}

				targetMode = _data.targetMode;
			}
		}
	}

	public CardContainer[] containers => _containers?.Select((CardContainer c) => c.Group).ToArray();

	public List<CardContainer> actualContainers => _containers;

	public CardContainer[] preContainers => _preContainers?.Select((CardContainer c) => c.Group).ToArray();

	public CardContainer[] preActualContainers => _preContainers;

	public bool NeedsTarget
	{
		get
		{
			if (targetMode.NeedsTarget)
			{
				return data.needsTarget;
			}

			return false;
		}
	}

	public bool canBeHit => cannotBeHitCount <= 0;

	public bool silenced => silenceCount > 0;

	public int DrawOrder
	{
		get
		{
			if (!canvas && (bool)display)
			{
				canvas = display.GetCanvas();
			}

			if (!(canvas != null))
			{
				return 0;
			}

			return canvas.sortingOrder;
		}
		set
		{
			if (!canvas && (bool)display)
			{
				canvas = display.GetCanvas();
			}

			if (!canvas)
			{
				return;
			}

			canvas.overrideSorting = !value.Equals(0);
			if (canvas.overrideSorting)
			{
				canvas.sortingOrder = value;
				Canvas componentInParent = GetComponentInParent<Canvas>();
				if ((bool)componentInParent)
				{
					canvas.sortingLayerID = componentInParent.sortingLayerID;
				}
			}
		}
	}

	public bool IsSnowed => SnowAmount() > 0;

	public void AddTo(CardContainer container)
	{
		_containers.Add(container);
	}

	public void RemoveFrom(CardContainer container)
	{
		_containers.Remove(container);
	}

	public List<Entity> GetAllAllies()
	{
		List<Entity> cardsOnBoard = Battle.GetCardsOnBoard(owner);
		cardsOnBoard.Remove(this);
		return cardsOnBoard;
	}

	public List<Entity> GetAllies()
	{
		List<Entity> list = new List<Entity>();
		foreach (CardContainer row in Battle.instance.GetRows(owner))
		{
			for (int i = 0; i < row.Count; i++)
			{
				Entity entity = row[i];
				if (entity != this)
				{
					list.Add(entity);
				}
			}
		}

		return list;
	}

	public List<Entity> GetAlliesInRow()
	{
		List<Entity> list = new List<Entity>();
		CardContainer[] array = containers;
		foreach (CardContainer cardContainer in array)
		{
			for (int j = 0; j < cardContainer.Count; j++)
			{
				Entity entity = cardContainer[j];
				if (entity != this)
				{
					list.Add(entity);
				}
			}
		}

		return list;
	}

	public List<Entity> GetAlliesInRow(int rowIndex)
	{
		List<Entity> list = new List<Entity>();
		foreach (Entity item in References.Battle.GetRow(owner, rowIndex))
		{
			if (item != this)
			{
				list.Add(item);
			}
		}

		return list;
	}

	public List<Entity> GetAllEnemies()
	{
		return Battle.GetCardsOnBoard(Battle.GetOpponent(owner));
	}

	public List<Entity> GetEnemies()
	{
		List<Entity> list = new List<Entity>();
		List<CardContainer> list2 = new List<CardContainer>();
		foreach (KeyValuePair<Character, List<CardContainer>> row in Battle.instance.rows)
		{
			if (row.Key != owner)
			{
				list2.AddRange(Battle.instance.GetRows(row.Key));
			}
		}

		foreach (CardContainer item in list2)
		{
			foreach (Entity item2 in item)
			{
				if (item2 != this && !list.Contains(item2))
				{
					list.Add(item2);
				}
			}
		}

		return list;
	}

	public List<Entity> GetEnemiesInRow(int rowIndex)
	{
		List<Entity> list = new List<Entity>();
		if (containers != null && rowIndex >= 0)
		{
			List<CardContainer> list2 = new List<CardContainer>();
			foreach (KeyValuePair<Character, List<CardContainer>> row in Battle.instance.rows)
			{
				if (row.Key != owner)
				{
					list2.Add(Battle.instance.GetRow(row.Key, rowIndex));
				}
			}

			foreach (CardContainer item in list2)
			{
				foreach (Entity item2 in item)
				{
					list.Add(item2);
				}
			}
		}

		return list;
	}

	public StatusEffectData FindStatus(string type)
	{
		return statusEffects.Find((StatusEffectData a) => a.type == type);
	}

	public StatusEffectData FindStatus(StatusEffectData dataType)
	{
		return statusEffects.Find((StatusEffectData a) => a.name == dataType.name);
	}

	public IEnumerator ClearStatuses()
	{
		for (int i = statusEffects.Count - 1; i >= 0; i--)
		{
			yield return statusEffects[i].Remove();
		}

		statusEffects.Clear();
		startingEffectsApplied = false;
	}

	public int SnowAmount()
	{
		StatusEffectData statusEffectData = FindStatus("snow");
		if (!statusEffectData)
		{
			return 0;
		}

		return statusEffectData.count;
	}

	public Vector3 GetScaleFromContainers()
	{
		if (containers == null || containers.Length == 0)
		{
			return Vector3.one;
		}

		Vector3 zero = Vector3.zero;
		CardContainer[] array = containers;
		foreach (CardContainer cardContainer in array)
		{
			zero += cardContainer.GetChildScale(this);
		}

		return zero / _containers.Count;
	}

	public Vector3 GetPositionFromContainers()
	{
		Vector3 zero = Vector3.zero;
		CardContainer[] array = containers;
		foreach (CardContainer cardContainer in array)
		{
			zero += cardContainer.transform.position + cardContainer.GetChildPosition(this);
		}

		return zero / _containers.Count;
	}

	public void RemoveFromContainers()
	{
		LeanTween.cancel(base.gameObject);
		_preContainers = actualContainers.ToArray();
		CardContainer[] array = containers;
		foreach (CardContainer cardContainer in array)
		{
			if ((bool)cardContainer)
			{
				cardContainer.Remove(this);
			}
		}
	}

	public void ResetDrawOrder()
	{
		int num = 0;
		CardContainer[] array = containers;
		foreach (CardContainer cardContainer in array)
		{
			num = Mathf.Max(num, cardContainer.GetChildDrawOrder(this));
		}

		DrawOrder = num;
	}

	public bool InHand()
	{
		if ((bool)owner && (bool)owner.handContainer && _containers.Count == 1)
		{
			return containers[0] == owner.handContainer;
		}

		return false;
	}

	public bool InContainer(CardContainer container)
	{
		foreach (CardContainer actualContainer in actualContainers)
		{
			if (actualContainer == container)
			{
				return true;
			}
		}

		return false;
	}

	public bool InContainerGroup(CardContainer container)
	{
		CardContainer[] array = containers;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == container)
			{
				return true;
			}
		}

		return false;
	}

	public void Awake()
	{
		random3 = PettyRandom.Vector3();
	}

	public void Update()
	{
		if (promptUpdate && base.enabled)
		{
			promptUpdate = false;
			if ((bool)display)
			{
				CoroutineManager.Start((!display.init) ? display.UpdateData() : display.UpdateDisplay());
			}

			if (alive && ReadyToDie())
			{
				DeathType deathType = ((forceKill == DeathType.None) ? DeathType.Normal : forceKill);
				CoroutineManager.Start(Kill(deathType));
				forceKill = DeathType.None;
			}
		}
	}

	public bool ReadyToDie()
	{
		if (forceKill != 0)
		{
			return true;
		}

		if (hp.current <= 0 && (!data || ((bool)data && data.cardType.canDie)))
		{
			return !statusEffects.Exists((StatusEffectData a) => a.preventDeath);
		}

		return false;
	}

	public IEnumerator Reset()
	{
		yield return ClearStatuses();
		if (display is Card card)
		{
			yield return card.UpdateData();
		}
	}

	[Button(null, EButtonEnableMode.Always)]
	public void PromptUpdate()
	{
		promptUpdate = true;
	}

	public bool CanPlayOn(Entity target, bool ignoreRowCheck = false)
	{
		if (data.playOnSlot || !NeedsTarget || (targetMode.TargetRow && !ignoreRowCheck) || target == this)
		{
			return false;
		}

		if (damage.current + tempDamage.Value > 0 && !target.canBeHit)
		{
			return false;
		}

		if (!targetMode.CanTarget(target))
		{
			return false;
		}

		bool flag = owner.team == target.owner.team;
		if (!data.canPlayOnEnemy && !flag)
		{
			return false;
		}

		if (!data.canPlayOnFriendly && flag)
		{
			return false;
		}

		if (!data.canPlayOnHand && target.containers.Contains(owner.handContainer))
		{
			return false;
		}

		if (!data.canPlayOnBoard && Battle.IsOnBoard(target))
		{
			return false;
		}

		TargetConstraint[] targetConstraints = data.targetConstraints;
		if (targetConstraints != null && targetConstraints.Length > 0 && data.targetConstraints.Any((TargetConstraint c) => !c.Check(target)))
		{
			return false;
		}

		if (damage.max <= 0 && attackEffects.Any((CardData.StatusEffectStacks s) => !s.data.CanPlayOn(target)))
		{
			return false;
		}

		return true;
	}

	public bool CanPlayOn(CardContainer container, bool ignoreRowCheck = false)
	{
		if (!container)
		{
			return false;
		}

		if (container == owner.discardContainer && this.CanRecall())
		{
			return true;
		}

		switch (data.playType)
		{
			case Card.PlayType.Place:
			if (container is CardSlot cardSlot && container.canBePlacedOn && container.owner == owner)
			{
				int num3 = positionPriority;
				if (num3 != -1 && num3 != 2)
				{
					return true;
				}

				if (cardSlot.Group is CardSlotLane cardSlotLane2)
				{
					return cardSlotLane2.slots.IndexOf(cardSlot) == ((positionPriority == -1) ? (cardSlotLane2.slots.Count - 1) : 0);
				}

				return false;
				}
	
				return false;
			case Card.PlayType.Play:
			if (!NeedsTarget || !container.canPlayOn)
			{
				return false;
				}
	
			if (targetMode.TargetRow && !ignoreRowCheck)
			{
				if (container is CardSlotLane && data.canPlayOnBoard)
				{
					if (data.playOnSlot && container.Count >= container.max)
					{
						return false;
					}

					if (!data.playOnSlot)
					{
						Entity[] targets = targetMode.GetTargets(this, null, container);
						if (targets == null || targets.Length <= 0)
						{
							return false;
						}
					}

					if (!(container.owner == owner))
					{
						return data.canPlayOnEnemy;
					}

					return data.canPlayOnFriendly;
				}
				}
			else if (data.playOnSlot && container is CardSlot && data.canPlayOnBoard && container.Group is CardSlotLane cardSlotLane)
			{
				if (!container.Empty)
				{
					if (owner.team != container.owner.team)
					{
						return false;
					}

					int num = cardSlotLane.Count;
					int num2 = cardSlotLane.max;
					if (data.canShoveToOtherRow)
					{
						foreach (CardContainer item in cardSlotLane.shoveTo)
						{
							num += item.Count;
							num2 += item.max;
						}
					}

					if (num >= num2)
					{
						return false;
					}
				}

				if (!(cardSlotLane.owner == owner))
				{
					return data.canPlayOnEnemy;
				}

				return data.canPlayOnFriendly;
				}
	
				return false;
			default:
				return false;
		}
	}

	public IEnumerator Kill(DeathType deathType = DeathType.Normal)
	{
		if (alive)
		{
			alive = false;
			if ((bool)display && (bool)display.hover)
			{
				display.hover.Disable();
			}

			LeanTween.cancel(base.gameObject);
			Routine.Clump clump = new Routine.Clump();
			clump.Add(StatusEffectSystem.EntityDestroyedEvent(this, deathType));
			yield return null;
			RemoveFromContainers();
			base.transform.SetParent(null);
			Events.InvokeEntityKilled(this, deathType);
			yield return clump.WaitForEnd();
		}
	}

	public void OnDisable()
	{
		Events.InvokeEntityDisabled(this);
	}

	public void OnDestroy()
	{
		Events.InvokeEntityDisabled(this);
		Events.InvokeEntityDestroyed(this);
		if (statusEffects.Count <= 0)
		{
			return;
		}

		Debug.Log($"[{this}] Destroyed! Removing [{statusEffects.Count}] status effects...");
		foreach (StatusEffectData item in statusEffects.Where((StatusEffectData status) => status))
		{
			item.Destroy();
		}

		statusEffects.Clear();
	}

	public TraitStacks GainTrait(TraitData traitData, int count, bool temporary = false)
	{
		TraitStacks traitStacks = traits.FirstOrDefault((TraitStacks a) => a.data == traitData);
		if (traitStacks != null)
		{
			traitStacks.count += count;
			if (temporary)
			{
				traitStacks.tempCount += count;
			}

			traits.Remove(traitStacks);
			traits.Add(traitStacks);
			return traitStacks;
		}

		TraitStacks traitStacks2 = new TraitStacks(traitData, count, temporary);
		traits.Add(traitStacks2);
		return traitStacks2;
	}

	public IEnumerator UpdateTraits(TraitStacks moveToFront = null)
	{
		if (traitUpdateRunning > 0)
		{
			yield return new WaitUntil(() => traitUpdateRunning <= 0);
			if (!this.IsAliveAndExists())
			{
				yield break;
			}
		}

		traitUpdateRunning++;
		if (moveToFront != null)
		{
			traits.Remove(moveToFront);
			traits.Insert(0, moveToFront);
		}

		for (int i = traits.Count - 1; i >= 0; i--)
		{
			TraitStacks traitStacks = traits[i];
			if (traitStacks.count <= 0)
			{
				Debug.Log("> [" + base.name + " " + traitStacks.data.name + "] Removed! Removing effects [" + string.Join(", ", traitStacks.passiveEffects) + "]");
				traits.RemoveAt(i);
				yield return traitStacks.DisableEffects();
			}
		}

		foreach (TraitStacks trait in traits)
		{
			trait.silenceCount = 0;
		}

		for (int num = traits.Count - 1; num >= 0; num--)
		{
			TraitStacks traitStacks2 = traits[num];
			TraitData[] overrides = traitStacks2.data.overrides;
			if (overrides != null && overrides.Length > 0)
			{
				for (int num2 = num - 1; num2 >= 0; num2--)
				{
					TraitStacks traitStacks3 = traits[num2];
					if (traitStacks2.data.overrides.Contains(traitStacks3.data))
					{
						traitStacks3.silenceCount++;
					}
				}
			}
		}

		foreach (TraitStacks item in traits.Where((TraitStacks a) => a.MustDisable))
		{
			Debug.Log("> [" + base.name + " " + item.data.name + "] Silenced! Removing effects [" + string.Join(", ", item.passiveEffects) + "]");
			yield return item.DisableEffects();
		}

		foreach (TraitStacks t2 in traits.Where((TraitStacks a) => a.ReadyToInit || a.MustEnable))
		{
			yield return t2.EnableEffects(this);
			Debug.Log("> [" + base.name + " " + t2.data.name + "] Enabled! Adding effects [" + string.Join(", ", t2.passiveEffects) + "]");
		}

		foreach (TraitStacks t2 in traits.Where((TraitStacks a) => a.StacksRemoved))
		{
			int i = t2.init - t2.count;
			yield return t2.RemoveEffectStacks(this, i);
			Debug.Log(string.Format("> [{0} {1}] Removing {2} Stacks of effects [{3}]", base.name, t2.data.name, i, string.Join(", ", t2.passiveEffects)));
		}

		traitUpdateRunning--;
	}

	public void OnGetFromPool()
	{
		inCardPool = false;
		wobbler.OnGetFromPool();
		flipper.OnGetFromPool();
		curveAnimator.OnGetFromPool();
		splatterSurface.OnGetFromPool();
		offset.localScale = Vector3.one;
		offset.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
		_preContainers = null;
	}

	public void OnReturnToPool()
	{
		inCardPool = true;
		Events.InvokeEntityDisabled(this);
		Events.InvokeEntityDestroyed(this);
		if (statusEffects.Count > 0)
		{
			Debug.Log($"[{this}] Destroyed! Removing [{statusEffects.Count}] status effects...");
			foreach (StatusEffectData statusEffect in statusEffects)
			{
				if ((bool)statusEffect)
				{
					statusEffect.Destroy();
				}
			}

			statusEffects.Clear();
		}

		if (GetComponent<IRemoveWhenPooled>() is MonoBehaviour obj)
		{
			UnityEngine.Object.Destroy(obj);
		}

		base.enabled = false;
		dragging = false;
		blockRecall = 0;
		alive = true;
		attackEffects.Clear();
		statusEffects.Clear();
		traits.Clear();
		effectBonus = 0;
		effectFactor = 1f;
		tempDamage.Value = 0;
		forceKill = DeathType.None;
		startingEffectsApplied = false;
		cannotBeHitCount = 0;
		silenceCount = 0;
		_containers.Clear();
		lastHit = null;
		wobbler.OnReturnToPool();
		flipper.OnReturnToPool();
		curveAnimator.OnReturnToPool();
		splatterSurface.OnReturnToPool();
		positionPriority = 1;
		promptUpdate = false;
		triggeredBy = null;
		uINavigationItem.enabled = true;
		LeanTween.cancel(base.gameObject);
	}
}
