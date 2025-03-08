#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class ScrollToNavigation : MonoBehaviour
{
	[SerializeField]
	public Scroller scroller;

	[SerializeField]
	public SmoothScrollRect scrollRect;

	public bool HasScroller => scroller;

	public bool HasScrollRect => scrollRect;

	public void OnEnable()
	{
		Events.OnUINavigation += Navigation;
	}

	public void OnDisable()
	{
		Events.OnUINavigation -= Navigation;
	}

	public void Navigation()
	{
		if (!MonoBehaviourSingleton<Cursor3d>.instance.usingMouse)
		{
			UINavigationItem currentNavigationItem = MonoBehaviourSingleton<UINavigationSystem>.instance.currentNavigationItem;
			if (HasScroller && scroller.ContentLargerThanBounds())
			{
				TryScrollScroller(currentNavigationItem.transform);
			}
			else if (HasScrollRect)
			{
				TryScrollScrollRect(currentNavigationItem.transform);
			}
		}
	}

	public void TryScrollScroller(Transform target)
	{
		if (target.IsChildOf(scroller.transform))
		{
			if (scroller.horizontal)
			{
				ScrollScrollerHorizontal(target);
			}
			else
			{
				ScrollScrollerVertical(target);
			}
		}
	}

	public void ScrollScrollerVertical(Transform target)
	{
		float value = scroller.transform.position.y - target.position.y;
		scroller.ScrollTo(scroller.targetPos.WithY(value));
	}

	public void ScrollScrollerHorizontal(Transform target)
	{
		float value = scroller.transform.position.x - target.position.x;
		scroller.ScrollTo(scroller.targetPos.WithX(value));
	}

	public void TryScrollScrollRect(Transform target)
	{
		if (target.IsChildOf(scrollRect.transform))
		{
			if (scrollRect.horizontal)
			{
				ScrollScrollRectHorizontal(target);
			}
			else
			{
				ScrollScrollRectVertical(target);
			}
		}
	}

	public void ScrollScrollRectVertical(Transform target)
	{
		float value = 0f - (target.position.y - scrollRect.content.position.y + scrollRect.viewport.rect.size.y * 0.5f);
		scrollRect.ScrollTo(scrollRect.content.anchoredPosition.WithY(value));
	}

	public void ScrollScrollRectHorizontal(Transform target)
	{
		float value = 0f - (target.position.x - scrollRect.content.position.x + scrollRect.viewport.rect.size.x * 0.5f);
		scrollRect.ScrollTo(scrollRect.content.anchoredPosition.WithX(value));
	}
}
