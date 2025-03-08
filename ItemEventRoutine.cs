#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.UI;

public class ItemEventRoutine : EventRoutine, IRerollable
{
	[SerializeField]
	public ChooseNewCardSequence sequence;

	[SerializeField]
	public CardController cardController;

	[SerializeField]
	public CardSelector cardSelector;

	[SerializeField]
	public CardContainer cardContainer;

	[SerializeField]
	public SpriteRenderer backgroundImage;

	[SerializeField]
	public UnityEngine.Animator animator;

	[SerializeField]
	public ParticleSystem pulseParticleSystem;

	[SerializeField]
	public Button skipButton;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString openKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString chooseKey;

	[SerializeField]
	public SfxLoop loop;

	[SerializeField]
	public GameObject backButton;

	public bool cardTaken;

	public bool showCards;

	public bool promptOpen;

	public bool analyticsEventSent;

	public bool IsOpen => base.data.Get<bool>("open");

	public void RunOpenRoutine(BaseEventData eventData)
	{
		if (!(eventData is PointerEventData pointerEventData) || pointerEventData.button == PointerEventData.InputButton.Left)
		{
			promptOpen = true;
		}
	}

	public IEnumerator OpenRoutine()
	{
		DeckpackBlocker.Block();
		SfxSystem.OneShot("event:/sfx/location/item_chest/claim");
		base.data["open"] = true;
		animator.SetBool("Open", value: true);
		Events.InvokeScreenRumble(0f, 1f, 0.05f, 0.7f, 0.2f, 0.05f);
		yield return Sequences.Wait(1f);
		Events.InvokeScreenShake(5f, 0f);
		animator.SetBool("Zoom", value: true);
		yield return null;
		Open();
		DeckpackBlocker.Unblock();
	}

	public void Open()
	{
		showCards = true;
		CinemaBarSystem.Top.SetPrompt(chooseKey.GetLocalizedString(), "");
		base.data["open"] = true;
		Image component = backgroundImage.GetComponent<Image>();
		if ((object)component != null)
		{
			component.enabled = false;
		}

		pulseParticleSystem?.Stop();
		UINavigationDefaultSystem.SetStartingItem();
		if ((bool)backButton)
		{
			backButton.SetActive(value: true);
		}
	}

	public void Close()
	{
		showCards = false;
		animator.SetBool("Open", value: false);
		CinemaBarSystem.Top.SetPrompt(openKey.GetLocalizedString(), "Select");
		Image component = backgroundImage.GetComponent<Image>();
		if ((object)component != null)
		{
			component.enabled = true;
		}

		pulseParticleSystem?.Play();
	}

	public override IEnumerator Populate()
	{
		string[] saveCollection = base.data.GetSaveCollection<string>("cards");
		cardContainer.SetSize(saveCollection.Length, 0.8f);
		Routine.Clump clump = new Routine.Clump();
		for (int i = 0; i < saveCollection.Length; i++)
		{
			string assetName = saveCollection[i];
			CardData cardDataClone = AddressableLoader.Get<CardData>("CardData", assetName).Clone();
			CheckAddUpgrades(i, cardDataClone);
			Card card = CardManager.Get(cardDataClone, cardController, base.player, inPlay: false, isPlayerCard: true);
			if (!cardContainer.gameObject.activeInHierarchy)
			{
				card.entity.flipper.FlipDownInstant();
			}

			cardContainer.Add(card.entity);
			clump.Add(card.UpdateData());
		}

		yield return clump.WaitForEnd();
		foreach (Entity item in cardContainer)
		{
			Transform obj = item.transform;
			obj.localPosition = cardContainer.GetChildPosition(item);
			obj.localScale = cardContainer.GetChildScale(item);
			obj.localEulerAngles = cardContainer.GetChildRotation(item);
		}

		if (base.data.Get<bool>("open"))
		{
			Open();
			animator.SetBool("Zoom", value: true);
		}
		else
		{
			Close();
		}
	}

	public override IEnumerator Run()
	{
		sequence.owner = base.player;
		cardController.owner = base.player;
		cardSelector.character = base.player;
		if (!base.data.Get<bool>("open"))
		{
			loop.Play();
		}

		while (!base.data.Get<bool>("open"))
		{
			if (promptOpen)
			{
				promptOpen = false;
				if (!base.data.Get<bool>("open"))
				{
					loop.Stop();
					yield return OpenRoutine();
				}
			}

			yield return null;
		}

		if (!analyticsEventSent)
		{
			foreach (Entity item in cardContainer)
			{
				Events.InvokeEntityOffered(item);
			}

			analyticsEventSent = true;
		}

		yield return sequence.Run();
		CinemaBarSystem.Clear();
		if (cardTaken)
		{
			node.SetCleared();
		}
	}

	public void TrySelect(Entity entity)
	{
		if (cardTaken)
		{
			return;
		}

		ActionSelect action = new ActionSelect(entity, delegate
		{
			cardSelector.TakeCard(entity);
			cardController.Disable();
			if ((bool)skipButton)
			{
				skipButton.interactable = false;
			}

			if ((bool)backButton)
			{
				backButton.SetActive(value: false);
			}

			cardTaken = true;
			Events.InvokeEntityChosen(entity);
		});
		if (Events.CheckAction(action))
		{
			ActionQueue.Add(action);
		}
	}

	public void TrySkip()
	{
		ActionSelect action = new ActionSelect(null, delegate
		{
			sequence.Skip();
		});
		if (Events.CheckAction(action))
		{
			ActionQueue.Add(action);
		}
	}

	public bool Reroll()
	{
		if (!cardContainer.gameObject.activeInHierarchy || !cardSelector.enabled || InspectSystem.IsActive())
		{
			return false;
		}

		InspectNewUnitSequence inspectNewUnitSequence = Object.FindObjectOfType<InspectNewUnitSequence>();
		if ((object)inspectNewUnitSequence != null && inspectNewUnitSequence.gameObject.activeSelf)
		{
			return false;
		}

		CardData[] list = cardController.owner.GetComponent<CharacterRewards>().Pull<CardData>(node, "Items", base.data.GetSaveCollection<string>("cards").Length);
		base.data["cards"] = list.ToSaveCollectionOfNames();
		foreach (Entity item in cardContainer)
		{
			CardManager.ReturnToPool(item);
		}

		cardContainer.Clear();
		StartCoroutine(Populate());
		CardPopUp.Clear();
		return true;
	}
}
