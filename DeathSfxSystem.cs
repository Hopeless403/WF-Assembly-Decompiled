#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using Dead;
using FMODUnity;
using UnityEngine;

public class DeathSfxSystem : GameSystem
{
	[Serializable]
	public class Profile
	{
		public EventReference eventReference;

		public CardRef[] cards;

		public float chance;
	}

	[Serializable]
	public class CardRef
	{
		public CardData card;

		public float pitchShift;
	}

	[SerializeField]
	public float globalChance = 0.1f;

	[SerializeField]
	public float globalChanceAdd = 0.02f;

	[SerializeField]
	public Profile[] profiles;

	public float currentGlobalChance;

	public readonly Dictionary<string, Tuple<Profile, float>> profileLookup = new Dictionary<string, Tuple<Profile, float>>();

	public void OnEnable()
	{
		currentGlobalChance = globalChance;
		Events.OnEntityKilled += EntityKilled;
		profileLookup.Clear();
		Profile[] array = profiles;
		foreach (Profile profile in array)
		{
			CardRef[] cards = profile.cards;
			foreach (CardRef cardRef in cards)
			{
				profileLookup[cardRef.card.name] = new Tuple<Profile, float>(profile, cardRef.pitchShift);
			}
		}
	}

	public void OnDisable()
	{
		Events.OnEntityKilled -= EntityKilled;
	}

	public void EntityKilled(Entity entity, DeathType deathType)
	{
		if (profileLookup.TryGetValue(entity.data.name, out var value))
		{
			Profile item = value.Item1;
			float item2 = value.Item2;
			if (CheckChance(item.chance))
			{
				SfxSystem.OneShot(item.eventReference).setPitch(1f + item2);
			}
		}
	}

	public bool CheckChance(float chance)
	{
		if (PettyRandom.Range(0f, 1f) < chance * currentGlobalChance)
		{
			currentGlobalChance = globalChance;
			return true;
		}

		currentGlobalChance += globalChanceAdd;
		return false;
	}
}
