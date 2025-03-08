#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnOrderNumber : MonoBehaviour
{
	[SerializeField]
	public Image glow;

	[SerializeField]
	public Color imminentGlowColour = Color.red;

	[SerializeField]
	public Image image;

	[SerializeField]
	public Sprite imminentSprite;

	[SerializeField]
	public TMP_Text textElement;

	public void Set(Entity entity, int number)
	{
		textElement.text = number.ToString();
		if (entity.counter.current <= 1 && !entity.IsSnowed)
		{
			glow.color = imminentGlowColour;
			image.sprite = imminentSprite;
		}
	}
}
