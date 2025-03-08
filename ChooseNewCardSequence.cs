#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using Dead;
using UnityEngine;
using UnityEngine.UI;

public class ChooseNewCardSequence : UISequence
{
	public Character owner;

	public RectTransform background;

	public float cardFlipDelay = 0.5f;

	public float cardFlipPauseBetween = 0.25f;

	[SerializeField]
	public GameObject cardGroupLayout;

	[SerializeField]
	public GameObject skipButtonLayout;

	public CardController cardController;

	public CardContainer cardContainer;

	public CardSelector cardSelector;

	[SerializeField]
	public Button skipButton;

	[SerializeField]
	public bool canSkip;

	[SerializeField]
	public int skipGold = 20;

	[SerializeField]
	public TweenUI endTween;

	public override IEnumerator Run()
	{
		cardGroupLayout.SetActive(value: false);
		if ((bool)skipButtonLayout)
		{
			skipButtonLayout.SetActive(value: false);
		}

		if ((bool)background)
		{
			background.gameObject.SetActive(value: false);
		}

		yield return Sequences.Wait(startDelay);
		base.gameObject.SetActive(value: true);
		if ((bool)background)
		{
			background.gameObject.SetActive(value: true);
			background.localScale = Vector3.zero;
			yield return null;
			background.LeanScale(Vector3.one, 1f).setEase(LeanTweenType.easeOutElastic);
			yield return Sequences.Wait(0.25f);
		}

		cardGroupLayout.SetActive(value: true);
		if (canSkip && (bool)skipButtonLayout)
		{
			skipButtonLayout.SetActive(value: true);
		}

		yield return Sequences.Wait(cardFlipDelay);
		if ((bool)cardContainer)
		{
			int dir = Dead.Random.Sign();
			int cardCount = cardContainer.Count;
			for (int i = 0; i < cardCount; i++)
			{
				cardContainer[(dir == 1) ? i : (cardCount - 1 - i)].flipper.FlipUp();
				yield return Sequences.Wait(cardFlipPauseBetween);
			}
		}

		while (!promptEnd)
		{
			yield return null;
		}

		cardController.Disable();
		if ((bool)skipButton)
		{
			skipButton.interactable = false;
		}

		if ((bool)background)
		{
			background.LeanScale(Vector3.zero, tweenOutDur).setEase(LeanTweenType.easeInBack);
		}

		if ((bool)endTween)
		{
			endTween.Fire();
			yield return new WaitForSeconds(endTween.GetDuration());
		}

		foreach (Entity item in cardContainer)
		{
			if ((bool)item && (bool)item.gameObject)
			{
				CardManager.ReturnToPool(item);
			}
		}

		yield return SceneManager.WaitUntilUnloaded("CardCombine");
		base.gameObject.SetActive(value: false);
	}

	public void Skip()
	{
		if (skipGold > 0 && (bool)owner?.data?.inventory)
		{
			Events.InvokeDropGold(skipGold, "SkipReward", owner, skipButton.transform.position);
		}

		cardController.Disable();
		End();
		skipButton.interactable = false;
	}

	public void TakeFirstCard()
	{
		if (!promptEnd)
		{
			cardSelector.TakeFirstCard(cardContainer);
			cardController.Disable();
		}
	}
}
