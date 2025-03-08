#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class CompanionLimitSequence : UISequence
{
	[SerializeField]
	public UnityEngine.Localization.LocalizedString textKey;

	[SerializeField]
	public string overColourHex = "f66";

	[SerializeField]
	public string underColourHex = "fff";

	[SerializeField]
	public TMP_Text title;

	public Character owner;

	public CardController controller;

	public GameObject background;

	public GameObject container;

	public CardContainer activeContainer;

	public CardContainer reserveContainer;

	public Transform continueButtonHolder;

	public Button continueButton;

	public override IEnumerator Run()
	{
		continueButton.interactable = true;
		Routine.Clump clump = new Routine.Clump();
		foreach (CardData item in owner.data.inventory.deck)
		{
			if (item.cardType.name == "Friendly")
			{
				clump.Add(CreateCard(activeContainer, item));
			}
		}

		foreach (CardData item2 in owner.data.inventory.reserve)
		{
			if (item2.cardType.name == "Friendly")
			{
				clump.Add(CreateCard(reserveContainer, item2));
			}
		}

		clump.Add(Sequences.Wait(startDelay));
		yield return clump.WaitForEnd();
		SetPositions();
		Resolve();
		background.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
		container.transform.localScale = Vector3.one * 0.5f;
		base.gameObject.SetActive(value: true);
		LeanTween.cancel(background);
		LeanTween.scale(background, Vector3.one, tweenInDur).setEase(tweenInEase);
		LeanTween.cancel(container);
		LeanTween.scale(container, Vector3.one, tweenInDur).setEase(tweenInEase);
		yield return Sequences.Wait(tweenInDur);
		yield return new WaitUntil(() => promptEnd);
		promptEnd = false;
		LeanTween.cancel(background);
		LeanTween.scale(background, Vector3.zero, tweenOutDur).setEase(tweenOutEase);
		LeanTween.cancel(container);
		LeanTween.scale(container, Vector3.zero, tweenOutDur).setEase(tweenOutEase);
		yield return Sequences.Wait(tweenOutDur);
		DestroyCards(activeContainer);
		DestroyCards(reserveContainer);
		base.gameObject.SetActive(value: false);
	}

	public IEnumerator CreateCard(CardContainer container, CardData data)
	{
		Card card = CardManager.Get(data, controller, owner, inPlay: false, isPlayerCard: true);
		container.Add(card.entity);
		container.SetSize(container.Count, 0.67f);
		yield return card.UpdateData();
	}

	public void Move(Entity entity)
	{
		if (entity.InContainerGroup(activeContainer))
		{
			MoveToReserve(entity);
		}
		else if (entity.InContainerGroup(reserveContainer))
		{
			MoveToDeck(entity);
		}
	}

	public void MoveToDeck(Entity entity)
	{
		if (!entity.InContainerGroup(activeContainer))
		{
			entity.RemoveFromContainers();
			activeContainer.Add(entity);
		}

		if (!owner.data.inventory.deck.Contains(entity.data))
		{
			owner.data.inventory.reserve.Remove(entity.data);
			owner.data.inventory.deck.Add(entity.data);
		}

		Resolve();
		UpdatePositions();
	}

	public void MoveToReserve(Entity entity)
	{
		if (!entity.InContainerGroup(reserveContainer))
		{
			entity.RemoveFromContainers();
			reserveContainer.Add(entity);
		}

		if (!owner.data.inventory.reserve.Contains(entity.data))
		{
			owner.data.inventory.deck.Remove(entity.data);
			owner.data.inventory.reserve.Add(entity.data);
		}

		Resolve();
		UpdatePositions();
	}

	public void Toggle()
	{
		if (!base.gameObject.activeSelf)
		{
			Begin();
		}
		else
		{
			Continue();
		}
	}

	public void Continue()
	{
		promptEnd = true;
		continueButton.interactable = false;
	}

	public void Resolve()
	{
		int count = activeContainer.Count;
		bool flag = count > owner.data.companionLimit;
		string localizedString = textKey.GetLocalizedString();
		title.text = string.Format(localizedString, count, owner.data.companionLimit, flag ? overColourHex : underColourHex);
		continueButtonHolder.gameObject.SetActive(!flag);
	}

	public void SetPositions()
	{
		activeContainer.SetSize(activeContainer.Count, 0.67f);
		reserveContainer.SetSize(reserveContainer.Count, 0.67f);
		activeContainer.SetChildPositions();
		reserveContainer.SetChildPositions();
	}

	public void UpdatePositions()
	{
		activeContainer.SetSize(activeContainer.Count, 0.67f);
		reserveContainer.SetSize(reserveContainer.Count, 0.67f);
		activeContainer.TweenChildPositions();
		reserveContainer.TweenChildPositions();
	}

	public static void DestroyCards(CardContainer container)
	{
		foreach (Entity item in container)
		{
			CardManager.ReturnToPool(item);
		}

		container.Clear();
	}
}
