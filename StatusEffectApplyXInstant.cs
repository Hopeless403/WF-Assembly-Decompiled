#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Instant Apply X", fileName = "Instant Apply X")]
public class StatusEffectApplyXInstant : StatusEffectApplyX
{
	public override bool Instant => true;

	public override int GetAmount(Entity entity, bool equalAmount = false, int equalTo = 0)
	{
		if (!scriptableAmount)
		{
			if (!equalAmount)
			{
				return GetAmount();
			}

			return equalTo;
		}

		return scriptableAmount.Get(entity);
	}

	public override int GetAmount()
	{
		return count;
	}

	public override bool TargetSilenced()
	{
		return false;
	}

	public override void Init()
	{
		base.OnBegin += Process;
	}

	public IEnumerator Process()
	{
		yield return Run(GetTargets());
		yield return Remove();
	}
}
