#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dead;
using DeadExtensions;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class CombineCardSequence : MonoBehaviour
{
	[SerializeField]
	public Fader fader;

	[SerializeField]
	public Graphic flash;

	[SerializeField]
	public AnimationCurve flashCurve;

	[SerializeField]
	public AnimationCurve bounceCurve;

	[SerializeField]
	public Transform group;

	[SerializeField]
	public Transform pointPrefab;

	[SerializeField]
	public ParticleSystem ps;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString titleKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString continueKey;

	[SerializeField]
	public CardSelector cardSelector;

	[SerializeField]
	public ParticleSystem hitPs;

	[SerializeField]
	public GameObject combinedFx;

	[SerializeField]
	public Transform finalEntityParent;

	public readonly List<Transform> points = new List<Transform>();

	public const float cardScale = 0.8f;

	public const float finalCardScale = 1f;

	public IEnumerator Run(string[] cardsToCombine, string resultingCard)
	{
		CardData[] array = new CardData[cardsToCombine.Length];
		CardData cardDataClone = AddressableLoader.GetCardDataClone(resultingCard);
		for (int i = 0; i < cardsToCombine.Length; i++)
		{
			string cardName = cardsToCombine[i];
			CardData cardData = References.PlayerData.inventory.deck.FirstOrDefault((CardData a) => a.name == cardName);
			if ((bool)cardData)
			{
				array[i] = cardData;
			}
		}

		yield return Run(array, cardDataClone);
	}

	public IEnumerator Run(CardData[] cards, CardData finalCard)
	{
		CinemaBarSystem.State cinemaBarState = new CinemaBarSystem.State();
		PauseMenu.Block();
		CinemaBarSystem.SetSortingLayer("UI2", 100);
		CinemaBarSystem.In();
		Entity[] entities = CreateEntities(cards);
		Entity finalEntity = CreateFinalEntity(finalCard);
		Routine.Clump clump = new Routine.Clump();
		Entity[] array = entities;
		foreach (Entity entity in array)
		{
			clump.Add(entity.display.UpdateData());
		}

		clump.Add(finalEntity.display.UpdateData());
		clump.Add(Sequences.Wait(0.5f));
		yield return clump.WaitForEnd();
		array = entities;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].transform.localScale = Vector3.one * 0.8f;
		}

		array = entities;
		foreach (Entity entity2 in array)
		{
			References.PlayerData.inventory.deck.Remove(entity2.data);
		}

		References.PlayerData.inventory.deck.Add(finalEntity.data);
		fader.In();
		Vector3 zero = Vector3.zero;
		array = entities;
		foreach (Entity entity3 in array)
		{
			zero += entity3.transform.position;
		}

		zero /= (float)entities.Length;
		group.position = zero;
		array = entities;
		foreach (Entity entity4 in array)
		{
			Transform transform = UnityEngine.Object.Instantiate(pointPrefab, entity4.transform.position, Quaternion.identity, group);
			transform.gameObject.SetActive(value: true);
			entity4.transform.SetParent(transform);
			points.Add(transform);
			LeanTween.alphaCanvas(((Card)entity4.display).canvasGroup, 1f, 0.4f).setEaseInQuad();
		}

		foreach (Transform point in points)
		{
			LeanTween.moveLocal(to: point.localPosition.normalized, gameObject: point.gameObject, time: 0.4f).setEaseInQuart();
		}

		yield return new WaitForSeconds(0.4f);
		Flash(0.5f);
		Events.InvokeScreenShake(1f, 0f);
		array = entities;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].wobbler.WobbleRandom();
		}

		hitPs.Play();
		foreach (Transform point2 in points)
		{
			LeanTween.moveLocal(to: point2.localPosition.normalized * 3f, gameObject: point2.gameObject, time: 1f).setEase(bounceCurve);
		}

		LeanTween.moveLocal(group.gameObject, new Vector3(0f, 0f, -2f), 1f).setEaseInOutQuad();
		LeanTween.rotateZ(group.gameObject, Dead.PettyRandom.Range(160f, 180f), 1f).setOnUpdateVector3(delegate
		{
			foreach (Transform point3 in points)
			{
				point3.transform.eulerAngles = Vector3.zero;
			}

		}).setEaseInOutQuad();
		yield return new WaitForSeconds(1f);
		Flash();
		Events.InvokeScreenShake(1f, 0f);
		if ((bool)ps)
		{
			ps.Play();
		}

		combinedFx.SetActive(value: true);
		finalEntity.transform.position = Vector3.zero;
		array = entities;
		for (int i = 0; i < array.Length; i++)
		{
			CardManager.ReturnToPool(array[i]);
		}

		group.transform.localRotation = Quaternion.identity;
		finalEntity.curveAnimator.Ping();
		finalEntity.wobbler.WobbleRandom();
		CinemaBarSystem.Top.SetScript(titleKey.GetLocalizedString());
		CinemaBarSystem.Bottom.SetPrompt(continueKey.GetLocalizedString(), "Select");
		while (!InputSystem.IsButtonPressed("Select"))
		{
			yield return null;
		}

		cinemaBarState.Restore();
		CinemaBarSystem.SetSortingLayer("CinemaBars");
		fader.gameObject.Destroy();
		cardSelector.character = References.Player;
		cardSelector.MoveCardToDeck(finalEntity);
		PauseMenu.Unblock();
	}

	public Entity[] CreateEntities(CardData[] cardDatas)
	{
		Entity[] array = new Entity[cardDatas.Length];
		float num = ((DeadExtensions.PettyRandom.value > 0.5f) ? DeadExtensions.PettyRandom.Range(-45f, 45f) : DeadExtensions.PettyRandom.Range(135f, 225f));
		float num2 = 360f / (float)cardDatas.Length;
		for (int i = 0; i < cardDatas.Length; i++)
		{
			Card card = CardManager.Get(cardDatas[i], null, null, inPlay: false, isPlayerCard: true);
			array[i] = card.entity;
			Vector2 vector = Lengthdir.ToVector2(10f, num * (MathF.PI / 180f)) + Dead.PettyRandom.Vector2() * 1f;
			card.transform.position = vector;
			card.transform.localScale = Vector3.zero;
			card.transform.SetParent(group);
			card.canvasGroup.alpha = 0f;
			num += num2;
		}

		return array;
	}

	public Entity CreateFinalEntity(CardData cardData)
	{
		Card card = CardManager.Get(cardData, null, null, inPlay: false, isPlayerCard: true);
		card.transform.localScale = Vector3.one * 1f;
		card.transform.SetParent(finalEntityParent);
		return card.entity;
	}

	public void Flash(float intensity = 1f, float duration = 0.15f)
	{
		GameObject obj = flash.gameObject;
		obj.SetActive(value: true);
		LeanTween.cancel(obj);
		LeanTween.value(obj, 0f, intensity, duration).setEase(flashCurve).setOnUpdate(delegate(float a)
		{
			flash.color = flash.color.With(-1f, -1f, -1f, a);
		})
			.setOnComplete((Action)delegate
			{
				flash.gameObject.SetActive(value: false);
			});
	}
}
