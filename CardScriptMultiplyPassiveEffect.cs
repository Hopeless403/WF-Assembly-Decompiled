#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Multiply Passive Effect", menuName = "Card Scripts/Multiply Passive Effect")]
public class CardScriptMultiplyPassiveEffect : CardScript
{
	[SerializeField]
	public StatusEffectData effect;

	[SerializeField]
	public float multiply = 1f;

	public override void Run(CardData target)
	{
		CardData.StatusEffectStacks[] startWithEffects = target.startWithEffects;
		foreach (CardData.StatusEffectStacks statusEffectStacks in startWithEffects)
		{
			if (statusEffectStacks.data.name == effect.name)
			{
				statusEffectStacks.count = Mathf.RoundToInt((float)statusEffectStacks.count * multiply);
			}
		}
	}
}
