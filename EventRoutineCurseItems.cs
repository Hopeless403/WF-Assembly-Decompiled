#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using Dead;
using FMODUnity;
using UnityEngine;
using UnityEngine.Localization;

public class EventRoutineCurseItems : EventRoutine
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
	public Transform curseCardContainer;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString chooseKey;

	[SerializeField]
	public float cardScale = 0.8f;

	[SerializeField]
	public GameObject backButton;

	[SerializeField]
	public Talker talker;

	[SerializeField]
	public EventReference takeSfxEvent;

	public bool analyticsEventSent;

	public readonly List<Entity> cards = new List<Entity>();

	public readonly List<Entity> curses = new List<Entity>();

	public override IEnumerator Populate()
	{
		Routine.Clump clump = new Routine.Clump();
		string[] saveCollection = base.data.GetSaveCollection<string>("cards");
		cardContainer.SetSize(saveCollection.Length, cardScale);
		for (int i = 0; i < saveCollection.Length; i++)
		{
			string assetName = saveCollection[i];
			CardData cardDataClone = AddressableLoader.Get<CardData>("CardData", assetName).Clone();
			CheckAddUpgrades(i, cardDataClone);
			Card card = CardManager.Get(cardDataClone, cardController, base.player, inPlay: false, isPlayerCard: true);
			cards.Add(card.entity);
			card.entity.flipper.FlipDownInstant();
			cardContainer.Add(card.entity);
			clump.Add(card.UpdateData());
		}

		string[] saveCollection2 = base.data.GetSaveCollection<string>("curses");
		foreach (string text in saveCollection2)
		{
			if (text != null)
			{
				Card card2 = CardManager.Get(AddressableLoader.Get<CardData>("CardData", text).Clone(), cardController, base.player, inPlay: false, isPlayerCard: true);
				card2.entity.uINavigationItem.enabled = false;
				curses.Add(card2.entity);
				card2.transform.SetParent(curseCardContainer);
				clump.Add(card2.UpdateData());
				card2.entity.flipper.FlipDownInstant();
			}
			else
			{
				curses.Add(null);
			}
		}

		yield return clump.WaitForEnd();
		for (int k = 0; k < cardContainer.Count; k++)
		{
			Entity entity = cardContainer[k];
			Transform transform = entity.transform;
			transform.localPosition = cardContainer.GetChildPosition(entity);
			transform.localScale = cardContainer.GetChildScale(entity);
			transform.localEulerAngles = cardContainer.GetChildRotation(entity);
			Entity entity2 = curses[k];
			if ((bool)entity2)
			{
				Transform obj = entity2.transform;
				obj.position = transform.position;
				obj.localScale = Vector3.one * 0.85f;
				obj.localEulerAngles = new Vector3(0f, 0f, 0f - UnityEngine.Random.Range(5f, 10f));
			}
		}
	}

	public override IEnumerator Run()
	{
		int num = base.data.Get("enterCount", 0) + 1;
		base.data["enterCount"] = num;
		if (num == 1)
		{
			talker.Say("greet", PettyRandom.Range(0.5f, 1f));
		}

		sequence.owner = base.player;
		cardController.owner = base.player;
		cardSelector.character = base.player;
		CinemaBarSystem.Top.SetScript(chooseKey.GetLocalizedString());
		if (!base.data.Get("analyticsEventSent", @default: false))
		{
			foreach (Entity item in cardContainer)
			{
				Events.InvokeEntityOffered(item);
			}

			base.data["analyticsEventSent"] = true;
		}

		yield return sequence.Run();
		CinemaBarSystem.Clear();
		if (base.data.Get<SaveCollection<string>>("cards").Count <= 0)
		{
			node.SetCleared();
		}
	}

	public void TrySelect(Entity entity)
	{
		ActionSelect action = new ActionSelect(entity, delegate
		{
			StartCoroutine(TakeCard(entity));
		});
		if (Events.CheckAction(action))
		{
			ActionQueue.Add(action);
		}
	}

	public IEnumerator TakeCard(Entity entity)
	{
		SfxSystem.OneShot(takeSfxEvent);
		cardController.Disable();
		backButton.SetActive(value: false);
		int index = cards.IndexOf(entity);
		Transform transform = entity.transform;
		Entity curse = curses[index];
		if ((bool)curse)
		{
			Transform obj = curse.transform;
			obj.position = transform.position;
			obj.localScale = Vector3.one * cardScale;
			obj.localRotation = Quaternion.identity;
			curse.gameObject.SetActive(value: true);
			curse.flipper.FlipDownInstant();
		}

		cards.RemoveAt(index);
		SaveCollection<string> saveCollection = base.data.Get<SaveCollection<string>>("cards");
		saveCollection.Remove(index);
		base.data["cards"] = saveCollection;
		curses.RemoveAt(index);
		SaveCollection<string> saveCollection2 = base.data.Get<SaveCollection<string>>("curses");
		saveCollection2.Remove(index);
		base.data["curses"] = saveCollection2;
		cardSelector.TakeCard(entity);
		Events.InvokeEntityChosen(entity);
		talker.Say("thanks", 0f, entity.data.title);
		if ((bool)curse)
		{
			yield return new WaitForSeconds(0.5f);
			curse.flipper.FlipUp();
			yield return new WaitForSeconds(0.5f);
			cardSelector.TakeCard(curse);
			Events.InvokeEntityChosen(curse);
		}

		yield return new WaitForSeconds(0.3f);
		cardController.Enable();
		backButton.SetActive(value: true);
	}

	public void Back()
	{
		cardContainer.DestroyAll();
		cardContainer.Clear();
		cards.Clear();
		foreach (Entity curse in curses)
		{
			if ((bool)curse)
			{
				CardManager.ReturnToPool(curse);
			}
		}

		curses.Clear();
		sequence.End();
	}
}
