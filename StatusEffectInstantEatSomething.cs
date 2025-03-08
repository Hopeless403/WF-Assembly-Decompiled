#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Eat Something", fileName = "Eat Something")]
public class StatusEffectInstantEatSomething : StatusEffectInstant
{
	[SerializeField]
	public float delayAfter = 0.25f;

	[SerializeField]
	public StatusEffectData eatEffect;

	[SerializeField]
	public Targets.Flag targetFlags;

	public override IEnumerator Process()
	{
		List<Entity> list = Targets.Get(target, targetFlags, eatEffect, targetConstraints);
		if (list != null && list.Count > 0)
		{
			foreach (Entity item in list)
			{
				if (item.IsAliveAndExists() && eatEffect.CanPlayOn(item))
				{
					Trigger trigger = new Trigger(target, target, "eat", new Entity[1] { item })
					{
						countsAsTrigger = false
					};
					Hit hit = new Hit(target, item, 0)
					{
						canBeNullified = false,
						canRetaliate = false,
						damageType = "eat",
						trigger = trigger
					};
					hit.AddStatusEffect(eatEffect, 1);
					trigger.hits = new Hit[1] { hit };
					yield return trigger.Process();
				}
			}

			yield return Sequences.Wait(delayAfter);
		}

		yield return base.Process();
	}
}
