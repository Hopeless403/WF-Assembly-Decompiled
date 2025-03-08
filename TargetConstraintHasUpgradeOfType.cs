#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Has Upgrade Of Type", menuName = "Target Constraints/Has Upgrade Of Type")]
public class TargetConstraintHasUpgradeOfType : TargetConstraint
{
	[SerializeField]
	public CardUpgradeData.Type type = CardUpgradeData.Type.Charm;

	[SerializeField]
	public int countRequired = 1;

	[SerializeField]
	public CardUpgradeData[] ignore;

	public override bool Check(CardData targetData)
	{
		int num = 0;
		foreach (CardUpgradeData upgrade in targetData.upgrades)
		{
			if (upgrade.type == type && !ignore.Contains(upgrade) && ++num >= countRequired)
			{
				break;
			}
		}

		if (num < countRequired)
		{
			return not;
		}

		return !not;
	}

	public override bool Check(Entity target)
	{
		return Check(target.data);
	}
}
