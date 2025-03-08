#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class TutorialCrownSystem : TutorialParentSystem
{
	public static bool promptSystemNeedsReset;

	public bool deckpackOpen;

	public bool promptShown;

	public override void OnEnable()
	{
		if (SaveSystem.LoadProgressData("tutorialCrownDone", defaultValue: false))
		{
			Object.Destroy(this);
			return;
		}

		base.OnEnable();
		Events.OnDeckpackOpen += DeckpackOpen;
		Events.OnDeckpackClose += DeckpackClose;
		Events.OnUpgradeAssign += UpgradeAssign;
	}

	public override void OnDisable()
	{
		base.OnDisable();
		Events.OnDeckpackOpen -= DeckpackOpen;
		Events.OnDeckpackClose -= DeckpackClose;
		Events.OnUpgradeAssign -= UpgradeAssign;
		HidePrompt();
	}

	public void DeckpackOpen()
	{
		deckpackOpen = true;
	}

	public void DeckpackClose()
	{
		deckpackOpen = false;
		HidePrompt();
	}

	public void UpgradeAssign(Entity entity, CardUpgradeData upgradeData)
	{
		if (deckpackOpen && upgradeData.type == CardUpgradeData.Type.Crown)
		{
			ShowPrompt();
			SaveSystem.SaveProgressData("tutorialCrownDone", value: true);
		}
	}

	public void ShowPrompt()
	{
		if (!promptShown)
		{
			PromptSystem.Hide();
			PromptSystem.SetSortingLayer("UI", 5);
			PromptSystem.Create(Prompt.Anchor.Left, 2f, 0.5f, 4f, Prompt.Emote.Type.Point);
			PromptSystem.SetTextAction(() => MonoBehaviourSingleton<StringReference>.instance.tutorialCrown.GetLocalizedString());
			promptSystemNeedsReset = true;
			promptShown = true;
		}
	}

	public void HidePrompt()
	{
		if (promptShown)
		{
			PromptSystem.Hide();
			CheckPromptSystemReset();
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
}
