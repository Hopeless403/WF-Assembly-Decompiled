#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Keyword", menuName = "Keyword")]
public class KeywordData : DataFile
{
	[SerializeField]
	public UnityEngine.Localization.LocalizedString titleKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString descKey;

	public Color titleColour = new Color(1f, 0.7921569f, 0.3411765f, 1f);

	public Color bodyColour = Color.white;

	public Color noteColour = Color.gray;

	public Sprite panelSprite;

	public Color panelColor;

	public string iconName;

	public string iconTintHex;

	public bool show = true;

	public bool showName;

	public bool showIcon = true;

	public bool canStack;

	public bool HasTitle => !titleKey.IsEmpty;

	public string title => titleKey.GetLocalizedString();

	public string body
	{
		get
		{
			string localizedString = descKey.GetLocalizedString();
			int num = localizedString.IndexOf('|');
			if (num <= 0)
			{
				return localizedString;
			}

			return localizedString.Substring(0, num);
		}
	}

	public string note
	{
		get
		{
			string localizedString = descKey.GetLocalizedString();
			int num = localizedString.IndexOf('|');
			if (num <= 0)
			{
				return null;
			}

			return localizedString.Substring(num + 1);
		}
	}
}
