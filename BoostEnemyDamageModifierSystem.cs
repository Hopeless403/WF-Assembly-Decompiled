#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

public class BoostEnemyDamageModifierSystem : GameSystem
{
	public const int attackAdd = 1;

	public const int healthAdd = 1;

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
		switch (cardData.cardType.name)
		{
			case "Enemy":
			case "Miniboss":
			case "Boss":
			case "BossSmall":
			if (cardData.hasAttack)
			{
				cardData.damage++;
				}
	
			if (cardData.hasHealth)
			{
				cardData.hp++;
				}
	
				break;
		}
	}
}
