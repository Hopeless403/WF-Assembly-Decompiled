#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Remove Attack Effect", menuName = "Card Scripts/Remove Attack Effect")]
public class CardScriptRemoveAttackEffect : CardScript
{
	[SerializeField]
	public bool removeAll;

	[SerializeField]
	[HideIf("removeAll")]
	public StatusEffectData[] toRemove;

	public override void Run(CardData target)
	{
		if (removeAll)
		{
			target.attackEffects = Array.Empty<CardData.StatusEffectStacks>();
			return;
		}

		List<CardData.StatusEffectStacks> list = target.attackEffects.ToList();
		list.RemoveAll((CardData.StatusEffectStacks a) => toRemove.Contains(a.data));
		target.attackEffects = list.ToArray();
	}
}
