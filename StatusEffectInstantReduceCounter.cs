#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Reduce Counter", fileName = "Reduce Counter")]
public class StatusEffectInstantReduceCounter : StatusEffectInstant
{
	public override IEnumerator Process()
	{
		Hit hit = new Hit(applier, target, 0)
		{
			countsAsHit = false,
			counterReduction = GetAmount()
		};
		yield return hit.Process();
		yield return base.Process();
	}
}
