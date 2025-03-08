#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Linq;
using System.Threading.Tasks;

public class DynamicReactionTutorialSystem : GameSystem
{
	public void OnEnable()
	{
		Events.OnCampaignGenerated += CampaignGenerated;
		if (SaveSystem.LoadProgressData("dynamicReactionTutorial", defaultValue: false) || Campaign.Data.GameMode.name != "GameModeNormal")
		{
			base.enabled = false;
		}
	}

	public void OnDisable()
	{
		Events.OnCampaignGenerated -= CampaignGenerated;
	}

	public static Task CampaignGenerated()
	{
		CampaignNode campaignNode = References.Campaign.nodes.FirstOrDefault((CampaignNode a) => a.type.isBattle);
		if (campaignNode != null && campaignNode.data.TryGetValue("waves", out var value) && value is SaveCollection<BattleWaveManager.WaveData> saveCollection && saveCollection.Count > 0)
		{
			BattleWaveManager.WaveData waveData = saveCollection[0];
			if (waveData != null)
			{
				string[] array = new string[1] { "Smackgoon" };
				string[] array2 = new string[5] { "Chungoon", "Grouchy", "Snoolf", "Snowbo", "NakedGnome" };
				bool flag = false;
				for (int i = 0; i < waveData.Count; i++)
				{
					string cardName = waveData.GetCardName(i);
					if (array.Contains(cardName))
					{
						flag = true;
						break;
					}
				}

				if (!flag)
				{
					for (int j = 0; j < waveData.Count; j++)
					{
						string cardName2 = waveData.GetCardName(j);
						if (array2.Contains(cardName2) && waveData is BattleWaveManager.WaveDataBasic waveDataBasic)
						{
							BattleWaveManager.Card card = waveDataBasic.Get(j);
							card.cardName = array.RandomItem();
							card.upgradeNames = null;
							break;
						}
					}
				}
			}
		}

		return Task.CompletedTask;
	}
}
