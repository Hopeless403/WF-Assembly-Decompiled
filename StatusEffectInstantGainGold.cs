#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Gain Gold", fileName = "Gain Gold")]
public class StatusEffectInstantGainGold : StatusEffectInstant
{
	[SerializeField]
	public bool take;

	public override IEnumerator Process()
	{
		if ((bool)target)
		{
			Character player = References.Player;
			if ((bool)player && player.data != null && (bool)player.data.inventory)
			{
				int amount = GetAmount();
				if (take)
				{
					amount = Mathf.Min(player.data.inventory.gold.Value, amount);
					if (amount > 0)
					{
						player.SpendGold(amount);
					}
				}
				else
				{
					Events.InvokeDropGold(amount, applier.data.name, player, applier.transform.position);
				}
			}
		}

		yield return base.Process();
	}
}
