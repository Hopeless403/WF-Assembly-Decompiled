#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialCharmSystem : TutorialParentSystem
{
	public class PhaseEquipCharm : Phase
	{
		public override void OnEnable()
		{
			Events.OnUpgradeAssign += UpgradeAssign;
			Routine.Create(PromptAfterDelay(1f));
		}

		public override void OnDisable()
		{
			PromptSystem.Hide();
			Events.OnUpgradeAssign -= UpgradeAssign;
		}

		public void UpgradeAssign(Entity entity, CardUpgradeData upgradeData)
		{
			if (upgradeData.type == CardUpgradeData.Type.Charm)
			{
				base.enabled = false;
			}
		}

		public IEnumerator PromptAfterDelay(float delay)
		{
			yield return new WaitForSeconds(delay);
			if (base.enabled)
			{
				PromptSystem.SetSortingLayer("UI", 5);
				PromptSystem.Create(Prompt.Anchor.Left, 2f, 0.5f, 4f, Prompt.Emote.Type.Explain);
				PromptSystem.SetTextAction(() => MonoBehaviourSingleton<StringReference>.instance.tutorialCharm1.GetLocalizedString());
				PromptSystem.Prompt.SetEmotePosition(Prompt.Emote.Position.Above, 1f, 0f, -1f);
				promptSystemNeedsReset = true;
			}
		}
	}

	public class PhaseCharmEquipped : Phase
	{
		public override void OnEnable()
		{
			Events.OnDeckpackClose += DeckpackClose;
			Routine.Create(PromptAfterDelay(2f));
			Done();
		}

		public override void OnDisable()
		{
			PromptSystem.Hide();
			Events.OnDeckpackClose -= DeckpackClose;
		}

		public void DeckpackClose()
		{
			base.enabled = false;
		}

		public IEnumerator PromptAfterDelay(float delay)
		{
			yield return new WaitForSeconds(delay);
			if (base.enabled)
			{
				PromptSystem.SetSortingLayer("UI", 5);
				PromptSystem.Create(Prompt.Anchor.Left, 1f, 1f, 5f, Prompt.Emote.Type.Scared);
				PromptSystem.SetTextAction(() => MonoBehaviourSingleton<StringReference>.instance.tutorialCharm2.GetLocalizedString());
				promptSystemNeedsReset = true;
			}
		}
	}

	public static bool promptSystemNeedsReset;

	public override void OnEnable()
	{
		if (SaveSystem.LoadProgressData("tutorialCharmDone", defaultValue: false))
		{
			Object.Destroy(this);
			return;
		}

		base.OnEnable();
		Events.OnDeckpackOpen += DeckpackOpen;
		Events.OnDeckpackClose += DeckpackClose;
	}

	public override void OnDisable()
	{
		base.OnDisable();
		Events.OnDeckpackOpen -= DeckpackOpen;
		Events.OnDeckpackClose -= DeckpackClose;
		CheckPromptSystemReset();
	}

	public void DeckpackOpen()
	{
		if (References.PlayerData.inventory.upgrades.Count((CardUpgradeData a) => a.type == CardUpgradeData.Type.Charm) > 0)
		{
			phases = new List<Phase>
			{
				new PhaseEquipCharm(),
				new PhaseCharmEquipped()
			};
		}
	}

	public void DeckpackClose()
	{
		phases = null;
		if (currentPhase != null)
		{
			currentPhase.enabled = false;
		}

		currentPhase = null;
		delay = 0f;
		CheckPromptSystemReset();
		if (SaveSystem.LoadProgressData("tutorialCharmDone", defaultValue: false))
		{
			Object.Destroy(this);
		}
	}

	public static void CheckPromptSystemReset()
	{
		if (promptSystemNeedsReset)
		{
			PromptSystem.SetSortingLayer("Prompt", 0);
			promptSystemNeedsReset = false;
		}
	}

	public static void Done()
	{
		SaveSystem.SaveProgressData("tutorialCharmDone", value: true);
	}
}
