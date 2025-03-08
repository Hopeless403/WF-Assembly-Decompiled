#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using Dead;
using UnityEngine;

public class JournalPageUnlockSystem : GameSystem
{
	[SerializeField]
	public JournalPageData[] pages;

	public void OnEnable()
	{
		Events.OnCampaignLoadPreset += InsertJournalPages;
	}

	public void OnDisable()
	{
		Events.OnCampaignLoadPreset -= InsertJournalPages;
	}

	public void InsertJournalPages(ref string[] lines)
	{
		List<string> unlockedList = MetaprogressionSystem.GetUnlockedList();
		JournalPageData[] array = pages;
		foreach (JournalPageData journalPageData in array)
		{
			if (!journalPageData.unlockOnMap || unlockedList.Contains(journalPageData.unlock.name) || !IsLegal(journalPageData))
			{
				continue;
			}

			int num = 0;
			for (int num2 = lines[0].Length - 1; num2 >= 0; num2--)
			{
				if (lines[2][num2] == journalPageData.mapTierIndex && (lines[0][num2] == journalPageData.mapAfterLetter || lines[1][num2] == journalPageData.mapAfterLetter))
				{
					num = num2 + 1;
					break;
				}
			}

			lines[0] = lines[0].Insert(num, journalPageData.mapNodeType.letter);
			lines[1] = lines[1].Insert(num, " ");
			lines[2] = lines[2].Insert(num, lines[2][num - 1].ToString());
			lines[3] = lines[3].Insert(num, lines[3][num - 1].ToString());
		}
	}

	public static bool IsLegal(JournalPageData page)
	{
		if (page.legalGameModes.Contains(Campaign.Data.GameMode) && page.legalTribes.Contains(References.PlayerData.classData) && HasRequiredStormPoints(page))
		{
			return HasRequiredModifiers(page);
		}

		return false;
	}

	public static bool HasRequiredStormPoints(JournalPageData page)
	{
		if (page.requiresStormPoints <= 0)
		{
			return true;
		}

		return StormBellManager.GetCurrentStormPoints(StormBellManager.GetActiveStormBells()) >= page.requiresStormPoints;
	}

	public static bool HasRequiredModifiers(JournalPageData page)
	{
		GameModifierData[] requiresModifiers = page.requiresModifiers;
		if (requiresModifiers != null && requiresModifiers.Length > 0)
		{
			if (Campaign.Data.Modifiers != null)
			{
				return Campaign.Data.Modifiers.ContainsAll(page.requiresModifiers);
			}

			return false;
		}

		return true;
	}

	public static CampaignGenerator.Node CreateNode(float x, float y, string type, int positionIndex)
	{
		Vector2 vector = Dead.Random.Vector2();
		x += vector.x;
		y += vector.y;
		return new CampaignGenerator.Node(x, y, 1f, 0, positionIndex, 0, type);
	}
}
