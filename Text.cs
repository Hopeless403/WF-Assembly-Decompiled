#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Localization;

public static class Text
{
	[Serializable]
	public struct ColourProfileHex
	{
		public string textColour;

		public string effectColour;

		public string effectBuffedColour;

		public string effectDebuffedColour;

		public string keywordColour;

		public string keywordSilencedColour;

		public string keywordBuffedColour;

		public string keywordDebuffedColour;

		public string flavourColour;

		public ColourProfileHex(string textColour, string effectColour, string effectBuffedColour, string effectDebuffedColour, string keywordColour, string keywordSilencedColour, string keywordBuffedColour, string keywordDebuffedColour, string flavourColour)
		{
			this.textColour = textColour;
			this.effectColour = effectColour;
			this.effectBuffedColour = effectBuffedColour;
			this.effectDebuffedColour = effectDebuffedColour;
			this.keywordColour = keywordColour;
			this.keywordSilencedColour = keywordSilencedColour;
			this.keywordBuffedColour = keywordBuffedColour;
			this.keywordDebuffedColour = keywordDebuffedColour;
			this.flavourColour = flavourColour;
		}
	}

	public static ColourProfileHex darkProfile = new ColourProfileHex("3B262C", "000", "41874AFF", "880033FF", "4B6A9CFF", "4B6A9Caa", "4188baff", "4B6A9Caa", "ffe6");

	public static ColourProfileHex lightProfile = new ColourProfileHex("FFF", "FFFF99FF", "BBFFCCFF", "FFCCBBFF", "FFFF99FF", "FFFF99aa", "BBFFCCFF", "FFCCBBFF", "ffe6");

	public static string Process(string original)
	{
		return Process(original, 0, 1f, lightProfile);
	}

	public static string Process(string original, ColourProfileHex profile)
	{
		return Process(original, 0, 1f, profile);
	}

	public static string Process(string original, int effectBonus, float effectFactor, ColourProfileHex profile)
	{
		string text = original.Trim();
		int length = text.Length;
		for (int i = 0; i < length; i++)
		{
			if (text[i] != '<')
			{
				continue;
			}

			string text2 = FindTag(text, i);
			if (text2.Length > 0)
			{
				text = text.Remove(i, text2.Length + 2);
				string text3 = ProcessTag(text, text2, effectBonus, effectFactor, profile);
				if (text3.Length > 0)
				{
					text = text.Insert(i, text3);
					i += text3.Length;
				}

				length = text.Length;
				i--;
			}
		}

		return text;
	}

	public static string FindTag(string text, int startIndex)
	{
		string result = "";
		int num = text.IndexOf('>', startIndex) - startIndex;
		if (num > 0)
		{
			result = text.Substring(startIndex + 1, num - 1);
		}

		return result;
	}

	public static string ProcessTag(string text, string tag, int effectBonus, float effectFactor, ColourProfileHex profile)
	{
		char c = tag[0];
		bool flag = char.IsDigit(c);
		if (tag.Length == 1 && !flag)
		{
			return "<" + tag + ">";
		}

		string text2 = string.Empty;
		string text3 = null;
		if (c == '+' || c == '-' || c == 'x')
		{
			text3 = c.ToString();
		}

		if ((flag || text3 != null) && int.TryParse(Regex.Replace(tag, "[^0-9]+", string.Empty), out var result))
		{
			int num = Mathf.Max(0, Mathf.RoundToInt((float)(result + effectBonus) * effectFactor));
			string text4 = "<color=#" + profile.effectColour + ">";
			string text5 = "</color>";
			if (num > result)
			{
				text4 = "<color=#" + profile.effectBuffedColour + ">";
			}
			else if (num < result)
			{
				text4 = "<color=#" + profile.effectDebuffedColour + ">";
			}

			text2 = $"<b>{text4}{text3}{num}{text5}</b>";
		}
		else
		{
			string[] array = tag.Split('=');
			if (array.Length == 2)
			{
				switch (array[0].Trim())
				{
					case "keyword":
					{
						string text7 = array[1].Trim();
						KeywordData keywordData = ToKeyword(text7);
						bool flag2 = !keywordData.iconName.IsNullOrWhitespace();
					if (flag2 && keywordData.showIcon && !keywordData.showName)
					{
						text2 = "<sprite name=" + keywordData.iconName + ">";
						}
					else
					{
						if (!keywordData.showName)
						{
							break;
						}

						string[] array2 = text7.Split(' ');
						bool flag3 = array2.Length > 2 && array2[2] == "silenced";
						string text8 = (flag3 ? profile.keywordSilencedColour : profile.keywordColour);
						if (flag2 && keywordData.showIcon)
						{
							text2 = text2 + "<sprite name=\"" + keywordData.iconName + "\" color=#" + text8 + ">";
						}

						text2 = text2 + "<color=#" + text8 + ">";
						if (flag3)
						{
							text2 += "<s>";
						}

						if (keywordData.canStack && array2.Length > 1)
						{
							result = 1;
							int num2 = 1;
							if (array2.Length != 0 && int.TryParse(array2[1], out result))
							{
								num2 = Mathf.Max(0, Mathf.RoundToInt((float)(result + effectBonus) * effectFactor));
							}

							string text9 = "<color=#" + text8 + ">";
							string text10 = "</color>";
							if (num2 > result)
							{
								text9 = "<color=#" + profile.keywordBuffedColour + ">";
							}
							else if (num2 < result)
							{
								text9 = "<color=#" + profile.keywordDebuffedColour + ">";
							}

							text2 += $"{keywordData.title} <b>{text9}{num2}{text10}</b>";
						}
						else
						{
							text2 += keywordData.title;
						}

						text2 += "</color>";
						if (flag3)
						{
							text2 += "</s>";
						}
						}
	
						break;
					}
					case "card":
					{
						string text6 = array[1].Trim();
						CardData cardData = AddressableLoader.Get<CardData>("CardData", text6);
					if (!cardData)
					{
						Debug.LogError("Error processing text: " + text + "\nCard [" + text6 + "] does not exist!");
						}
	
						text2 = "<#" + profile.keywordColour + ">" + cardData.title + "</color>";
						break;
					}
					case "sprite":
					case "sprite name":
					case "spr":
						text2 = "<sprite name=" + array[1].Trim() + ">";
						break;
					case "color":
					case "size":
						text2 = "<" + tag + ">";
						break;
				}
			}
		}

		if (text2.IsNullOrWhitespace())
		{
			text2 = ((!c.Equals('/')) ? ("<color=#" + profile.effectColour + ">" + tag + "</color>") : ("<" + tag + ">"));
		}

		return text2;
	}

