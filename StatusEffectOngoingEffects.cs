#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Ongoing/Increase Effects", fileName = "Ongoing Increase Effects")]
public class StatusEffectOngoingEffects : StatusEffectOngoing
{
	[SerializeField]
	public bool add = true;

	[SerializeField]
	[HideIf("add")]
	public bool multiply;

	public override IEnumerator Add(int add)
	{
		if (this.add)
		{
			target.effectBonus += add;
		}
		else if (multiply)
		{
			target.effectFactor += add;
		}

		target.PromptUpdate();
		yield return Sequences.Null();
	}

	public override IEnumerator Remove(int remove)
	{
		if (add)
		{
			target.effectBonus -= remove;
		}
		else if (multiply)
		{
			target.effectFactor -= remove;
		}

		target.PromptUpdate();
		yield return Sequences.Null();
	}
}
