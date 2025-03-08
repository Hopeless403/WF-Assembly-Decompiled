#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.Localization.Tables;

[CreateAssetMenu(menuName = "Status Effects/Specific/Remove Effects", fileName = "Remove Effects")]
public class StatusEffectRemoveEffects : StatusEffectData
{
	public override bool RunBeginEvent()
	{
		if (target.data.playType != Card.PlayType.Place)
		{
			target.data.playType = Card.PlayType.Play;
			target.data.needsTarget = true;
			target.data.canPlayOnBoard = true;
			target.data.canPlayOnHand = false;
			target.data.canPlayOnFriendly = true;
			target.data.canPlayOnEnemy = true;
			target.data.playOnSlot = false;
			target.data.defaultPlayPosition = CardData.PlayPosition.None;
			target.data.targetConstraints = null;
			target.data.textKey.TableReference = default(TableReference);
			target.data.desc = "";
			target.display.promptUpdateDescription = true;
			target.PromptUpdate();
		}

		return false;
	}
}
