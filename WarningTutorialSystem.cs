#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Linq;
using UnityEngine;
using UnityEngine.Localization;

public class WarningTutorialSystem : GameSystem
{
	[SerializeField]
	public UnityEngine.Localization.LocalizedString promptStringRef;

	public bool promptShown;

	public void OnEnable()
	{
		Events.OnCheckAction += CheckAction;
		Events.OnActionPerform += ActionPerform;
	}

	public void OnDisable()
	{
		Events.OnCheckAction -= CheckAction;
		Events.OnActionPerform -= ActionPerform;
	}

	public void CheckAction(ref PlayAction action, ref bool allow)
	{
		if (promptShown || !(action is ActionTriggerAgainst actionTriggerAgainst) || !(actionTriggerAgainst.triggeredBy == References.Player.entity))
		{
			return;
		}

		GetDamageDetails(actionTriggerAgainst.entity, out var damage, out var instantKill);
		if (!(damage > 0 || instantKill))
		{
			return;
		}

		Entity entity = null;
		if ((bool)actionTriggerAgainst.targetContainer)
		{
			entity = actionTriggerAgainst.targetContainer.FirstOrDefault((Entity a) => a.data.cardType.miniboss && a.owner == References.Player);
		}
		else if (actionTriggerAgainst.target.data.cardType.miniboss && actionTriggerAgainst.target.owner == References.Player)
		{
			entity = actionTriggerAgainst.target;
		}

		if (!entity)
		{
			return;
		}

		if (instantKill)
		{
			allow = false;
			ShowPrompt(actionTriggerAgainst.entity);
		}
		else if (!entity.FindStatus("block"))
		{
			int num = entity.hp.current;
			StatusEffectData statusEffectData = entity.FindStatus("shell");
			if ((bool)statusEffectData)
			{
				num += statusEffectData.count;
			}

			if (num <= damage)
			{
				allow = false;
				ShowPrompt(actionTriggerAgainst.entity);
			}
		}
	}

	public static void GetDamageDetails(Entity entity, out int damage, out bool instantKill)
	{
		instantKill = false;
		damage = entity.damage.current + entity.tempDamage.Value;
		foreach (CardData.StatusEffectStacks attackEffect in entity.attackEffects)
		{
			if (attackEffect.data is StatusEffectInstantSacrifice)
			{
				instantKill = true;
			}
		}
	}

	public void ActionPerform(PlayAction action)
	{
		if (promptShown)
		{
			HidePrompt();
		}
	}

	public void ShowPrompt(Entity attackingCard)
	{
		PromptSystem.Hide();
		PromptSystem.Create(Prompt.Anchor.Left, 0f, 1f, 3f, Prompt.Emote.Type.Scared);
		PromptSystem.SetTextAction(() => promptStringRef.GetLocalizedString(attackingCard.data.title));
		PromptSystem.Shake();
		promptShown = true;
	}

	public void HidePrompt()
	{
		PromptSystem.Hide();
		promptShown = false;
	}
}
