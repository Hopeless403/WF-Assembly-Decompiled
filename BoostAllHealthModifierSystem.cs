#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

public class BoostAllHealthModifierSystem : GameSystem
{
	public const int healthAdd = 2;

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
		if (cardData.cardType.name == "Friendly" && cardData.hasHealth)
		{
			cardData.hp += 2;
		}
	}
}
