#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class BattleLogDisplayBuilder : MonoBehaviour
{
	[Serializable]
	public struct EntryLookup
	{
		public BattleLogType type;

		public AssetReferenceGameObject entryPrefab;
	}

	[SerializeField]
	public BattleLogDisplay battleLogDisplay;

	[SerializeField]
	public Transform tempGroup;

	[SerializeField]
	public Transform finalGroup;

	[SerializeField]
	public AssetReferenceGameObject groupPrefabRef;

	[SerializeField]
	public EntryLookup[] entryTypes;

	public readonly Dictionary<BattleLogType, AssetReferenceGameObject> entryLookup = new Dictionary<BattleLogType, AssetReferenceGameObject>();

	public readonly List<Transform> progress = new List<Transform>();

	public BattleLogSystem battleLogSystem;

	public readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

	public CancellationToken cancellationToken;

	public int entries { get; set; }

	public bool running { get; set; }

	public void Awake()
	{
		this.entryLookup.Clear();
		EntryLookup[] array = entryTypes;
		for (int i = 0; i < array.Length; i++)
		{
			EntryLookup entryLookup = array[i];
			this.entryLookup[entryLookup.type] = entryLookup.entryPrefab;
		}

		cancellationToken = cancellationTokenSource.Token;
	}

	public async void OnEnable()
	{
		battleLogSystem = UnityEngine.Object.FindObjectOfType<BattleLogSystem>();
		if (!battleLogSystem)
		{
			entries = -1;
			return;
		}

		bool num = entries != battleLogSystem.list.Count;
		entries = battleLogSystem.list.Count;
		if (!num)
		{
			return;
		}

		battleLogDisplay.PromptScrollToBottom();
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
		finalGroup.DestroyAllChildren();
		await Populate();
		foreach (Transform item in progress)
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				item.SetParent(finalGroup);
				continue;
			}

			break;
		}

		progress.Clear();
		tempGroup.DestroyAllChildren();
		running = false;
	}

	public async Task Populate()
	{
		if (cancellationToken.IsCancellationRequested)
		{
			return;
		}

		int num = 0;
		int count = battleLogSystem.list.Count;
		List<Task> list = new List<Task>();
		for (int i = 0; i < count; i++)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return;
			}

			BattleLog battleLog = battleLogSystem.list[i];
			if (i == 0 || battleLog.type == BattleLogType.Turn)
			{
				int num2 = battleLogSystem.list.FindIndex(i + 1, (BattleLog a) => a.type == BattleLogType.Turn);
				if (num2 == -1)
				{
					num2 = count;
				}

				List<BattleLog> range = battleLogSystem.list.GetRange(i, num2 - i);
				i = num2 - 1;
				if (cancellationToken.IsCancellationRequested)
				{
					return;
				}

				progress.Add(null);
				list.Add(CreateGroup(num++, range));
			}
		}

		await Task.WhenAll(list);
	}

	public async Task CreateGroup(int groupIndex, List<BattleLog> logs)
	{
		if (cancellationToken.IsCancellationRequested)
		{
			return;
		}

		Transform transform = await CreateGroup(tempGroup);
		progress[groupIndex] = transform;
		List<Task<Transform>> tasks = new List<Task<Transform>>();
		foreach (BattleLog log in logs)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return;
			}

			tasks.Add(CreateEntry(log, transform));
		}

		await Task.WhenAll(tasks);
		if (cancellationToken.IsCancellationRequested)
		{
			return;
		}

		for (int i = 0; i < tasks.Count; i++)
		{
			Transform result = tasks[i].Result;
			if ((bool)result)
			{
				result.SetSiblingIndex(i + 1);
			}
		}
	}

	public async Task<Transform> CreateGroup(Transform parent)
	{
		return (await groupPrefabRef.InstantiateAsync(parent).Task).transform;
	}

	public async Task<Transform> CreateEntry(BattleLog log, Transform group)
	{
		AssetReferenceGameObject assetReferenceGameObject = entryLookup[log.type];
		if (cancellationToken.IsCancellationRequested)
		{
			return null;
		}

		GameObject result = await assetReferenceGameObject.InstantiateAsync(group).Task;
		await result.GetComponent<BattleLogEntry>().SetUp(log);
		return result.transform;
	}

	public void Cancel()
	{
		cancellationTokenSource.Cancel();
	}
}
