#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CompanionRecoverSequence : UISequence
{
	public Character owner;

	[SerializeField]
	public TweenUI revealTween;

	[SerializeField]
	public TweenUI hideTween;

	[SerializeField]
	public CardContainer container;

	[SerializeField]
	public CardController controller;

	[SerializeField]
	public Button continueButton;

	[SerializeField]
	[Range(0f, 1f)]
	public float cardScale = 0.75f;

	public void OnEnable()
	{
		continueButton.interactable = true;
		controller.Enable();
	}

	public void OnDisable()
	{
		Clear();
	}

	public IEnumerator CreateCards(IEnumerable<CardData> cards)
	{
		Routine.Clump clump = new Routine.Clump();
		foreach (CardData card in cards)
		{
			clump.Add(CreateCard(card));
		}

		yield return clump.WaitForEnd();
	}

	public IEnumerator CreateCard(CardData data)
	{
		Card card = CardManager.Get(data, controller, owner, inPlay: false, isPlayerCard: true);
		container.Add(card.entity);
		container.max = container.Count;
		yield return card.UpdateData();
	}

	public void Clear()
	{
		foreach (Entity item in container)
		{
			CardManager.ReturnToPool(item);
		}

		container.Clear();
	}

	public CardData[] FindRecoveries()
	{
		List<CardData> list = new List<CardData>();
		foreach (CardData item in owner.data.inventory.deck.Where(IsInjured))
		{
			list.Add(item);
		}

		foreach (CardData item2 in owner.data.inventory.reserve.Where(IsInjured))
		{
			list.Add(item2);
		}

		Debug.Log("Injured companions: [" + string.Join(", ", list) + "]");
		CardData[] injuriesThisBattle = InjurySystem.GetInjuriesThisBattle();
		if (injuriesThisBattle.Length != 0)
		{
			list.RemoveMany(injuriesThisBattle);
			object[] values = injuriesThisBattle;
			Debug.Log("[" + string.Join(", ", values) + "] cannot recover since they died last battle!");
		}

		return list.ToArray();
	}

	public static bool IsInjured(CardData card)
	{
		if (card.injuries != null)
		{
			return card.injuries.Count > 0;
		}

		return false;
	}

	public override IEnumerator Run()
	{
		CardData[] array = FindRecoveries();
		if (array.Length != 0)
		{
			Clear();
			RemoveInjuries(array);
			Routine.Clump clump = new Routine.Clump();
			yield return CreateCards(array);
			clump.Add(Sequences.Wait(startDelay));
			yield return clump.WaitForEnd();
			container.SetChildPositions();
			base.gameObject.SetActive(value: true);
			yield return Sequences.Wait(revealTween.GetDuration());
			yield return new WaitUntil(() => promptEnd);
			hideTween.Fire();
			promptEnd = false;
		}
	}

	public static void RemoveInjuries(IEnumerable<CardData> targets)
	{
		foreach (CardData target in targets)
		{
			target.injuries.Clear();
		}
	}
}
