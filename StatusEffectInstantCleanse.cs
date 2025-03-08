#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Cleanse", fileName = "Cleanse")]
public class StatusEffectInstantCleanse : StatusEffectInstant
{
	public override IEnumerator Process()
	{
		int num = target.statusEffects.Count;
		for (int i = num - 1; i >= 0; i--)
		{
			StatusEffectData statusEffectData = target.statusEffects[i];
			if (statusEffectData.IsNegativeStatusEffect())
			{
				yield return statusEffectData.Remove();
			}
		}

		target.PromptUpdate();
		yield return base.Process();
	}
}
