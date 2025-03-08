#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boost Attack Effects Or Damage", menuName = "Card Scripts/Boost Attack Effects Or Damage")]
public class CardScriptBoostAttackEffectsOrAttack : CardScript
{
	[SerializeField]
	public Vector2Int range = new Vector2Int(2, 2);

	[SerializeField]
	public float reducePerCharm = 0.5f;

	public override void Run(CardData target)
	{
		int num = range.Random();
		List<CardUpgradeData> upgrades = target.upgrades;
		if (upgrades != null && upgrades.Count > 0)
		{
			float num2 = num;
			foreach (CardUpgradeData upgrade in target.upgrades)
			{
				if (upgrade.type == CardUpgradeData.Type.Charm)
				{
					num2 -= reducePerCharm;
					if (num2 <= 0f)
					{
						break;
					}
				}
			}

			num = Mathf.CeilToInt(num2);
		}

		if (num <= 0)
		{
			return;
		}

		int num3 = (int)Mathf.Sign(num);
		if (target.attackEffects.Length != 0)
		{
			while (num > 0)
			{
				CardData.StatusEffectStacks[] attackEffects = target.attackEffects;
				foreach (CardData.StatusEffectStacks statusEffectStacks in attackEffects)
				{
					statusEffectStacks.count = Mathf.Max(1, statusEffectStacks.count + num3);
					if (--num <= 0)
					{
						break;
					}
				}
			}
		}
		else if (target.hasAttack)
		{
			target.damage += num;
			target.damage = Mathf.Max(0, target.damage);
		}
	}
}
