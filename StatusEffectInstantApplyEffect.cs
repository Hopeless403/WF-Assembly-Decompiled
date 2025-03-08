#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Apply Status Effect", fileName = "Apply Status Effect")]
public class StatusEffectInstantApplyEffect : StatusEffectInstant
{
	[SerializeField]
	public StatusEffectData effectToApply;

	[SerializeField]
	public ScriptableAmount scriptableAmount;

	public override IEnumerator Process()
	{
		int num = (scriptableAmount ? scriptableAmount.Get(target) : GetAmount());
		yield return StatusEffectSystem.Apply(target, applier, effectToApply, num);
		yield return base.Process();
	}
}
