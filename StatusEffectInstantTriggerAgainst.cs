#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Trigger Against", fileName = "TriggerAgainst")]
public class StatusEffectInstantTriggerAgainst : StatusEffectInstant
{
	[SerializeField]
	public bool countsAsTrigger = true;

	[SerializeField]
	public string triggerType = "basic";

	[SerializeField]
	public bool reduceUses;

	public override IEnumerator Process()
	{
		if (applier.IsAliveAndExists() && target.IsAliveAndExists())
		{
			ActionQueue.Stack(new ActionTriggerAgainst(applier, target, target, target.containers[0])
			{
				countsAsTrigger = countsAsTrigger
			}, fixedPosition: true);
			if (reduceUses)
			{
				ActionQueue.Add(new ActionReduceUses(applier));
			}

			yield return base.Process();
		}
	}
}
