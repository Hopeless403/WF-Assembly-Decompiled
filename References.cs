#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Linq;
using UnityEngine;

public class References : MonoBehaviourSingleton<References>
{
	public static PlayerData PlayerData;

	public static Character Player;

	public static MapNew Map;

	public static Campaign Campaign;

	public static Battle Battle;

	public ClassData[] classes;

	public AreaData[] areas;

	public HardModeModifierData[] hardModeModifiers;

	public BossRewardPool[] bossRewardPools;

	public static Transform _minibossCameraMover;

	public const string DefaultGameModeName = "GameModeNormal";

	public const string TutorialGameModeName = "GameModeTutorial";

	public static CardData LeaderData => PlayerData.inventory.deck.First((CardData a) => a.cardType.miniboss);

	public static ClassData[] Classes => MonoBehaviourSingleton<References>.instance.classes;

	public static AreaData[] Areas => MonoBehaviourSingleton<References>.instance.areas;

	public static Transform MinibossCameraMover
	{
		get
		{
			if ((bool)_minibossCameraMover)
			{
				return _minibossCameraMover;
			}

			GameObject gameObject = GameObject.FindWithTag("MinibossCameraTransform");
			if ((object)gameObject != null)
			{
				_minibossCameraMover = gameObject.transform;
			}

			return _minibossCameraMover;
		}
	}

	public static AreaData GetCurrentArea()
	{
		CampaignNode campaignNode = Campaign.FindCharacterNode(Player);
		return Areas[campaignNode.areaIndex];
	}
}
