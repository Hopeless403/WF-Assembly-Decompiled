#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Heal", fileName = "Heal")]
public class StatusEffectInstantHeal : StatusEffectInstant
{
	[SerializeField]
	public bool doPing = true;

	public override IEnumerator Process()
	{
		if (target.alive)
		{
			if (doPing)
			{
				target.curveAnimator?.Ping();
			}

			Hit hit = new Hit(applier, target, -GetAmount());
			yield return hit.Process();
		}

		yield return base.Process();
	}
}
