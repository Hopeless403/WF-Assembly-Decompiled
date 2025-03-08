#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Shell", fileName = "Shell")]
public class StatusEffectShell : StatusEffectData
{
	public override void Init()
	{
		base.OnHit += Check;
	}

	public override bool RunHitEvent(Hit hit)
	{
		if (hit.target == target)
		{
			return hit.damage > 0;
		}

		return false;
	}

	public IEnumerator Check(Hit hit)
	{
		while (hit.damage > 0 && count > 0)
		{
			count--;
			hit.damage--;
			hit.damageBlocked++;
		}

		if (count <= 0)
		{
			yield return Remove();
		}

		target.PromptUpdate();
	}
}
