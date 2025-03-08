#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;

public static class CreatedByLookup
{
	public static readonly Dictionary<string, string> lookup = new Dictionary<string, string>
	{
		{ "Dregg", "Egg" },
		{ "TailsOne", "TailsTwo" },
		{ "TailsTwo", "TailsThree" },
		{ "TailsThree", "TailsFour" },
		{ "TailsFour", "TailsFive" },
		{ "Beepop", "BeepopMask" },
		{ "Plep", "JunjunMask" },
		{ "Fallow", "FallowMask" },
		{ "Tigris", "TigrisMask" },
		{ "Leech", "Leecher" },
		{ "Pigeon", "PigeonCage" },
		{ "Popper", "PopPopper" },
		{ "Snuffer", "SnufferMask" }
	};

	public static bool TryGetCreatedBy(string cardDataName, out string parentCardDataName)
	{
		if (lookup.TryGetValue(cardDataName, out var value))
		{
			parentCardDataName = value;
			return true;
		}

		parentCardDataName = cardDataName;
		return false;
	}

	public static void TryGetCreatedByRoot(string cardDataName, out string rootCardDataName)
	{
		rootCardDataName = cardDataName;
		while (TryGetCreatedBy(rootCardDataName, out rootCardDataName))
		{
		}
	}

	public static List<string> GetCreatedByThis(string cardDataName)
	{
		List<string> list = new List<string>();
		foreach (var (text3, text4) in lookup)
		{
			if (text4 == cardDataName)
			{
				list.Add(text3);
				list.AddRange(GetCreatedByThis(text3));
			}
		}

		return list;
	}

	public static void Add(string cardDataName, string parentCardDataName)
	{
		lookup.Add(cardDataName, parentCardDataName);
	}
}
