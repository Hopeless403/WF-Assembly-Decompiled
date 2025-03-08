#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using TMPro;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
	[SerializeField]
	public TMP_Text textElement;

	public void SetSize(float size)
	{
		textElement.fontSize = size;
	}

	public void SetTextColour(Color color)
	{
		textElement.color = color;
	}

	public void SetTextSpriteAsset(TMP_SpriteAsset spriteAsset)
	{
		textElement.spriteAsset = spriteAsset;
	}

	public void SetText(string text)
	{
		textElement.text = text;
	}
}
