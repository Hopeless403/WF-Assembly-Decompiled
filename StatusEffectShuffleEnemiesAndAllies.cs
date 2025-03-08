#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Shuffle Enemies And Allies", fileName = "Shuffle Enemies And Allies")]
public class StatusEffectShuffleEnemiesAndAllies : StatusEffectData
{
	public override void Init()
	{
		base.OnCardPlayed += Run;
	}

	public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
	{
		return entity == target;
	}

	public IEnumerator Run(Entity entity, Entity[] targets)
	{
		Debug.Log($"{this} Running...");
		List<CardContainer> rows = Battle.instance.GetRows(target.owner);
		rows.AddRange(Battle.instance.GetRows(Battle.GetOpponent(target.owner)));
		foreach (CardContainer item in rows)
		{
			List<Entity> list = new List<Entity>();
			foreach (Entity item2 in item)
			{
				list.Add(item2);
			}

			if (list.Count > 0)
			{
				list.Shuffle();
			}

			foreach (Entity item3 in list)
			{
				item.Remove(item3);
			}

			foreach (Entity item4 in list)
			{
				item.Add(item4);
			}

			item.TweenChildPositions();
		}

		target.curveAnimator.Ping();
		yield return Sequences.Wait(0.13f);
	}
}
