#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Has Trait", menuName = "Target Constraints/Has Trait")]
public class TargetConstraintHasTrait : TargetConstraint
{
	[SerializeField]
	public TraitData trait;

	[SerializeField]
	public bool ignoreSilenced;

	public override bool Check(Entity target)
	{
		if (ignoreSilenced && target.silenced)
		{
			return not;
		}

		bool flag = false;
		foreach (Entity.TraitStacks trait in target.traits)
		{
			if (trait.data.name == this.trait.name)
			{
				flag = true;
				break;
			}
		}

		if (!flag)
		{
			return not;
		}

		return !not;
	}

	public override bool Check(CardData targetData)
	{
		bool flag = false;
		foreach (CardData.TraitStacks trait in targetData.traits)
		{
			if (trait.data.name == this.trait.name)
			{
				flag = true;
				break;
			}
		}

		if (!flag)
		{
			return not;
		}

		return !not;
	}
}
