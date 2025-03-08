#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Add Passive Effect", menuName = "Card Scripts/Add Passive Effect")]
public class CardScriptAddPassiveEffect : CardScript
{
	[SerializeField]
	public StatusEffectData effect;

	[SerializeField]
	public Vector2Int countRange;

	public override void Run(CardData target)
	{
		target.startWithEffects = target.startWithEffects.With(new CardData.StatusEffectStacks(effect, countRange.Random()));
	}
}
