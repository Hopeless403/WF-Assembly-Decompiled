#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;

public class BombskullClunkersModifierSystem : GameSystem
{
	public static CardUpgradeData _upgradeData;

	public readonly List<ulong> ids = new List<ulong>();

	public static CardUpgradeData upgradeData => _upgradeData ?? (_upgradeData = AddressableLoader.Get<CardUpgradeData>("CardUpgradeData", "CardUpgradeBombskull"));

	public void OnEnable()
	{
		Events.OnCardDataCreated += CardDataCreated;
		Events.OnEntityCreated += EntityCreated;
	}

	public void OnDisable()
	{
		Events.OnCardDataCreated -= CardDataCreated;
		Events.OnEntityCreated -= EntityCreated;
	}

	public void CardDataCreated(CardData cardData)
	{
		if (cardData.cardType.name == "Clunker")
		{
			ids.Add(cardData.id);
		}
	}

	public void EntityCreated(Entity entity)
	{
		if (ids.Contains(entity.data.id))
		{
			ids.Remove(entity.data.id);
			if ((bool)entity.owner && entity.owner.team == References.Player.team)
			{
				upgradeData.Clone().Assign(entity.data);
			}
		}
	}
}
