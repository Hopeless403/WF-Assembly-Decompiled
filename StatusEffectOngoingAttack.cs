#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Ongoing/Increase Attack", fileName = "Ongoing Increase Attack")]
public class StatusEffectOngoingAttack : StatusEffectOngoing
{
	public override IEnumerator Add(int add)
	{
		target.tempDamage += add;
		target.PromptUpdate();
		yield break;
	}

	public override IEnumerator Remove(int remove)
	{
		target.tempDamage -= remove;
		target.PromptUpdate();
		yield break;
	}
}
