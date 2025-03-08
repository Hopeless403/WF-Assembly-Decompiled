#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using FMODUnity;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FinalBossSequenceSystem : GameSystem
{
	[SerializeField]
	public WispAnimator wispPrefab;

	[SerializeField]
	public Canvas canvas;

	[SerializeField]
	public Image background;

	[SerializeField]
	public ParticleSystem blipFX;

	[SerializeField]
	public ParticleSystem bigBlipFX;

	[SerializeField]
	public AnimationCurve hitMoveCurve;

	[SerializeField]
	public AnimationCurve hitRotateCurve;

	[SerializeField]
	public string sealCard = "LuminVase";

	[SerializeField]
	public Color possessFlashColor;

	[FormerlySerializedAs("flashColor")]
	[SerializeField]
	public Color luminFlashColor;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString continueKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString throwKey;

	[Header("SFX")]
	[SerializeField]
	public EventReference shadeSpawnSfxEvent;

	[SerializeField]
	public EventReference shadeFleeSfxEvent;

	[SerializeField]
	public EventReference shadeMoveSfxEvent;

	[SerializeField]
	public EventReference shadeFlashSfxEvent;

	[SerializeField]
	public EventReference cameraInSfxEvent;

	[SerializeField]
	public EventReference cameraOutHeroSfxEvent;

	[SerializeField]
	public EventReference cameraOutVaseSfxEvent;

	[SerializeField]
	public EventReference shakeHeroSfxEvent;

	[SerializeField]
	public EventReference shakeVaseSfxEvent;

	[SerializeField]
	public EventReference vaseSpawnSfxEvent;

	[SerializeField]
	public EventReference hitHeroSfxEvent;

	[SerializeField]
	public EventReference hitVaseSfxEvent;

	[SerializeField]
	public EventReference pingHeroSfxEvent;

	[SerializeField]
	public EventReference pingVaseSfxEvent;

	[SerializeField]
	public SfxLoop darkLoop;

	[SerializeField]
	public SfxLoop brightLoop;

	[SerializeField]
	public SfxLoop shadeLoop;

	public WispAnimator wisp;

	public bool running;

	public bool blockWisp;

	public Entity leader;

	public Entity blockCard;

	public CampaignNode playerNode;

	public void OnEnable()
	{
		Events.OnEntityKilled += EntityKilled;
		Events.PreBattleEnd += PreBattleEnd;
		playerNode = Campaign.FindCharacterNode(References.Player);
		if (!playerNode.finalNode || !Campaign.Data.GameMode.mainGameMode)
		{
			Object.Destroy(this);
		}
	}

	public void OnDisable()
	{
		Events.OnEntityKilled -= EntityKilled;
		Events.PreBattleEnd -= PreBattleEnd;
	}

	public void EntityKilled(Entity entity, DeathType type)
	{
		if ((bool)References.Battle && entity.data.cardType.miniboss && entity.owner.team == References.Battle.enemy.team && References.Battle.minibosses.Count((Entity a) => a != entity && a.owner.team == References.Battle.enemy.team) <= 0)
		{
			wisp = Object.Instantiate(wispPrefab, entity.transform.position, Quaternion.identity, base.transform);
			wisp.KnockBackFrom(Vector3.zero);
			leader = Battle.GetCards(References.Battle.player).FirstOrDefault((Entity a) => a.data.cardType.miniboss);
			if ((bool)leader && leader.IsAliveAndExists())
			{
				wisp.SetTarget(leader.transform);
			}

			SfxSystem.OneShot(shadeSpawnSfxEvent);
			shadeLoop.Play();
		}
	}

	public async Task PreBattleEnd()
	{
		if (!wisp)
		{
			return;
		}

		if (!wisp.TargetExists())
		{
			wisp.maxSpeed = 0f;
			wisp.gravitate *= 0.5f;
			return;
		}

		if (!playerNode.finalNode)
		{
			StartCoroutine(Flee());
		}
		else
		{
			StartCoroutine(PossessLeader());
		}

		while (running)
		{
			await Task.Delay(25);
		}
	}

	public IEnumerator PossessLeader()
	{
		if (!leader.IsAliveAndExists())
		{
			yield break;
		}

		PauseMenu.Block();
		running = true;
		CardData blockCardData = GetBlockCard();
		blockWisp = blockCardData;
		if (!blockWisp)
		{
			References.LeaderData.SetCustomData("eyes", "frost");
		}

		wisp.maxSpeed = 0f;
		wisp.gravitate *= 0.5f;
		yield return new WaitForSeconds(1f);
		AmbienceSystem.SetParam("shade_visit", 1f);
		SfxSystem.OneShot(cameraInSfxEvent);
		if (blockWisp)
		{
			brightLoop.Play();
		}
		else
		{
			darkLoop.Play();
		}

		CinemaBarSystem.In();
		CinemaBarSystem.SetSortingLayer("Inspect", 1);
		canvas.gameObject.SetActive(value: true);
		leader.transform.SetParent(canvas.transform);
		Vector2 v = new Vector2(1.5f, 2f);
		LeanTween.move(leader.gameObject, new Vector3(-3f, 0.25f, -3f), v.Random()).setEase(LeanTweenType.easeInOutQuart);
		LeanTween.move(wisp.gameObject, new Vector3(3f, 0.25f, -3f), v.Random()).setEase(LeanTweenType.easeInOutQuart);
		wisp.SetSortingLayer("Inspect", 1);
		yield return new WaitForSeconds(v.y);
		if (blockWisp)
		{
			yield return BlockWisp(blockCardData);
		}

		SfxSystem.OneShot(shadeMoveSfxEvent);
		SfxSystem.OneShot(shadeFlashSfxEvent);
		wisp.JumpToTarget();
		wisp.FadeToColour(new Color(0.6f, 0.2f, 1f), 0.5f, 0.4f);
		Events.InvokeScreenRumble(1f, 0f, 0f, 0.1f, 0.9f, 0.001f);
		yield return new WaitForSeconds(1f);
		Events.InvokeScreenShake(1f, 0f);
		wisp.gameObject.Destroy();
		blipFX.transform.position = wisp.transform.position;
		blipFX.Play();
		shadeLoop.Stop();
		Entity hitCard = leader;
		if (blockWisp)
		{
			SfxSystem.OneShot(hitVaseSfxEvent);
			hitCard = blockCard;
			ReturnLeader();
		}
		else
		{
			SfxSystem.OneShot(hitHeroSfxEvent);
		}

		LeanTween.value(base.gameObject, Time.timeScale, 0.1f, 0.05f).setEase(LeanTweenType.linear).setOnUpdate(Events.InvokeTimeScaleChange);
		ScreenFlashSystem.SetDrawOrder("ParticlesFront", 999);
		if (blockWisp)
		{
			ScreenFlashSystem.SetColour(luminFlashColor);
			ScreenFlashSystem.Run(0.25f);
		}
		else
		{
			ScreenFlashSystem.SetColour(possessFlashColor);
			ScreenFlashSystem.SetMaterialAdditive();
			ScreenFlashSystem.Run(0.15f);
		}

		hitCard.wobbler.WobbleRandom();
		hitCard.curveAnimator.Move(new Vector3(-2f, 0f, 0f), hitMoveCurve, 0f, 1f);
		hitCard.curveAnimator.Rotate(new Vector3(0f, 0f, 10f), hitRotateCurve, 1f);
		yield return new WaitForSeconds(0.25f);
		LeanTween.value(base.gameObject, Time.timeScale, 1f, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(Events.InvokeTimeScaleChange);
		yield return new WaitForSeconds(0.25f);
		SfxSystem.OneShot(blockWisp ? shakeVaseSfxEvent : shakeHeroSfxEvent);
		ZoomInOn(hitCard.gameObject);
		Events.InvokeScreenRumble(0f, 1f, 0.4f, 0.3f, 0.2f, 0.1f);
		if (blockWisp)
		{
			LeanTween.value(1f, 0.9f, 1f).setEaseInOutQuart().setOnUpdate(delegate(float a)
			{
				background.color = background.color.WithAlpha(a);
			});
		}

		yield return new WaitForSeconds(1.1f);
		brightLoop.Stop();
		darkLoop.Stop();
		SfxSystem.OneShot(blockWisp ? pingVaseSfxEvent : pingHeroSfxEvent);
		Ping(hitCard);
		if (!blockWisp)
		{
			FrostEyeSystem.Create(leader);
		}

		CinemaBarSystem.Top.SetPrompt(continueKey.GetLocalizedString(), "Select");
		yield return new WaitUntil(InputSystem.IsSelectPressed);
		AmbienceSystem.SetParam("shade_visit", 0f);
		canvas.gameObject.SetActive(value: false);
		if (blockWisp)
		{
			CardManager.ReturnToPool(blockCard);
			SfxSystem.OneShot(cameraOutVaseSfxEvent);
		}
		else
		{
			ReturnLeader();
			SfxSystem.OneShot(cameraOutHeroSfxEvent);
		}

		CinemaBarSystem.Out();
		CinemaBarSystem.SetSortingLayer("CinemaBars");
		running = false;
		PauseMenu.Unblock();
	}

	public CardData GetBlockCard()
	{
		return References.PlayerData.inventory.deck.FirstOrDefault((CardData a) => a.name == sealCard);
	}

	public IEnumerator BlockWisp(CardData blockCardData)
	{
		blockCard = CardManager.Get(blockCardData, leader.display.hover.controller, leader.owner, inPlay: true, isPlayerCard: true).entity;
		yield return blockCard.display.UpdateData();
		SfxSystem.OneShot(vaseSpawnSfxEvent);
		LeanTween.value(base.gameObject, Time.timeScale, 0.1f, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(Events.InvokeTimeScaleChange);
		blockCard.transform.localScale = leader.transform.localScale;
		blockCard.transform.SetParent(canvas.transform);
		blockCard.transform.SetPositionAndRotation(new Vector3(0f, -5f, -2f), Quaternion.identity);
		LeanTween.move(blockCard.gameObject, blockCard.transform.position.WithY(-3f), 0.5f).setIgnoreTimeScale(useUnScaledTime: true).setEaseOutBack();
		blockCard.DrawOrder = 2;
		CinemaBarSystem.Top.SetPrompt(StringExt.Format(throwKey.GetLocalizedString(), blockCardData.title), "Select");
		yield return new WaitUntil(InputSystem.IsSelectPressed);
		CinemaBarSystem.Top.RemovePrompt();
		LeanTween.move(blockCard.gameObject, leader.transform.position.WithX(-2.5f), 1f).setEaseOutQuint();
		LeanTween.move(leader.gameObject, leader.transform.position.WithX(-5f), 0.75f).setEaseOutQuint();
		LeanTween.value(base.gameObject, Time.timeScale, 1f, 0.15f).setEase(LeanTweenType.linear).setOnUpdate(Events.InvokeTimeScaleChange);
	}

	public IEnumerator Flee()
	{
		SfxSystem.OneShot(shadeFleeSfxEvent);
		shadeLoop.Stop();
		running = true;
		wisp.SetTarget(References.Battle.enemy.reserveContainer.transform);
		wisp.JumpToTarget();
		yield return new WaitForSeconds(1f);
		running = false;
	}

	public static void ZoomInOn(GameObject target, float zPos = -2f)
	{
		LeanTween.cancel(target);
		LeanTween.moveLocal(target, Vector3.zero.WithZ(zPos), 0.5f).setEase(LeanTweenType.easeInOutQuad);
		LeanTween.rotateLocal(target, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInOutQuad);
	}

	public void Ping(Entity entity)
	{
		entity.curveAnimator.Ping();
		entity.wobbler.WobbleRandom(2f);
		Events.InvokeScreenShake(2f, 0f);
		bigBlipFX.transform.position = entity.transform.position;
		bigBlipFX.Play();
	}

	public void ReturnLeader()
	{
		leader.transform.SetParent(leader.actualContainers[0].holder);
		leader.TweenToContainer();
		leader.wobbler.WobbleRandom();
	}
}
