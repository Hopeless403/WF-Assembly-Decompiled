#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BoardDisplay : MonoBehaviour
{
	public Character player;

	public Character enemy;

	public int playerRowLength = 3;

	public int enemyRowLength = 3;

	[Range(0f, 1f)]
	public float cardScale = 1f / 3f;

	[SerializeField]
	public LayoutGroup layout;

	[Header("Card Container References")]
	public CardContainer playerReserve;

	public CardContainer[] playerRows;

	public CardContainer enemyReserve;

	public CardContainer[] enemyRows;

	public IEnumerator SetUp(CampaignNode node, CardController cardController)
	{
		layout.enabled = false;
		if (playerReserve != null)
		{
			playerReserve.owner = player;
			player.reserveContainer = playerReserve;
			playerReserve.SetSize(999, cardScale);
			playerReserve.AssignController(cardController);
		}

		if (enemyReserve != null)
		{
			enemyReserve.owner = enemy;
			enemy.reserveContainer = enemyReserve;
			enemyReserve.SetSize(999, cardScale);
			enemyReserve.AssignController(cardController);
		}

		for (int i = 0; i < playerRows.Length; i++)
		{
			CardContainer cardContainer = playerRows[i];
			cardContainer.owner = player;
			cardContainer.SetSize(playerRowLength, cardScale);
			cardContainer.AssignController(cardController);
			if (cardContainer is CardSlotLane cardSlotLane)
			{
				cardSlotLane.SetDirection(1);
			}

			References.Battle.rows[player].Add(cardContainer);
			References.Battle.rowIndices[cardContainer] = i;
		}

		for (int j = 0; j < enemyRows.Length; j++)
		{
			CardContainer cardContainer2 = enemyRows[j];
			cardContainer2.owner = enemy;
			cardContainer2.SetSize(enemyRowLength, cardScale);
			cardContainer2.AssignController(cardController);
			if (cardContainer2 is CardSlotLane cardSlotLane2)
			{
				cardSlotLane2.SetDirection(-1);
			}

			References.Battle.rows[enemy].Add(cardContainer2);
			References.Battle.rowIndices[cardContainer2] = j;
		}

		yield return null;
		layout.enabled = true;
	}
}
