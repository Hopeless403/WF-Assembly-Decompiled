#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Copy Previous Charm", menuName = "Card Scripts/Copy Previous Charm")]
public class CardScriptCopyPreviousCharm : CardScript
{
	[SerializeField]
	public string[] illegal;

	public override void Run(CardData target)
	{
		CardUpgradeData cardUpgradeData = target.upgrades.FindLast((CardUpgradeData a) => a.type == CardUpgradeData.Type.Charm && !illegal.Contains(a.name));
		if ((bool)cardUpgradeData)
		{
			cardUpgradeData.AdjustStats(target);
			cardUpgradeData.RunScripts(target);
			cardUpgradeData.AdjustEffectBonus(target);
			cardUpgradeData.GainEffects(target);
		}
	}
}
