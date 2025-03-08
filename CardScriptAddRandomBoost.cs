#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Add Random Boost", menuName = "Card Scripts/Add Boost")]
public class CardScriptAddRandomBoost : CardScript
{
	[SerializeField]
	public Vector2Int boostRange;

	public override void Run(CardData target)
	{
		int num = boostRange.Random();
		if (num == 0)
		{
			return;
		}

		CardData.StatusEffectStacks[] attackEffects = target.attackEffects;
		foreach (CardData.StatusEffectStacks statusEffectStacks in attackEffects)
		{
			statusEffectStacks.count = Mathf.Max(1, statusEffectStacks.count + num);
		}

		attackEffects = target.startWithEffects;
		foreach (CardData.StatusEffectStacks statusEffectStacks2 in attackEffects)
		{
			if (!statusEffectStacks2.data.visible)
			{
				statusEffectStacks2.count = Mathf.Max(1, statusEffectStacks2.count + num);
			}
		}

		foreach (CardData.TraitStacks trait in target.traits)
		{
			if (trait.data.keyword.canStack)
			{
				trait.count = Mathf.Max(1, trait.count + num);
			}
		}
	}
}
