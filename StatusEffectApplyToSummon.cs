#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Traits/Apply To Summon", fileName = "Apply To Summon")]
public class StatusEffectApplyToSummon : StatusEffectData
{
	[SerializeField]
	public StatusEffectData effectToApply;

	public override void Init()
	{
		Events.OnEntitySummoned += EntitySummoned;
	}

	public void OnDestroy()
	{
		Events.OnEntitySummoned -= EntitySummoned;
	}

	public void EntitySummoned(Entity entity, Entity summonedBy)
	{
		if (summonedBy.data.id == target.data.id)
		{
			int amount = GetAmount();
			if (amount > 0)
			{
				ActionQueue.Stack(new ActionApplyStatus(entity, summonedBy, effectToApply, amount)
				{
					fixedPosition = true
				});
			}
		}
	}
}
