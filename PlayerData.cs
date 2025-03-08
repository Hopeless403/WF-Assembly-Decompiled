#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
	public ClassData classData;

	[Header("Stats")]
	public int handSize = 6;

	public int redrawBell = 4;

	public int companionLimit = 3;

	public float enemyGoldFactor = 1f;

	public float comboGoldFactor = 1f;

	[Header("Inventory")]
	public Inventory inventory;

	public PlayerData(ClassData classData, Inventory inventory)
	{
		this.classData = classData;
		this.inventory = inventory;
	}
}
