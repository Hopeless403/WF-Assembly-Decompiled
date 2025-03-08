#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventPointerEnter : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler
{
	[SerializeField]
	public UnityEvent pointerEnter;

	[SerializeField]
	public bool afterFrame;

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (afterFrame)
		{
			StartCoroutine(InvokeAfterFrame());
		}
		else
		{
			pointerEnter.Invoke();
		}
	}

	public IEnumerator InvokeAfterFrame()
	{
		yield return new WaitForEndOfFrame();
		pointerEnter.Invoke();
	}
}
