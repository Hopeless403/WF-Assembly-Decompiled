#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Replace Attack With Apply", menuName = "Card Scripts/Replace Attack With Apply")]
public class CardScriptReplaceAttackWithApply : CardScript
{
	[SerializeField]
	public StatusEffectData effect;

	public override void Run(CardData target)
	{
		if (target.hasAttack && target.damage > 0)
		{
			CardData.StatusEffectStacks[] newEffects = new CardData.StatusEffectStacks[1]
			{
				new CardData.StatusEffectStacks(effect, target.damage)
			};
			target.attackEffects = CardData.StatusEffectStacks.Stack(target.attackEffects, newEffects);
			target.damage = 0;
		}
	}
}
