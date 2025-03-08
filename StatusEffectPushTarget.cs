#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Traits/Push Target", fileName = "Push Target")]
public class StatusEffectPushTarget : StatusEffectData
{
	public Hit storedHit;

	public override bool RunPreAttackEvent(Hit hit)
	{
		if (hit.attacker == target && target.alive && target.enabled && hit.target != null && hit.target.containers != null)
		{
			hit.FlagAsOffensive();
			storedHit = hit;
		}

		return false;
	}

	public override bool RunHitEvent(Hit hit)
	{
		if (hit == storedHit)
		{
			Entity entity = hit.target;
			CardContainer cardContainer = entity.containers[0];
			int num = cardContainer.IndexOf(entity);
			int num2 = Mathf.Min(num + GetAmount(), cardContainer.max - 1);
			if (num != num2)
			{
				cardContainer.RemoveAt(num);
				cardContainer.Insert(num2, entity);
				foreach (Entity item in cardContainer)
				{
					cardContainer.TweenChildPosition(item);
				}
			}
		}

		return false;
	}
}
