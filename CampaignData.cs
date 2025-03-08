#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using Dead;
using UnityEngine;

[Serializable]
public class CampaignData
{
	public GameMode GameMode { get; set; }

	public List<GameModifierData> Modifiers { get; set; }

	public string GameVersion { get; set; }

	public int Seed { get; set; } = -1;


	public CampaignData()
	{
	}

	public CampaignData(string gameModeName, int seed = -1)
	{
		if (AddressableLoader.TryGet<GameMode>("GameMode", gameModeName, out var result))
		{
			Init(result, seed);
		}
		else
		{
			Init(AddressableLoader.Get<GameMode>("GameMode", "GameModeNormal"), seed);
		}
	}

	public CampaignData(GameMode gameMode, int seed = -1)
	{
		Init(gameMode, seed);
	}

	public static CampaignData Load(GameMode gameMode)
	{
		CampaignData obj = new CampaignData
		{
			GameMode = gameMode,
			GameVersion = SaveSystem.LoadCampaignData<string>(gameMode, "gameVersion"),
			Seed = SaveSystem.LoadCampaignData(gameMode, "seed", 0)
		};
		Debug.Log(obj);
		return obj;
	}

	public void Init(GameMode gameMode, int seed = -1)
	{
		GameMode = gameMode;
		GameVersion = Config.data.version;
		if (seed >= 0)
		{
			Seed = seed;
		}
		else if (!gameMode.seed.IsNullOrWhitespace())
		{
			Seed = gameMode.seed.ToSeed();
		}
		else
		{
			Seed = SaveSystem.LoadProgressData("nextSeed", -1);
			if (Seed <= 0)
			{
				Seed = Dead.Random.Seed();
				SaveSystem.SaveProgressData("nextSeed", Seed);
			}
		}

		Debug.Log(this);
		Events.InvokeCampaignDataCreated(this);
	}

	public override string ToString()
	{
		return $"CampaignData (GameMode: {GameMode.name}, GameVersion: {GameVersion}, Seed: {Seed})";
	}
}
