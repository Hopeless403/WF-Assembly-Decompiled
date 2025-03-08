#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss Rewards/Modifier", fileName = "Modifier")]
public class BossRewardDataModifier : BossRewardData
{
	[Serializable]
	public new class Data : BossRewardData.Data
	{
		public string modifierName;

		public GameModifierData GetModifier()
		{
			return AddressableLoader.Get<GameModifierData>("GameModifierData", modifierName);
		}

		public override void Select()
		{
			GameModifierData modifier = GetModifier();
			ModifierSystem.AddModifier(Campaign.Data, modifier);
			Routine.Clump clump = new Routine.Clump();
			Script[] startScripts = modifier.startScripts;
			foreach (Script script in startScripts)
			{
				clump.Add(script.Run());
			}

			startScripts = modifier.setupScripts;
			foreach (Script script2 in startScripts)
			{
				clump.Add(script2.Run());
			}

			string[] systemsToAdd = modifier.systemsToAdd;
			foreach (string text in systemsToAdd)
			{
				Debug.Log($"[{modifier}] adding system: {text}");
				Campaign.instance.gameObject.AddComponentByName(text);
			}
		}
	}

	public override BossRewardData.Data Pull()
	{
		string modifierName = References.Player.GetComponent<CharacterRewards>().Pull<GameModifierData>(this, "Modifiers").name;
		return new Data
		{
			type = Type.Modifier,
			modifierName = modifierName
		};
	}
}
