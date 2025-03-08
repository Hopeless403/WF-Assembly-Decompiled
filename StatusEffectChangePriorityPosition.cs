#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Traits/Change Priority Position", fileName = "Change Priority Position")]
public class StatusEffectChangePriorityPosition : StatusEffectData
{
	[Header("-1 = Backline, 2 = Frontline")]
	[SerializeField]
	public int positionPriorityChange = -1;

	public int pre;

	public override bool RunBeginEvent()
	{
		pre = target.positionPriority;
		if (!target.silenced)
		{
			target.positionPriority = positionPriorityChange;
		}

		return false;
	}

	public override bool RunEndEvent()
	{
		target.positionPriority = pre;
		return false;
	}

	public override bool RunEffectBonusChangedEvent()
	{
		RunEndEvent();
		RunBeginEvent();
		return false;
	}
}
