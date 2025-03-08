#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardViewer : MonoBehaviour
{
	[SerializeField]
	public Transform cardHolder;

	[SerializeField]
	public CardData startingData;

	[SerializeField]
	public InspectSystem inspectSystem;

	public int current;

	public List<CardData> cards;

	public void Start()
	{
		cards = (from a in AddressableLoader.GetGroup<CardData>("CardData")
			where a.cardType.name != "Leader" && a.cardType.name != "Boss" && a.mainSprite.name != "Nothing"
			select a).ToList();
		Set(startingData);
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			Set(-1);
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			Set(1);
		}
	}

	public void Set(CardData data)
	{
		current = cards.IndexOf(data);
		StopAllCoroutines();
		StartCoroutine(SetRoutine(data));
	}

	public IEnumerator SetRoutine(CardData data)
	{
		cardHolder.DestroyAllChildren();
		Card card = CardManager.Get(data, null, null, inPlay: false, isPlayerCard: false);
		card.entity.returnToPool = false;
		yield return card.UpdateData();
		card.transform.SetParent(cardHolder);
		card.imageIdleAnimator.FadeIn();
		card.imageIdleAnimator.SetSpeed(1f, 2f / MathF.PI, 0f);
		card.backgroundIdleAnimator.FadeIn();
		card.backgroundIdleAnimator.SetSpeed(1f, 2f / MathF.PI, 0f);
		Transform obj = card.transform;
		obj.localPosition = Vector3.zero;
		obj.localEulerAngles = Vector3.zero;
		obj.localScale = Vector3.one;
		if ((bool)inspectSystem)
		{
			Events.InvokeEntityHover(card.entity);
			inspectSystem.ClearPopups();
			inspectSystem.inspect = card.entity;
			inspectSystem.CreatePopups();
		}
	}

	public void Set(int change)
	{
		int num = (current + change) % cards.Count;
		if (num < 0)
		{
			num += cards.Count;
		}

		Set(cards[num]);
	}
}
