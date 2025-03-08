#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using Dead;
using NaughtyAttributes;
using UnityEngine;

public class LeaderNameHistoryDisplay : MonoBehaviour
{
	[SerializeField]
	public LeaderNameHistoryEntry namePrefab;

	public void OnEnable()
	{
		Populate();
	}

	public void OnDisable()
	{
		base.transform.DestroyAllChildren();
	}

	public void Populate()
	{
		List<JournalNameHistory.Name> list = SaveSystem.LoadProgressData<List<JournalNameHistory.Name>>("leaderNameHistory");
		if (list == null)
		{
			return;
		}

		Transform parent = base.transform;
		foreach (JournalNameHistory.Name item in list)
		{
			LeaderNameHistoryEntry leaderNameHistoryEntry = Object.Instantiate(namePrefab, parent);
			leaderNameHistoryEntry.transform.localPosition = item.position;
			leaderNameHistoryEntry.Assign(item);
		}
	}

	[Button(null, EButtonEnableMode.Always)]
	public void AddName()
	{
		if (PettyRandom.Range(0f, 1f) > 0.25f)
		{
			JournalNameHistory.MostRecentNameKilled();
		}
		else
		{
			JournalNameHistory.MostRecentNameMissing();
		}

		JournalNameHistory.FadePrevious();
		JournalNameHistory.AddName(Names.Pull("SnowdwellerMale"));
		Repopulate();
	}

	public void Repopulate()
	{
		base.transform.DestroyAllChildren();
		Populate();
	}
}
