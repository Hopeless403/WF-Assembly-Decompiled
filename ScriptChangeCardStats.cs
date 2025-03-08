#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Change Card Stats", menuName = "Scripts/Change Card Stats")]
public class ScriptChangeCardStats : Script
{
	[SerializeField]
	public int cardIndex;

	[SerializeField]
	public CardScript[] scriptPool;

	[SerializeField]
	public Vector2Int countRange;

	public override IEnumerator Run()
	{
		int num = countRange.Random();
		CardData target = References.PlayerData.inventory.deck[cardIndex];
		foreach (CardScript item in scriptPool.InRandomOrder())
		{
			if (num <= 0)
			{
				break;
			}

			item.Run(target);
			num--;
		}

		yield break;
	}
}
