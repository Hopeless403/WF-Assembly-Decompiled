#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;

public class DeathSystem : GameSystem
{
	public static readonly Dictionary<ulong, int> treatAsTeam = new Dictionary<ulong, int>();

	public void OnEnable()
	{
		Events.OnEntityKilled += EntityKilled;
		Events.OnEntityCreated += EntityCreated;
	}

	public void OnDisable()
	{
		Events.OnEntityKilled -= EntityKilled;
		Events.OnEntityCreated -= EntityCreated;
	}

	public static void EntityCreated(Entity entity)
	{
		treatAsTeam.Remove(entity.data.id);
	}

	public void EntityKilled(Entity entity, DeathType deathType)
	{
		if ((bool)entity && entity.display is Card card && card.GetComponent<ICardDestroyed>() == null)
		{
			switch (deathType)
			{
				default:
					Destroy(card);
					break;
				case DeathType.Sacrifice:
					Sacrifice(card);
					break;
				case DeathType.Consume:
					Consume(card);
					break;
			}
		}
	}

	public void Destroy(Card card)
	{
		CardDestroyed cardDestroyed = card.gameObject.AddComponent<CardDestroyed>();
		cardDestroyed.canvasGroup = card.canvasGroup;
		cardDestroyed.Knockback(card.entity.lastHit);
		card.transform.parent = base.transform;
	}

	public void Sacrifice(Card card)
	{
		card.gameObject.AddComponent<CardDestroyedSacrifice>();
		card.transform.parent = base.transform;
	}

	public void Consume(Card card)
	{
		card.gameObject.AddComponent<CardDestroyedConsume>();
		card.transform.parent = base.transform;
	}

	public static bool KilledByOwnTeam(Entity entity)
	{
		if (entity.lastHit != null && (bool)entity.lastHit.owner && entity.lastHit.owner.team == entity.owner.team)
		{
			return entity.lastHit.attacker != entity;
		}

		return false;
	}

	public static void TreatAsTeam(ulong cardDataId, int team)
	{
		treatAsTeam.Add(cardDataId, team);
	}

	public static bool CheckTeamIsAlly(Entity entity, Entity checkAgainst)
	{
		if (treatAsTeam.TryGetValue(entity.data.id, out var value))
		{
			return checkAgainst.owner.team == value;
		}

		return checkAgainst.owner.team == entity.owner.team;
	}
}
