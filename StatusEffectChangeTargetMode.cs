#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Traits/Change Target Mode", fileName = "Change Target Mode")]
public class StatusEffectChangeTargetMode : StatusEffectData
{
	[SerializeField]
	public TargetMode targetMode;

	public TargetMode pre;

	public override bool RunBeginEvent()
	{
		pre = target.targetMode;
		if (!target.silenced)
		{
			target.targetMode = targetMode;
		}

		return false;
	}

	public override bool RunEndEvent()
	{
		target.targetMode = pre;
		return false;
	}

	public override bool RunEffectBonusChangedEvent()
	{
		RunEndEvent();
		RunBeginEvent();
		return false;
	}
}
