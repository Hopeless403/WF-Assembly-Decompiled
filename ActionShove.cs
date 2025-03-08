#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;

public class ActionShove : PlayAction
{
	public readonly Dictionary<Entity, List<CardSlot>> shoveData;

	public readonly bool updatePositions;

	public ActionShove(Dictionary<Entity, List<CardSlot>> shoveData, bool updatePositions = false)
	{
		this.shoveData = shoveData;
		this.updatePositions = updatePositions;
	}

	public override IEnumerator Run()
	{
		return ShoveSystem.DoShove(shoveData, updatePositions);
	}
}
