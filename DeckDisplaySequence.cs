#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeckDisplaySequence : UISequence
{
	public Character owner;

	public Transform background;

	public RectTransform container;

	public DeckDisplayGroup activeCardsGroup;

	public DeckDisplayGroup reserveCardsGroup;

	[SerializeField]
	public CardCharmHolder charmHolder;

	[SerializeField]
	public CrownHolder crownHolder;

	[SerializeField]
	public CardCharmDragHandler charmDragHandler;

	[SerializeField]
	public RectTransform borderLeft;

	[SerializeField]
	public RectTransform borderRight;

	[SerializeField]
	public ScrollRect scrollRect;

	[SerializeField]
	public CardController cardController;

	public override IEnumerator Run()
	{
		yield return Sequences.Wait(startDelay);
		container.localScale = Vector3.one;
		StopWatch.Start();
		Routine.Clump clump = new Routine.Clump();
		activeCardsGroup.Clear();
		foreach (CardData item in owner.data.inventory.deck)
		{
			clump.Add(activeCardsGroup.CreateCard(item));
		}

		yield return clump.WaitForEnd();
		activeCardsGroup.UpdatePositions();
		reserveCardsGroup.Clear();
		foreach (CardData item2 in owner.data.inventory.reserve)
		{
			clump.Add(reserveCardsGroup.CreateCard(item2));
		}

		yield return clump.WaitForEnd();
		reserveCardsGroup.UpdatePositions();
		charmHolder.Clear();
		crownHolder.Clear();
		foreach (CardUpgradeData upgrade in owner.data.inventory.upgrades)
		{
			switch (upgrade.type)
			{
				case CardUpgradeData.Type.Charm:
					charmHolder.Create(upgrade);
					break;
				case CardUpgradeData.Type.Crown:
					crownHolder.Create(upgrade);
					break;
			}
		}

		charmHolder.SetPositions();
		crownHolder.SetPositions();
		Debug.Log($"DECK CARDS CREATED ({StopWatch.Stop()}ms)");
		background.transform.localScale = Vector3.one;
		container.transform.localScale = Vector3.one * 0.5f;
		base.gameObject.SetActive(value: true);
		cardController.Enable();
		yield return FixLayoutsRoutine();
		LeanTween.cancel(background.gameObject);
		LeanTween.scale(background.gameObject, new Vector3(60f, 60f, 1f), 1.5f).setEase(tweenInEase);
		LeanTween.cancel(container.gameObject);
		LeanTween.scale(container.gameObject, Vector3.one, tweenInDur).setEase(tweenInEase);
		float time = 0.5f;
		LeanTweenType ease = LeanTweenType.easeOutBack;
		float delay = 0.1f;
		float borderFrom = 4f;
		LeanTween.cancel(borderLeft);
		borderLeft.anchoredPosition3D = new Vector3(0f - borderFrom, 0f, 0f);
		LeanTween.move(borderLeft, new Vector3(-0.7f, 0f, 0f), time).setEase(ease).setDelay(delay);
		LeanTween.cancel(borderRight);
		borderRight.anchoredPosition3D = new Vector3(borderFrom, 0f, 0f);
		LeanTween.move(borderRight, new Vector3(0.7f, 0f, 0f), time).setEase(ease).setDelay(delay);
		yield return null;
		scrollRect.normalizedPosition = new Vector2(0.5f, 1f);
		yield return new WaitUntil(() => promptEnd);
		promptEnd = false;
		cardController.Disable();
		if (charmDragHandler.IsDragging)
		{
			charmDragHandler.CancelDrag();
		}

		CardCharmInteraction[] componentsInChildren = charmHolder.GetComponentsInChildren<CardCharmInteraction>();
		foreach (CardCharmInteraction obj in componentsInChildren)
		{
			obj.UnHover();
			obj.canHover = false;
		}

		componentsInChildren = crownHolder.GetComponentsInChildren<CardCharmInteraction>();
		foreach (CardCharmInteraction obj2 in componentsInChildren)
		{
			obj2.UnHover();
			obj2.canHover = false;
		}

		NavigationState.Start(new NavigationStateWait(disableInput: true));
		LeanTween.cancel(background.gameObject);
		LeanTween.scale(background.gameObject, Vector3.zero, tweenOutDur).setEase(tweenOutEase);
		LeanTween.cancel(container.gameObject);
		LeanTween.scale(container.gameObject, Vector3.zero, tweenOutDur).setEase(tweenOutEase);
		LeanTween.cancel(borderLeft);
		LeanTween.move(borderLeft, new Vector3(0f - borderFrom, 0f, 0f), tweenOutDur).setEase(tweenOutEase);
		LeanTween.cancel(borderRight);
		LeanTween.move(borderRight, new Vector3(borderFrom, 0f, 0f), tweenOutDur).setEase(tweenOutEase);
		yield return Sequences.Wait(tweenOutDur);
		activeCardsGroup.Clear();
		reserveCardsGroup.Clear();
		charmHolder.Clear();
		crownHolder.Clear();
		NavigationState.BackToPreviousState();
		base.gameObject.SetActive(value: false);
	}

	public IEnumerator FixLayoutsRoutine()
	{
		yield return ((RectTransform)activeCardsGroup.transform.parent).FixLayoutGroup();
	}

	public IEnumerator FixLayoutsRoutinePreserveScroll()
	{
		Vector2 scrollPos = scrollRect.normalizedPosition;
		yield return FixLayoutsRoutine();
		yield return null;
		scrollRect.normalizedPosition = scrollPos;
	}

	public void UpdatePositions()
	{
		CardContainerGrid[] grids = activeCardsGroup.grids;
		for (int i = 0; i < grids.Length; i++)
		{
			grids[i].TweenChildPositions();
		}

		grids = reserveCardsGroup.grids;
		for (int i = 0; i < grids.Length; i++)
		{
			grids[i].TweenChildPositions();
		}

		StartCoroutine(FixLayoutsRoutinePreserveScroll());
	}
}
