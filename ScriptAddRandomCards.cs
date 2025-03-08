#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Add Random Cards", menuName = "Scripts/Add Random Cards")]
public class ScriptAddRandomCards : Script
{
	[SerializeField]
	public int replaceIndex = -1;

	[SerializeField]
	public bool anyCard;

	[SerializeField]
	[ShowIf("anyCard")]
	public CardType[] ofType;

	[SerializeField]
	[HideIf("anyCard")]
	public CardData[] pool;

	[SerializeField]
	public Vector2Int countRange;

	public override IEnumerator Run()
	{
		CardDataList deck = References.PlayerData.inventory.deck;
		if (replaceIndex >= 0)
		{
			deck.RemoveAt(replaceIndex);
		}

		List<CardData> list = new List<CardData>();
		int num = countRange.Random();
		for (int i = 0; i < num; i++)
		{
			if (list.Count <= 0)
			{
				PopulateCardList(list);
			}

			CardData cardData = list.TakeRandom().Clone();
			Debug.Log(base.name + " → Adding [" + cardData.name + "] to player's deck");
			if (replaceIndex >= 0)
			{
				deck.Insert(replaceIndex, cardData);
			}
			else
			{
				deck.Add(cardData);
			}
		}

		yield break;
	}

	public void PopulateCardList(List<CardData> list)
	{
		if (anyCard)
		{
			RewardPool[] rewardPools = References.PlayerData.classData.rewardPools;
			for (int i = 0; i < rewardPools.Length; i++)
			{
				foreach (DataFile item in rewardPools[i].list)
				{
					if (item is CardData cardData && ofType.Contains(cardData.cardType))
					{
						list.Add(cardData);
					}
				}
			}
		}
		else
		{
			list.AddRange(pool);
		}
	}
}
