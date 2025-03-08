#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CinemaBarSystem : GameSystem
{
	public class State
	{
		public readonly bool wasActive;

		public readonly string topScript;

		public readonly string topPrompt;

		public readonly string topPromptAction;

		public readonly string bottomScript;

		public readonly string bottomPrompt;

		public readonly string bottomPromptAction;

		public readonly string sortingLayerName;

		public readonly int sortingOrderInLayer;

		public State()
		{
			wasActive = IsActive();
			topScript = Top.script.text;
			topPrompt = Top.prompt.text;
			topPromptAction = Top.buttonImage.actionName;
			bottomScript = Bottom.script.text;
			bottomPrompt = Bottom.prompt.text;
			bottomPromptAction = Bottom.buttonImage.actionName;
			sortingLayerName = instance.canvas.sortingLayerName;
			sortingOrderInLayer = instance.canvas.sortingOrder;
		}

		public void Restore()
		{
			bool flag = IsActive();
			if (flag && !wasActive)
			{
				Out();
			}
			else if (!flag && wasActive)
			{
				In();
			}

			Top.SetScript(topScript);
			Top.SetPrompt(topPrompt, topPromptAction);
			Bottom.SetScript(bottomScript);
			Bottom.SetPrompt(bottomPrompt, bottomPromptAction);
			SetSortingLayer(sortingLayerName, sortingOrderInLayer);
		}
	}

	[Serializable]
	public class Section
	{
		public RectTransform transform;

		public TMP_Text prompt;

		public ControllerButtonImage buttonImage;

		public TMP_Text script;

		public TextTypewrite typewriter;

		public void SetPrompt(string text, string actionName)
		{
			prompt.text = text;
			buttonImage.Set(actionName);
		}

		public void RemovePrompt()
		{
			SetPrompt("", "");
		}

		public void SetScript(string text)
		{
			script.text = text;
		}

		public void RemoveScript()
		{
			SetScript("");
		}

		public void Clear()
		{
			SetPrompt("", "");
			SetScript("");
		}
	}

	public static CinemaBarSystem _instance;

	[SerializeField]
	public Canvas canvas;

	[SerializeField]
	public Section top;

	[SerializeField]
	public Section bottom;

	public static Section Top;

	public static Section Bottom;

	public const float from = -1.5f;

	public const float to = 0.4f;

	public const float inDur = 0.5f;

	public const LeanTweenType inEase = LeanTweenType.easeOutBack;

	public const float outDur = 0.2f;

	public const LeanTweenType outEase = LeanTweenType.easeInQuad;

	public static CinemaBarSystem instance => _instance ?? (_instance = UnityEngine.Object.FindObjectOfType<CinemaBarSystem>(includeInactive: true));

	public void Awake()
	{
		Top = top;
		Bottom = bottom;
	}

	public void OnEnable()
	{
		Events.OnCampaignFinal += CampaignFinal;
	}

	public void OnDisable()
	{
		Events.OnCampaignFinal -= CampaignFinal;
	}

	public static void CampaignFinal()
	{
		Clear();
		OutInstant();
	}

	public static void Clear()
	{
		instance.StopAllCoroutines();
		Top.Clear();
		Bottom.Clear();
	}

	public static void SetScript(string text, bool typewriterAnimation)
	{
		string text2 = "";
		if (text.Contains("|"))
		{
			int num = text.IndexOf('|');
			text2 = text.Substring(0, num);
			text = text.Substring(num + 1);
		}

		Top.script.text = text2;
		Bottom.script.text = text;
		instance.StopAllCoroutines();
		if (typewriterAnimation)
		{
			instance.StartCoroutine(Typewrite());
		}
	}

	public static IEnumerator Typewrite()
	{
		if (!Top.script.text.IsNullOrWhitespace())
		{
			Bottom.script.maxVisibleCharacters = 0;
			yield return Top.typewriter.Write();
			yield return new WaitForSeconds(1f);
		}

		yield return Bottom.typewriter.Write();
	}

	public static bool IsActive()
	{
		return instance.gameObject.activeSelf;
	}

	public static void In()
	{
		GameObject obj = instance.gameObject;
		obj.SetActive(value: true);
		Clear();
		LeanTween.cancel(obj);
		SetPosition(-1.5f);
		LeanTween.value(obj, -1.5f, 0.4f, 0.5f).setEase(LeanTweenType.easeOutBack).setOnUpdate(SetPosition);
	}

	public static void InInstant()
	{
		GameObject obj = instance.gameObject;
		obj.SetActive(value: true);
		Clear();
		LeanTween.cancel(obj);
		SetPosition(0.4f);
	}

	public static void Out()
	{
		if (!(instance == null) && !(instance.gameObject == null) && instance.gameObject.activeSelf)
		{
			LeanTween.cancel(instance.gameObject);
			SetPosition(0.4f);
			LeanTween.value(instance.gameObject, 0.4f, -1.5f, 0.2f).setEase(LeanTweenType.easeInQuad).setOnUpdate(SetPosition)
				.setOnComplete((Action)delegate
				{
					ResetSortingLayer();
					instance.gameObject.SetActive(value: false);
				});
		}
	}

	public static void OutInstant()
	{
		if ((bool)instance && (bool)instance.gameObject && instance.gameObject.activeSelf)
		{
			LeanTween.cancel(instance.gameObject);
			SetPosition(-1.5f);
			ResetSortingLayer();
			instance.gameObject.SetActive(value: false);
		}
	}

	public static void SetSortingLayer(string name, int orderInLayer = 0)
	{
		instance.canvas.sortingLayerName = name;
		instance.canvas.sortingOrder = orderInLayer;
	}

	public static void ResetSortingLayer()
	{
		SetSortingLayer("CinemaBars");
	}

	public static void SetPosition(float value)
	{
		Top.transform.anchoredPosition = Top.transform.anchoredPosition.WithY(0f - value);
		Bottom.transform.anchoredPosition = Bottom.transform.anchoredPosition.WithY(value);
	}
}
