#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity With Lowest Attack", menuName = "Select Scripts/Entity With Lowest Attack")]
public class SelectScriptEntityLowestAttack : SelectScript<Entity>
{
	[SerializeField]
	public int selectAmount = 1;

	public override List<Entity> Run(List<Entity> group)
	{
		Entity entity = group.Where((Entity a) => a.IsAliveAndExists() && a.HasAttackIcon()).OrderBy(GetAttack).FirstOrDefault();
		int lowestAttack = (entity ? GetAttack(entity) : 0);
		return group.Where((Entity a) => a.IsAliveAndExists() && a.HasAttackIcon() && GetAttack(a) == lowestAttack).InRandomOrder().Take(selectAmount)
			.ToList();
	}

	public static int GetAttack(Entity entity)
	{
		return entity.damage.current + entity.tempDamage.Value;
	}
}
