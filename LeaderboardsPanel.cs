#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using Steamworks;
using Steamworks.Data;
using UnityEngine;

public class LeaderboardsPanel : MonoBehaviour
{
	[SerializeField]
	public GameObject loading;

	[SerializeField]
	public GameObject noEntries;

	[SerializeField]
	public GameObject noConnection;

	[SerializeField]
	public GameObject tooManyRequests;

	[SerializeField]
	public Transform entriesGroup;

	[SerializeField]
	public DateTextSetter dateTextSetter;

	[SerializeField]
	public LeaderboardsFetcher fetcher;

	[SerializeField]
	public LeaderboardsEntry entryPrefab;

	[SerializeField]
	public bool fetchEveryTime = true;

	[SerializeField]
	public int entriesPerPage = 10;

	[SerializeField]
	public LeaderboardsFetcher.Type type;

	[Header("Buttons")]
	[SerializeField]
	public ButtonAnimator globalButton;

	[SerializeField]
	public ButtonAnimator friendsButton;

	[SerializeField]
	public ButtonAnimator playerRankButton;

	[SerializeField]
	public ButtonAnimator nextPageButton;

	[SerializeField]
	public ButtonAnimator prePageButton;

	[SerializeField]
	public ButtonAnimator nextDayButton;

	[SerializeField]
	public ButtonAnimator preDayButton;

	public int page;

	public int dayOffset;

	public int playerRankPage;

	public const int maxDayOffset = 100;

	public int maxPage = 9999;

	public bool fetched;

	public bool fetch
	{
		get
		{
			if (fetched)
			{
				return fetchEveryTime;
			}

			return true;
		}
	}

	public void OnEnable()
	{
		page = 0;
		dayOffset = 0;
		nextDayButton.interactable = false;
		if (fetch)
		{
			Clear();
			StartCoroutine(Fetch(aroundPlayer: true));
		}
	}

	public void OnDisable()
	{
		StopAllCoroutines();
	}

	public void Clear()
	{
		noEntries.SetActive(value: false);
		noConnection.SetActive(value: false);
		tooManyRequests.SetActive(value: false);
		entriesGroup.DestroyAllChildren();
		prePageButton.interactable = false;
		nextPageButton.interactable = false;
	}

	public IEnumerator Fetch(bool aroundPlayer)
	{
		yield break;
	}

	public void Populate(IReadOnlyCollection<LeaderboardEntry> entries)
	{
		bool flag = entries != null && entries.Count > 0;
		noEntries.SetActive(!flag);
		if (!flag)
		{
			return;
		}

		foreach (LeaderboardEntry entry in entries)
		{
			LeaderboardsEntry leaderboardsEntry = Object.Instantiate(entryPrefab, entriesGroup);
			Friend user = entry.User;
			bool isMe = user.IsMe;
			user = entry.User;
			leaderboardsEntry.Set(isMe, user.Name, entry.GlobalRank, entry.Score, SecondsToTimeString(entry.Details[0]));
		}
	}

	public void PreviousDay()
	{
		if (dayOffset < 100)
		{
			SetDayOffset(dayOffset + 1);
			Refetch(aroundPlayer: true);
		}
	}

	public void NextDay()
	{
		if (dayOffset > 0)
		{
			SetDayOffset(dayOffset - 1);
			Refetch(aroundPlayer: true);
		}
	}

	public void SetDayOffset(int value)
	{
		dayOffset = value;
		dateTextSetter.SetText(-dayOffset);
		fetcher.ResetPlayerRank();
		maxPage = 0;
		type = LeaderboardsFetcher.Type.Global;
		SetPage(0);
		playerRankButton.interactable = false;
		preDayButton.interactable = dayOffset < 100;
		nextDayButton.interactable = dayOffset > 0;
	}

	public void NextPage()
	{
		if (page < maxPage)
		{
			SetPage(page + 1);
			Refetch(aroundPlayer: false);
		}
	}

	public void PreviousPage()
	{
		if (page > 0)
		{
			SetPage(page - 1);
			Refetch(aroundPlayer: false);
		}
	}

	public void SetPage(int value)
	{
		page = value;
		nextPageButton.interactable = page < maxPage;
		prePageButton.interactable = page > 0;
	}

	public void Global()
	{
		if (type != 0 || page > 0)
		{
			type = LeaderboardsFetcher.Type.Global;
			SetPage(0);
			Refetch(aroundPlayer: false);
		}
	}

	public void Friends()
	{
		if (type != LeaderboardsFetcher.Type.Friends)
		{
			type = LeaderboardsFetcher.Type.Friends;
			SetPage(0);
			Refetch(aroundPlayer: false);
		}
	}

	public void JumpToPlayer()
	{
		if (fetcher.playerHasRank && page != playerRankPage)
		{
			type = LeaderboardsFetcher.Type.Global;
			SetPage(playerRankPage);
			Refetch(aroundPlayer: false);
		}
	}

	public void Refetch(bool aroundPlayer)
	{
		Clear();
		StopAllCoroutines();
		StartCoroutine(Fetch(aroundPlayer));
	}

	public static string SecondsToTimeString(int seconds)
	{
		float num = (float)seconds / 60f / 60f;
		int num2 = Mathf.FloorToInt(num);
		int num3 = Mathf.FloorToInt((num - (float)num2) * 60f);
		if (num2 <= 0)
		{
			return $"{num3}m";
		}

		return $"{num2}h {num3}m";
	}
}
