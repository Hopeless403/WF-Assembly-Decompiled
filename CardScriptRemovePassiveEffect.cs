#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Remove Passive Effect", menuName = "Card Scripts/Remove Passive Effect")]
public class CardScriptRemovePassiveEffect : CardScript
{
	[SerializeField]
	public bool removeAll;

	[SerializeField]
	[HideIf("removeAll")]
	public StatusEffectData[] toRemove;

	[SerializeField]
	[ShowIf("removeAll")]
	public bool excludingStatusEffects;

	[SerializeField]
	[ShowIf("removeAll")]
	public StatusEffectData[] excluding;

	[SerializeField]
	[ShowIf("removeAll")]
	public string[] excludingTypes;

	[InfoBox("if set to ZERO it will remove all stacks", EInfoBoxType.Normal)]
	[SerializeField]
	public int reduceStacks;

	public override void Run(CardData target)
	{
		List<CardData.StatusEffectStacks> list = target.startWithEffects.ToList();
		if (removeAll)
		{
			for (int num = list.Count - 1; num >= 0; num--)
			{
				CardData.StatusEffectStacks statusEffectStacks = list[num];
				if (!Exclude(statusEffectStacks.data) && (!excludingStatusEffects || !statusEffectStacks.data.isStatus))
				{
					statusEffectStacks.count -= reduceStacks;
					if (reduceStacks <= 0 || statusEffectStacks.count <= 0)
					{
						list.RemoveAt(num);
					}
				}
			}
		}
		else if (reduceStacks <= 0)
		{
			list.RemoveAll((CardData.StatusEffectStacks a) => toRemove.Contains(a.data));
		}
		else
		{
			for (int num2 = list.Count - 1; num2 >= 0; num2--)
			{
				CardData.StatusEffectStacks statusEffectStacks2 = list[num2];
				if (toRemove.Contains(statusEffectStacks2.data))
				{
					statusEffectStacks2.count -= reduceStacks;
					if (statusEffectStacks2.count <= 0)
					{
						list.RemoveAt(num2);
					}
				}
			}
		}

		target.startWithEffects = list.ToArray();
	}

	public bool Exclude(StatusEffectData effectData)
	{
		if (!excluding.Contains(effectData))
		{
			if (!effectData.type.IsNullOrEmpty())
			{
				return excludingTypes.Contains(effectData.type);
			}

			return false;
		}

		return true;
	}
}
