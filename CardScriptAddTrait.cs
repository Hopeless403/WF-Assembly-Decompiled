#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Add Trait", menuName = "Card Scripts/Add Trait")]
public class CardScriptAddTrait : CardScript
{
	[SerializeField]
	public TraitData trait;

	[SerializeField]
	public Vector2Int countRange;

	[SerializeField]
	public bool @override = true;

	public override void Run(CardData target)
	{
		if (countRange.Random() <= 0)
		{
			return;
		}

		for (int num = target.traits.Count - 1; num >= 0; num--)
		{
			CardData.TraitStacks traitStacks = target.traits[num];
			if (trait.overrides.Contains(traitStacks.data))
			{
				if (!@override)
				{
					Debug.Log("Cannot add [" + trait.name + "] because of [" + traitStacks.data.name + "]");
					return;
				}

				target.traits.RemoveAt(num);
				Debug.Log("[" + trait.name + "] overrides [" + traitStacks.data.name + "]");
			}
		}

		target.traits.Add(new CardData.TraitStacks
		{
			data = trait,
			count = countRange.Random()
		});
	}
}
