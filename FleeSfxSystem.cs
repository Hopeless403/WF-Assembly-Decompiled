#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Dead;
using FMODUnity;
using UnityEngine;

public class FleeSfxSystem : GameSystem
{
	[Serializable]
	public class Profile
	{
		public EventReference eventReference;

		public CardData[] cards;

		public float chance;
	}

	[SerializeField]
	public float globalChance = 1f;

	[SerializeField]
	public Profile[] profiles;

	[SerializeField]
	public EventReference fallbackEnemy;

	[SerializeField]
	public EventReference fallbackPlayer;

	[SerializeField]
	public CardData[] excludeFromFallback;

	public readonly Dictionary<string, Profile> profileLookup = new Dictionary<string, Profile>();

	public void OnEnable()
	{
		Events.OnEntityFlee += EntityFlee;
		profileLookup.Clear();
		Profile[] array = profiles;
		foreach (Profile profile in array)
		{
			CardData[] cards = profile.cards;
			foreach (CardData cardData in cards)
			{
				profileLookup[cardData.name] = profile;
			}
		}
	}

	public void OnDisable()
	{
		Events.OnEntityFlee -= EntityFlee;
	}

	public void EntityFlee(Entity entity)
	{
		if (profileLookup.TryGetValue(entity.data.name, out var value))
		{
			if (CheckChance(value.chance * globalChance))
			{
				SfxSystem.OneShot(value.eventReference);
			}
		}
		else if (excludeFromFallback.All((CardData a) => a.name != entity.data.name) && CheckChance(globalChance))
		{
			SfxSystem.OneShot((entity.owner.team == References.Player.team) ? fallbackPlayer : fallbackEnemy);
			SfxSystem.OneShot(fallbackEnemy);
		}
	}

	public static bool CheckChance(float chance)
	{
		return PettyRandom.Range(0f, 1f) < chance;
	}
}
