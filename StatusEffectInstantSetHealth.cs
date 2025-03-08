#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Set Health", fileName = "Set Health")]
public class StatusEffectInstantSetHealth : StatusEffectInstant
{
	[SerializeField]
	public bool maxOnly;

	[SerializeField]
	public bool equalAmount = true;

	[SerializeField]
	[HideIf("equalAmount")]
	public StatusEffectInstantSetHealth setMaxHealthEffect;

	[SerializeField]
	[HideIf("equalAmount")]
	public float factor = 1f;

	public override IEnumerator Process()
	{
		if (equalAmount)
		{
			int amount = GetAmount();
			target.hp.max = amount;
			target.hp.current = (maxOnly ? Mathf.Min(target.hp.current, target.hp.max) : amount);
		}
		else if (factor < 1f)
		{
			int num = Mathf.CeilToInt((float)target.hp.max * factor);
			int num2 = Mathf.FloorToInt((float)target.hp.current * factor);
			if (num2 > 0)
			{
				Hit hit = new Hit(null, target, num2)
				{
					countsAsHit = false,
					screenShake = 0f
				};
				hit.AddStatusEffect(setMaxHealthEffect, num);
				yield return hit.Process();
			}
		}
		else
		{
			target.hp.max = Mathf.CeilToInt((float)target.hp.max * factor);
			target.hp.current = Mathf.CeilToInt((float)target.hp.current * factor);
		}

		target.ResetWhenHealthLostEffects();
		target.PromptUpdate();
		yield return base.Process();
	}
}
