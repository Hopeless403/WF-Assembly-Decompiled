#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialItemSystem2 : GameSystem
{
	public bool init;

	public ItemEventRoutine itemEvent;

	public bool waitForOpen;

	public float promptDelay;

	public void OnEnable()
	{
		Events.OnEventStart += EventStart;
		Events.OnActionPerform += ActionPerformed;
		Events.OnSceneChanged += SceneChanged;
	}

	public void OnDisable()
	{
		Events.OnEventStart -= EventStart;
		Events.OnActionPerform -= ActionPerformed;
		Events.OnSceneChanged -= SceneChanged;
		PromptSystem.Hide();
	}

	public void Init(ItemEventRoutine itemEvent)
	{
		init = true;
		this.itemEvent = itemEvent;
		waitForOpen = true;
	}

	public void Update()
	{
		if (init && waitForOpen && (bool)itemEvent && itemEvent.IsOpen)
		{
			waitForOpen = false;
			promptDelay = 1.5f;
		}

		if (!(promptDelay > 0f))
		{
			return;
		}

		promptDelay -= Time.deltaTime;
		if (promptDelay <= 0f)
		{
			PromptSystem.Create(Prompt.Anchor.Left, 0.5f, 0.5f, 4f, Prompt.Emote.Type.Explain);
			PromptSystem.SetTextAction(() => MonoBehaviourSingleton<StringReference>.instance.tutorialItem.GetLocalizedString());
		}
	}

	public void EventStart(CampaignNode node, EventRoutine @event)
	{
		if (@event is ItemEventRoutine itemEventRoutine)
		{
			Init(itemEventRoutine);
		}
	}

	public static void ActionPerformed(PlayAction action)
	{
		if (action is ActionSelect)
		{
			PromptSystem.Hide();
		}
	}

	public void SceneChanged(Scene scene)
	{
		Object.Destroy(this);
	}
}
