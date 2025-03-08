#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Swap Effects Based On", menuName = "Card Scripts/Swap Effects Based On")]
public class CardScriptSwapEffectsBasedOn : CardScript
{
	[SerializeField]
	public StatusEffectData statusA;

	[SerializeField]
	public StatusEffectData statusB;

	public override void Run(CardData target)
	{
		CardData.StatusEffectStacks[] attackEffects = target.attackEffects;
		foreach (CardData.StatusEffectStacks statusEffectStacks in attackEffects)
		{
			if (statusEffectStacks.data.type == statusA.type)
			{
				statusEffectStacks.data = statusB;
			}
			else if (statusEffectStacks.data.type == statusB.type)
			{
				statusEffectStacks.data = statusA;
			}

			else if (statusEffectStacks.data is StatusEffectInstantDoubleX effect)
			{
				TrySwap(effect, statusEffectStacks, statusA, statusB);
			}
		}

		attackEffects = target.startWithEffects;
		foreach (CardData.StatusEffectStacks statusEffectStacks2 in attackEffects)
		{
			StatusEffectData data = statusEffectStacks2.data;
			if (!(data is StatusEffectApplyXWhenYAppliedTo effect2))
			{
				if (!(data is StatusEffectApplyXWhenYAppliedToAlly effect3))
				{
					if (!(data is StatusEffectApplyXWhenYAppliedToSelf effect4))
					{
						if (!(data is StatusEffectApplyX effect5))
						{
							if (data is StatusEffectBonusDamageEqualToX effect6)
							{
								TrySwap(effect6, statusEffectStacks2, statusA, statusB);
							}
						}
						else
						{
							TrySwap(effect5, statusEffectStacks2, statusA, statusB);
						}
					}
					else
					{
						TrySwap(effect4, statusEffectStacks2, statusA, statusB);
					}
				}
				else
				{
					TrySwap(effect3, statusEffectStacks2, statusA, statusB);
				}
			}
			else
			{
				TrySwap(effect2, statusEffectStacks2, statusA, statusB);
			}
		}
	}

	public static bool Swap(CardData.StatusEffectStacks stacks, StatusEffectData a, StatusEffectData b)
	{
		string text = stacks.data.name.Replace(a.name, b.name);
		StatusEffectData statusEffectData = AddressableLoader.Get<StatusEffectData>("StatusEffectData", text);
		if ((bool)statusEffectData)
		{
			stacks.data = statusEffectData;
			return true;
		}

		Debug.LogError("[" + text + "] effect does not exist! Cannot swap effect [" + stacks.data.name + "] :(");
		return false;
	}

	public static void TrySwap(StatusEffectInstantDoubleX effect, CardData.StatusEffectStacks stacks, StatusEffectData a, StatusEffectData b)
	{
		if ((bool)effect.statusToDouble)
		{
			if (effect.statusToDouble.type == a.type)
			{
				Swap(stacks, a, b);
			}
			else if (effect.statusToDouble.type == b.type)
			{
				Swap(stacks, b, a);
			}
		}
	}

	public static void TrySwap(StatusEffectApplyX effect, CardData.StatusEffectStacks stacks, StatusEffectData a, StatusEffectData b)
	{
		if ((bool)effect.effectToApply)
		{
			if (effect.effectToApply.type == a.type)
			{
				Swap(stacks, a, b);
			}
			else if (effect.effectToApply.type == b.type)
			{
				Swap(stacks, b, a);
			}
		}
	}

	public static void TrySwap(StatusEffectApplyXWhenYAppliedTo effect, CardData.StatusEffectStacks stacks, StatusEffectData a, StatusEffectData b)
	{
		if (effect.whenAppliedTypes.Contains(a.type) || ((bool)effect.effectToApply && effect.effectToApply.type == a.type))
		{
			Swap(stacks, a, b);
		}
		else if (effect.whenAppliedTypes.Contains(b.type) || ((bool)effect.effectToApply && effect.effectToApply.type == b.type))
		{
			Swap(stacks, b, a);
		}
	}

	public static void TrySwap(StatusEffectApplyXWhenYAppliedToAlly effect, CardData.StatusEffectStacks stacks, StatusEffectData a, StatusEffectData b)
	{
		if (effect.whenAppliedType == a.type || ((bool)effect.effectToApply && effect.effectToApply.type == a.type))
		{
			Swap(stacks, a, b);
		}
		else if (effect.whenAppliedType == b.type || ((bool)effect.effectToApply && effect.effectToApply.type == b.type))
		{
			Swap(stacks, b, a);
		}
	}

	public static void TrySwap(StatusEffectApplyXWhenYAppliedToSelf effect, CardData.StatusEffectStacks stacks, StatusEffectData a, StatusEffectData b)
	{
		if (effect.whenAppliedType == a.type || ((bool)effect.effectToApply && effect.effectToApply.type == a.type))
		{
			Swap(stacks, a, b);
		}
		else if (effect.whenAppliedType == b.type || ((bool)effect.effectToApply && effect.effectToApply.type == b.type))
		{
			Swap(stacks, b, a);
		}
	}

	public static void TrySwap(StatusEffectBonusDamageEqualToX effect, CardData.StatusEffectStacks stacks, StatusEffectData a, StatusEffectData b)
	{
		if (effect.effectType == a.type)
		{
			Swap(stacks, a, b);
		}
		else if (effect.effectType == b.type)
		{
			Swap(stacks, b, a);
		}
	}
}
