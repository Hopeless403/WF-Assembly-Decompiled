#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Campaign Tier", menuName = "Campaign/Tier")]
public class CampaignTier : ScriptableObject
{
	public Vector2Int pointRange;

	public BattleData[] battlePool;

	public CampaignNodeType[] rewardPool;
}
