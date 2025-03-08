#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Change Companion Limit", menuName = "Scripts/Change Companion Limit")]
public class ScriptChangeCompanionLimit : Script
{
	[SerializeField]
	public bool set;

	[SerializeField]
	[HideIf("set")]
	public bool add = true;

	[SerializeField]
	public int value = 1;

	public override IEnumerator Run()
	{
		if (set)
		{
			References.PlayerData.companionLimit = value;
		}
		else if (add)
		{
			References.PlayerData.companionLimit += value;
		}

		yield break;
	}
}
