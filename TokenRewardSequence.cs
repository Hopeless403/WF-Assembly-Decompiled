#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenRewardSequence : UISequence
{
	public class Target
	{
		public Entity entity;

		public CardContainer[] previousActualContainers;

		public CardController previousController;

		public Target(Entity entity, CardController controller)
		{
			this.entity = entity;
			previousActualContainers = new CardContainer[1] { entity.owner.drawContainer };
			Card component = entity.GetComponent<Card>();
			previousController = component.hover.controller;
			component.hover.controller = controller;
		}

		public void MoveTo(params CardContainer[] containers)
		{
			entity.RemoveFromContainers();
			for (int i = 0; i < containers.Length; i++)
			{
				containers[i].Add(entity);
			}
		}

		public void Return()
		{
			MoveTo(previousActualContainers);
			CardContainer[] array = previousActualContainers;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].TweenChildPositions();
			}

			entity.GetComponent<Card>().hover.controller = previousController;
		}

		public override string ToString()
		{
			if (!(entity == null))
			{
				return entity.name;
			}

			return "NULL";
		}
	}

	[SerializeField]
	public CanvasGroup canvasGroup;

	[SerializeField]
	public RectTransform titleTransform;

	[SerializeField]
	public Vector3 titleToPosition;

	[SerializeField]
	public TweenUI revealTween;

	[SerializeField]
	public TweenUI hideTween;

	[SerializeField]
	public CardContainer cardHolder;

	[SerializeField]
	public Button skipButton;

	[SerializeField]
	public int goldGainFromSkip = 10;

	[SerializeField]
	public CardController cardController;

	[SerializeField]
	public CardCharmDragHandler tokenDragHandler;

	[SerializeField]
	public RectTransform tokenSlotGroup;

	[SerializeField]
	public CardCharmHolder tokenSlotPrefab;

	[SerializeField]
	public CardUpgradeData[] tokenData;

	public List<Target> targets;

	public override IEnumerator Run()
	{
		Character player = References.Player;
		List<Entity> cardsOnBoard = Battle.GetCardsOnBoard(player);
		if (targets == null)
		{
			targets = new List<Target>();
		}

		foreach (Entity item in cardsOnBoard)
		{
			if (item.data?.cardType?.name == "Friendly")
			{
				targets.Add(new Target(item, cardController));
			}
		}

		Debug.Log(string.Format("{0} Targets: [{1}]", this, string.Join(", ", targets)));
		if (targets.Count <= 0)
		{
			yield break;
		}

		cardController.owner = player;
		LeanTween.moveLocal(titleTransform.gameObject, titleToPosition, 0.75f).setEase(LeanTweenType.easeOutBack);
		yield return Sequences.Wait(0.33f);
		base.gameObject.SetActive(value: true);
		revealTween.Fire();
		tokenDragHandler.gameObject.SetActive(value: true);
		CardUpgradeData[] array = tokenData;
		foreach (CardUpgradeData obj in array)
		{
			CardCharmHolder cardCharmHolder = Object.Instantiate(tokenSlotPrefab, tokenSlotGroup);
			cardCharmHolder.gameObject.SetActive(value: true);
			CardUpgradeData upgradeData = obj.Clone();
			UpgradeDisplay token = cardCharmHolder.Create(upgradeData);
			CardCharmInteraction component = token.GetComponent<CardCharmInteraction>();
			component.canHover = true;
			component.canDrag = true;
			component.onDrag.AddListener(delegate
			{
				tokenDragHandler.Drag(token);
			});
		}

		cardHolder.gameObject.SetActive(value: true);
		cardHolder.SetSize(targets.Count, cardHolder.CardScale);
		foreach (Target target2 in targets)
		{
			target2.MoveTo(cardHolder);
			target2.entity.wobbler?.WobbleRandom();
			cardHolder.TweenChildPositions();
			target2.entity.DrawOrder = 0;
			yield return Sequences.Wait(0.2f);
		}

		yield return Sequences.Wait(0.15f);
		for (int j = targets.Count - 1; j >= 0; j--)
		{
			Target target = targets[j];
			target.entity.curveAnimator.Ping();
			CoroutineManager.Start(target.entity.Reset());
			yield return Sequences.Wait(0.2f);
		}

		skipButton.gameObject.SetActive(value: true);
		yield return new WaitUntil(() => promptEnd);
		skipButton.gameObject.SetActive(value: false);
		for (int j = targets.Count - 1; j >= 0; j--)
		{
			targets[j].Return();
			yield return Sequences.Wait(0.167f);
		}

		targets = null;
		cardHolder.gameObject.SetActive(value: false);
		tokenDragHandler.gameObject.SetActive(value: false);
		hideTween.Fire();
		yield return Sequences.Wait(hideTween.GetDuration());
		base.gameObject.SetActive(value: false);
	}

	public void TokenAssigned()
	{
		StartCoroutine(TokenAssignedRoutine());
	}

	public IEnumerator TokenAssignedRoutine()
	{
		yield return Sequences.Wait(0.1f);
		End();
		skipButton.interactable = false;
	}

	public void Skip()
	{
		References.Player.GainGold(goldGainFromSkip);
		End();
		skipButton.interactable = false;
	}
}
