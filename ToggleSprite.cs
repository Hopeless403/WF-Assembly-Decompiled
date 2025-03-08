#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

public class ToggleSprite : MonoBehaviour
{
	[SerializeField]
	public Image image;

	[SerializeField]
	public Sprite onSprite;

	[SerializeField]
	public Sprite offSprite;

	public void Set(bool value)
	{
		image.sprite = (value ? onSprite : offSprite);
	}
}
