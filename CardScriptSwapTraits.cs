#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Swap Traits", menuName = "Card Scripts/Swap Traits")]
public class CardScriptSwapTraits : CardScript
{
	[SerializeField]
	public TraitData traitA;

	[SerializeField]
	public TraitData traitB;

	public override void Run(CardData target)
	{
		foreach (CardData.TraitStacks trait in target.traits)
		{
			if (trait.data == traitA)
			{
				trait.data = traitB;
			}
			else if (trait.data == traitB)
			{
				trait.data = traitA;
			}
		}
	}
}
