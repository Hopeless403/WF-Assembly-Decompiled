#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventPointerClick : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	[SerializeField]
	public UnityEvent pointerClick;

	[SerializeField]
	public bool afterFrame;

	public void OnPointerClick(PointerEventData eventData)
	{
		if (afterFrame)
		{
			StartCoroutine(InvokeAfterFrame());
		}
		else
		{
			pointerClick.Invoke();
		}
	}

	public IEnumerator InvokeAfterFrame()
	{
		yield return new WaitForEndOfFrame();
		pointerClick.Invoke();
	}
}
