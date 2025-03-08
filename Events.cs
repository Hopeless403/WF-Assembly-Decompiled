#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public static class Events
{
	public delegate void UnityActionRef<T>(ref T arg0);

	public delegate void UnityActionRef<T0, T1>(ref T0 arg0, ref T1 arg1);

	public delegate void UnityActionRef1<T0, T1>(ref T0 arg0, T1 arg1);

	public delegate void UnityActionRef<T0, T1, T2>(ref T0 arg0, ref T1 arg1, ref T2 arg2);

	public delegate void UnityActionRef<T0, T1, T2, T3>(T0 arg0, ref T1 arg1, ref T2 arg2, ref T3 arg3);

	public delegate void UnityActionCheck<T0, T1>(T0 arg0, ref T1 arg1);

	public delegate void UnityActionCheck<T0, T1, T2>(T0 arg0, T1 arg1, ref T2 arg2);

	public delegate void UnityAction<T0, T1, T2, T3, T4>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);

	public delegate void UnityAction<T0, T1, T2, T3, T4, T5>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

	public delegate IEnumerator RoutineAction();

	public delegate Task AsyncAction();

	public delegate IEnumerator RoutineAction<T>(T arg);

	public static event UnityAction<Scene> OnSceneLoaded;

	public static event UnityAction<Scene> OnSceneChanged;

	public static event UnityAction<Scene> OnSceneUnload;

	public static event UnityAction OnBackToMainMenu;

	public static event UnityAction<Entity> OnEntityCreated;

	public static event UnityAction<Entity> OnEntityDataUpdated;

	public static event UnityAction<Entity> OnEntityDestroyed;

	public static event UnityAction<Card> OnCardPooled;

	public static event UnityAction<Entity> OnEntityEnabled;

	public static event UnityAction<Entity> OnEntityDisabled;

	public static event UnityAction<Entity, DeathType> OnEntityKilled;

	public static event UnityAction<Entity> OnEntityFlee;

	public static event UnityAction<Entity> OnEntityHover;

	public static event UnityAction<Entity> OnEntityUnHover;

	public static event UnityAction<Entity> OnEntitySelect;

	public static event UnityActionRef<Entity, bool> OnCheckEntityDrag;

	public static event UnityAction<Entity> OnEntityDrag;

	public static event UnityAction<Entity> OnEntityRelease;

	public static event UnityAction<Entity> OnEntityOffered;

	public static event UnityAction<Entity> OnEntityChosen;

	public static event UnityAction<Entity> OnEntityShowUnlocked;

	public static event UnityActionRef<Entity, bool> OnCheckEntityShove;

	public static event UnityAction<Entity> OnPreProcessTrigger;

	public static event UnityActionRef<Trigger> OnEntityPreTrigger;

	public static event UnityActionRef<Trigger> OnEntityTrigger;

	public static event UnityActionRef<Trigger> OnEntityTriggered;

	public static event UnityAction<Entity> OnEntityMove;

	public static event UnityAction<Entity, CardContainer[], bool> OnEntityPlace;

	public static event UnityAction<Entity> OnDiscard;

	public static event UnityAction<Hit> OnEntityHit;

	public static event UnityAction<Hit> OnEntityPostHit;

	public static event UnityAction<Hit> OnEntityDodge;

	public static event UnityAction<Entity> OnEntityFlipUp;

	public static event UnityAction<Entity> OnEntityFlipDown;

	public static event UnityAction<StatusIcon> OnStatusIconCreated;

	public static event UnityAction<StatusIcon, Stat, Stat> OnStatusIconChanged;

	public static event UnityAction<Entity> OnEntityDisplayUpdated;

	public static event UnityAction<Entity> OnEntityFocus;

	public static event UnityAction<Entity, CardPocket> OnEntityEnterPocket;

	public static event UnityAction<Entity> OnEntityEnterBackpack;

	public static event UnityAction<Entity> OnEntityChangePhase;

	public static event UnityAction<Entity> OnNoomlinShow;

	public static event UnityAction<Entity> OnNoomlinUsed;

	public static event UnityAction<Entity> OnEntityFlipComplete;

	public static event UnityAction<Entity, Entity> OnEntitySummoned;

	public static event UnityAction<GameObject> OnEntityPing;

	public static event UnityActionRef<Entity, int> OnCheckRecycleAmount;

	public static event UnityAction<CardContainer> OnContainerHover;

	public static event UnityAction<CardContainer> OnContainerUnHover;

	public static event UnityAction<CardSlot> OnSlotHover;

	public static event UnityAction<CardSlot> OnSlotUnHover;

	public static event UnityAction<PlayAction> OnActionQueued;

	public static event UnityAction<PlayAction> OnActionPerform;

	public static event UnityAction<PlayAction> OnActionFinished;

	public static event UnityActionRef<PlayAction, bool> OnCheckAction;

	public static event UnityAction<CardContainer> OnAbilityTargetAdd;

	public static event UnityAction<CardContainer> OnAbilityTargetRemove;

	public static event UnityAction<int> OnCardDraw;

	public static event UnityAction OnCardDrawEnd;

	public static event UnityAction<CampaignNode> PreBattleSetUp;

	public static event UnityAction<CampaignNode> PostBattleSetUp;

	public static event UnityAction<Battle.Phase> OnBattlePhaseStart;

	public static event UnityAction OnBattleStart;

	public static event AsyncAction PreBattleEnd;

	public static event UnityAction OnBattleEnd;

	public static event UnityAction OnBattleWinPreRewards;

	public static event UnityAction OnBattleWin;

	public static event UnityAction<CampaignNode> PostBattle;

	public static event UnityActionCheck<StatusEffectData, int> OnStatusEffectCountDown;

	public static event UnityAction<StatusEffectApply> OnStatusEffectApplied;

	public static event UnityAction<Character> OnPreProcessUnits;

	public static event UnityAction<Character> OnPostProcessUnits;

	public static event UnityActionCheck<Entity, int> OnEntityCountDown;

	public static event UnityAction<int> OnBattlePreTurnStart;

	public static event UnityAction<int> OnBattleTurnStart;

	public static event UnityAction<int> OnBattleTurnEnd;

	public static event UnityAction<Entity> OnMinibossIntro;

	public static event UnityAction<Entity> OnMinibossIntroDone;

	public static event UnityAction<int> OnKillCombo;

	public static event UnityAction<RedrawBellSystem> OnRedrawBellHit;

	public static event UnityAction<RedrawBellSystem> OnRedrawBellRevealed;

	public static event UnityAction<CardData> OnCardInjured;

	public static event UnityAction OnBattleStateBuild;

	public static event UnityAction<BattleSaveData> OnBattleStateBuilt;

	public static event UnityAction<Character> OnCharacterActionPerformed;

	public static event UnityAction<int> OnWaveDeployerPreCountDown;

	public static event UnityAction<int> OnWaveDeployerPostCountDown;

	public static event UnityAction OnWaveDeployerEarlyDeploy;

	public static event UnityAction<float> OnMapPathReveal;

	public static event UnityAction<MapNode> OnMapNodeReveal;

	public static event UnityAction<MapNode> OnMapNodeSelect;

	public static event UnityAction<MapNode> OnMapNodeHover;

	public static event UnityAction<MapNode> OnMapNodeUnHover;

	public static event UnityAction<UnlockData> OnTownUnlock;

	public static event UnityAction<ShopItem> OnShopItemHover;

	public static event UnityAction<ShopItem> OnShopItemUnHover;

	public static event UnityAction<ShopItem> OnShopItemPurchase;

	public static event UnityAction<ShopItem> OnShopItemHaggled;

	public static event UnityAction<KeywordData, CardPopUpPanel> OnPopupCreated;

	public static event UnityAction<ButtonType> OnButtonHover;

	public static event UnityAction<ButtonType> OnButtonPress;

	public static event UnityAction<string, float> OnAudioVolumeChange;

	public static event UnityAction<string, float> OnAudioPitchChange;

	public static event UnityAction<Entity> OnBombardShoot;

	public static event UnityAction<BombardRocket> OnBombardRocketFall;

	public static event UnityAction<BombardRocket> OnBombardRocketExplode;

	public static event UnityAction<float> OnProgressStart;

	public static event UnityAction<float> OnProgressUpdate;

	public static event UnityAction OnProgressStop;

	public static event UnityAction OnProgressDing;

	public static event UnityAction OnProgressBlip;

	public static event UnityAction OnSaveSystemEnabled;

	public static event UnityAction OnSaveSystemDisabled;

	public static event UnityAction OnSaveSystemProfileChanged;

	public static event UnityAction OnCampaignSaved;

	public static event UnityAction OnCampaignLoaded;

	public static event UnityAction OnCampaignDeleted;

	public static event UnityAction OnBattleSaved;

	public static event UnityAction OnBattleLoaded;

	public static event UnityAction OnGameStart;

	public static event UnityAction OnGameEnd;

	public static event UnityAction OnCampaignStart;

	public static event UnityAction OnCampaignFinal;

	public static event UnityAction<Campaign.Result, CampaignStats, PlayerData> OnCampaignEnd;

	public static event UnityAction<CampaignStats> OnOverallStatsSaved;

	public static event UnityAction<CampaignData> OnCampaignDataCreated;

	public static event RoutineAction OnCampaignPreInit;

	public static event RoutineAction OnCampaignInit;

	public static event UnityAction OnPreCampaignPopulate;

	public static event UnityActionRef1<List<CampaignGenerator.Node>, Vector2> OnCampaignNodesCreated;

	public static event UnityActionRef<string[]> OnCampaignLoadPreset;

	public static event AsyncAction OnCampaignGenerated;

	public static event UnityAction<string, string, int, int> OnStatChanged;

	public static event UnityAction<CardUpgradeData> OnUpgradeGained;

	public static event UnityAction<Entity, CardUpgradeData> OnUpgradeAssign;

	public static event UnityAction<UpgradeDisplay> OnUpgradeHover;

	public static event UnityAction<UpgradeDisplay> OnUpgradePickup;

	public static event UnityAction<UpgradeDisplay> OnUpgradeDrop;

	public static event UnityAction<bool> OnUpdateInputSystem;

	public static event UnityAction<float, float?> OnScreenShake;

	public static event UnityAction<float, float, float, float, float, float> OnScreenRumble;

	public static event UnityAction<string> OnCameraAnimation;

	public static event UnityAction<int, string, Character, Vector3> OnDropGold;

	public static event UnityAction<int, Character, Vector3> OnGoldFlyToBag;

	public static event UnityAction<int> OnCollectGold;

	public static event UnityAction<int> OnSpendGold;

	public static event UnityAction<float> OnTimeScaleChange;

	public static event UnityAction<Entity> OnInspect;

	public static event UnityAction<Entity> OnInspectEnd;

	public static event UnityAction<Entity> OnInspectNewCard;

	public static event UnityAction<CardController> OnCardControllerEnabled;

	public static event UnityAction<CardController> OnCardControllerDisabled;

	public static event UnityAction OnDeckpackOpen;

	public static event UnityAction OnDeckpackClose;

	public static event UnityAction<TransitionType> OnTransitionStart;

	public static event UnityAction<TransitionType> OnTransitionEnd;

	public static event UnityAction<float, float> OnSetWeatherIntensity;

	public static event UnityAction<GoldDisplay, float> OnGoldCounterStart;

	public static event UnityAction<CampaignNode, EventRoutine> OnEventStart;

	public static event UnityAction<EventRoutine> OnEventPopulated;

	public static event UnityAction OnMuncherDrag;

	public static event UnityAction OnMuncherDragCancel;

	public static event UnityAction<Entity> OnMuncherFeed;

	public static event UnityAction<string, object> OnSettingChanged;

	public static event UnityAction OnUINavigationReset;

	public static event UnityAction OnUINavigation;

	public static event UnityAction OnButtonStyleChanged;

	public static event UnityAction OnControllerSwitched;

	public static event UnityActionRef<Entity, string, bool> OnCheckRename;

	public static event UnityAction<Entity, string> OnRename;

	public static event UnityActionRef<object, string, int, List<DataFile>> OnPullRewards;

	public static event UnityAction<CardData> OnCardDataCreated;

	public static event UnityAction<int> OnTutorialProgress;

	public static event UnityAction OnTutorialSkip;

	public static event UnityAction<ChallengeData> OnChallengeCompletedSaved;

	public static event UnityActionRef<int> OnGetHandSize;

	public static event UnityAction<WildfrostMod> OnModLoaded;

	public static event UnityAction<WildfrostMod> OnModUnloaded;

	public static void InvokeSceneLoaded(Scene scene)
	{
		Events.OnSceneLoaded?.Invoke(scene);
	}

	public static void InvokeSceneChanged(Scene scene)
	{
		Events.OnSceneChanged?.Invoke(scene);
	}

	public static void InvokeSceneUnload(Scene scene)
	{
		Events.OnSceneUnload?.Invoke(scene);
	}

	public static void InvokeBackToMainMenu()
	{
		Events.OnBackToMainMenu?.Invoke();
	}

	public static void InvokeEntityCreated(Entity entity)
	{
		Events.OnEntityCreated?.Invoke(entity);
	}

	public static void InvokeEntityDataUpdated(Entity entity)
	{
		Events.OnEntityDataUpdated?.Invoke(entity);
	}

	public static void InvokeEntityDestroyed(Entity entity)
	{
		Events.OnEntityDestroyed?.Invoke(entity);
	}

	public static void InvokeCardPooled(Card card)
	{
		Events.OnCardPooled?.Invoke(card);
	}

	public static void InvokeEntityEnabled(Entity entity)
	{
		Events.OnEntityEnabled?.Invoke(entity);
	}

	public static void InvokeEntityDisabled(Entity entity)
	{
		Events.OnEntityDisabled?.Invoke(entity);
	}

	public static void InvokeEntityKilled(Entity entity, DeathType deathType)
	{
		Events.OnEntityKilled?.Invoke(entity, deathType);
	}

	public static void InvokeEntityFlee(Entity entity)
	{
		Events.OnEntityFlee?.Invoke(entity);
	}

	public static void InvokeEntityHover(Entity entity)
	{
		Events.OnEntityHover?.Invoke(entity);
	}

	public static void InvokeEntityUnHover(Entity entity)
	{
		Events.OnEntityUnHover?.Invoke(entity);
	}

	public static void InvokeEntitySelect(Entity entity)
	{
		Events.OnEntitySelect?.Invoke(entity);
	}

	public static bool CheckEntityDrag(Entity entity)
	{
		bool arg = true;
		Events.OnCheckEntityDrag?.Invoke(ref entity, ref arg);
		return arg;
	}

	public static void InvokeEntityDrag(Entity entity)
	{
		Events.OnEntityDrag?.Invoke(entity);
	}

	public static void InvokeEntityRelease(Entity entity)
	{
		Events.OnEntityRelease?.Invoke(entity);
	}

	public static void InvokeEntityOffered(Entity entity)
	{
		Events.OnEntityOffered?.Invoke(entity);
	}

	public static void InvokeEntityChosen(Entity entity)
	{
		Events.OnEntityChosen?.Invoke(entity);
	}

	public static void InvokeEntityShowUnlocked(Entity entity)
	{
		Events.OnEntityShowUnlocked?.Invoke(entity);
	}

	public static bool CheckEntityShove(Entity entity)
	{
		bool arg = true;
		Events.OnCheckEntityShove?.Invoke(ref entity, ref arg);
		return arg;
	}

	public static void InvokePreProcessTrigger(Entity entity)
	{
		Events.OnPreProcessTrigger?.Invoke(entity);
	}

	public static void InvokeEntityPreTrigger(ref Trigger trigger)
	{
		Events.OnEntityPreTrigger?.Invoke(ref trigger);
	}

	public static void InvokeEntityTrigger(ref Trigger trigger)
	{
		Events.OnEntityTrigger?.Invoke(ref trigger);
	}

	public static void InvokeEntityTriggered(ref Trigger trigger)
	{
		Events.OnEntityTriggered?.Invoke(ref trigger);
	}

	public static void InvokeEntityMove(Entity entity)
	{
		Events.OnEntityMove?.Invoke(entity);
	}

	public static void InvokeEntityPlace(Entity entity, CardContainer[] containers, bool freeMove)
	{
		Events.OnEntityPlace?.Invoke(entity, containers, freeMove);
	}

	public static void InvokeDiscard(Entity entity)
	{
		Events.OnDiscard?.Invoke(entity);
	}

	public static void InvokeEntityHit(Hit hit)
	{
		Events.OnEntityHit?.Invoke(hit);
	}

	public static void InvokeEntityPostHit(Hit hit)
	{
		Events.OnEntityPostHit?.Invoke(hit);
	}

	public static void InvokeEntityDodge(Hit hit)
	{
		Events.OnEntityDodge?.Invoke(hit);
	}

	public static void InvokeEntityFlipUp(Entity entity)
	{
		Events.OnEntityFlipUp?.Invoke(entity);
	}

	public static void InvokeEntityFlipDown(Entity entity)
	{
		Events.OnEntityFlipDown?.Invoke(entity);
	}

	public static void InvokeStatusIconCreated(StatusIcon icon)
	{
		Events.OnStatusIconCreated?.Invoke(icon);
	}

	public static void InvokeStatusIconChanged(StatusIcon icon, Stat previousValue, Stat newValue)
	{
		Events.OnStatusIconChanged?.Invoke(icon, previousValue, newValue);
	}

	public static void InvokeEntityDisplayUpdated(Entity entity)
	{
		Events.OnEntityDisplayUpdated?.Invoke(entity);
	}

	public static void InvokeEntityFocus(Entity entity)
	{
		Events.OnEntityFocus?.Invoke(entity);
	}

	public static void InvokeEntityEnterPocket(Entity card, CardPocket pocket)
	{
		Events.OnEntityEnterPocket?.Invoke(card, pocket);
	}

	public static void InvokeEntityEnterBackpack(Entity entity)
	{
		Events.OnEntityEnterBackpack?.Invoke(entity);
	}

	public static void InvokeEntityChangePhase(Entity entity)
	{
		Events.OnEntityChangePhase?.Invoke(entity);
	}

	public static void InvokeNoomlinShow(Entity entity)
	{
		Events.OnNoomlinShow?.Invoke(entity);
	}

	public static void InvokeNoomlinUsed(Entity entity)
	{
		Events.OnNoomlinUsed?.Invoke(entity);
	}

	public static void InvokeEntityFlipComplete(Entity entity)
	{
		Events.OnEntityFlipComplete?.Invoke(entity);
	}

	public static void InvokeEntitySummoned(Entity entity, Entity summonedBy)
	{
		Events.OnEntitySummoned?.Invoke(entity, summonedBy);
	}

	public static void InvokeEntityPing(GameObject obj)
	{
		Events.OnEntityPing?.Invoke(obj);
	}

	public static void CheckRecycleAmount(Entity entity, ref int amount)
	{
		Events.OnCheckRecycleAmount?.Invoke(ref entity, ref amount);
	}

	public static void InvokeContainerHover(CardContainer container)
	{
		Events.OnContainerHover?.Invoke(container);
	}

	public static void InvokeContainerUnHover(CardContainer container)
	{
		Events.OnContainerUnHover?.Invoke(container);
	}

	public static void InvokeSlotHover(CardSlot slot)
	{
		Events.OnSlotHover?.Invoke(slot);
	}

	public static void InvokeSlotUnHover(CardSlot slot)
	{
		Events.OnSlotUnHover?.Invoke(slot);
	}

	public static void InvokeActionQueued(PlayAction action)
	{
		Events.OnActionQueued?.Invoke(action);
	}

	public static void InvokeActionPerform(PlayAction action)
	{
		Events.OnActionPerform?.Invoke(action);
	}

	public static void InvokeActionFinished(PlayAction action)
	{
		Events.OnActionFinished?.Invoke(action);
	}

	public static bool CheckAction(PlayAction action)
	{
		bool arg = true;
		Events.OnCheckAction?.Invoke(ref action, ref arg);
		return arg;
	}

	public static void InvokeAbilityTargetAdd(CardContainer container)
	{
		Events.OnAbilityTargetAdd?.Invoke(container);
	}

	public static void InvokeAbilityTargetRemove(CardContainer container)
	{
		Events.OnAbilityTargetRemove?.Invoke(container);
	}

	public static void InvokeCardDraw(int count)
	{
		Events.OnCardDraw?.Invoke(count);
	}

	public static void InvokeCardDrawEnd()
	{
		Events.OnCardDrawEnd?.Invoke();
	}

	public static void InvokePreBattleSetUp(CampaignNode node)
	{
		Events.PreBattleSetUp?.Invoke(node);
	}

	public static void InvokePostBattleSetUp(CampaignNode node)
	{
		Events.PostBattleSetUp?.Invoke(node);
	}

	public static void InvokeBattlePhaseStart(Battle.Phase phase)
	{
		Events.OnBattlePhaseStart?.Invoke(phase);
	}

	public static void InvokeBattleStart()
	{
		Events.OnBattleStart?.Invoke();
	}

	public static IEnumerator InvokePreBattleEnd()
	{
		if (Events.PreBattleEnd != null)
		{
			Task task = Events.PreBattleEnd();
			yield return new WaitUntil(() => task.IsCompleted);
		}
	}

	public static void InvokeBattleEnd()
	{
		Events.OnBattleEnd?.Invoke();
	}

	public static void InvokeBattleWinPreRewards()
	{
		Events.OnBattleWinPreRewards?.Invoke();
	}

	public static void InvokeBattleWin()
	{
		Events.OnBattleWin?.Invoke();
	}

	public static void InvokePostBattle(CampaignNode campaignNode)
	{
		Events.PostBattle?.Invoke(campaignNode);
	}

	public static void InvokeStatusEffectCountDown(StatusEffectData status, ref int amount)
	{
		Events.OnStatusEffectCountDown?.Invoke(status, ref amount);
	}

	public static void InvokeStatusEffectApplied(StatusEffectApply apply)
	{
		Events.OnStatusEffectApplied?.Invoke(apply);
	}

	public static void InvokePreProcessUnits(Character character)
	{
		Events.OnPreProcessUnits?.Invoke(character);
	}

	public static void InvokePostProcessUnits(Character character)
	{
		Events.OnPostProcessUnits?.Invoke(character);
	}

	public static void InvokeEntityCountDown(Entity entity, ref int amount)
	{
		Events.OnEntityCountDown?.Invoke(entity, ref amount);
	}

	public static void InvokeBattlePreTurnStart(int turnNumber)
	{
		Events.OnBattlePreTurnStart?.Invoke(turnNumber);
	}

	public static void InvokeBattleTurnStart(int turnNumber)
	{
		Events.OnBattleTurnStart?.Invoke(turnNumber);
	}

	public static void InvokeBattleTurnEnd(int turnNumber)
	{
		Events.OnBattleTurnEnd?.Invoke(turnNumber);
	}

	public static void InvokeMinibossIntro(Entity entity)
	{
		Events.OnMinibossIntro?.Invoke(entity);
	}

	public static void InvokeMinibossIntroDone(Entity entity)
	{
		Events.OnMinibossIntroDone?.Invoke(entity);
	}

	public static void InvokeKillCombo(int combo)
	{
		Events.OnKillCombo?.Invoke(combo);
	}

	public static void InvokeRedrawBellHit(RedrawBellSystem redrawBellSystem)
	{
		Events.OnRedrawBellHit?.Invoke(redrawBellSystem);
	}

	public static void InvokeRedrawBellRevealed(RedrawBellSystem redrawBellSystem)
	{
		Events.OnRedrawBellRevealed?.Invoke(redrawBellSystem);
	}

	public static void InvokeCardInjured(CardData cardData)
	{
		Events.OnCardInjured?.Invoke(cardData);
	}

	public static void InvokeBattleStateBuild()
	{
		Events.OnBattleStateBuild?.Invoke();
	}

	public static void InvokeBattleStateBuilt(BattleSaveData battleState)
	{
		Events.OnBattleStateBuilt?.Invoke(battleState);
	}

	public static void InvokeCharacterActionPerformed(Character character)
	{
		Events.OnCharacterActionPerformed?.Invoke(character);
	}

	public static void InvokeWaveDeployerPreCountDown(int counter)
	{
		Events.OnWaveDeployerPreCountDown?.Invoke(counter);
	}

	public static void InvokeWaveDeployerPostCountDown(int counter)
	{
		Events.OnWaveDeployerPostCountDown?.Invoke(counter);
	}

	public static void InvokeWaveDeployerEarlyDeploy()
	{
		Events.OnWaveDeployerEarlyDeploy?.Invoke();
	}

	public static void InvokeMapPathReveal(float totalTime)
	{
		Events.OnMapPathReveal?.Invoke(totalTime);
	}

	public static void InvokeMapNodeReveal(MapNode node)
	{
		Events.OnMapNodeReveal?.Invoke(node);
	}

	public static void InvokeMapNodeSelect(MapNode node)
	{
		Events.OnMapNodeSelect?.Invoke(node);
	}

	public static void InvokeMapNodeHover(MapNode node)
	{
		Events.OnMapNodeHover?.Invoke(node);
	}

	public static void InvokeMapNodeUnHover(MapNode node)
	{
		Events.OnMapNodeUnHover?.Invoke(node);
	}

	public static void InvokeTownUnlock(UnlockData unlockData)
	{
		Events.OnTownUnlock?.Invoke(unlockData);
	}

	public static void InvokeShopItemHover(ShopItem item)
	{
		Events.OnShopItemHover?.Invoke(item);
	}

	public static void InvokeShopItemUnHover(ShopItem item)
	{
		Events.OnShopItemUnHover?.Invoke(item);
	}

	public static void InvokeShopItemPurchase(ShopItem item)
	{
		Events.OnShopItemPurchase?.Invoke(item);
	}

	public static void InvokeShopItemHaggled(ShopItem item)
	{
		Events.OnShopItemHaggled?.Invoke(item);
	}

	public static void InvokePopupPanelCreated(KeywordData keyword, CardPopUpPanel panel)
	{
		Events.OnPopupCreated?.Invoke(keyword, panel);
	}

	public static void InvokeButtonHover(ButtonType buttonType)
	{
		Events.OnButtonHover?.Invoke(buttonType);
	}

	public static void InvokeButtonPress(ButtonType buttonType)
	{
		Events.OnButtonPress?.Invoke(buttonType);
	}

	public static void InvokeAudioVolumeChange(string busName, float value)
	{
		Events.OnAudioVolumeChange?.Invoke(busName, value);
	}

	public static void InvokeAudioPitchChange(string busName, float value)
	{
		Events.OnAudioPitchChange?.Invoke(busName, value);
	}

	public static void InvokeBombardShoot(Entity entity)
	{
		Events.OnBombardShoot?.Invoke(entity);
	}

	public static void InvokeBombardRocketFall(BombardRocket rocket)
	{
		Events.OnBombardRocketFall?.Invoke(rocket);
	}

	public static void InvokeBombardRocketExplode(BombardRocket rocket)
	{
		Events.OnBombardRocketExplode?.Invoke(rocket);
	}

	public static void InvokeProgressStart(float fill)
	{
		Events.OnProgressStart?.Invoke(fill);
	}

	public static void InvokeProgressUpdate(float fill)
	{
		Events.OnProgressUpdate?.Invoke(fill);
	}

	public static void InvokeProgressStop()
	{
		Events.OnProgressStop?.Invoke();
	}

	public static void InvokeProgressDing()
	{
		Events.OnProgressDing?.Invoke();
	}

	public static void InvokeProgressBlip()
	{
		Events.OnProgressBlip?.Invoke();
	}

	public static void InvokeSaveSystemEnabled()
	{
		Events.OnSaveSystemEnabled?.Invoke();
	}

	public static void InvokeSaveSystemDisabled()
	{
		Events.OnSaveSystemDisabled?.Invoke();
	}

	public static void InvokeSaveSystemProfileChanged()
	{
		Events.OnSaveSystemProfileChanged?.Invoke();
	}

	public static void InvokeCampaignSaved()
	{
		Events.OnCampaignSaved?.Invoke();
	}

	public static void InvokeCampaignLoaded()
	{
		Events.OnCampaignLoaded?.Invoke();
	}

	public static void InvokeCampaignDeleted()
	{
		Events.OnCampaignDeleted?.Invoke();
	}

	public static void InvokeBattleSaved()
	{
		Events.OnBattleSaved?.Invoke();
	}

	public static void InvokeBattleLoaded()
	{
		Events.OnBattleLoaded?.Invoke();
	}

	public static void InvokeGameStart()
	{
		Events.OnGameStart?.Invoke();
	}

	public static void InvokeGameEnd()
	{
		Events.OnGameEnd?.Invoke();
	}

	public static void InvokeCampaignStart()
	{
		Events.OnCampaignStart?.Invoke();
	}

	public static void InvokeCampaignFinal()
	{
		Events.OnCampaignFinal?.Invoke();
	}

	public static void InvokeCampaignEnd(Campaign.Result result, CampaignStats stats, PlayerData playerData)
	{
		Events.OnCampaignEnd?.Invoke(result, stats, playerData);
	}

	public static void InvokeOverallStatsSaved(CampaignStats stats)
	{
		Events.OnOverallStatsSaved?.Invoke(stats);
	}

	public static void InvokeCampaignDataCreated(CampaignData data)
	{
		Events.OnCampaignDataCreated?.Invoke(data);
	}

	public static IEnumerator InvokeCampaignPreInit()
	{
		if (Events.OnCampaignPreInit != null)
		{
			yield return Events.OnCampaignPreInit();
		}
	}

	public static IEnumerator InvokeCampaignInit()
	{
		if (Events.OnCampaignInit != null)
		{
			yield return Events.OnCampaignInit();
		}
	}

	public static void InvokePreCampaignPopulate()
	{
		Events.OnPreCampaignPopulate?.Invoke();
	}

	public static void InvokeCampaignNodesCreated(ref List<CampaignGenerator.Node> nodes, Vector2 nodeSpacing)
	{
		Events.OnCampaignNodesCreated?.Invoke(ref nodes, nodeSpacing);
	}

	public static void InvokeCampaignLoadPreset(ref string[] lines)
	{
		Events.OnCampaignLoadPreset?.Invoke(ref lines);
	}

	public static IEnumerator InvokeCampaignGenerated()
	{
		if (Events.OnCampaignGenerated == null)
		{
			yield break;
		}

		Delegate[] invocationList = Events.OnCampaignGenerated.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			if (invocationList[i] is AsyncAction asyncAction)
			{
				Task task = asyncAction();
				yield return new WaitUntil(() => task.IsCompleted);
			}
		}
	}

	public static void InvokeStatChanged(string stat, string key, int oldValue, int newValue)
	{
		Events.OnStatChanged?.Invoke(stat, key, oldValue, newValue);
	}

	public static void InvokeUpgradeGained(CardUpgradeData upgradeData)
	{
		Events.OnUpgradeGained?.Invoke(upgradeData);
	}

	public static void InvokeUpgradeAssign(Entity entity, CardUpgradeData upgradeData)
	{
		Events.OnUpgradeAssign?.Invoke(entity, upgradeData);
	}

	public static void InvokeUpgradeHover(UpgradeDisplay upgradeDisplay)
	{
		Events.OnUpgradeHover?.Invoke(upgradeDisplay);
	}

	public static void InvokeUpgradePickup(UpgradeDisplay upgradeDisplay)
	{
		Events.OnUpgradePickup?.Invoke(upgradeDisplay);
	}

	public static void InvokeUpgradeDrop(UpgradeDisplay upgradeDisplay)
	{
		Events.OnUpgradeDrop?.Invoke(upgradeDisplay);
	}

	public static void InvokeUpdateInputSystem(bool forceTouch)
	{
		Events.OnUpdateInputSystem?.Invoke(forceTouch);
	}

	public static void InvokeScreenShake(float magnitude = 1f, float? direction = 0f)
	{
		Events.OnScreenShake?.Invoke(magnitude, direction);
	}

	public static void InvokeScreenRumble(float startStrength, float endStrength, float delay, float fadeInTime, float holdTime, float fadeOutTime)
	{
		Events.OnScreenRumble?.Invoke(startStrength, endStrength, delay, fadeInTime, holdTime, fadeOutTime);
	}

	public static void InvokeCameraAnimation(string name)
	{
		Events.OnCameraAnimation?.Invoke(name);
	}

	public static void InvokeDropGold(int amount, string source, Character owner, Vector3 position)
	{
		Events.OnDropGold?.Invoke(amount, source, owner, position);
	}

	public static void InvokeGoldFlyToBag(int amount, Character owner, Vector3 position)
	{
		Events.OnGoldFlyToBag?.Invoke(amount, owner, position);
	}

	public static void InvokeCollectGold(int amount)
	{
		Events.OnCollectGold?.Invoke(amount);
	}

	public static void InvokeSpendGold(int amount)
	{
		Events.OnSpendGold?.Invoke(amount);
	}

	public static void InvokeTimeScaleChange(float value)
	{
		Events.OnTimeScaleChange?.Invoke(value);
	}

	public static void InvokeInspect(Entity entity)
	{
		Events.OnInspect?.Invoke(entity);
	}

	public static void InvokeInspectEnd(Entity entity)
	{
		Events.OnInspectEnd?.Invoke(entity);
	}

	public static void InvokeInspectNewCard(Entity entity)
	{
		Events.OnInspectNewCard?.Invoke(entity);
	}

	public static void InvokeCardControllerEnabled(CardController controller)
	{
		Events.OnCardControllerEnabled?.Invoke(controller);
	}

	public static void InvokeCardControllerDisabled(CardController controller)
	{
		Events.OnCardControllerDisabled?.Invoke(controller);
	}

	public static void InvokeDeckpackOpen()
	{
		Events.OnDeckpackOpen?.Invoke();
	}

	public static void InvokeDeckpackClose()
	{
		Events.OnDeckpackClose?.Invoke();
	}

	public static void InvokeTransitionStart(TransitionType transition)
	{
		Events.OnTransitionStart?.Invoke(transition);
	}

	public static void InvokeTransitionEnd(TransitionType transition)
	{
		Events.OnTransitionEnd?.Invoke(transition);
	}

	public static void InvokeSetWeatherIntensity(float amount, float updateDuration)
	{
		Events.OnSetWeatherIntensity?.Invoke(amount, updateDuration);
	}

	public static void InvokeGoldCounterStart(GoldDisplay goldDisplay, float addAmount)
	{
		Events.OnGoldCounterStart?.Invoke(goldDisplay, addAmount);
	}

	public static void InvokeEventStart(CampaignNode node, EventRoutine @event)
	{
		Events.OnEventStart?.Invoke(node, @event);
	}

	public static void InvokeEventPopulated(EventRoutine @event)
	{
		Events.OnEventPopulated?.Invoke(@event);
	}

	public static void InvokeMuncherDrag()
	{
		Events.OnMuncherDrag?.Invoke();
	}

	public static void InvokeMuncherDragCancel()
	{
		Events.OnMuncherDragCancel?.Invoke();
	}

	public static void InvokeMuncherFeed(Entity entity)
	{
		Events.OnMuncherFeed?.Invoke(entity);
	}

	public static void InvokeSettingChanged(string key, object value)
	{
		Events.OnSettingChanged?.Invoke(key, value);
	}

	public static void InvokeUINavigationReset()
	{
		Events.OnUINavigationReset?.Invoke();
	}

	public static void InvokeUINavigation()
	{
		Events.OnUINavigation?.Invoke();
	}

	public static void InvokeButtonStyleChanged()
	{
		Events.OnButtonStyleChanged?.Invoke();
	}

	public static void InvokeControllerSwitched()
	{
		Events.OnControllerSwitched?.Invoke();
	}

	public static bool CheckRename(ref Entity entity, ref string newName)
	{
		bool arg = true;
		Events.OnCheckRename?.Invoke(ref entity, ref newName, ref arg);
		return arg;
	}

	public static void InvokeRename(Entity entity, string newName)
	{
		Events.OnRename?.Invoke(entity, newName);
	}

	public static List<DataFile> PullRewards(object pulledBy, string poolName, ref int count)
	{
		List<DataFile> arg = new List<DataFile>();
		Events.OnPullRewards?.Invoke(pulledBy, ref poolName, ref count, ref arg);
		return arg;
	}

	public static void InvokeCardDataCreated(CardData cardData)
	{
		Events.OnCardDataCreated?.Invoke(cardData);
	}

	public static void InvokeTutorialProgress(int value)
	{
		Events.OnTutorialProgress?.Invoke(value);
	}

	public static void InvokeTutorialSkip()
	{
		Events.OnTutorialSkip?.Invoke();
	}

	public static void InvokeChallengeCompletedSaved(ChallengeData challengeData)
	{
		Events.OnChallengeCompletedSaved?.Invoke(challengeData);
	}

	public static int GetHandSize(int baseHandSize)
	{
		int arg = baseHandSize;
		Events.OnGetHandSize?.Invoke(ref arg);
		return arg;
	}

	public static void InvokeModLoaded(WildfrostMod mod)
	{
		Events.OnModLoaded?.Invoke(mod);
	}

	public static void InvokeModUnloaded(WildfrostMod mod)
	{
		Events.OnModUnloaded?.Invoke(mod);
	}
}
