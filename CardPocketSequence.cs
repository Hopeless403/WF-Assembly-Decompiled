#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dead;
using UnityEngine;
using UnityEngine.UI;

public class CardPocketSequence : UISequence
{
	public class Card
	{
		public readonly Entity entity;

		public CardContainer[] preContainers;

		public CardController preController;

		public bool preInPlay;

		public bool preEnabled;

		public Card(Entity entity)
		{
			this.entity = entity;
		}

		public void Reset()
		{
			entity.inPlay = preInPlay;
			entity.enabled = preEnabled;
		}

		public void Take()
		{
			preContainers = entity.containers;
			preController = entity.display?.hover?.controller;
			preInPlay = entity.inPlay;
			preEnabled = entity.enabled;
			entity.RemoveFromContainers();
			entity.inPlay = false;
		}

		public void Return()
		{
			entity.RemoveFromContainers();
			entity.enabled = preEnabled;
			CardContainer[] array = preContainers;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Add(entity);
			}

			CardHover cardHover = entity.display?.hover;
			if ((object)cardHover != null)
			{
				cardHover.controller = preController;
			}

			entity.inPlay = preInPlay;
		}

		public void ReturnTween()
		{
			entity.TweenToContainer();
		}

		public override string ToString()
		{
			return entity.name;
		}
	}

	[Header("Custom Values")]
	public CardController cardController;

	public Image background;

	public CardContainer container;

	[SerializeField]
	public Vector2 shuffleDelay = new Vector2(0.01f, 0.02f);

	[SerializeField]
	public GameObject backButton;

	[Header("Initial card position/rotation offset")]
	[SerializeField]
	public Vector3 randomOffset;

	[SerializeField]
	public Vector3 randomRotation;

	public readonly List<Card> cards = new List<Card>();

	public void AddCards(CardContainer from)
	{
		foreach (Entity item in from)
		{
			cards.Add(new Card(item));
		}

		foreach (Card item2 in cards.OrderBy((Card _) => PettyRandom.Range(0f, 1f)))
		{
			item2.Take();
			CardHover cardHover = item2.entity.display?.hover;
			if ((object)cardHover != null)
			{
				cardHover.controller = cardController;
			}

			container.Add(item2.entity);
		}
	}

	public IEnumerator ReturnCards()
	{
		foreach (Card card in cards)
		{
			card.Return();
			card.ReturnTween();
		}

		cards.Clear();
		yield break;
	}

	public override IEnumerator Run()
	{
		cardController.enabled = false;
		base.gameObject.SetActive(value: true);
		Routine.Clump clump = new Routine.Clump();
		foreach (Entity item in container.OrderBy((Entity _) => PettyRandom.Range(0f, 1f)))
		{
			clump.Add(MoveCard(item));
			yield return Sequences.Wait(shuffleDelay.PettyRandom());
		}

		backButton.SetActive(value: true);
		yield return clump.WaitForEnd();
		cardController.enabled = true;
		yield return new WaitUntil(() => promptEnd);
		promptEnd = false;
		cardController.UnHover();
		backButton.SetActive(value: false);
		cardController.enabled = false;
		foreach (Card card in cards)
		{
			card.Reset();
		}

		yield return ReturnCards();
		base.gameObject.SetActive(value: false);
	}

	public IEnumerator MoveCard(Entity entity)
	{
		Move(entity, includeRandomness: true);
		yield return Sequences.Wait(PettyRandom.Range(0.01f, 0.3f));
		entity.flipper.FlipUp();
		entity.enabled = true;
	}

	public void FixPosition(Entity entity)
	{
		Move(entity, includeRandomness: false);
	}

	public void Move(Entity entity, bool includeRandomness)
	{
		if (entity.transform.parent == container.holder)
		{
			Vector3 childPosition = container.GetChildPosition(entity);
			Vector3 childRotation = container.GetChildRotation(entity);
			Vector3 childScale = container.GetChildScale(entity);
			if (includeRandomness)
			{
				Vector3 a = PettyRandom.Vector3();
				childPosition += Vector3.Scale(a, randomOffset);
				childRotation += Vector3.Scale(a, randomRotation);
			}

			LeanTween.cancel(entity.gameObject);
			LeanTween.moveLocal(entity.gameObject, childPosition, container.movementDurRand.PettyRandom()).setEase(container.movementEase);
			LeanTween.rotateLocal(entity.gameObject, childRotation, container.movementDurRand.PettyRandom()).setEase(container.movementEase);
			LeanTween.scale(entity.gameObject, childScale, container.scaleDurRand.PettyRandom()).setEase(container.scaleEase);
		}
	}

	public void OpenFrom(Transform transform)
	{
		SfxSystem.OneShot((transform.position.x > 0f) ? "event:/sfx/inventory/deck_opening_right" : "event:/sfx/inventory/deck_opening_left");
	}
}
