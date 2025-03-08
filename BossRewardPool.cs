#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Boss Reward Pool", fileName = "Boss Reward Pool")]
public class BossRewardPool : ScriptableObject
{
	public int areaIndex;

	public int canTake = 2;

	public BossRewardData[] bossRewards;
}
