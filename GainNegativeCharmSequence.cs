#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dead;
using UnityEngine;

public class GainNegativeCharmSequence : MonoBehaviour
{
	[SerializeField]
	public GameObject holderGroup;

	[SerializeField]
	public CardCharmHolder[] holders;

	[SerializeField]
	public CardCharmHolder activeCharmHolder;

	[SerializeField]
	public GameObject cardGridGroup;

	[SerializeField]
	public DeckDisplayGroup deckDisplayGroup;

	[SerializeField]
	public CardController cardController;

	[SerializeField]
	public CardUpgradeData[] charmOptions;

	[SerializeField]
	public CinemaBarShower cinemaBarShower;

	[SerializeField]
	public Fader backgroundFader;

	public bool charmChosen;

	public bool promptEnd;

	public IEnumerator Run()
	{
		CampaignNode campaignNode = Campaign.FindCharacterNode(References.Player);
		UnityEngine.Random.State state = UnityEngine.Random.state;
		UnityEngine.Random.InitState(campaignNode.seed);
		CreateUpgrades();
		UnityEngine.Random.state = state;
		yield return new WaitUntil(() => promptEnd);
		cinemaBarShower.Hide();
		backgroundFader.Out(0.25f);
		yield return new WaitForSeconds(0.5f);
	}

	public void CreateUpgrades()
	{
		List<CardUpgradeData> list = new List<CardUpgradeData>(charmOptions);
		list.Shuffle();
		int num = holders.Length;
		foreach (CardUpgradeData item in list)
		{
			if (UpgradeCanBeAssignedToSomethingInDeck(item))
			{
				CreateUpgrade(item.Clone());
				if (--num <= 0)
				{
					break;
				}
			}
		}
	}

	public bool UpgradeCanBeAssignedToSomethingInDeck(CardUpgradeData upgradeData)
	{
		foreach (CardData item in References.PlayerData.inventory.deck)
		{
			if (upgradeData.CanAssign(item))
			{
				return true;
			}
		}

		return false;
	}

	public void CreateUpgrade(CardUpgradeData upgradeDataClone)
	{
		CardCharmHolder cardCharmHolder = holders.FirstOrDefault((CardCharmHolder a) => a.transform.childCount == 0);
		if ((bool)cardCharmHolder)
		{
			UpgradeDisplay upgrade = cardCharmHolder.Create(upgradeDataClone);
			CardCharmInteraction component = upgrade.GetComponent<CardCharmInteraction>();
			component.popUpOffset = new Vector2(0.8f, -0.25f);
			component.onDrag.AddListener(delegate
			{
				Take(upgrade);
			});
		}
	}

	public void Take(UpgradeDisplay upgrade)
	{
		if (!charmChosen)
		{
			charmChosen = true;
			activeCharmHolder.Create(upgrade.data);
			holderGroup.SetActive(value: false);
			StartCoroutine(OpenCardGrid(upgrade.data));
		}
	}

	public IEnumerator OpenCardGrid(CardUpgradeData upgradeData)
	{
		cardGridGroup.SetActive(value: true);
		Routine.Clump clump = new Routine.Clump();
		foreach (CardData item in References.PlayerData.inventory.deck)
		{
			if (upgradeData.CanAssign(item))
			{
				clump.Add(CreateCard(item));
			}
		}

		yield return clump.WaitForEnd();
		deckDisplayGroup.UpdatePositions();
		yield return ((RectTransform)deckDisplayGroup.transform.parent).FixLayoutGroup();
		yield return FlipCardsUp();
	}

	public void HideCardGrid()
	{
		LeanTween.cancel(cardGridGroup);
		LeanTween.scale(cardGridGroup, Vector3.zero, 0.33f).setEaseInBack();
	}

	public IEnumerator FlipCardsUp()
	{
		foreach (Entity item in deckDisplayGroup.grids[0])
		{
			item.flipper.FlipUp(force: true);
			yield return new WaitForSeconds(PettyRandom.Range(0f, 0.1f));
		}
	}

	public IEnumerator CreateCard(CardData cardData)
	{
		Card card = CardManager.Get(cardData, cardController, References.Player, inPlay: false, isPlayerCard: true);
		deckDisplayGroup.AddCard(card);
		card.entity.flipper.FlipDownInstant();
		yield return card.UpdateData();
	}

	public void End()
	{
		promptEnd = true;
	}
}
