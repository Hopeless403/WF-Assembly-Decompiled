#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Apply X When Clunker Destroyed", fileName = "Apply X When Clunker Destroyed")]
public class StatusEffectApplyXWhenClunkerDestroyed : StatusEffectApplyX
{
	public override void Init()
	{
		base.OnEntityDestroyed += Check;
	}

	public override bool RunEntityDestroyedEvent(Entity entity, DeathType deathType)
	{
		if (target.enabled && entity.data.cardType.name == "Clunker")
		{
			return Battle.IsOnBoard(target);
		}

		return false;
	}

	public IEnumerator Check(Entity entity, DeathType deathType)
	{
		return Run(GetTargets());
	}
}
