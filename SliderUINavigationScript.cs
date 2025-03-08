#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

public class SliderUINavigationScript : MonoBehaviour
{
	public float valueMultiplier = 0.1f;

	public Slider slider;

	[SerializeField]
	public SliderSfx sfx;

	public void OnChangeSliderValue(float inValueChange)
	{
		inValueChange *= valueMultiplier;
		slider.value += inValueChange;
		if ((bool)sfx)
		{
			sfx.Fire();
		}
	}
}
