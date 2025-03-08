#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class ColourFader : MonoBehaviour
{
	[Serializable]
	public struct Colour
	{
		public string name;

		public Color colour;
	}

	public Graphic _graphic;

	[SerializeField]
	public Colour[] colours;

	[SerializeField]
	public float duration;

	public Color targetColour;

	public float fadeAmount;

	public float fadeDuration;

	public Graphic graphic => _graphic ?? (_graphic = GetComponent<Graphic>());

	public void FadeToColour(string name)
	{
		Colour[] array = colours;
		for (int i = 0; i < array.Length; i++)
		{
			Colour colour = array[i];
			if (colour.name == name)
			{
				FadeToColour(colour.colour);
				break;
			}
		}
	}

	public void FadeToColour(Color colour)
	{
		fadeAmount = 0f;
		fadeDuration = duration;
		targetColour = colour;
	}

	public void Update()
	{
		if (fadeAmount < fadeDuration)
		{
			fadeAmount += Time.deltaTime * fadeDuration;
			graphic.color = Color.Lerp(graphic.color, targetColour, fadeAmount);
		}
	}
}
