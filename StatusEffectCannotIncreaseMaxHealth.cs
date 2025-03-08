#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Traits/Cannot Increase Max Health", fileName = "Cannot Increase Max Health")]
public class StatusEffectCannotIncreaseMaxHealth : StatusEffectData
{
	public override bool RunApplyStatusEvent(StatusEffectApply apply)
	{
		if (apply.target == target && !target.silenced && CheckEffectType(apply.effectData))
		{
			apply.count = 0;
		}

		return false;
	}

	public static bool CheckEffectType(StatusEffectData effectData)
	{
		if ((bool)effectData)
		{
			string text = effectData.type;
			return text == "max health up" || text == "max health up only";
		}

		return false;
	}
}
