#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using Dead;
using UnityEngine;

public static class JournalNameHistory
{
	[Serializable]
	public class Name
	{
		public string text;

		public bool killed;

		public bool missing;

		public float opacity;

		public Vector2 position;

		public Vector2 offset;

		public float angle;

		public Name()
		{
		}

		public Name(string text, Vector2 randomOffset, float randomAngle)
		{
			this.text = text;
			opacity = 1f;
			offset = new Vector2(randomOffset.x * PettyRandom.Range(-1f, 1f), randomOffset.y * PettyRandom.Range(-1f, 1f));
			angle = randomAngle * PettyRandom.Range(-1f, 1f);
		}
	}

	public const int maxEntries = 26;

	public const float fadePrevious = 0.035f;

	public static readonly Vector2 startPos = new Vector2(-1.733333f, 3f);

	public static readonly Vector2 spacing = new Vector2(1.733333f, 1f);

	public static readonly Vector2 bounds = new Vector2(1.733333f, -3f);

	public static readonly Vector2 randomOffset = new Vector2(0.5f, 0.25f);

	public const float randomAngle = 3f;

	public static void AddName(string name)
	{
		Name name2 = new Name(name, randomOffset, 3f);
		List<Name> list = SaveSystem.LoadProgressData<List<Name>>("leaderNameHistory");
		if (list == null)
		{
			list = new List<Name>();
		}

		Vector2 position;
		if (list.Count <= 0)
		{
			position = startPos;
		}
		else
		{
			List<Name> list2 = list;
			int index = list2.Count - 1;
			position = list2[index].position;
		}

		Vector2 position2 = position;
		if (list.Count > 0)
		{
			position2.y -= spacing.y;
			if (position2.y < bounds.y)
			{
				position2.y = startPos.y;
				position2.x += spacing.x;
				if (position2.x > bounds.x)
				{
					position2.x = startPos.x;
				}
			}
		}

		name2.position = position2;
		list.Add(name2);
		SaveSystem.SaveProgressData("leaderNameHistory", list);
	}

	public static void FadePrevious()
	{
		List<Name> list = SaveSystem.LoadProgressData<List<Name>>("leaderNameHistory");
		if (list == null || list.Count <= 0)
		{
			return;
		}

		while (list.Count > 25)
		{
			list.RemoveAt(0);
		}

		foreach (Name item in list)
		{
			item.opacity -= 0.035f;
		}

		SaveSystem.SaveProgressData("leaderNameHistory", list);
	}

	public static void MostRecentNameKilled()
	{
		List<Name> list = SaveSystem.LoadProgressData<List<Name>>("leaderNameHistory");
		if (list != null && list.Count > 0)
		{
			int index = list.Count - 1;
			list[index].killed = true;
			SaveSystem.SaveProgressData("leaderNameHistory", list);
		}
	}

	public static void MostRecentNameMissing()
	{
		List<Name> list = SaveSystem.LoadProgressData<List<Name>>("leaderNameHistory");
		if (list != null && list.Count > 0)
		{
			int index = list.Count - 1;
			list[index].missing = true;
			SaveSystem.SaveProgressData("leaderNameHistory", list);
		}
	}
}
