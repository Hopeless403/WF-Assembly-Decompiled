#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using Dead;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Randomize Stats", fileName = "Randomize Stats")]
public class StatusEffectInstantRandomizeStats : StatusEffectInstant
{
	[SerializeField]
	public int min = 2;

	[SerializeField]
	public int max = 5;

	public override IEnumerator Process()
	{
		target.hp.max = Dead.Random.Range(min, max);
		target.damage.max = Dead.Random.Range(min, max);
		target.counter.max = Dead.Random.Range(min, max);
		target.hp.current = target.hp.max;
		target.damage.current = target.damage.max;
		target.counter.current = target.counter.max;
		target.ResetWhenHealthLostEffects();
		yield return base.Process();
	}
}
