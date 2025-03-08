#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderSfx : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	[SerializeField]
	public Slider slider;

	[SerializeField]
	public EventReference sfxEvent;

	public bool drag;

	public void Fire()
	{
		SfxSystem.OneShot(sfxEvent);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		drag = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (drag)
		{
			Fire();
		}

		drag = false;
	}
}
