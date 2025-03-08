#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Immunity/Immune To X", fileName = "Immune To X")]
public class StatusEffectImmuneToX : StatusEffectData
{
	[SerializeField]
	public string immunityType = "snow";

	public const int max = 1;

	public override void Init()
	{
		base.OnBegin += Begin;
	}

	public IEnumerator Begin()
	{
		StatusEffectData statusEffectData = target.FindStatus(immunityType);
		if ((bool)statusEffectData && statusEffectData.count > 1)
		{
			yield return statusEffectData.RemoveStacks(statusEffectData.count - 1, removeTemporary: false);
		}
	}

	public override bool RunApplyStatusEvent(StatusEffectApply apply)
	{
		if (apply.target == target && (bool)apply.effectData && apply.effectData.type == immunityType)
		{
			StatusEffectData statusEffectData = target.FindStatus(immunityType);
			int num = (statusEffectData ? statusEffectData.count : 0);
			apply.count = Mathf.Max(1 - num);
		}

		return false;
	}
}
