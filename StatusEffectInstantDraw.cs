#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Draw", fileName = "Instant Draw")]
public class StatusEffectInstantDraw : StatusEffectInstant
{
	public override IEnumerator Process()
	{
		Character player = References.Player;
		if (player.drawContainer.Empty && player.discardContainer.Empty)
		{
			if (NoTargetTextSystem.Exists())
			{
				yield return NoTargetTextSystem.Run(target, NoTargetType.NoCardsToDraw);
			}
		}
		else
		{
			ActionQueue.Stack(new ActionDraw(player, GetAmount()), fixedPosition: true);
		}

		yield return base.Process();
	}
}
