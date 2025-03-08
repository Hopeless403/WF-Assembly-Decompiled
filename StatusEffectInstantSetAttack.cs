#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Set Attack", fileName = "Set Attack")]
public class StatusEffectInstantSetAttack : StatusEffectInstant
{
	[SerializeField]
	public bool maxOnly;

	[SerializeField]
	public bool equalAmount = true;

	[SerializeField]
	[HideIf("equalAmount")]
	public StatusEffectInstantSetAttack setMaxAttackEffect;

	[SerializeField]
	[HideIf("equalAmount")]
	public float factor = 1f;

	public override IEnumerator Process()
	{
		if (equalAmount)
		{
			int amount = GetAmount();
			target.damage.max = amount;
			target.damage.current = (maxOnly ? Mathf.Min(target.damage.current, target.damage.max) : amount);
		}
		else if (factor < 1f)
		{
			int num = Mathf.CeilToInt((float)target.damage.max * factor);
			int num2 = Mathf.FloorToInt((float)target.damage.current * factor);
			if (num2 > 0)
			{
				Hit hit = new Hit(null, target, num2)
				{
					countsAsHit = false,
					screenShake = 0f
				};
				hit.AddStatusEffect(setMaxAttackEffect, num);
				yield return hit.Process();
			}
		}
		else
		{
			target.damage.max = Mathf.CeilToInt((float)target.damage.max * factor);
			target.damage.current = Mathf.CeilToInt((float)target.damage.current * factor);
		}

		target.PromptUpdate();
		yield return base.Process();
	}
}
