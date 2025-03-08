#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Ongoing/Increase Max Health", fileName = "Ongoing Increase Max Health")]
public class StatusEffectOngoingMaxHealth : StatusEffectOngoing
{
	public override IEnumerator Add(int add)
	{
		target.hp.max += add;
		target.hp.current += add;
		target.PromptUpdate();
		yield return Sequences.Null();
	}

	public override IEnumerator Remove(int remove)
	{
		target.hp.max -= remove;
		target.hp.current -= remove;
		target.PromptUpdate();
		yield return Sequences.Null();
	}
}
