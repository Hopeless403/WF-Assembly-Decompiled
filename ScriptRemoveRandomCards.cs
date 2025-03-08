#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Remove Random Cards", menuName = "Scripts/Remove Random Cards")]
public class ScriptRemoveRandomCards : Script
{
	[SerializeField]
	public Vector2Int countRange;

	[SerializeField]
	public CardType[] cardTypes;

	public override IEnumerator Run()
	{
		CardData[] array = References.PlayerData.inventory.deck.Where(Eligible).InRandomOrder().ToArray();
		int num = countRange.Random();
		for (int i = 0; i < num; i++)
		{
			CardData cardData = array[i];
			References.PlayerData.inventory.deck.Remove(cardData);
			Debug.Log(base.name + " → Removing " + cardData.name + " from player's deck");
		}

		yield break;
	}

	public bool Eligible(CardData cardData)
	{
		if (cardTypes.Length == 0)
		{
			return true;
		}

		return cardTypes.Contains(cardData.cardType);
	}
}
