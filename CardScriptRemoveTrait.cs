#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Remove Trait", menuName = "Card Scripts/Remove Trait")]
public class CardScriptRemoveTrait : CardScript
{
	[SerializeField]
	public bool removeAll;

	[SerializeField]
	[HideIf("removeAll")]
	public TraitData[] toRemove;

	[SerializeField]
	[ShowIf("removeAll")]
	public TraitData[] excluding;

	public override void Run(CardData target)
	{
		if (removeAll)
		{
			target.traits = target.traits.Where((CardData.TraitStacks a) => excluding.Contains(a.data)).ToList();
			return;
		}

		List<CardData.TraitStacks> list = target.traits.ToList();
		list.RemoveAll((CardData.TraitStacks a) => toRemove.Contains(a.data));
		target.traits = list.ToList();
	}
}
