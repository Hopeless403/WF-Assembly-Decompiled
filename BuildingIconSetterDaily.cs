#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class BuildingIconSetterDaily : BuildingIconSetter
{
	[SerializeField]
	public string gameModeName = "GameModeDaily";

	public GameMode _gameMode;

	public GameMode gameMode => _gameMode ?? (_gameMode = AddressableLoader.Get<GameMode>("GameMode", gameModeName));

	public override string Get(Building building)
	{
		string result = "";
		if (DailyFetcher.CanPlay())
		{
			result = "Unlock";
		}
		else if (Campaign.CheckContinue(gameMode))
		{
			result = "Unlock";
		}

		return result;
	}
}
