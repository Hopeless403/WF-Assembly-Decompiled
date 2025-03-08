#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Change Enemy Gold Factor", menuName = "Scripts/Change Enemy Gold Factor")]
public class ScriptChangeEnemyGoldFactor : Script
{
	[SerializeField]
	public float value = 1f;

	public override IEnumerator Run()
	{
		References.PlayerData.enemyGoldFactor = value;
		yield break;
	}
}
