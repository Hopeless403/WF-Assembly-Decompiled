#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

public class BoostArea2EnemyDamageModifierSystem : GameSystem
{
	public const int area = 1;

	public const int damageAdd = 1;

	public bool correctArea;

	public void OnEnable()
	{
		Events.PreBattleSetUp += PreBattleSetUp;
		Events.PostBattleSetUp += PostBattleSetUp;
	}

	public void OnDisable()
	{
		Events.PreBattleSetUp -= PreBattleSetUp;
		Events.PostBattleSetUp -= PostBattleSetUp;
	}

	public void PreBattleSetUp(CampaignNode node)
	{
		correctArea = node.areaIndex == 1;
		if (correctArea)
		{
			Events.OnCardDataCreated += CardDataCreated;
		}
	}

	public void PostBattleSetUp(CampaignNode node)
	{
		if (correctArea)
		{
			Events.OnCardDataCreated -= CardDataCreated;
		}

		correctArea = false;
	}

	public void CardDataCreated(CardData cardData)
	{
		if (!correctArea)
		{
			return;
		}

		switch (cardData.cardType.name)
		{
			case "Enemy":
			case "Miniboss":
			case "Boss":
			if (cardData.hasAttack)
			{
				cardData.damage++;
				}
	
				break;
		}
	}
}
