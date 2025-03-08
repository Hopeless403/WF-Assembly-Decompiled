#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public class NameTag : MonoBehaviour
{
	[SerializeField]
	public NameTagStyle[] styles;

	public NameTagStyle currentStyle;

	public readonly Dictionary<string, NameTagStyle> styleDictionary = new Dictionary<string, NameTagStyle>();

	public void Awake()
	{
		NameTagStyle[] array = styles;
		foreach (NameTagStyle nameTagStyle in array)
		{
			styleDictionary[nameTagStyle.name] = nameTagStyle;
		}

		array = styles;
		foreach (NameTagStyle nameTagStyle2 in array)
		{
			if (nameTagStyle2.gameObject.activeSelf)
			{
				currentStyle = nameTagStyle2;
				break;
			}
		}
	}

	public void SetStyle(string styleName)
	{
		NameTagStyle nameTagStyle = styleDictionary[styleName];
		if (nameTagStyle != currentStyle)
		{
			currentStyle.gameObject.SetActive(value: false);
			nameTagStyle.gameObject.SetActive(value: true);
			currentStyle = nameTagStyle;
		}
	}
}
