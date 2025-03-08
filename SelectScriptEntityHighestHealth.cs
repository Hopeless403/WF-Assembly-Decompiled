#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity With Highest Health", menuName = "Select Scripts/Entity With Highest Health")]
public class SelectScriptEntityHighestHealth : SelectScript<Entity>
{
	[SerializeField]
	public int selectAmount = 1;

	public override List<Entity> Run(List<Entity> group)
	{
		Entity entity = group.Where((Entity a) => a.IsAliveAndExists() && a.data.hasHealth).OrderByDescending(GetHealth).FirstOrDefault();
		int highestHealth = (entity ? GetHealth(entity) : 0);
		return group.Where((Entity a) => a.IsAliveAndExists() && a.data.hasHealth && a.hp.current == highestHealth).InRandomOrder().Take(selectAmount)
			.ToList();
	}

	public static int GetHealth(Entity entity)
	{
		return entity.hp.current;
	}
}
