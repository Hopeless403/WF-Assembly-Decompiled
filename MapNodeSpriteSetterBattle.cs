#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.Localization.Components;

public class MapNodeSpriteSetterBattle : MapNodeSpriteSetter
{
	[SerializeField]
	public SpriteRenderer @base;

	[SerializeField]
	public SpriteRenderer icon;

	[SerializeField]
	public LocalizeStringEvent battleNameString;

	[SerializeField]
	public GameObject iconObj;

	[SerializeField]
	public GameObject flagObj;

	public override void Set(MapNode mapNode)
	{
		if ((bool)@base)
		{
			AreaData areaData = References.Areas[mapNode.campaignNode.areaIndex];
			@base.sprite = areaData.battleBaseSprite;
		}

		if (mapNode.campaignNode.type is CampaignNodeTypeBattle && mapNode.campaignNode.data.TryGetValue("battle", out var value) && value is string assetName)
		{
			BattleData battleData = AddressableLoader.Get<BattleData>("BattleData", assetName);
			if ((object)battleData != null)
			{
				icon.sprite = battleData.sprite;
				if ((bool)battleNameString)
				{
					battleNameString.StringReference = battleData.nameRef;
				}
			}
		}

		if (mapNode.campaignNode.cleared && (bool)flagObj)
		{
			flagObj.SetActive(value: true);
			iconObj.SetActive(value: false);
		}
	}
}
