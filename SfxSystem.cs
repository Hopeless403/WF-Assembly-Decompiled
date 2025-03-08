#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class SfxSystem : GameSystem
{
	public readonly struct Param
	{
		public readonly string name;

		public readonly float value;

		public Param(string name, float value)
		{
			this.name = name;
			this.value = value;
		}
	}

	public class Cooldown
	{
		public string eventName;

		public float current;

		public readonly float max;

		public Cooldown(string eventName, float value)
		{
			this.eventName = eventName;
			current = 0f;
			max = value;
		}

		public void Max()
		{
			current = max;
		}
	}

	public static SfxSystem instance;

	public const float DRAG_THRESHOLD = 0.35f;

	public static readonly Dictionary<string, float> cooldownTimers = new Dictionary<string, float>
	{
		{ "event:/sfx/card/hover", 0.1f },
		{ "event:/sfx/card/flip_single", 0.075f },
		{ "event:/sfx/card/enter_pocket", 0.1f },
		{ "event:/sfx/status/block", 0.05f },
		{ "event:/sfx/status/demonize", 0.05f },
		{ "event:/sfx/status/heal", 0.05f },
		{ "event:/sfx/status/power", 0.05f },
		{ "event:/sfx/status/shell", 0.05f },
		{ "event:/sfx/status/shroom", 0.05f },
		{ "event:/sfx/status/snow", 0.05f },
		{ "event:/sfx/status/spice", 0.05f },
		{ "event:/sfx/status/sun", 0.05f },
		{ "event:/sfx/status/teeth", 0.05f },
		{ "event:/sfx/status/overburn", 0.05f },
		{ "event:/sfx/status/scrap", 0.05f },
		{ "event:/sfx/status/frenzy", 0.05f },
		{ "event:/sfx/status/frost", 0.05f },
		{ "event:/sfx/status/haze", 0.05f },
		{ "event:/sfx/status/ink", 0.05f },
		{ "event:/sfx/status/bom", 0.05f },
		{ "event:/sfx/location/shop/crown_hover", 0.05f },
		{ "event:/sfx/location/shop/charm_hover", 0.05f },
		{ "event:/sfx/card/ping", 0.05f }
	};

	public static readonly Dictionary<string, Cooldown> cooldowns = new Dictionary<string, Cooldown>();

	[SerializeField]
	public AnimationCurve pathRevealPitch;

	[SerializeField]
	public EventReference test;

	public List<EventInstance> running;

	public Entity dragging;

	public Entity draggingItem;

	public bool dragTrigger;

	public Vector2 dragFrom;

	public EventInstance itemAim;

	public int revealActionsInQueue;

	public EventInstance flipMulti;

	public EventInstance transitionSnow;

	public EventInstance goldCounter;

	public EventInstance muncherFeed;

	public EventInstance drawMulti;

	public EventInstance townProgressionLoop;

	public GoldDisplay goldDisplay;

	public void OnEnable()
	{
		instance = this;
		Events.OnEntityHit += EntityHit;
		Events.OnEntityHover += EntityHover;
		Events.OnEntityKilled += EntityKilled;
		Events.OnEntitySelect += EntitySelect;
		Events.OnEntityDrag += EntityDrag;
		Events.OnEntityRelease += EntityRelease;
		Events.OnEntityPlace += EntityPlace;
		Events.OnEntityFlipUp += EntityFlipUp;
		Events.OnEntityFlipDown += EntityFlipDown;
		Events.OnEntityTrigger += EntityTrigger;
		Events.OnEntityMove += EntityMove;
		Events.OnEntityFocus += Focus;
		Events.OnEntityEnterPocket += EntityEnterPocket;
		Events.OnEntityEnterBackpack += EntityEnterBackpack;
		Events.OnBattlePhaseStart += BattlePhaseStart;
		Events.OnStatusEffectApplied += StatusApplied;
		Events.OnCardDraw += CardDraw;
		Events.OnCardDrawEnd += CardDrawEnd;
		Events.OnStatusIconChanged += StatusIconChanged;
		Events.OnDropGold += DropGold;
		Events.OnGoldFlyToBag += GoldFlyToBag;
		Events.OnCollectGold += CollectGold;
		Events.OnDeckpackOpen += DeckpackOpen;
		Events.OnDeckpackClose += DeckpackClose;
		Events.OnMapPathReveal += MapPathReveal;
		Events.OnMapNodeReveal += MapNodeReveal;
		Events.OnMapNodeHover += MapNodeHover;
		Events.OnMapNodeSelect += MapNodeSelect;
		Events.OnActionQueued += ActionQueued;
		Events.OnActionPerform += ActionPerform;
		Events.OnActionFinished += ActionFinished;
		Events.OnTransitionStart += TransitionStart;
		Events.OnTransitionEnd += TransitionEnd;
		Events.OnGoldCounterStart += GoldCounterStart;
		Events.OnMuncherDrag += MuncherDrag;
		Events.OnMuncherDragCancel += MuncherDragEnd;
		Events.OnMuncherFeed += MuncherFeed;
		Events.OnBombardShoot += BombardShoot;
		Events.OnBombardRocketFall += BombardRocketFall;
		Events.OnBombardRocketExplode += BombardRocketExplode;
		Events.OnButtonHover += ButtonHover;
		Events.OnButtonPress += ButtonPress;
		Events.OnProgressStart += ProgressStart;
		Events.OnProgressUpdate += ProgressUpdate;
		Events.OnProgressStop += ProgressStop;
		Events.OnProgressDing += ProgressDing;
		Events.OnProgressBlip += ProgressBlip;
		Events.OnTownUnlock += TownUnlockPopUp;
		Events.OnUpgradeHover += UpgradeHover;
		Events.OnUpgradePickup += UpgradePickup;
		Events.OnUpgradeDrop += UpgradeDrop;
		Events.OnUpgradeAssign += UpgradeAssign;
		Events.OnShopItemHover += ShopItemHover;
		Events.OnAbilityTargetAdd += AbilityTargetAdd;
		Events.OnEntityPing += EntityPing;
		running = new List<EventInstance>();
		cooldowns.Clear();
		foreach (KeyValuePair<string, float> cooldownTimer in cooldownTimers)
		{
			cooldowns[cooldownTimer.Key] = new Cooldown(cooldownTimer.Key, cooldownTimer.Value);
		}
	}

	public void OnDisable()
	{
		Events.OnEntityHit -= EntityHit;
		Events.OnEntityHover -= EntityHover;
		Events.OnEntityKilled -= EntityKilled;
		Events.OnEntitySelect -= EntitySelect;
		Events.OnEntityDrag -= EntityDrag;
		Events.OnEntityRelease -= EntityRelease;
		Events.OnEntityPlace -= EntityPlace;
		Events.OnEntityFlipUp -= EntityFlipUp;
		Events.OnEntityFlipDown -= EntityFlipDown;
		Events.OnEntityTrigger -= EntityTrigger;
		Events.OnEntityMove -= EntityMove;
		Events.OnEntityFocus -= Focus;
		Events.OnEntityEnterPocket -= EntityEnterPocket;
		Events.OnEntityEnterBackpack -= EntityEnterBackpack;
		Events.OnBattlePhaseStart -= BattlePhaseStart;
		Events.OnStatusEffectApplied -= StatusApplied;
		Events.OnCardDraw -= CardDraw;
		Events.OnCardDrawEnd -= CardDrawEnd;
		Events.OnStatusIconChanged -= StatusIconChanged;
		Events.OnDropGold -= DropGold;
		Events.OnGoldFlyToBag -= GoldFlyToBag;
		Events.OnCollectGold -= CollectGold;
		Events.OnDeckpackOpen -= DeckpackOpen;
		Events.OnDeckpackClose -= DeckpackClose;
		Events.OnMapPathReveal -= MapPathReveal;
		Events.OnMapNodeReveal -= MapNodeReveal;
		Events.OnMapNodeHover -= MapNodeHover;
		Events.OnMapNodeSelect -= MapNodeSelect;
		Events.OnActionQueued -= ActionQueued;
		Events.OnActionPerform -= ActionPerform;
		Events.OnActionFinished -= ActionFinished;
		Events.OnTransitionStart -= TransitionStart;
		Events.OnTransitionEnd -= TransitionEnd;
		Events.OnGoldCounterStart -= GoldCounterStart;
		Events.OnMuncherDrag -= MuncherDrag;
		Events.OnMuncherDragCancel -= MuncherDragEnd;
		Events.OnMuncherFeed -= MuncherFeed;
		Events.OnBombardShoot -= BombardShoot;
		Events.OnBombardRocketFall -= BombardRocketFall;
		Events.OnBombardRocketExplode -= BombardRocketExplode;
		Events.OnButtonHover -= ButtonHover;
		Events.OnButtonPress -= ButtonPress;
		Events.OnProgressStart -= ProgressStart;
		Events.OnProgressUpdate -= ProgressUpdate;
		Events.OnProgressStop -= ProgressStop;
		Events.OnProgressDing -= ProgressDing;
		Events.OnProgressBlip -= ProgressBlip;
		Events.OnTownUnlock -= TownUnlockPopUp;
		Events.OnUpgradeHover -= UpgradeHover;
		Events.OnUpgradePickup -= UpgradePickup;
		Events.OnUpgradeDrop -= UpgradeDrop;
		Events.OnUpgradeAssign -= UpgradeAssign;
		Events.OnShopItemHover -= ShopItemHover;
		Events.OnAbilityTargetAdd -= AbilityTargetAdd;
		Events.OnEntityPing -= EntityPing;
		StopAll();
		running = null;
	}

	public void Update()
	{
		for (int num = running.Count - 1; num >= 0; num--)
		{
			EventInstance eventInstance = running[num];
			if (!IsRunning(eventInstance))
			{
				eventInstance.release();
				running.RemoveAt(num);
			}
		}

		foreach (KeyValuePair<string, Cooldown> cooldown in cooldowns)
		{
			if (cooldown.Value.current > 0f)
			{
				cooldown.Value.current -= Time.deltaTime;
			}
		}

		if (dragTrigger)
		{
			if (!dragging)
			{
				dragTrigger = false;
			}
			else if (((Vector2)dragging.transform.position - dragFrom).sqrMagnitude > 0.35f)
			{
				dragTrigger = false;
				OneShot("event:/sfx/card/drag");
			}
		}

		if (IsRunning(goldCounter) && (!goldDisplay || goldDisplay.add == 0f))
		{
			Stop(goldCounter);
			goldDisplay = null;
		}
	}

	public static EventInstance OneShot(EventReference eventRef)
	{
		return OneShot(eventRef.Guid);
	}

	public static EventInstance OneShot(GUID guid)
	{
		EventInstance result = RuntimeManager.CreateInstance(guid);
		result.start();
		return result;
	}

	public static void OneShot(string eventName)
	{
		RuntimeManager.PlayOneShot(eventName);
	}

	public static void OneShotCheckCooldown(string eventName, bool resetCooldown = true)
	{
		if (CheckCooldown(eventName))
		{
			OneShot(eventName);
			if (resetCooldown)
			{
				SetCooldown(eventName);
			}
		}
	}

	public static void OneShot(string eventName, params Param[] parameters)
	{
		EventInstance eventInstance = RuntimeManager.CreateInstance(eventName);
		for (int i = 0; i < parameters.Length; i++)
		{
			Param param = parameters[i];
			eventInstance.setParameterByName(param.name, param.value);
		}

		eventInstance.start();
		eventInstance.release();
	}

	public EventInstance Play(string eventPath, params Param[] parameters)
	{
		try
		{
			EventInstance eventInstance = RuntimeManager.CreateInstance(eventPath);
			for (int i = 0; i < parameters.Length; i++)
			{
				Param param = parameters[i];
				eventInstance.setParameterByName(param.name, param.value);
			}

			running.Add(eventInstance);
			eventInstance.start();
			return eventInstance;
		}
		catch (EventNotFoundException message)
		{
			UnityEngine.Debug.LogWarning(message);
			return default(EventInstance);
		}
	}

	public EventInstance Play(EventReference eventRef, params Param[] parameters)
	{
		try
		{
			EventInstance eventInstance = RuntimeManager.CreateInstance(eventRef);
			for (int i = 0; i < parameters.Length; i++)
			{
				Param param = parameters[i];
				eventInstance.setParameterByName(param.name, param.value);
			}

			running.Add(eventInstance);
			eventInstance.start();
			return eventInstance;
		}
		catch (EventNotFoundException message)
		{
			UnityEngine.Debug.LogWarning(message);
			return default(EventInstance);
		}
	}

	public static void SetParam(EventInstance eventInstance, string param, float value)
	{
		if (IsRunning(eventInstance))
		{
			eventInstance.setParameterByName(param, value);
		}
	}

	public static void Stop(EventInstance eventInstance, FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
	{
		if (IsRunning(eventInstance))
		{
			eventInstance.stop(stopMode);
			eventInstance.release();
		}
	}

	public static bool IsRunning(EventInstance eventInstance)
	{
		if (eventInstance.isValid())
		{
			eventInstance.getPlaybackState(out var state);
			if (state != PLAYBACK_STATE.STOPPED)
			{
				return true;
			}
		}

		return false;
	}

	public static void SetCooldown(string eventName)
	{
		if (cooldowns.ContainsKey(eventName))
		{
			cooldowns[eventName].Max();
		}
	}

	public static bool CheckCooldown(string eventName)
	{
		if (cooldowns.ContainsKey(eventName))
		{
			return cooldowns[eventName].current <= 0f;
		}

		return true;
	}

	public static void SetGlobalParam(string paramName, float value)
	{
		RuntimeManager.StudioSystem.setParameterByName(paramName, value);
	}

	public static EventInstance Loop(string eventName)
	{
		return instance.Play(eventName);
	}

	public static EventInstance Loop(EventReference eventRef)
	{
		return instance.Play(eventRef);
	}

	public static void EndLoop(EventInstance? inst)
	{
		if (inst.HasValue)
		{
			EventInstance valueOrDefault = inst.GetValueOrDefault();
			if (IsRunning(valueOrDefault))
			{
				Stop(valueOrDefault);
			}
		}
	}

	public static int GetHitPower(Hit hit)
	{
		return hit.damage + hit.damageBlocked + hit.extraOffensiveness;
	}

	public static void EntityHit(Hit hit)
	{
		if (!hit.Offensive || !hit.doAnimation || !hit.countsAsHit || !hit.target)
		{
			return;
		}

		int hitPower = GetHitPower(hit);
		if (hitPower >= 0)
		{
			switch (hit.damageType)
			{
				case "basic":
					OneShot("event:/sfx/attack/hit_level", new Param("power", Mathf.Max(1, hitPower)));
					break;
				case "shroom":
					OneShot("event:/sfx/status/shroom_damage");
					break;
				case "spikes":
					OneShot("event:/sfx/status/teeth_damage");
					break;
				case "overload":
					OneShot("event:/sfx/status/overburn_damage");
					break;
			}
		}
	}

	public static void EntityHover(Entity entity)
	{
		OneShotCheckCooldown("event:/sfx/card/hover", resetCooldown: false);
	}

	public static void EntityKilled(Entity entity, DeathType deathType)
	{
		if (deathType == DeathType.Normal || deathType == DeathType.Eaten)
		{
			OneShot("event:/sfx/card/destroy");
		}

		if ((bool)entity && (bool)entity.data)
		{
			CardType cardType = entity.data.cardType;
			if ((object)cardType != null && cardType.miniboss)
			{
				OneShot("event:/sfx/card/destroy_boss");
			}
		}
	}

	public static void EntitySelect(Entity entity)
	{
		OneShot("event:/sfx/card/click");
	}

	public void EntityDrag(Entity entity)
	{
		dragging = entity;
		if (!entity.inPlay)
		{
			dragTrigger = true;
			dragFrom = entity.transform.position;
			return;
		}

		if (entity.data.cardType.item)
		{
			string eventPath = (entity.IsOffensive() ? "event:/sfx/attack/item_aim_offensive" : "event:/sfx/attack/item_aim_supportive");
			itemAim = Play(eventPath);
			draggingItem = entity;
		}

		if (entity.data.playType == Card.PlayType.Place)
		{
			dragTrigger = true;
			dragFrom = entity.transform.position;
		}
	}

	public void EntityRelease(Entity entity)
	{
		if (dragging == entity)
		{
			dragging = null;
			SetCooldown("event:/sfx/card/hover");
		}

		if (draggingItem == entity && IsRunning(itemAim))
		{
			SetParam(itemAim, "stop", 1f);
			draggingItem = null;
		}
	}

	public static void EntityPlace(Entity entity, CardContainer[] containers, bool freeMove)
	{
		OneShot("event:/sfx/card/place");
	}

	public static void EntityFlipUp(Entity entity)
	{
		OneShotCheckCooldown("event:/sfx/card/flip_single");
	}

	public static void EntityFlipDown(Entity entity)
	{
		OneShotCheckCooldown("event:/sfx/card/flip_single");
	}

	public static void EntityTrigger(ref Trigger trigger)
	{
		CardType cardType = trigger.entity.data.cardType;
		if ((object)cardType != null && cardType.item && trigger.triggeredBy == trigger.entity.owner.entity)
		{
			OneShot("event:/sfx/attack/item_use");
		}
	}

	public static void EntityMove(Entity entity)
	{
		Character enemy = Battle.instance.enemy;
		if ((object)enemy != null && entity.owner == enemy && entity.preContainers.Contains(enemy.reserveContainer))
		{
			OneShot("event:/sfx/card/enemy_showup");
		}
	}

	public static void Focus(Entity entity)
	{
		OneShot("event:/sfx/card/drag");
	}

	public static void EntityEnterPocket(Entity entity, CardPocket pocket)
	{
		OneShotCheckCooldown("event:/sfx/card/enter_pocket");
	}

	public static void EntityEnterBackpack(Entity entity)
	{
		OneShotCheckCooldown("event:/sfx/card/enter_backpack");
	}

	public static void BattlePhaseStart(Battle.Phase phase)
	{
		if (phase == Battle.Phase.Battle)
		{
			OneShot("event:/sfx/inventory/battle_zoom_in");
		}
		else if (Battle.instance.phase == Battle.Phase.Battle)
		{
			OneShot("event:/sfx/inventory/battle_zoom_out");
		}
	}

	public static void StatusApplied(StatusEffectApply apply)
	{
		if ((bool)apply?.effectData && apply.target.display.init && apply.target.startingEffectsApplied && !Transition.Running)
		{
			switch (apply.effectData.type)
			{
				case "snow":
					OneShotCheckCooldown("event:/sfx/status/snow");
					break;
				case "shroom":
					OneShotCheckCooldown("event:/sfx/status/shroom");
					break;
				case "shell":
					OneShotCheckCooldown("event:/sfx/status/shell");
					break;
				case "spice":
					OneShotCheckCooldown("event:/sfx/status/spice");
					break;
				case "demonize":
					OneShotCheckCooldown("event:/sfx/status/demonize");
					break;
				case "block":
					OneShotCheckCooldown("event:/sfx/status/block");
					break;
				case "frost":
					OneShotCheckCooldown("event:/sfx/status/frost");
					break;
				case "teeth":
					OneShotCheckCooldown("event:/sfx/status/teeth");
					break;
				case "overload":
					OneShotCheckCooldown("event:/sfx/status/overburn");
					break;
				case "scrap":
					OneShotCheckCooldown("event:/sfx/status/scrap");
					break;
				case "frenzy":
					OneShotCheckCooldown("event:/sfx/status/frenzy");
					break;
				case "haze":
					OneShotCheckCooldown("event:/sfx/status/haze");
					break;
				case "ink":
					OneShotCheckCooldown("event:/sfx/status/ink");
					break;
				case "vim":
					OneShotCheckCooldown("event:/sfx/status/bom");
					break;
				case "heal":
					OneShotCheckCooldown("event:/sfx/status/heal");
					break;
				case "max health up":
					OneShotCheckCooldown("event:/sfx/status/heal");
					break;
				case "damage up":
					OneShotCheckCooldown("event:/sfx/status/power");
					break;
				case "counter down":
					OneShotCheckCooldown("event:/sfx/status/sun");
					break;
				case "max counter down":
					OneShotCheckCooldown("event:/sfx/status/sun");
					break;
			}
		}
	}

	public void CardDraw(int amount)
	{
		if (IsRunning(drawMulti))
		{
			Stop(drawMulti);
		}

		drawMulti = Play("event:/sfx/card/draw_multi");
	}

	public void CardDrawEnd()
	{
		if (IsRunning(drawMulti))
		{
			SetParam(drawMulti, "draw_stop", 1f);
		}
	}

	public static void StatusIconChanged(StatusIcon icon, Stat previousValue, Stat newValue)
	{
		if (Transition.Running)
		{
			return;
		}

		switch (icon.type)
		{
			case "counter":
			if (newValue.current < previousValue.current)
			{
				OneShot("event:/sfx/status_icon/counter_decrease");
				}
			else if (newValue.current > previousValue.current)
			{
				OneShot("event:/sfx/status_icon/counter_increase");
				}
	
				break;
			case "snow":
			if (newValue.current < previousValue.current)
			{
				OneShot("event:/sfx/status_icon/snow_decrease");
				}
	
				break;
			case "scrap":
			if (newValue.current < previousValue.current)
			{
				OneShot("event:/sfx/status_icon/scrap_decrease");
				}
	
				break;
			case "shell":
			if (newValue.current < previousValue.current)
			{
				OneShot("event:/sfx/status_icon/shell_decrease");
				}
	
				break;
		}
	}

	public static void DropGold(int amount, string source, Character owner, Vector3 position)
	{
		if (!(source == "Flee"))
		{
			int num = ((SceneManager.ActiveSceneName == "Battle") ? 1 : 0);
			OneShot("event:/sfx/inventory/bling_dropping", new Param("battle", num));
		}
	}

	public static void GoldFlyToBag(int amount, Character owner, Vector3 position)
	{
		OneShot("event:/sfx/inventory/bling_flying");
	}

	public static void CollectGold(int amount)
	{
		OneShot("event:/sfx/inventory/bling_collect");
	}

	public static void DeckpackOpen()
	{
		OneShot("event:/sfx/inventory/backpack_opening");
	}

	public static void DeckpackClose()
	{
		OneShot("event:/sfx/inventory/backpack_closing");
	}

	public void MapPathReveal(float totalTime)
	{
		EventInstance mapPathReveal = Play("event:/sfx/map/path_showup");
		UnityEngine.Debug.Log("> Playing path_showup");
		StartCoroutine(MapPathRevealRoutine(mapPathReveal, totalTime));
	}

	public IEnumerator MapPathRevealRoutine(EventInstance mapPathReveal, float totalTime)
	{
		float time = 0f;
		while (time < totalTime && IsRunning(mapPathReveal))
		{
			time += Time.deltaTime;
			float time2 = time / totalTime;
			float pitch = pathRevealPitch.Evaluate(time2);
			mapPathReveal.setPitch(pitch);
			if (time >= totalTime)
			{
				Stop(mapPathReveal);
				break;
			}

			yield return new WaitForFixedUpdate();
		}
	}

	public static void MapNodeReveal(MapNode node)
	{
		OneShot("event:/sfx/map/location_showup");
	}

	public static void MapNodeHover(MapNode node)
	{
		OneShot("event:/sfx/map/location_hover");
	}

	public static void MapNodeSelect(MapNode node)
	{
		if (node != null)
		{
			OneShot("event:/sfx/map/location_select");
			if (node.campaignNode.type.isBattle)
			{
				OneShot(node.campaignNode.type.isBoss ? "event:/sfx/map/location_select_battle_boss" : "event:/sfx/map/location_select_battle");
			}
		}
		else
		{
			OneShot("event:/sfx/ui/deny");
		}
	}

	public void ActionQueued(PlayAction action)
	{
		if (action is ActionReveal)
		{
			revealActionsInQueue++;
		}
	}

	public void ActionPerform(PlayAction action)
	{
		if (action is ActionReveal && revealActionsInQueue > 1 && !IsRunning(flipMulti))
		{
			flipMulti = Play("event:/sfx/card/flip_multi");
		}
	}

	public void ActionFinished(PlayAction action)
	{
		if (action is ActionReveal && --revealActionsInQueue <= 0)
		{
			SetParam(flipMulti, "flip_stop", 1f);
		}
	}

	public void TransitionStart(TransitionType transition)
	{
		if (transition is TransitionSnow)
		{
			transitionSnow = Play("event:/sfx/transition/snow");
		}
	}

	public void TransitionEnd(TransitionType transition)
	{
		if (transition is TransitionSnow && IsRunning(transitionSnow))
		{
			SetParam(transitionSnow, "transition_end", 1f);
		}
	}

	public void GoldCounterStart(GoldDisplay display, float addAmount)
	{
		GoldCounterStop();
		if (addAmount > 0f)
		{
			goldDisplay = display;
			goldCounter = Play("event:/sfx/inventory/bling_counter_up");
		}
		else if (addAmount < 0f)
		{
			goldDisplay = display;
			goldCounter = Play("event:/sfx/inventory/bling_counter_down");
		}
	}

	public void GoldCounterStop()
	{
		if (IsRunning(goldCounter))
		{
			Stop(goldCounter);
		}
	}

	public void MuncherDrag()
	{
		muncherFeed = Play("event:/sfx/location/muncher/feed");
	}

	public void MuncherDragEnd()
	{
		if (IsRunning(muncherFeed))
		{
			Stop(muncherFeed);
		}
	}

	public void MuncherFeed(Entity entity)
	{
		if (IsRunning(muncherFeed))
		{
			Stop(muncherFeed);
			OneShot("event:/sfx/location/muncher/eat");
		}
	}

	public static void BombardShoot(Entity entity)
	{
		OneShot("event:/sfx/specific/boss_shooting");
	}

	public static void BombardRocketFall(BombardRocket rocket)
	{
		OneShot("event:/sfx/specific/boss_rockets_flying");
	}

	public static void BombardRocketExplode(BombardRocket rocket)
	{
		OneShot("event:/sfx/specific/boss_rockets_impact");
	}

	public static void ButtonHover(ButtonType buttonType)
	{
		if (buttonType != ButtonType.Bell)
		{
			OneShot("event:/sfx/ui/menu_hover");
		}
		else
		{
			OneShot("event:/sfx/modifiers/bell_hovering");
		}
	}

	public static void ButtonPress(ButtonType buttonType)
	{
		switch (buttonType)
		{
			default:
				OneShot("event:/sfx/ui/menu_click");
				break;
			case ButtonType.Sub:
				OneShot("event:/sfx/ui/menu_click_sub");
				break;
			case ButtonType.Back:
				OneShot("event:/sfx/ui/menu_click_back");
				break;
		}
	}

	public void ProgressStart(float fill)
	{
		townProgressionLoop = Loop("event:/sfx/town_progress/ramp");
		SetParam(townProgressionLoop, "townramp", fill);
	}

	public void ProgressUpdate(float fill)
	{
		if (IsRunning(townProgressionLoop))
		{
			SetParam(townProgressionLoop, "townramp", fill);
		}
	}

	public void ProgressStop()
	{
		Stop(townProgressionLoop);
	}

	public static void ProgressDing()
	{
		OneShot("event:/sfx/town_progress/achieved");
	}

	public static void ProgressBlip()
	{
		OneShot("event:/sfx/town_progress/blip");
	}

	public static void TownUnlockPopUp(UnlockData unlockData)
	{
		OneShot("event:/sfx/town_progress/notification");
	}

	public static void UpgradeHover(UpgradeDisplay upgradeDisplay)
	{
		if (upgradeDisplay.data.type != CardUpgradeData.Type.Crown)
		{
			OneShot("event:/sfx/inventory/charm_hover");
		}
		else
		{
			OneShot("event:/sfx/inventory/crown_hover");
		}
	}

	public static void UpgradePickup(UpgradeDisplay upgradeDisplay)
	{
		if (upgradeDisplay.data.type != CardUpgradeData.Type.Crown)
		{
			OneShot("event:/sfx/inventory/charm_pickup");
		}
		else
		{
			OneShot("event:/sfx/inventory/crown_pickup");
		}
	}

	public static void UpgradeDrop(UpgradeDisplay upgradeDisplay)
	{
		if (upgradeDisplay.data.type != CardUpgradeData.Type.Crown)
		{
			OneShot("event:/sfx/inventory/charm_return");
		}
		else
		{
			OneShot("event:/sfx/inventory/crown_return");
		}
	}

	public static void UpgradeAssign(Entity entity, CardUpgradeData upgradeData)
	{
		if (upgradeData.type == CardUpgradeData.Type.Crown)
		{
			OneShot("event:/sfx/inventory/crown_assign");
		}
	}

	public static void ShopItemHover(ShopItem shopItem)
	{
		CrownHolderShop component = shopItem.GetComponent<CrownHolderShop>();
		if ((object)component != null && component.hasCrown && component.enabled)
		{
			OneShotCheckCooldown("event:/sfx/location/shop/crown_hover");
			return;
		}

		CharmMachine component2 = shopItem.GetComponent<CharmMachine>();
		if ((object)component2 != null && component2.enabled)
		{
			OneShotCheckCooldown("event:/sfx/location/shop/charm_hover");
		}
	}

	public static void AbilityTargetAdd(CardContainer container)
	{
		OneShot("event:/sfx/specific/boss_targets");
	}

	public static void EntityPing(GameObject obj)
	{
		OneShot("event:/sfx/card/ping");
	}

	public void StopAll(FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.IMMEDIATE)
	{
		foreach (EventInstance item in running)
		{
			item.stop(stopMode);
			item.release();
		}

		cooldowns.Clear();
	}
}
