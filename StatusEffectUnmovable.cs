#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Traits/Unmovable", fileName = "Unmovable")]
public class StatusEffectUnmovable : StatusEffectData
{
	public override void Init()
	{
		Events.OnCheckAction += CheckAction;
	}

	public void OnDestroy()
	{
		Events.OnCheckAction -= CheckAction;
	}

	public void CheckAction(ref PlayAction action, ref bool allow)
	{
		if (allow && !target.silenced && action is ActionMove actionMove && actionMove.entity == target && Battle.IsOnBoard(target) && Battle.IsOnBoard(actionMove.toContainers))
		{
			allow = false;
			if (NoTargetTextSystem.Exists())
			{
				new Routine(NoTargetTextSystem.Run(target, NoTargetType.CantMove));
			}
		}
	}
}
