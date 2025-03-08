#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class CardOrganizer : MonoBehaviour
{
	[Serializable]
	public class Data
	{
		[Serializable]
		public class Card
		{
			public string cardDataName;

			public float posX;

			public float posY;

			public Card(Entity entity)
			{
				cardDataName = entity.data.name;
				posX = entity.transform.localPosition.x;
				posY = entity.transform.localPosition.y;
			}
		}

		public Card[] cards;

		public Data(params Entity[] entities)
		{
			List<Card> list = new List<Card>();
			foreach (Entity entity in entities)
			{
				list.Add(new Card(entity));
			}

			cards = list.ToArray();
		}
	}

	[SerializeField]
	public Canvas canvas;

	[SerializeField]
	public CardContainer cardHolder;

	[SerializeField]
	public CardController cardController;

	[Header("Card Grid")]
	[SerializeField]
	public int gridColumns = 8;

	[SerializeField]
	public Vector2 gridSpacing;

	public Vector2 startPos;

	public bool inspecting;

	public static string filePath => Application.streamingAssetsPath + "/Cards.json";

	public Vector2 GetCellSize()
	{
		return GameManager.CARD_SIZE + gridSpacing;
	}

	public void OnEnable()
	{
		Events.OnInspect += InspectStart;
		Events.OnInspectEnd += InspectEnd;
	}

	public void OnDisable()
	{
		Events.OnInspect -= InspectStart;
		Events.OnInspectEnd -= InspectEnd;
	}

	public void InspectStart(Entity entity)
	{
		inspecting = true;
	}

	public void InspectEnd(Entity entity)
	{
		inspecting = false;
	}

	public IEnumerator Start()
	{
		if (!GameManager.Ready)
		{
			yield break;
		}

		yield return AddressableLoader.LoadGroup("CardData");
		List<CardData> cardDataList = AddressableLoader.GetGroup<CardData>("CardData");
		int num = Mathf.RoundToInt(cardDataList.Count / gridColumns);
		int num2 = Mathf.Min(gridColumns, cardDataList.Count);
		float num3 = (float)num2 * 3f + (float)(num2 - 1) * gridSpacing.x;
		float num4 = (float)num * 4.5f + (float)(num - 1) * gridSpacing.y;
		startPos.x = (0f - num3) / 2f + 1.5f;
		startPos.y = num4 / 2f - 2.25f;
		yield return Load();
		List<Entity> list = cardHolder.GetComponentsInChildren<Entity>().ToList();
		Routine.Clump clump = new Routine.Clump();
		for (int i = 0; i < cardDataList.Count; i++)
		{
			CardData cardData = cardDataList[i];
			if (!list.Exists((Entity a) => a.data.name == cardData.name))
			{
				Vector2 cardPos = GetCardPos(i);
				clump.Add(CreateCard(cardData, cardPos));
			}
		}

		yield return clump.WaitForEnd();
		Transition.End();
	}

	public Vector2 GetCardPos(int cardIndex)
	{
		int num = Mathf.FloorToInt(cardIndex / gridColumns);
		int num2 = cardIndex % gridColumns;
		float x = startPos.x + (float)num2 * (3f + gridSpacing.x);
		float y = startPos.y - (float)num * (4.5f + gridSpacing.y);
		return new Vector2(x, y);
	}

	public IEnumerator CreateCard(CardData cardData, Vector2 pos)
	{
		Card card = CardManager.Get(cardData.Clone(), cardController, null, inPlay: false, isPlayerCard: true);
		card.entity.returnToPool = false;
		yield return card.UpdateData();
		card.entity.transform.localPosition = pos.WithZ(0f);
		cardHolder.Add(card.entity);
	}

	public void OnDestroy()
	{
		if (GameManager.Ready)
		{
			Save();
		}
	}

	public void Update()
	{
		if (inspecting)
		{
			return;
		}

		if (Input.GetKeyDown(KeyCode.D) && Input.GetKey(KeyCode.LeftControl))
		{
			Entity hover = GetHover();
			if ((bool)hover)
			{
				cardController.UnHover();
				new Routine(CreateCard(hover.data, hover.transform.localPosition + new Vector3(0.25f, -0.25f)));
				Debug.Log("Duplicating [" + hover.name + "]");
			}
			else
			{
				Debug.Log("Nothing to duplicate!");
			}
		}

		if (!Input.GetKeyDown(KeyCode.Delete))
		{
			return;
		}

		Entity target = GetHover();
		if ((bool)target)
		{
			if (cardHolder.GetComponentsInChildren<Entity>(includeInactive: true).Count((Entity a) => a.data.name == target.data.name) > 1)
			{
				Debug.Log("Deleting [" + target.name + "]");
				cardController.UnHover();
				UnityEngine.Object.Destroy(target.gameObject);
			}
			else
			{
				Debug.Log("Cannot delete [" + target.name + "]");
				target.wobbler?.WobbleRandom();
			}
		}
		else
		{
			Debug.Log("Nothing to delete!");
		}
	}

	public Entity GetHover()
	{
		return cardController.dragging ?? cardController.hoverEntity;
	}

	public void Save()
	{
		Debug.Log($"[{this}] SAVING DATA");
		string contents = JsonUtility.ToJson(new Data(cardHolder.GetComponentsInChildren<Entity>(includeInactive: true)));
		File.WriteAllText(filePath, contents);
		Debug.Log("Done!");
	}

	public IEnumerator Load()
	{
		Debug.Log($"[{this}] LOADING DATA");
		if (File.Exists(filePath))
		{
			Data data = JsonUtility.FromJson<Data>(File.ReadAllText(filePath));
			Routine.Clump clump = new Routine.Clump();
			Data.Card[] cards = data.cards;
			foreach (Data.Card card in cards)
			{
				if (AddressableLoader.TryGet<CardData>("CardData", card.cardDataName, out var result))
				{
					clump.Add(CreateCard(result, new Vector2(card.posX, card.posY)));
				}
			}

			yield return clump.WaitForEnd();
		}
		else
		{
			Debug.Log("[" + filePath + "] does not exist...");
		}
	}
}
