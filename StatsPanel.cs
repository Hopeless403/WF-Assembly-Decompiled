#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Linq;
using TMPro;
using UnityEngine;

public class StatsPanel : MonoBehaviour
{
	public struct Stat
	{
		public readonly GameStatData statData;

		public readonly float value;

		public readonly string stringValue;

		public readonly float priority;

		public Stat(GameStatData statData, CampaignStats runStats)
		{
			this.statData = statData;
			value = statData.GetValue(runStats);
			stringValue = statData.GetStringValue(runStats, value);
			priority = statData.GetPriority(value);
		}
	}

	[SerializeField]
	public TMP_Text titleElement;

	[SerializeField]
	public TMP_Text subtitleElement;

	[SerializeField]
	public Transform statsGroup;

	[SerializeField]
	public StatDisplay statPrefab;

	[SerializeField]
	public int maxStats = 6;

	[SerializeField]
	public GameStatData[] stats;

	public void Awake()
	{
		CardData leaderData = References.LeaderData;
		titleElement.text = leaderData.title;
		subtitleElement.gameObject.SetActive(value: false);
		CampaignStats runStats = StatsSystem.Get();
		stats.Shuffle();
		foreach (Stat item in from s in (from s in stats
				select new Stat(s, runStats) into s

				where s.value > 0f
				orderby Random.Range(0f, s.priority) descending
				select s).Take(maxStats)
			orderby s.statData.displayOrder descending
			select s)
		{
			StatDisplay statDisplay = Object.Instantiate(statPrefab, statsGroup);
			statDisplay.gameObject.SetActive(value: true);
			statDisplay.Assign(item.statData, item.stringValue);
		}
	}
}
