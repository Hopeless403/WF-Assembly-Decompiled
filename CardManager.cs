#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardManager : MonoBehaviourSingleton<CardManager>
{
	public static readonly Dictionary<string, GameObject> cardIcons = new Dictionary<string, GameObject>();

	public static readonly Vector3 startPos = new Vector3(-99f, -99f, 0f);

	public static readonly Dictionary<string, ObjectPool<Card>> cardPools = new Dictionary<string, ObjectPool<Card>>();

	public static bool init = false;

	public AsyncOperationHandle<IList<GameObject>> cardIconLoadHandle;

	public const float SCALE = 1f;

	public const float HOVER_SCALE = 1f;

	public IEnumerator Start()
	{
		Transform t = base.transform;
		List<CardType> group = AddressableLoader.GetGroup<CardType>("CardType");
		foreach (CardType cardType in group)
		{
			AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(cardType.prefabRef);
			yield return handle;
			GameObject prefab = handle.Result;
			for (int i = 0; i < 3; i++)
			{
				cardPools.Add($"{cardType.name}{i}", new ObjectPool<Card>(() => Object.Instantiate(prefab, startPos, quaternion.identity, t).GetComponent<Card>(), delegate(Card card)
				{
					card.OnGetFromPool();
					card.entity.OnGetFromPool();
					card.transform.position = startPos;
					card.transform.localRotation = Quaternion.identity;
					card.transform.localScale = Vector3.one;
					card.gameObject.SetActive(value: true);
				}, delegate(Card card)
				{
					card.transform.SetParent(t);
					card.OnReturnToPool();
					card.entity.OnReturnToPool();
					Events.InvokeCardPooled(card);
					card.gameObject.SetActive(value: false);
				}, delegate(Card card)
				{
					Object.Destroy(card.gameObject);
				}, collectionCheck: false, 10, 20));
			}
		}

		LoadCardIcons();
		init = true;
	}

	public void LoadCardIcons()
	{
		if (cardIconLoadHandle.IsValid())
		{
			Addressables.Release(cardIconLoadHandle);
		}

		Debug.Log("CardManager Loading Card Icon Prefabs");
		cardIconLoadHandle = Addressables.LoadAssetsAsync<GameObject>("CardIcons", null);
		foreach (GameObject item in cardIconLoadHandle.WaitForCompletion())
		{
			if (item != null)
			{
				StatusIcon component = item.GetComponent<StatusIcon>();
				if ((object)component != null)
				{
					cardIcons[component.type] = item;
				}
			}
		}

		Debug.Log($"{cardIcons.Count} icons loaded");
	}

	public static Card Get(CardData data, CardController controller, Character owner, bool inPlay, bool isPlayerCard)
	{
		int num = (isPlayerCard ? CardFramesSystem.GetFrameLevel(data.name) : 0);
		Card card = cardPools[$"{data.cardType.name}{num}"].Get();
		card.frameLevel = num;
		card.entity.data = data;
		card.entity.inPlay = inPlay;
		card.hover.controller = controller;
		card.entity.owner = owner;
		card.frameSetter.Load(num);
		Events.InvokeEntityCreated(card.entity);
		return card;
	}

	public static bool ReturnToPool(Entity entity)
	{
		if (entity.display is Card card)
		{
			return ReturnToPool(entity, card);
		}

		Object.Destroy(entity.gameObject);
		return false;
	}

	public static bool ReturnToPool(Card card)
	{
		return ReturnToPool(card.entity, card);
	}

	public static bool ReturnToPool(Entity entity, Card card)
	{
		if (GameManager.End || entity.inCardPool)
		{
			return false;
		}

		if (!entity.returnToPool)
		{
			Object.Destroy(entity.gameObject);
		}

		cardPools[$"{entity.data.cardType.name}{card.frameLevel}"].Release(card);
		return true;
	}

	public static StatusIcon NewStatusIcon(string type, Transform iconParent)
	{
		StatusIcon result = null;
		if (cardIcons.ContainsKey(type))
		{
			result = Object.Instantiate(cardIcons[type], iconParent).GetComponent<StatusIcon>();
		}

		return result;
	}

	public static void ReturnToPoolNextFrame(Card card)
	{
		MonoBehaviourSingleton<CardManager>.instance.StartCoroutine(ReturnToPoolNextFrameRoutine(card));
	}

	public static IEnumerator ReturnToPoolNextFrameRoutine(Card card)
	{
		yield return null;
		ReturnToPool(card);
	}
}
