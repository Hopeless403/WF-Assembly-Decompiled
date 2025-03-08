#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class MapInspectSequence : MonoBehaviour
{
	[Serializable]
	public struct Profile
	{
		public UnityEngine.Localization.LocalizedString titleKey;

		public UnityEngine.Localization.LocalizedString descKey;

		public Sprite icon;
	}

	[SerializeField]
	public LocalizeStringEvent title;

	[SerializeField]
	public LocalizeStringEvent desc;

	[SerializeField]
	public Image image;

	[SerializeField]
	public Profile[] profiles;

	public void Inspect(int index)
	{
		Inspect(profiles[index]);
	}

	public void Inspect(Profile profile)
	{
		Inspect(profile.titleKey, profile.descKey, profile.icon);
	}

	public void Inspect(UnityEngine.Localization.LocalizedString titleKey, UnityEngine.Localization.LocalizedString descKey, Sprite icon)
	{
		title.StringReference = titleKey;
		desc.StringReference = descKey;
		image.sprite = icon;
		base.gameObject.SetActive(value: true);
	}
}
