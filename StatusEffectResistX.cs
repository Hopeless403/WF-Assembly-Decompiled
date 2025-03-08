#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Immunity/Resist X", fileName = "Resist X")]
public class StatusEffectResistX : StatusEffectData
{
	[SerializeField]
	public string resistType = "snow";

	public override bool RunApplyStatusEvent(StatusEffectApply apply)
	{
		if (apply.target == target && apply.effectData?.type == resistType)
		{
			apply.count -= count;
			if (apply.count <= 0)
			{
				apply.effectData = null;
				apply.count = 0;
			}
		}

		return false;
	}
}
