#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using TMPro;
using UnityEngine;

public class CardPopUpPanel : Tooltip
{
	[Header("Text")]
	[SerializeField]
	public float titleSize = 0.3f;

	[SerializeField]
	public float iconSize = 0.275f;

	[SerializeField]
	public float bodySize = 0.25f;

	[SerializeField]
	public float noteSize = 0.21f;

	[SerializeField]
	public TextMeshProUGUI textElement;

	[SerializeField]
	public Fitter fitter;

	public static readonly Color defaultTitleColor = new Color(1f, 0.7921569f, 0.3411765f, 1f);

	public static readonly Color defaultBodyColor = Color.white;

	public static readonly Color defaultNoteColor = new Color(0.65f, 0.65f, 0.65f);

	public string titleText;

	public string bodyText;

	public string noteText;

	public string text => textElement.text;

	public void Set(string iconName, string iconTintHex, string title, Color titleColour, string body, Color bodyColour, Sprite panelSprite, Color panelColor)
	{
		SetRoutine(iconName, iconTintHex, title, titleColour, body, bodyColour, "", Color.white, panelSprite, panelColor);
	}

	public void Set(string iconName, string iconTintHex, string title, Color titleColour, string body, Color bodyColour)
	{
		SetRoutine(iconName, iconTintHex, title, titleColour, body, bodyColour, "", Color.white, defaultPanelSprite, defaultPanelColor);
	}

	public void Set(string title, string body)
	{
		Set(title, defaultTitleColor, body, defaultBodyColor);
	}

	public void Set(string title, Color titleColour, string body, Color bodyColour)
	{
		SetRoutine("", "", title, titleColour, body, bodyColour, "", Color.white, defaultPanelSprite, defaultPanelColor);
	}

	public void Set(string text, Color textColour)
	{
		SetRoutine("", "", "", Color.white, text, textColour, "", Color.white, defaultPanelSprite, defaultPanelColor);
	}

	public void Set(KeywordData keyword, string forceBody = null)
	{
		string title = (keyword.HasTitle ? keyword.title : "");
		string body = forceBody ?? keyword.body;
		SetRoutine(keyword.iconName, keyword.iconTintHex, title, keyword.titleColour, body, keyword.bodyColour, keyword.note, keyword.noteColour, keyword.panelSprite, keyword.panelColor);
	}

	public void SetRoutine(string iconName, string iconTintHex, string title, Color titleColour, string body, Color bodyColour, string note, Color noteColour, Sprite panelSprite, Color panelColor)
	{
		SetTitle(title, titleColour, iconName, iconTintHex);
		SetBody(body, bodyColour);
		SetNote(note, noteColour);
		BuildTextElement();
		panel.sprite = (panelSprite ? panelSprite : defaultPanelSprite);
		panel.color = (panelSprite ? panelColor : defaultPanelColor);
		if ((bool)fitter)
		{
			LeanTween.cancel(base.gameObject);
			base.transform.localScale = Vector3.one;
			canvasGroup.alpha = 0f;
			fitter.Fit(base.Ping);
		}
		else
		{
			Ping();
		}
	}

	public void SetTitle(string title, Color titleColour, string iconName, string iconTintHex)
	{
		titleText = "";
		if (!title.IsNullOrWhitespace())
		{
			string arg = titleColour.ToHexRGBA();
			titleText += $"<size={titleSize}><#{arg}>{Text.Process(title)}";
		}

		if (!iconName.IsNullOrWhitespace())
		{
			if (!iconTintHex.IsNullOrWhitespace())
			{
				titleText += $"<size={iconSize}><sprite name=\"{iconName}\" color=#{iconTintHex}></size>";
			}
			else
			{
				titleText += $"<size={iconSize}><sprite name=\"{iconName}\"></size>";
			}
		}
	}

	public void AddToTitle(string text, bool newline = true)
	{
		if (titleText.IsNullOrWhitespace())
		{
			SetTitle(text, defaultTitleColor, "", "");
			return;
		}

		if (newline)
		{
			titleText += "\n";
		}

		titleText += Text.Process(text);
	}

	public void SetBody(string body, Color bodyColour)
	{
		bodyText = "";
		if (!body.IsNullOrWhitespace())
		{
			string arg = bodyColour.ToHexRGBA();
			bodyText = $"<size={bodySize}><#{arg}>{Text.Process(body)}";
		}
	}

	public void AddToBody(string text, bool newline = true)
	{
		if (bodyText.IsNullOrWhitespace())
		{
			SetBody(text, defaultBodyColor);
			return;
		}

		if (newline)
		{
			bodyText += "\n";
		}

		bodyText += Text.Process(text);
	}

	public void SetNote(string note, Color noteColour)
	{
		noteText = "";
		if (!note.IsNullOrWhitespace())
		{
			string arg = noteColour.ToHexRGBA();
			noteText = $"<size={noteSize}><#{arg}>{note}";
		}
	}

	public void AddToNote(string text, bool newline = true)
	{
		if (noteText.IsNullOrWhitespace())
		{
			SetNote(text, defaultNoteColor);
			return;
		}

		if (newline)
		{
			noteText += "\n";
		}

		noteText += text;
	}

	public void BuildTextElement()
	{
		string text = titleText;
		if (!bodyText.IsNullOrWhitespace())
		{
			if (!text.IsNullOrWhitespace())
			{
				text += "<line-height=96%>\n</line-height>";
			}

			text += bodyText;
		}

		if (!noteText.IsNullOrWhitespace())
		{
			if (!text.IsNullOrWhitespace())
			{
				text += "<line-height=96%>\n</line-height>";
			}

			text += noteText;
		}

		textElement.text = text;
	}
}
