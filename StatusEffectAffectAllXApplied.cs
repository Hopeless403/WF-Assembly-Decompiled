#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Affect All X Applied", fileName = "Affect All X Applied")]
public class StatusEffectAffectAllXApplied : StatusEffectData
{
	[SerializeField]
	public StatusEffectData effectToAffect;

	[SerializeField]
	public bool setToSpecificValue;

	[SerializeField]
	[ShowIf("setToSpecificValue")]
	public int specificValue;

	[SerializeField]
	[HideIf("setToSpecificValue")]
	public int add;

	[SerializeField]
	[HideIf("setToSpecificValue")]
	public float multiplyBy = 2f;

	[SerializeField]
	public bool targetCanBeFriendly = true;

	[SerializeField]
	public bool targetCanBeEnemy = true;

	[SerializeField]
	public bool applierCanBeFriendly = true;

	[SerializeField]
	public bool applierCanBeEnemy = true;

	public override bool RunApplyStatusEvent(StatusEffectApply apply)
	{
		if (apply?.effectData != null && apply.effectData.type == effectToAffect.type && apply.count > 0 && CheckTeamOfTarget(apply.target) && CheckTeamOfApplier(apply.applier))
		{
			if (setToSpecificValue)
			{
				apply.count = specificValue;
			}
			else
			{
				apply.count += add * GetAmount();
				apply.count = Mathf.CeilToInt((float)apply.count * multiplyBy);
			}
		}

		return false;
	}

	public bool CheckTeamOfTarget(Entity target)
	{
		if (!targetCanBeFriendly || !(target.owner == base.target.owner))
		{
			if (targetCanBeEnemy)
			{
				return target.owner != base.target.owner;
			}

			return false;
		}

		return true;
	}

	public bool CheckTeamOfApplier(Entity applier)
	{
		if (!applierCanBeFriendly || !(applier.owner == target.owner))
		{
			if (applierCanBeEnemy)
			{
				return applier.owner != target.owner;
			}

			return false;
		}

		return true;
	}
}
