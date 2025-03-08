#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Change Recycle Requirement", fileName = "Change Recycle Requirement")]
public class StatusEffectChangeRecycleRequirement : StatusEffectData
{
	[SerializeField]
	public bool lower = true;

	public override void Init()
	{
		Events.OnCheckRecycleAmount += CheckRecycleAmount;
	}

	public void OnDestroy()
	{
		Events.OnCheckRecycleAmount -= CheckRecycleAmount;
	}

	public void CheckRecycleAmount(ref Entity entity, ref int amount)
	{
		if (!(entity != target))
		{
			int amount2 = GetAmount();
			amount = Mathf.Max(0, lower ? (amount - amount2) : (amount + amount2));
		}
	}
}
