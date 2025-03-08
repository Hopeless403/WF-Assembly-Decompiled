#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class Prompt : MonoBehaviourRect
{
	public delegate string GetTextCallback();

	public enum Anchor
	{
		Mid,
		Top,
		Left,
		Right,
		Bottom,
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight
	}

	[Serializable]
	public struct Emote
	{
		public enum Type
		{
			None,
			Basic,
			Point,
			Scared,
			Talk,
			Explain,
			Sad,
			Happy
		}

		public enum Position
		{
			Left,
			Right,
			Above,
			Below
		}

		public Type type;

		public Sprite sprite;
	}

	[SerializeField]
	public TMP_Text textAsset;

	[SerializeField]
	public LayoutElement layoutElement;

	[SerializeField]
	public RectTransform textBox;

	[SerializeField]
	public float noteSize = 0.3f;

	[SerializeField]
	public string noteColorHex = "fffa";

	[SerializeField]
	public Emote[] emotes;

	[SerializeField]
	public Image emoteImage;

	[SerializeField]
	public string focusFormat = "<#fff>{0}</color>";

	public GetTextCallback SetTextAction;

	public float width;

	public Emote.Type emoteType;

	public Emote.Position emotePosition;

	public bool active { get; set; }

	public void OnEnable()
	{
		LocalizationSettings.Instance.OnSelectedLocaleChanged += LocaleChanged;
	}

	public void OnDisable()
	{
		LocalizationSettings.Instance.OnSelectedLocaleChanged -= LocaleChanged;
	}

	public void LocaleChanged(Locale locale)
	{
		RunSetTextAction();
	}

	public void RunSetTextAction()
	{
		SetText(SetTextAction?.Invoke());
	}

	public void SetAnchor(Anchor anchor)
	{
		switch (anchor)
		{
			case Anchor.Mid:
				SetAnchor(0.5f, 0.5f);
				break;
			case Anchor.Top:
				SetAnchor(0.5f, 1f);
				break;
			case Anchor.Left:
				SetAnchor(0f, 0.5f);
				break;
			case Anchor.Right:
				SetAnchor(1f, 0.5f);
				break;
			case Anchor.Bottom:
				SetAnchor(0.5f, 0f);
				break;
			case Anchor.TopLeft:
				SetAnchor(0f, 1f);
				break;
			case Anchor.TopRight:
				SetAnchor(1f, 1f);
				break;
			case Anchor.BottomLeft:
				SetAnchor(0f, 0f);
				break;
			case Anchor.BottomRight:
				SetAnchor(1f, 0f);
				break;
			default:
				throw new ArgumentException($"Invalid Anchor {anchor}");
		}
	}

	public void SetAnchor(float x, float y)
	{
		SetAnchor(new Vector2(x, y));
	}

	public void SetAnchor(Vector2 anchor)
	{
		base.rectTransform.anchorMin = anchor;
		base.rectTransform.anchorMax = anchor;
		base.rectTransform.pivot = anchor;
	}

	public void SetPosition(Vector2 position)
	{
		base.rectTransform.anchoredPosition = position;
	}

	public void SetPosition(Vector2 position, Anchor anchor)
	{
		SetAnchor(anchor);
		base.rectTransform.anchoredPosition = position;
	}

	public void SetMaxWidth(float value)
	{
		layoutElement.preferredWidth = value;
		width = value;
	}

	public void SetText(string text)
	{
		float result = 0f;
		int num = text.IndexOf('+');
		if (num >= 0)
		{
			string s = text.Substring(num + 1);
			text = text.Substring(0, num);
			float.TryParse(s, out result);
		}

		layoutElement.preferredWidth = width + result;
		string note = "";
		if (text.Contains("|"))
		{
			int num2 = text.IndexOf('|');
			note = text.Substring(num2 + 1);
			text = text.Substring(0, num2);
		}

		SetText(ProcessText(text), note);
		SetEmote(emoteType, emotePosition);
	}

	public void SetText(string text, string note = "")
	{
		if (!note.IsNullOrWhitespace())
		{
			text += $"\n\n<size={noteSize}><#{noteColorHex}>{note}";
		}

		textAsset.text = text;
	}

	public string ProcessText(string input)
	{
		string text = input.Trim();
		_ = input.Length;
		while (true)
		{
			int num = text.IndexOf('[');
			if (num < 0)
			{
				break;
			}

			int num2 = text.IndexOf(']', num);
			if (num2 <= num)
			{
				break;
			}

			string text2 = text.Substring(num, num2 - num).Substring(1, num2 - num - 1);
			string text3 = ProcessTag(text2);
			string text4 = text.Substring(0, num);
			string text5 = text.Substring(num2 + 1);
			text = text4 + text3 + text5;
		}

		return text;
	}

	public string ProcessTag(string tag)
	{
		return string.Format(focusFormat, tag);
	}

	public void SetEmote(Emote.Type emoteType, Emote.Position position)
	{
		this.emoteType = emoteType;
		emotePosition = position;
		Emote emote = emotes.FirstOrDefault((Emote e) => e.type == emoteType);
		if ((bool)emote.sprite)
		{
			emoteImage.gameObject.SetActive(value: true);
			emoteImage.sprite = emote.sprite;
			SetEmotePosition(position);
		}
		else
		{
			emoteImage.gameObject.SetActive(value: false);
		}
	}

	public void SetEmotePosition(Emote.Position position, float offsetX = 0f, float offsetY = 0f, float forceFlip = 0f)
	{
		StopAllCoroutines();
		StartCoroutine(SetEmotePositionRoutine(position, offsetX, offsetY, forceFlip));
	}

	public IEnumerator SetEmotePositionRoutine(Emote.Position position, float offsetX = 0f, float offsetY = 0f, float forceFlip = 0f)
	{
		Rect rect2 = textBox.rect;
		while (rect2 == textBox.rect)
		{
			yield return null;
		}

		rect2 = textBox.rect;
		float num = rect2.width * 0.5f + 0.1f;
		float num2 = rect2.height * 0.5f + 0.69f;
		RectTransform rectTransform = (RectTransform)emoteImage.transform;
		switch (position)
		{
			case Emote.Position.Left:
				rectTransform.anchoredPosition = new Vector2(0f - num + offsetX, 0f + offsetY);
				break;
			case Emote.Position.Right:
				rectTransform.anchoredPosition = new Vector2(num + offsetX, 0f + offsetY);
				break;
			case Emote.Position.Above:
				rectTransform.anchoredPosition = new Vector2((0f - num) * 0.5f + offsetX, num2 + offsetY);
				break;
			case Emote.Position.Below:
				rectTransform.anchoredPosition = new Vector2(0f + offsetX, 0f - num2 + offsetY);
				break;
		}

		float x = ((forceFlip != 0f) ? forceFlip : ((rectTransform.position.x < 0f) ? 1f : (-1f)));
		rectTransform.localScale = new Vector3(x, 1f, 1f);
	}

	public void Ping()
	{
		LeanTween.cancel(base.gameObject);
		base.transform.localScale = Vector3.one * 0f;
		LeanTween.scale(base.gameObject, Vector3.one, 1.5f).setEase(LeanTweenType.easeOutElastic);
	}

	public void Hide()
	{
		active = false;
		LeanTween.cancel(base.gameObject);
		base.transform.localScale = Vector3.one;
		LeanTween.scale(base.gameObject, Vector3.zero, 0.167f).setEase(LeanTweenType.easeInBack).setOnComplete((Action)delegate
		{
			base.gameObject.SetActive(value: false);
		});
	}

	public void Enable()
	{
		active = true;
		base.gameObject.SetActive(value: true);
	}
}
