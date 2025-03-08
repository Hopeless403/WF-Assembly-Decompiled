#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PromptSystem : GameSystem
{
	public static PromptSystem instance;

	[SerializeField]
	public Prompt prompt;

	[SerializeField]
	public EventReference popUpSfx;

	[SerializeField]
	public EventReference denySfx;

	public static Prompt Prompt => instance.prompt;

	public static void SetSortingLayer(string layerName, int sortingOrder)
	{
		Canvas component = instance.GetComponent<Canvas>();
		if ((bool)component)
		{
			component.sortingLayerName = layerName;
			component.sortingOrder = sortingOrder;
		}
	}

	public void OnEnable()
	{
		Events.OnSceneChanged += SceneChanged;
		instance = this;
	}

	public void OnDisable()
	{
		Events.OnSceneChanged -= SceneChanged;
	}

	public void SceneChanged(Scene scene)
	{
		if ((bool)prompt && prompt.gameObject.activeSelf)
		{
			prompt.gameObject.SetActive(value: false);
		}
	}

	public static void Create(Prompt.Anchor anchor, Vector2 anchoredPosition, float maxWidth, Prompt.Emote.Type emoteType = Prompt.Emote.Type.Basic, Prompt.Emote.Position emotePosition = Prompt.Emote.Position.Above)
	{
		if ((bool)instance)
		{
			instance.DoCreate(anchor, anchoredPosition, maxWidth, emoteType, emotePosition);
		}
	}

	public static void Create(Prompt.Anchor anchor, float x, float y, float maxWidth, Prompt.Emote.Type emoteType = Prompt.Emote.Type.Basic, Prompt.Emote.Position emotePosition = Prompt.Emote.Position.Above)
	{
		Create(anchor, new Vector2(x, y), maxWidth, emoteType, emotePosition);
	}

	public static void SetTextAction(Prompt.GetTextCallback action)
	{
		Prompt.SetTextAction = action;
		Prompt.RunSetTextAction();
	}

	public void DoCreate(Prompt.Anchor anchor, Vector2 anchoredPosition, float maxWidth, Prompt.Emote.Type emoteType = Prompt.Emote.Type.Basic, Prompt.Emote.Position emotePosition = Prompt.Emote.Position.Above)
	{
		prompt.Enable();
		prompt.SetPosition(anchoredPosition, anchor);
		prompt.SetMaxWidth(maxWidth);
		prompt.SetEmote(emoteType, emotePosition);
		prompt.Ping();
		SfxSystem.OneShot(popUpSfx);
		SfxSystem.OneShot("event:/sfx/map/location_showup");
	}

	public static void Hide()
	{
		Object.FindObjectOfType<PromptSystem>()?.DoHide();
	}

	public void DoHide()
	{
		if ((bool)prompt && prompt.active)
		{
			prompt.Hide();
		}
	}

	public static void Shake()
	{
		if ((bool)instance)
		{
			GameObject obj = instance.gameObject;
			LeanTween.cancel(obj);
			obj.transform.localPosition = new Vector3(0.25f, 0f, -3f);
			LeanTween.moveLocal(obj, new Vector3(0f, 0f, -3f), 1f).setEase(LeanTweenType.easeOutElastic);
			SfxSystem.OneShot(instance.denySfx);
		}
	}
}
