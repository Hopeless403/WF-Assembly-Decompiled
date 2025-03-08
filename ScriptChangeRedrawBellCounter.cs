#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Change Redraw Bell Counter", menuName = "Scripts/Change Redraw Bell Counter")]
public class ScriptChangeRedrawBellCounter : Script
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
			References.PlayerData.redrawBell = value;
		}
		else if (add)
		{
			References.PlayerData.redrawBell += value;
		}

		yield break;
	}
}
