#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Lose X", fileName = "Lose X")]
public class StatusEffectInstantLoseX : StatusEffectInstant
{
	[SerializeField]
	public StatusEffectData statusToLose;

	[SerializeField]
	public bool doPing;

	public override IEnumerator Process()
	{
		StatusEffectData statusEffectData = target.statusEffects.Find((StatusEffectData a) => a.name == statusToLose.name);
		if ((bool)statusEffectData)
		{
			if (doPing)
			{
				target.curveAnimator.Ping();
			}

			yield return statusEffectData.RemoveStacks(count, removeTemporary: false);
		}

		yield return base.Process();
	}
}
