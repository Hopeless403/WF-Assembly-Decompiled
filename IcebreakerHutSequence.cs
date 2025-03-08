#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class IcebreakerHutSequence : BuildingSequence
{
	[SerializeField]
	public string metaprogressionKey = "events";

	[SerializeField]
	public ImageSprite[] nodes;

	[SerializeField]
	public ButtonAnimator[] buttons;

	[SerializeField]
	public ChallengeData[] challenges;

	[SerializeField]
	public ChallengeDisplayCreator challengeDisplayCreator;

	[SerializeField]
	public string firstGreetKey = "icebreakerFirstGreet";

	[SerializeField]
	public MapInspectSequence mapInspectSequence;

	[SerializeField]
	public EventReference denySfx;

	public readonly List<bool> unlocked = new List<bool>();

	public override IEnumerator Sequence()
	{
		if (building.HasUncheckedUnlocks)
		{
			foreach (string uncheckedUnlock in building.uncheckedUnlocks)
			{
				Unlock(uncheckedUnlock);
			}

			building.uncheckedUnlocks.Clear();
			TalkerSay("new event", 0.5f);
		}
		else if (!firstGreetKey.IsNullOrEmpty() && !SaveSystem.LoadProgressData(firstGreetKey, defaultValue: false))
		{
			TalkerFirstGreet();
			SaveSystem.SaveProgressData(firstGreetKey, value: true);
		}
		else
		{
			TalkerGreet();
		}

		int num = building.checkedUnlocks?.Count ?? 0;
		SetUpMapNodes(num);
		SetCurrentChallenge(num);
		yield return null;
	}

	public void Unlock(string unlockDataName)
	{
		List<string> list = SaveSystem.LoadProgressData<List<string>>(base.building.type.unlockedCheckedKey, null);
		if (list == null)
		{
			list = new List<string>();
		}

		list.Add(unlockDataName);
		SaveSystem.SaveProgressData(base.building.type.unlockedCheckedKey, list);
		Building building = base.building;
		if (building.unlocks == null)
		{
			building.unlocks = new List<string>();
		}

		base.building.unlocks.Add(unlockDataName);
	}

	public void SetUpMapNodes(int unlocked)
	{
		List<string> list = MetaprogressionSystem.Get<List<string>>(metaprogressionKey);
		for (int i = 0; i < list.Count; i++)
		{
			this.unlocked.Add(i < unlocked);
		}

		for (int j = 0; j < unlocked && j < list.Count; j++)
		{
			string assetName = list[j];
			CampaignNodeType campaignNodeType = AddressableLoader.Get<CampaignNodeType>("CampaignNodeType", assetName);
			nodes[j].SetSprite(campaignNodeType.mapNodeSprite);
			buttons[j].interactable = true;
		}
	}

	public void SetCurrentChallenge(int unlocked)
	{
		if (unlocked < challenges.Length)
		{
			challengeDisplayCreator.challenge = challenges[unlocked];
			challengeDisplayCreator.Check();
		}
	}

	public void TryInspect(int mapNodeIndex)
	{
		if (unlocked[mapNodeIndex])
		{
			mapInspectSequence.Inspect(mapNodeIndex);
		}
		else
		{
			Deny(nodes[mapNodeIndex].gameObject);
		}
	}

	public void Deny(GameObject obj)
	{
		SfxSystem.OneShot(denySfx);
		LeanTween.cancel(obj);
		LeanTween.moveLocal(obj, Vector3.zero, 0.67f).setFrom(new Vector3(0.5f.WithRandomSign(), 0f, 0f)).setEaseOutElastic();
	}
}
