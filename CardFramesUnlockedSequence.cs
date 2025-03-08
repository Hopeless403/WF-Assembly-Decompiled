#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class CardFramesUnlockedSequence : MonoBehaviour
{
	[SerializeField]
	public UnityEngine.Localization.LocalizedString chiseledFrameUnlocked;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString chiseledFramesUnlocked;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString goldFrameUnlocked;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString goldFramesUnlocked;

	[SerializeField]
	public TMP_Text text;

	[SerializeField]
	public CardHand container1;

	[SerializeField]
	public CardHand container2;

	[SerializeField]
	public CardController cardController;

	[SerializeField]
	public GameObject group;

	[Header("Dynamic Positioning Based")]
	[SerializeField]
	public AnimationCurve angleAddBasedOnCount;

	[SerializeField]
	public AnimationCurve zPositionBasedOnCount;

	public const int splitTo2ndContainerThreshold = 3;

	public const float cardScale = 0.67f;

	public IEnumerator Run(int level, string[] cards)
	{
		text.text = ((level != 1) ? ((cards.Length > 1) ? goldFramesUnlocked : goldFrameUnlocked) : ((cards.Length > 1) ? chiseledFramesUnlocked : chiseledFrameUnlocked)).GetLocalizedString().Format(cards.Length);
		yield return CreateCards(cards);
	}

	public IEnumerator CreateCards(string[] cards)
	{
		SetScaleAndPosition(cards.Length);
		Routine.Clump clump = new Routine.Clump();
		int num = ((cards.Length > 3) ? Mathf.CeilToInt((float)cards.Length / 2f) : cards.Length);
		container1.SetSize(num, 0.67f);
		container2.SetSize(cards.Length - num, 0.67f);
		for (int i = 0; i < cards.Length; i++)
		{
			string assetName = cards[i];
			CardHand cardContainer = ((i < num) ? container1 : container2);
			CardData cardData = AddressableLoader.Get<CardData>("CardData", assetName);
			if (cardData != null)
			{
				clump.Add(CreateCard(cardData, cardContainer, startFlipped: false));
			}
		}

		yield return clump.WaitForEnd();
		group.SetActive(value: true);
		container1.SetChildPositions();
		container2.SetChildPositions();
	}

	public void SetScaleAndPosition(int numberOfCards)
	{
		float fanCircleAngleAdd = angleAddBasedOnCount.Evaluate(numberOfCards);
		float value = zPositionBasedOnCount.Evaluate(numberOfCards);
		container1.fanCircleAngleAdd = fanCircleAngleAdd;
		container2.fanCircleAngleAdd = fanCircleAngleAdd;
		container1.transform.localPosition = container1.transform.localPosition.WithZ(value);
		container2.transform.localPosition = container2.transform.localPosition.WithZ(value);
		if (numberOfCards <= 3)
		{
			container1.transform.localPosition = container1.transform.localPosition.WithY(0f);
		}
	}

	public IEnumerator CreateCard(CardData cardData, CardContainer cardContainer, bool startFlipped)
	{
		Card card = CardManager.Get(cardData, cardController, null, inPlay: false, isPlayerCard: true);
		if (startFlipped)
		{
			card.entity.flipper.FlipDownInstant();
		}

		cardContainer.Add(card.entity);
		yield return card.UpdateData();
		if (startFlipped)
		{
			card.entity.flipper.FlipUp(force: true);
		}
	}

	public void End()
	{
		container1.DestroyAll();
		container2.DestroyAll();
		base.gameObject.SetActive(value: false);
		new Routine(SceneManager.Unload("CardFramesUnlocked"));
	}
}
