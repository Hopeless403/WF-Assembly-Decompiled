#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseNewCompanion : UISequence
{
	[SerializeField]
	public UISequence sequence;

	[SerializeField]
	public CardSelector cardSelector;

	[SerializeField]
	public CardController cardController;

	[SerializeField]
	public CardContainer cardContainer;

	[SerializeField]
	public int rewardOptions = 3;

	[Header("Banners")]
	[SerializeField]
	public RectTransform topBanner;

	[SerializeField]
	public RectTransform bottomBanner;

	[SerializeField]
	public RectTransform background;

	[SerializeField]
	public Vector2 bannerTweenDur = new Vector2(0.9f, 1.1f);

	[SerializeField]
	public LeanTweenType bannerTweenEase = LeanTweenType.easeOutBounce;

	[SerializeField]
	public Vector2 bannerTweenOutDur = new Vector2(0.3f, 0.4f);

	[SerializeField]
	public LeanTweenType bannerTweenOutEase = LeanTweenType.easeInBack;

	public CardData[] storedRewards;

	public override IEnumerator Run()
	{
		Populate();
		base.gameObject.SetActive(value: true);
		if (background != null)
		{
			background.gameObject.SetActive(value: true);
			background.localScale = Vector3.zero;
			yield return null;
			background.LeanScale(Vector3.one, 1f).setEase(LeanTweenType.easeOutElastic);
			yield return Sequences.Wait(0.25f);
		}

		if (topBanner != null)
		{
			topBanner.gameObject.SetActive(value: true);
			Vector3 localPosition = topBanner.localPosition;
			topBanner.localPosition = localPosition.WithY(localPosition.y + 5f);
			LeanTween.cancel(topBanner.gameObject);
			LeanTween.moveLocal(topBanner.gameObject, localPosition, bannerTweenDur.PettyRandom()).setEase(bannerTweenEase);
		}

		if (bottomBanner != null)
		{
			bottomBanner.gameObject.SetActive(value: true);
			Vector3 localPosition2 = bottomBanner.localPosition;
			bottomBanner.localPosition = localPosition2.WithY(localPosition2.y - 5f);
			LeanTween.cancel(bottomBanner.gameObject);
			LeanTween.moveLocal(bottomBanner.gameObject, localPosition2, bannerTweenDur.PettyRandom()).setEase(bannerTweenEase);
		}

		sequence.gameObject.SetActive(value: true);
		yield return sequence.Run();
		if (background != null)
		{
			background.LeanScale(Vector3.zero, tweenOutDur).setEase(LeanTweenType.easeInBack);
			yield return Sequences.Wait(tweenOutDur);
		}

		if (topBanner != null)
		{
			LeanTween.cancel(topBanner.gameObject);
			LeanTween.moveLocal(topBanner.gameObject, topBanner.localPosition.WithY(topBanner.localPosition.y + 5f), bannerTweenOutDur.PettyRandom()).setEase(bannerTweenOutEase);
		}

		if (bottomBanner != null)
		{
			LeanTween.cancel(bottomBanner.gameObject);
			LeanTween.moveLocal(bottomBanner.gameObject, bottomBanner.localPosition.WithY(bottomBanner.localPosition.y - 5f), bannerTweenOutDur.PettyRandom()).setEase(bannerTweenOutEase);
		}
	}

	public void Populate()
	{
		Character player = References.Player;
		cardSelector.character = player;
		cardController.owner = player;
		if (storedRewards == null || storedRewards.Length == 0)
		{
			storedRewards = player.GetComponent<CharacterRewards>().Pull<CardData>(this, "Units", rewardOptions);
			Debug.Log("Unit Reward Options: [" + string.Join(", ", (IEnumerable<CardData>)storedRewards) + "]");
		}

		cardContainer.SetSize(storedRewards.Length, 0.8f);
		cardContainer.owner = player;
		Routine.Clump clump = new Routine.Clump();
		CardData[] array = storedRewards;
		for (int i = 0; i < array.Length; i++)
		{
			Card card = CardManager.Get(array[i].Clone(), cardController, player, inPlay: false, isPlayerCard: true);
			card.entity.flipper.FlipDownInstant();
			cardContainer.Add(card.entity);
			clump.Add(card.UpdateData());
		}

		foreach (Entity item in cardContainer)
		{
			Transform obj = item.transform;
			obj.localPosition = cardContainer.GetChildPosition(item);
			obj.localScale = cardContainer.GetChildScale(item);
			obj.localEulerAngles = cardContainer.GetChildRotation(item);
		}
	}
}
