#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialCompanionSystem : GameSystem
{
	public EventRoutineCompanion companionEvent;

	public bool init;

	public bool waitForBreak = true;

	public bool waitForInspect;

	public bool inspectDone;

	public float promptDelay;

	public bool prompt2Show;

	public void OnEnable()
	{
		Events.OnCheckAction += CheckAction;
		Events.OnEventStart += EventStart;
		Events.OnInspect += Inspected;
		Events.OnActionPerform += ActionPerformed;
		Events.OnSceneChanged += SceneChanged;
	}

	public void OnDisable()
	{
		Events.OnCheckAction -= CheckAction;
		Events.OnEventStart -= EventStart;
		Events.OnInspect -= Inspected;
		Events.OnActionPerform -= ActionPerformed;
		Events.OnSceneChanged -= SceneChanged;
		PromptSystem.Hide();
	}

	public void Init(EventRoutineCompanion companionEvent)
	{
		init = true;
		this.companionEvent = companionEvent;
		waitForBreak = true;
		waitForInspect = false;
		inspectDone = false;
	}

	public void Update()
	{
		if (init && waitForBreak && companionEvent != null && companionEvent.broken)
		{
			waitForBreak = false;
			waitForInspect = true;
			promptDelay = 1f;
		}

		if (!(promptDelay > 0f))
		{
			return;
		}

		promptDelay -= Time.deltaTime;
		if (!(promptDelay <= 0f))
		{
			return;
		}

		PromptSystem.Create(Prompt.Anchor.TopLeft, 1f, -3f, 5f, Prompt.Emote.Type.Explain);
		PromptSystem.SetTextAction(delegate
		{
			if (!MonoBehaviourSingleton<Cursor3d>.instance.usingMouse)
			{
				return ControllerButtonSystem.ProcessActionTags(MonoBehaviourSingleton<StringReference>.instance.tutorialCompanion1Gamepad);
			}

			return (!TouchInputModule.active) ? ControllerButtonSystem.ProcessActionTags(MonoBehaviourSingleton<StringReference>.instance.tutorialCompanion1) : ControllerButtonSystem.ProcessActionTags(MonoBehaviourSingleton<StringReference>.instance.tutorialCompanion1Touch);
		});
	}

	public void CheckAction(ref PlayAction action, ref bool allow)
	{
		if (!inspectDone)
		{
			if (!(action is ActionInspect))
			{
				allow = false;
				PromptSystem.Shake();
			}
		}
		else
		{
			if (!(action is ActionSelect actionSelect) || !(actionSelect.entity == null))
			{
				return;
			}

			allow = false;
			if (prompt2Show)
			{
				PromptSystem.Shake();
				return;
			}

			PromptSystem.Create(Prompt.Anchor.Left, 1f, 1f, 5f);
			PromptSystem.SetTextAction(() => MonoBehaviourSingleton<StringReference>.instance.tutorialCompanion2.GetLocalizedString());
			prompt2Show = true;
		}
	}

	public void EventStart(CampaignNode node, EventRoutine @event)
	{
		if (@event is EventRoutineCompanion eventRoutineCompanion)
		{
			Init(eventRoutineCompanion);
		}
	}

	public void Inspected(Entity entity)
	{
		if (waitForInspect && !inspectDone)
		{
			inspectDone = true;
			PromptSystem.Hide();
		}
	}

	public void ActionPerformed(PlayAction action)
	{
		if (action is ActionSelect)
		{
			PromptSystem.Hide();
			prompt2Show = false;
		}
	}

	public void SceneChanged(Scene scene)
	{
		Object.Destroy(this);
	}
}
