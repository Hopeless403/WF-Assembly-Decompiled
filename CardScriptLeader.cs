#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Leader", menuName = "Card Scripts/Leader")]
public class CardScriptLeader : CardScript
{
	[SerializeField]
	public CharacterType[] characterTypeOptions;

	[SerializeField]
	public LeaderProfileData[] leaderProfileOptions;

	public override void Run(CardData target)
	{
		CharacterType characterType = characterTypeOptions.RandomItem();
		CharacterData characterData = new CharacterData
		{
			race = characterType.race,
			gender = characterType.gender
		};
		LeaderProfileData leaderProfileData = leaderProfileOptions.RandomItem();
		leaderProfileData.Apply(characterType);
		target.backgroundSprite = leaderProfileData.GetRandomBackground();
		characterData.Randomize(characterType);
		leaderProfileData.UnApply(characterType);
		target.forceTitle = characterData.title;
		target.SetCustomData("CharacterData", characterData);
	}
}
