#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialInjurySystem : TutorialParentSystem
{
	public bool promptShown;

	public override void OnEnable()
	{
		if (SaveSystem.LoadProgressData("tutorialInjuryDone", defaultValue: false))
		{
			Object.Destroy(this);
			return;
		}

		base.OnEnable();
		Events.OnBattleEnd += BattleEnd;
		Events.OnSceneChanged += SceneChanged;
	}

	public override void OnDisable()
	{
		base.OnDisable();
		Events.OnBattleEnd -= BattleEnd;
		Events.OnSceneChanged -= SceneChanged;
	}

	public void SceneChanged(Scene scene)
	{
		if (promptShown)
		{
			PromptSystem.Hide();
			promptShown = false;
		}

		if (SaveSystem.LoadProgressData("tutorialInjuryDone", defaultValue: false))
		{
			Object.Destroy(this);
		}
	}

	public void BattleEnd()
	{
		if (References.Battle.winner == References.Battle.player)
		{
			CardData[] injuriesThisBattle = InjurySystem.GetInjuriesThisBattle();
			if (injuriesThisBattle.Length != 0)
			{
				StartCoroutine(Routine(injuriesThisBattle));
			}
		}
	}

	public IEnumerator Routine(CardData[] injuries)
	{
		yield return WaitForInjuriesPanel();
		PromptSystem.Create(Prompt.Anchor.Left, 0.5f, 0f, 5f, Prompt.Emote.Type.Sad);
		PromptSystem.SetTextAction(() => (injuries.Length != 1) ? MonoBehaviourSingleton<StringReference>.instance.tutorialInjuryMultiple.GetLocalizedString() : string.Format(MonoBehaviourSingleton<StringReference>.instance.tutorialInjury.GetLocalizedString(), injuries[0].title));
		promptShown = true;
		SaveSystem.SaveProgressData("tutorialInjuryDone", value: true);
	}

	public static IEnumerator WaitForInjuriesPanel()
	{
		yield return new WaitUntil(() => SceneManager.IsLoaded("BattleWin"));
	}
}
