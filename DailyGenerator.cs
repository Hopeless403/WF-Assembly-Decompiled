#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DailyGenerator : MonoBehaviour
{
	[SerializeField]
	public Script[] deckRandomizers;

	[SerializeField]
	public GameModifierData[] goodModifiers;

	[SerializeField]
	public GameModifierData[] badModifiers;

	[SerializeField]
	public GameModifierData[] neutralModifiers;

	[HideInInspector]
	public bool running;

	public IEnumerator Run(int seed, GameMode gameMode)
	{
		running = true;
		Random.InitState(seed);
		Names.Reset();
		ClassData classData = gameMode.classes.RandomItem();
		References.PlayerData = new PlayerData(classData, classData.startingInventory.Clone());
		string assetName = MetaprogressionSystem.GetAllPets().RandomItem();
		CardData item = AddressableLoader.Get<CardData>("CardData", assetName).Clone();
		References.PlayerData.inventory.deck.Insert(0, item);
		CardData item2 = classData.leaders.RandomItem().Clone();
		References.PlayerData.inventory.deck.Insert(0, item2);
		Campaign.Data = new CampaignData(gameMode, seed)
		{
			GameMode = gameMode,
			GameVersion = Config.data.version,
			Seed = seed
		};
		yield return deckRandomizers.RandomItem().Run();
		int num = Random.Range(-100, 0);
		foreach (CardData item3 in References.PlayerData.inventory.deck)
		{
			num += item3.value - 25;
			if (item3.upgrades != null)
			{
				num += item3.upgrades.Count * 50;
			}
		}

		Debug.Log($"Daily Generator → Deck Value: {num}");
		int num2 = 0;
		List<GameModifierData> list = badModifiers.InRandomOrder().ToList();
		for (int i = 0; i < 2; i++)
		{
			GameModifierData gameModifierData = list.FirstOrDefault((GameModifierData a) => a.visible);
			if (!gameModifierData)
			{
				break;
			}

			list.Remove(gameModifierData);
			ModifierSystem.AddModifier(Campaign.Data, gameModifierData);
			num += gameModifierData.value;
			Debug.Log($"Daily Generator → Adding [{gameModifierData.name}] Modifier. New Deck Value: {num}");
			num2++;
		}

		List<GameModifierData> list2 = goodModifiers.InRandomOrder().ToList();
		for (int j = 0; j < 1; j++)
		{
			GameModifierData gameModifierData2 = list2.FirstOrDefault((GameModifierData a) => a.visible);
			if (!gameModifierData2)
			{
				break;
			}

			list2.Remove(gameModifierData2);
			ModifierSystem.AddModifier(Campaign.Data, gameModifierData2);
			num += gameModifierData2.value;
			Debug.Log($"Daily Generator → Adding [{gameModifierData2.name}] Modifier. New Deck Value: {num}");
			num2++;
		}

		while (num2 < 6 && Mathf.Abs(num) > 20)
		{
			List<GameModifierData> list3 = ((num > 0 && list.Count > 0) ? list : list2);
			if (list3.Count <= 0)
			{
				break;
			}

			GameModifierData gameModifierData3 = list3.FirstOrDefault((GameModifierData a) => !a.visible);
			if (!gameModifierData3)
			{
				break;
			}

			list3.Remove(gameModifierData3);
			ModifierSystem.AddModifier(Campaign.Data, gameModifierData3);
			num += gameModifierData3.value;
			Debug.Log($"Daily Generator → Adding [{gameModifierData3.name}] Modifier. New Deck Value: {num}");
			num2++;
			if (num2 >= 3 && Random.Range(0f, 1f) < 0.5f)
			{
				break;
			}
		}

		yield return Events.InvokeCampaignInit();
		running = false;
	}

	public IEnumerator Test(GameMode gameMode, int days = 730)
	{
		Debug.Log($"Daily Generator → Testing {days} Days");
		int dayOffset = DailyFetcher.DayOffset;
		Dictionary<GameModifierData, int> modifiers = new Dictionary<GameModifierData, int>();
		GameModifierData[] array = goodModifiers;
		foreach (GameModifierData key in array)
		{
			modifiers[key] = 0;
		}

		array = badModifiers;
		foreach (GameModifierData key2 in array)
		{
			modifiers[key2] = 0;
		}

		array = neutralModifiers;
		foreach (GameModifierData key3 in array)
		{
			modifiers[key3] = 0;
		}

		int total = 0;
		for (int i = 0; i < days; i++)
		{
			DailyFetcher.DayOffset = dayOffset + i;
			yield return DailyFetcher.FetchDateTime();
			int seed = DailyFetcher.GetSeed();
			yield return Run(seed, gameMode);
			foreach (GameModifierData modifier in Campaign.Data.Modifiers)
			{
				modifiers[modifier]++;
				total++;
			}
		}

		DailyFetcher.DayOffset = dayOffset;
		yield return DailyFetcher.FetchDateTime();
		Debug.Log("Daily Generator → Results:");
		foreach (KeyValuePair<GameModifierData, int> item in modifiers)
		{
			Debug.Log($"{item.Key.name} Count: {item.Value} ({Mathf.RoundToInt((float)item.Value / (float)total * 100f)}%)");
		}
	}
}
