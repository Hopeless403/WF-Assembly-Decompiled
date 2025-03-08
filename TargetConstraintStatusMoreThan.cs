#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Status More Than", menuName = "Target Constraints/Status More Than")]
public class TargetConstraintStatusMoreThan : TargetConstraint
{
	[SerializeField]
	public StatusEffectData status;

	[SerializeField]
	public int amount;

	public override bool Check(Entity target)
	{
		StatusEffectData statusEffectData = target.FindStatus(status.type);
		if (!statusEffectData || statusEffectData.count <= amount)
		{
			return not;
		}

		return !not;
	}

	public override bool Check(CardData targetData)
	{
		CardData.StatusEffectStacks[] startWithEffects = targetData.startWithEffects;
		foreach (CardData.StatusEffectStacks statusEffectStacks in startWithEffects)
		{
			if (statusEffectStacks.data.type == status.type && statusEffectStacks.count > amount)
			{
				return !not;
			}
		}

		return not;
	}
}
