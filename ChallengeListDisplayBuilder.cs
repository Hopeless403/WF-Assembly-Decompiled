#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ChallengeListDisplayBuilder : MonoBehaviour
{
	[SerializeField]
	public Transform tempGroup;

	[SerializeField]
	public Transform finalGroup;

	[SerializeField]
	public AssetReferenceGameObject inProgressPrefab;

	[SerializeField]
	public AssetReferenceGameObject completedPrefab;

	[SerializeField]
	public AssetReferenceGameObject lockedPrefab;

	public readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

	public CancellationToken cancellationToken;

	public Transform[] progress;

	public bool running { get; set; }

	public void Awake()
	{
		cancellationToken = cancellationTokenSource.Token;
	}

	public async void OnEnable()
	{
		if (running)
		{
			cancellationTokenSource.Cancel();
			while (running)
			{
				await Task.Yield();
			}
		}

		await Build();
	}

	public async Task Build()
	{
		running = true;
		GetChallengeLists(out var inProgressChallenges, out var completedChallenges, out var lockedChallenges, out var challengeProgress);
		finalGroup.DestroyAllChildren();
		int num = inProgressChallenges.Count + completedChallenges.Count + lockedChallenges.Count;
		progress = new Transform[num];
		int challengeIndex = 0;
		foreach (ChallengeData challenge in inProgressChallenges)
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				ChallengeProgress progressAmount = challengeProgress?.FirstOrDefault((ChallengeProgress a) => a.challengeName == challenge.name);
				await CreateInProgressEntry(challengeIndex++, challenge, progressAmount);
				continue;
			}

			break;
		}

		foreach (ChallengeData item in completedChallenges)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				break;
			}

			await CreateCompletedEntry(challengeIndex++, item);
		}

		foreach (ChallengeData item2 in lockedChallenges)
		{
			_ = item2;
			if (cancellationToken.IsCancellationRequested)
			{
				break;
			}

			await CreateLockedEntry(challengeIndex++);
		}

		Transform[] array = progress;
		foreach (Transform transform in array)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				break;
			}

			transform.SetParent(finalGroup);
		}

		progress = null;
		tempGroup.DestroyAllChildren();
		running = false;
	}

	public void GetChallengeLists(out List<ChallengeData> inProgressChallenges, out List<ChallengeData> completedChallenges, out List<ChallengeData> lockedChallenges, out List<ChallengeProgress> challengeProgress)
	{
		IEnumerable<ChallengeData> allChallenges = ChallengeSystem.GetAllChallenges();
		ChallengeSystem challengeSystem = Object.FindObjectOfType<ChallengeSystem>();
		bool flag = challengeSystem != null;
		List<string> list = SaveSystem.LoadProgressData<List<string>>("completedChallenges");
		if (list == null)
		{
			list = new List<string>();
		}

		if (flag)
		{
			list.AddRange(challengeSystem.saveRequired.Select((ChallengeData a) => a.name));
		}

		ChallengeProgressSystem challengeProgressSystem = Object.FindObjectOfType<ChallengeProgressSystem>();
		challengeProgress = (((object)challengeProgressSystem != null) ? challengeProgressSystem.progress : SaveSystem.LoadProgressData<List<ChallengeProgress>>("challengeProgress"));
		completedChallenges = new List<ChallengeData>();
		lockedChallenges = new List<ChallengeData>();
		inProgressChallenges = (flag ? challengeSystem.activeChallenges.Where((ChallengeData a) => !a.hidden).ToList() : new List<ChallengeData>());
		foreach (ChallengeData challenge in allChallenges)
		{
			if (list.Contains(challenge.name))
			{
				completedChallenges.Add(challenge);
				continue;
			}

			if (flag)
			{
				if (challenge.hidden || !inProgressChallenges.FirstOrDefault((ChallengeData a) => a.name == challenge.name))
				{
					lockedChallenges.Add(challenge);
				}

				continue;
			}

			bool flag2 = !challenge.hidden;
			if (flag2)
			{
				ChallengeData[] requires = challenge.requires;
				foreach (ChallengeData challengeData in requires)
				{
					if (!list.Contains(challengeData.name))
					{
						flag2 = false;
						break;
					}
				}
			}

			if (!flag2)
			{
				lockedChallenges.Add(challenge);
			}
			else
			{
				inProgressChallenges.Add(challenge);
			}
		}
	}

	public async Task CreateInProgressEntry(int challengeIndex, ChallengeData challengeData, ChallengeProgress progressAmount)
	{
		GameObject gameObject = await inProgressPrefab.InstantiateAsync(tempGroup).Task;
		progress[challengeIndex] = gameObject.transform;
		if (!cancellationToken.IsCancellationRequested)
		{
			ChallengeEntry component = gameObject.GetComponent<ChallengeEntry>();
			if ((object)component != null)
			{
				component.Assign(challengeData, completed: false);
				component.SetProgress(progressAmount?.currentValue ?? 0);
			}
		}
	}

	public async Task CreateCompletedEntry(int challengeIndex, ChallengeData challengeData)
	{
		GameObject gameObject = await completedPrefab.InstantiateAsync(tempGroup).Task;
		progress[challengeIndex] = gameObject.transform;
		if (!cancellationToken.IsCancellationRequested)
		{
			gameObject.GetComponent<ChallengeEntry>()?.Assign(challengeData, completed: true);
		}
	}

	public async Task CreateLockedEntry(int challengeIndex)
	{
		GameObject gameObject = await lockedPrefab.InstantiateAsync(tempGroup).Task;
		progress[challengeIndex] = gameObject.transform;
	}
}
