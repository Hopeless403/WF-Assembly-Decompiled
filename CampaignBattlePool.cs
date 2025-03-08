#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using UnityEngine;

public class CampaignBattlePool : MonoBehaviour
{
	[Serializable]
	public struct Tier
	{
		public Vector2Int pointRange;

		public int pointsAdd;

		public BattleData[] battles;

		public BattleData[] bosses;
	}

	[SerializeField]
	public int basePoints;

	[SerializeField]
	public Tier[] tiers;

	public BattleData GetRandomBattle(int tier)
	{
		return tiers[tier].battles.RandomItem();
	}

	public BattleData GetRandomBossBattle(int tier)
	{
		return tiers[tier].bosses.RandomItem();
	}

	public int GetPoints(int tier, int battleLevel)
	{
		Tier tier2 = tiers[tier];
		return basePoints + tier2.pointRange.Random() + battleLevel * tier2.pointsAdd;
	}
}
