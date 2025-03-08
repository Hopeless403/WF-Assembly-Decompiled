#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class TitleSetter : MonoBehaviour
{
	[SerializeField]
	public new GameObject gameObject;

	[SerializeField]
	public LocalizeStringEvent text;

	[SerializeField]
	public Image underline;

	[SerializeField]
	public bool setActive = true;

	[SerializeField]
	[ShowIf("setActive")]
	public UnityEngine.Localization.LocalizedString setKey;

	[SerializeField]
	[ShowIf("setActive")]
	public Sprite setUnderlineSprite;

	public void Set()
	{
		gameObject.SetActive(setActive);
		if (setActive)
		{
			text.StringReference = setKey;
			underline.sprite = setUnderlineSprite;
		}
	}
}
