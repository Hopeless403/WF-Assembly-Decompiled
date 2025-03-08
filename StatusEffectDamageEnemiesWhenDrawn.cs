#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X To Enemies When Drawn", fileName = "Apply X To Enemies When Drawn")]
public class StatusEffectDamageEnemiesWhenDrawn : StatusEffectApplyX
{
	public override void Init()
	{
		base.OnEnable += Check;
	}

	public override bool RunEnableEvent(Entity entity)
	{
		if (entity == target)
		{
			return target.InHand();
		}

		return false;
	}

	public IEnumerator Check(Entity entity)
	{
		return Run(GetTargets());
	}
}
