#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;

public class ChallengeListenerSystemHighCardsInHand : ChallengeListenerSystem
{
	public const int required = 12;

	public readonly List<Entity> summonedToCheck = new List<Entity>();

	public void OnEnable()
	{
		Events.OnCardDrawEnd += CardDrawEnd;
		Events.OnEntitySummoned += EntitySummoned;
		Events.OnEntityMove += EntityMove;
	}

	public void OnDisable()
	{
		Events.OnCardDrawEnd -= CardDrawEnd;
		Events.OnEntitySummoned -= EntitySummoned;
		Events.OnEntityMove -= EntityMove;
	}

	public void CardDrawEnd()
	{
		CheckRequirement();
	}

	public void EntitySummoned(Entity entity, Entity summonedBy)
	{
		summonedToCheck.Add(entity);
	}

	public void EntityMove(Entity entity)
	{
		int num = summonedToCheck.IndexOf(entity);
		if (num >= 0)
		{
			summonedToCheck.RemoveAt(num);
			CheckRequirement();
		}
	}

	public void CheckRequirement()
	{
		if (References.Player.handContainer.Count >= 12)
		{
			Complete();
		}
	}
}
