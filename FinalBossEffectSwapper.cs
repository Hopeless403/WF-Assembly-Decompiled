#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Campaign/Final Boss Effect Swapper", fileName = "Final Boss Effect Swapper")]
public class FinalBossEffectSwapper : ScriptableObject
{
	public StatusEffectData effect;

	public bool remove = true;

	[ShowIf("remove")]
	public StatusEffectData[] replaceWithOptions;

	[ShowIf("remove")]
	public StatusEffectData replaceWithAttackEffect;

	public Vector2Int boostRange;

	public void Process(CardData card, CardData.StatusEffectStacks stack, int stackIndex)
	{
		if (remove)
		{
			List<CardData.StatusEffectStacks> list = card.startWithEffects.ToList();
			if (replaceWithOptions.Length != 0)
			{
				stack.data = replaceWithOptions.RandomItem();
			}
			else
			{
				list.RemoveAt(stackIndex);
			}

			if ((bool)replaceWithAttackEffect)
			{
				int count = stack.count + boostRange.Random();
				CardData.StatusEffectStacks statusEffectStacks = new CardData.StatusEffectStacks(replaceWithAttackEffect, count);
				card.attackEffects = CardData.StatusEffectStacks.Stack(card.attackEffects, new CardData.StatusEffectStacks[1] { statusEffectStacks });
			}

			card.startWithEffects = list.ToArray();
		}

		stack.count += boostRange.Random();
	}
}
