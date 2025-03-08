#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextTypewrite : MonoBehaviour
{
	[SerializeField]
	public TMP_Text textElement;

	public const float startDelay = 0.25f;

	public const float defaultPause = 0.05f;

	public static readonly Dictionary<char, float> lookup = new Dictionary<char, float>
	{
		{ ' ', 0.005f },
		{ ',', 0.4f },
		{ '，', 0.4f },
		{ '.', 0.15f },
		{ '…', 0.4f }
	};

	public IEnumerator Write()
	{
		textElement.maxVisibleCharacters = 0;
		yield return new WaitForSeconds(0.25f);
		int total = textElement.textInfo.characterCount;
		for (int i = 0; i < total; i++)
		{
			textElement.maxVisibleCharacters = i + 1;
			char character = textElement.textInfo.characterInfo[i].character;
			float seconds = (lookup.ContainsKey(character) ? lookup[character] : 0.05f);
			yield return new WaitForSeconds(seconds);
		}
	}
}
