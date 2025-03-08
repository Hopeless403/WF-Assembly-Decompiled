#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;

public class EventRoutineCompanion : EventRoutine, IRerollable
{
	[SerializeField]
	public ChooseNewCardSequence sequence;

	[SerializeField]
	public InspectNewUnitSequence inspectSequence;

	[SerializeField]
	public CardController cardController;

	[SerializeField]
	public CardSelector cardSelector;

	[SerializeField]
	public CardContainer cardContainer;

	[SerializeField]
	public UnityEngine.Animator animator;

	[SerializeField]
	public ParticleSystem chunkParticles;

	[SerializeField]
	public ParticleSystem chunkBigParticles;

	[SerializeField]
	public ParticleSystem breakFx;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString breakKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString chooseKey;

	[SerializeField]
	public SfxLoop loop1;

	[SerializeField]
	public SfxLoop loop2;

	public bool analyticsEventSent;

	public bool broken => base.data.Get<int>("damage") > 3;

	public void Hit(BaseEventData eventData)
	{
		if ((!(eventData is PointerEventData pointerEventData) || pointerEventData.button == PointerEventData.InputButton.Left) && !broken)
		{
			base.data["damage"] = base.data.Get<int>("damage") + 1;
			UpdateAnimator();
			TakeHit();
		}
	}

	public void UpdateAnimator()
	{
		animator.SetInteger("Damage", base.data.Get<int>("damage"));
		animator.SetBool("Broken", broken);
	}

	public void TakeHit()
	{
		animator.SetTrigger("Hit");
		if (broken)
		{
			Events.InvokeScreenShake(5f, 0f);
			if (chunkParticles != null)
			{
				chunkParticles.Play();
			}

			if (chunkBigParticles != null)
			{
				chunkBigParticles.Play();
			}

			if (breakFx != null)
			{
				breakFx.Play();
			}

			ScreenFlashSystem.SetDrawOrder("Transition", 0);
			ScreenFlashSystem.SetColour(Color.white.WithAlpha(0.45f));
			ScreenFlashSystem.Run(0.175f);
			CinemaBarSystem.Top.SetPrompt(chooseKey.GetLocalizedString(), "Select");
			SfxSystem.OneShot("event:/sfx/location/travelers/break");
		}
		else
		{
			Events.InvokeScreenShake(1f, 0f);
			if (chunkParticles != null)
			{
				chunkParticles.Play();
			}

			SfxSystem.OneShot("event:/sfx/location/travelers/hit");
		}
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

		UpdateAnimator();
		if (broken)
		{
			CinemaBarSystem.Clear();
		}
		else
		{
			CinemaBarSystem.Top.SetPrompt(breakKey.GetLocalizedString(), "Select");
		}
	}

	public override IEnumerator Run()
	{
		cardController.owner = base.player;
		cardSelector.character = base.player;
		loop1.Play();
		bool loop2Started = false;
		while (!broken)
		{
			if (!loop2Started && base.data.Get<int>("damage") > 2)
			{
				loop2.Play();
				loop2Started = true;
			}

			yield return null;
		}

		loop1.Stop();
		loop2.Stop();
		if (!analyticsEventSent)
		{
			foreach (Entity item in cardContainer)
			{
				Events.InvokeEntityOffered(item);
			}

			analyticsEventSent = true;
		}

		yield return Sequences.Wait(0.3f);
		yield return sequence.Run();
		CinemaBarSystem.Clear();
		node.SetCleared();
	}

	public void TrySelect(Entity entity)
	{
		ActionSelect action = new ActionSelect(entity, delegate
		{
			inspectSequence.SetUnit(entity);
			inspectSequence.Begin();
			cardController.enabled = false;
			cardController.UnHover(entity);
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

	public void CardSelected(Entity entity)
	{
		sequence.End();
		cardController.enabled = false;
		Events.InvokeEntityChosen(entity);
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

		CardData[] list = cardController.owner.GetComponent<CharacterRewards>().Pull<CardData>(node, "Units", base.data.GetSaveCollection<string>("cards").Length);
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