	public static KeywordData ToKeyword(string text)
	{
		int num = text.IndexOf(' ');
		if (num > 0)
		{
			text = text.Remove(num, text.Length - num);
		}

		KeywordData keywordData = AddressableLoader.Get<KeywordData>("KeywordData", text);
		Debug.Log((keywordData != null) ? ("Keyword for \"" + text + "\" = [" + keywordData.name + "]") : ("Keyword for \"" + text + "\" = NULL"));
		if (!keywordData)
		{
			Debug.LogError("Keyword \"" + text + "\" not found!");
		}

		return keywordData;
	}

	public static HashSet<KeywordData> GetKeywords(string text)
	{
		HashSet<KeywordData> hashSet = new HashSet<KeywordData>();
		for (int i = 0; i < text.Length; i++)
		{
			if (!text[i].Equals('<'))
			{
				continue;
			}

			string text2 = FindTag(text, i);
			if (text2.Length <= 0 || !text2.Contains("="))
			{
				continue;
			}

			string[] array = text2.Split('=');
			if (array.Length == 2 && array[0].Trim() == "keyword")
			{
				KeywordData keywordData = ToKeyword(array[1].Trim());
				if (keywordData.show)
				{
					hashSet.Add(keywordData);
				}
			}
		}

		return hashSet;
	}

	public static HashSet<CardData> GetMentionedCards(string text)
	{
		HashSet<CardData> hashSet = new HashSet<CardData>();
		for (int i = 0; i < text.Length; i++)
		{
			if (!text[i].Equals('<'))
			{
				continue;
			}

			string text2 = FindTag(text, i);
			if (text2.Length > 0 && text2.Contains("="))
			{
				string[] array = text2.Split('=');
				if (array.Length == 2 && array[0].Trim() == "card")
				{
					CardData item = AddressableLoader.Get<CardData>("CardData", array[1].Trim());
					hashSet.Add(item);
				}
			}
		}

		return hashSet;
	}

	public static string GetEffectText(UnityEngine.Localization.LocalizedString textKey, string textInsert, int amount, bool silenced = false)
	{
		string localizedString = textKey.GetLocalizedString();
		localizedString = localizedString.Replace("{0}", textInsert);
		localizedString = localizedString.Replace("{e}", textInsert);
		localizedString = localizedString.Replace("{a}", amount.ToString());
		localizedString = HandleBracketTags(localizedString);
		return silenced ? ("<s>" + localizedString + "</s>") : localizedString;
	}

	public static string GetEffectText(IEnumerable<UnityEngine.Localization.LocalizedString> textKeys, string textInsert, int amount, bool silenced = false)
	{
		string text = "{0}";
		foreach (UnityEngine.Localization.LocalizedString textKey in textKeys)
		{
			text = text.Replace("{0}", textKey.GetLocalizedString());
		}

		text = text.Replace("{0}", textInsert);
		text = text.Replace("{e}", textInsert);
		text = text.Replace("{a}", amount.ToString());
		text = HandleBracketTags(text);
		return silenced ? ("<s>" + text + "</s>") : text;
	}

	public static string HandleBracketTags(string text)
	{
		while (true)
		{
			int num = text.IndexOf('[');
			if (num < 0)
			{
				break;
			}

			int num2 = text.IndexOf(']') - num + 1;
			if (num2 <= 0)
			{
				break;
			}

			string text2 = text.Substring(num + 1, num2 - 2);
			char c = ((text2.Length > 1 && num > 0) ? text2[1] : text2[0]);
			text = text.Remove(num, num2);
			text = text.Insert(num, c.ToString());
		}

		return text;
	}
}
