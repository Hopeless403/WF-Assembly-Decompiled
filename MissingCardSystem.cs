#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MissingCardSystem
{
	public static CardData Get()
	{
		return AddressableLoader.Get<CardData>("CardData", "MissingCard");
	}

	public static CardData GetClone(string cardDataName, bool runCreateScripts = true)
	{
		CardData cardData = Get();
		if (!cardData)
		{
			return null;
		}

		CardData cardData2 = cardData.Clone(runCreateScripts);
		cardData2.forceTitle = "Missing Card " + cardDataName;
		return cardData2;
	}

	public static CardData GetCloneWithId(string cardDataName, Vector3 random3, ulong id, bool runCreateScripts = true)
	{
		CardData cardData = Get();
		if (!cardData)
		{
			return null;
		}

		CardData cardData2 = cardData.Clone(random3, id, runCreateScripts);
		cardData2.forceTitle = "Missing Card " + cardDataName;
		return cardData2;
	}

	public static bool IsMissing(CardData cardData)
	{
		return IsMissing(cardData.name);
	}

	public static bool IsMissing(CardSaveData cardSaveData)
	{
		return IsMissing(cardSaveData.name);
	}

	public static bool IsMissing(string cardDataName)
	{
		CardData cardData = AddressableLoader.Get<CardData>("CardData", cardDataName);
		if (!cardData)
		{
			return true;
		}

		if (cardData.name == "MissingCard")
		{
			return true;
		}

		return false;
	}

	public static bool HasMissingData(IEnumerable<string> cardDataNames)
	{
		foreach (string cardDataName in cardDataNames)
		{
			if (IsMissing(cardDataName))
			{
				return true;
			}
		}

		return false;
	}

	public static bool HasMissingData(CardSaveData[] cardSaveDatas)
	{
		return HasMissingData(cardSaveDatas.LoadList<CardData, CardSaveData>());
	}

	public static bool HasMissingData(IEnumerable<CardData> cardDataList)
	{
		return cardDataList.Any((CardData a) => IsMissing(a.name));
	}
}
