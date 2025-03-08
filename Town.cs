#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Town : MonoBehaviourSingleton<Town>
{
	[SerializeField]
	public HelpPanelShower tutorialPrompt;

	[SerializeField]
	public EventReference denySfxEvent;

	public override void Awake()
	{
		base.Awake();
		CheckStartConstruction();
	}

	public IEnumerator Start()
	{
		Building[] array = Object.FindObjectsOfType<Building>(includeInactive: true);
		for (int i = 0; i < array.Length; i++)
		{
			array[i].CheckIfUnlocked();
		}

		if ((bool)CardFramesSystem.instance && CardFramesSystem.instance.AnyNewFrames())
		{
			yield return CardFramesSystem.instance.DisplayNewFrames();
		}

		if (NewFinalBossChecker.Check())
		{
			yield return NewFinalBossChecker.Run();
		}

		if (MetaprogressionSystem.AnyUnlocksReady())
		{
			yield return SceneManager.Load("TownUnlocks", SceneType.Temporary);
			yield return SceneManager.WaitUntilUnloaded("TownUnlocks");
		}

		CheckTutorialPrompt();
	}

	public static void SelectBuilding(Building building)
	{
		if (building.built)
		{
			if (!building.Select())
			{
				Events.InvokeCameraAnimation("Shake");
				SfxSystem.OneShot(MonoBehaviourSingleton<Town>.instance.denySfxEvent);
			}
		}
		else
		{
			Events.InvokeCameraAnimation("Shake");
			SfxSystem.OneShot(MonoBehaviourSingleton<Town>.instance.denySfxEvent);
		}
	}

	public static void CheckStartConstruction()
	{
		List<string> unlockedList = MetaprogressionSystem.GetUnlockedList();
		List<string> list = MetaprogressionSystem.Get<List<string>>("buildings");
		List<UnlockData> list2 = new List<UnlockData>();
		foreach (string item in list)
		{
			if (!unlockedList.Contains(item))
			{
				UnlockData value = AddressableLoader.Get<UnlockData>("UnlockData", item);
				list2.AddIfNotNull(value);
			}
		}

		UnlockData unlockData = null;
		foreach (UnlockData item2 in list2)
		{
			if (item2.IsActive && MetaprogressionSystem.CheckUnlockRequirements(item2, unlockedList))
			{
				unlockData = item2;
				break;
			}
		}

		if ((bool)unlockData)
		{
			unlockedList.Add(unlockData.name);
			SaveSystem.SaveProgressData("unlocked", unlockedList);
			MetaprogressionSystem.SetUnlocksReady(unlockData.name);
		}
	}

	public void CheckTutorialPrompt()
	{
		if (!SaveSystem.LoadProgressData("tutorialTownDone", defaultValue: false))
		{
			tutorialPrompt.Show();
			SaveSystem.SaveProgressData("tutorialTownDone", value: true);
		}
	}
}
