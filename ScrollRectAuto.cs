#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectAuto : MonoBehaviour, IDragHandler, IEventSystemHandler, IScrollHandler
{
	public ScrollRect _scrollRect;

	[SerializeField]
	public float activateTime = 1f;

	[SerializeField]
	public float scrollSpeed = 1f;

	[SerializeField]
	public float acceleration = 1f;

	[SerializeField]
	public bool disableAtBottom = true;

	[SerializeField]
	public bool disableOnDrag = true;

	[SerializeField]
	public bool disableOnMouseScroll = true;

	[SerializeField]
	public bool reactivate = true;

	[SerializeField]
	[ShowIf("reactivate")]
	public float reactivateTime = 1f;

	[SerializeField]
	public UnityEvent onReachBottom;

	public float scrollSpeedCurrent;

	public static readonly Vector2 scroll = new Vector2(0f, 1f);

	public bool active = true;

	public float reactivateTimer;

	public ScrollRect scrollRect => _scrollRect ?? (_scrollRect = GetComponent<ScrollRect>());

	public void Update()
	{
		if (!scrollRect.content)
		{
			return;
		}

		if (activateTime > 0f)
		{
			activateTime -= Time.deltaTime;
			return;
		}

		if (!active)
		{
			if (reactivateTimer > 0f)
			{
				reactivateTimer -= Time.deltaTime;
				if (reactivateTimer <= 0f)
				{
					active = true;
				}
			}

			return;
		}

		scrollSpeedCurrent = Delta.Lerp(scrollSpeedCurrent, scrollSpeed, acceleration, Time.deltaTime);
		scrollRect.content.anchoredPosition += scroll * (scrollSpeedCurrent * Time.deltaTime);
		if (scrollRect.normalizedPosition.y <= 0f)
		{
			onReachBottom?.Invoke();
			if (disableAtBottom)
			{
				Stop();
				reactivateTimer = 0f;
			}
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (disableOnDrag)
		{
			Stop();
		}
	}

	public void OnScroll(PointerEventData eventData)
	{
		if (disableOnMouseScroll)
		{
			Stop();
		}
	}

	public void Stop()
	{
		active = false;
		if (reactivate)
		{
			reactivateTimer = reactivateTime;
		}

		scrollSpeedCurrent = 0f;
	}
}
