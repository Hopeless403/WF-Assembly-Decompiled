#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class BuildingBalloon : MonoBehaviour
{
	[SerializeField]
	public string gameModeName = "GameModeDaily";

	[SerializeField]
	public GameObject displayPrefab;

	public Building _building;

	public Building building => _building ?? (_building = GetComponent<Building>());

	public void Select()
	{
		GameMode gameMode = AddressableLoader.Get<GameMode>("GameMode", gameModeName);
		if (Campaign.CheckContinue(gameMode))
		{
			Campaign.Data = CampaignData.Load(gameMode);
			DailyFetcher.SetContinueDateTime();
			StartCoroutine(ContinueRoutine());
		}
		else
		{
			building.CreateDisplay(displayPrefab);
		}
	}

	public static IEnumerator ContinueRoutine()
	{
		yield return SceneManager.Load("ContinueRun", SceneType.Temporary);
		yield return SceneManager.WaitUntilUnloaded("ContinueRun");
	}
}
