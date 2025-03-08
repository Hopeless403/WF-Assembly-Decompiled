#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity With Highest Attack", menuName = "Select Scripts/Entity With Highest Attack")]
public class SelectScriptEntityHighestAttack : SelectScript<Entity>
{
	[SerializeField]
	public int selectAmount = 1;

	public override List<Entity> Run(List<Entity> group)
	{
		Entity entity = group.Where((Entity a) => a.IsAliveAndExists() && a.HasAttackIcon()).OrderByDescending(GetAttack).FirstOrDefault();
		int highestAttack = (entity ? GetAttack(entity) : 0);
		return group.Where((Entity a) => a.IsAliveAndExists() && a.HasAttackIcon() && GetAttack(a) == highestAttack).InRandomOrder().Take(selectAmount)
			.ToList();
	}

	public static int GetAttack(Entity entity)
	{
		return entity.damage.current + entity.tempDamage.Value;
	}
}
