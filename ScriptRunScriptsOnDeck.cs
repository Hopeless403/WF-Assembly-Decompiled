#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Run Scripts On Deck", menuName = "Scripts/Run Scripts On Deck")]
public class ScriptRunScriptsOnDeck : Script
{
	[SerializeField]
	public AssetReferenceT<CardScript>[] scriptRefs;

	[SerializeField]
	public TargetConstraint[] constraints;

	[SerializeField]
	public Vector2Int countRange;

	[SerializeField]
	public bool includeReserve;

	public override IEnumerator Run()
	{
		List<CardData> list = new List<CardData>();
		AddRangeIfConstraints(list, References.PlayerData.inventory.deck, constraints);
		if (includeReserve)
		{
			AddRangeIfConstraints(list, References.PlayerData.inventory.reserve, constraints);
		}

		if (list.Count > 0)
		{
			Affect(list);
		}

		yield break;
	}

	public static void AddRangeIfConstraints(ICollection<CardData> collection, CardDataList toAdd, TargetConstraint[] constraints)
	{
		foreach (CardData item in toAdd)
		{
			AddIfConstraints(collection, item, constraints);
		}
	}

	public static void AddIfConstraints(ICollection<CardData> collection, CardData item, TargetConstraint[] constraints)
	{
		if (!constraints.Any((TargetConstraint c) => !c.Check(item)))
		{
			collection.Add(item);
		}
	}

	public void Affect(IReadOnlyCollection<CardData> cards)
	{
		int num = countRange.Random();
		Debug.Log("[" + base.name + "] Affecting [" + string.Join(", ", cards) + "]");
		using (AddressableGroup<CardScript> addressableGroup = new AddressableGroup<CardScript>(scriptRefs))
		{
			foreach (CardData item in cards.InRandomOrder())
			{
				foreach (CardScript item2 in addressableGroup)
				{
					item2.Run(item);
				}

				num--;
				if (num <= 0)
				{
					break;
				}
			}
		}
	}
}
