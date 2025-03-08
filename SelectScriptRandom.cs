#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Random", menuName = "Select Scripts/Random")]
public class SelectScriptRandom : SelectScript<Entity>
{
	[SerializeField]
	public int selectAmount = 1;

	public override List<Entity> Run(List<Entity> group)
	{
		return group.InRandomOrder().Take(selectAmount).ToList();
	}
}
