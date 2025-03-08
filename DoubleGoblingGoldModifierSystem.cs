#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class DoubleGoblingGoldModifierSystem : GameSystem
{
	public const float multiplier = 2f;

	public void OnEnable()
	{
		Events.OnCardDataCreated += CardDataCreated;
	}

	public void OnDisable()
	{
		Events.OnCardDataCreated -= CardDataCreated;
	}

	public static void CardDataCreated(CardData cardData)
	{
		if (cardData.name == "Gobling")
		{
			CardData.StatusEffectStacks obj = cardData.startWithEffects[1];
			obj.count = Mathf.RoundToInt((float)obj.count * 2f);
		}
	}
}
