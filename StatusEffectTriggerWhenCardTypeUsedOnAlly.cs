#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Reactions/Trigger When Card Type Used On Ally", fileName = "Trigger When Card Type Used On Ally")]
public class StatusEffectTriggerWhenCardTypeUsedOnAlly : StatusEffectReaction
{
	[SerializeField]
	public bool includeSelf;

	[SerializeField]
	public string[] cardTypeTriggers;

	public override bool RunCardPlayedEvent(Entity entity, Entity[] targets)
	{
		if (CheckCardType(entity) && CheckTargets(targets) && Battle.IsOnBoard(target) && CanTrigger())
		{
			ActionQueue.Stack(new ActionTrigger(target, entity), fixedPosition: true);
		}

		return false;
	}

	public bool CheckCardType(Entity entity)
	{
		bool result = false;
		string text = entity.data?.cardType?.tag;
		if (text != null)
		{
			string[] array = cardTypeTriggers;
			foreach (string value in array)
			{
				if (text.Equals(value))
				{
					result = true;
					break;
				}
			}
		}

		return result;
	}

	public bool CheckTargets(Entity[] targets)
	{
		bool result = false;
		foreach (Entity entity in targets)
		{
			if (entity.owner == target.owner && (entity != target || includeSelf))
			{
				result = true;
				break;
			}
		}

		return result;
	}
}
