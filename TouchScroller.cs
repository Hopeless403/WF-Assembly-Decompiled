#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class TouchScroller : MonoBehaviour
{
	[SerializeField]
	public Scroller scroller;

	[SerializeField]
	public bool horizontal;

	[SerializeField]
	public bool vertical = true;

	[SerializeField]
	public float sensitivity = 0.0215f;

	[SerializeField]
	public float inertia = 0.15f;

	public float vx;

	public float vy;

	public const float threshold = 0.01f;

	public void Update()
	{
		if (!TouchInputModule.active || (!horizontal && !vertical) || !scroller.interactable || !scroller.CheckNavigationLayer() || scroller.DeltaTime <= 0f)
		{
			vx = 0f;
			vy = 0f;
			return;
		}

		Vector2 anchoredPosition = scroller.rectTransform.anchoredPosition;
		bool flag = false;
		if (horizontal)
		{
			float num = (scroller.interactable ? (TouchInputModule.ScrollX * sensitivity) : 0f);
			if (TouchInputModule.touching)
			{
				if (Mathf.Abs(num) > 0.01f)
				{
					anchoredPosition.x += num;
					vx = num;
					flag = true;
				}
			}
			else if (Mathf.Abs(vx) > 0.01f)
			{
				anchoredPosition.x += vx;
				flag = true;
			}
		}

		if (vertical)
		{
			float num2 = (scroller.interactable ? (TouchInputModule.ScrollY * sensitivity) : 0f);
			if (TouchInputModule.touching)
			{
				if (Mathf.Abs(num2) > 0.01f)
				{
					anchoredPosition.y += num2;
					vy = num2;
					flag = true;
				}
			}
			else if (Mathf.Abs(vy) > 0.01f)
			{
				anchoredPosition.y += vy;
				flag = true;
			}
		}

		if (flag)
		{
			scroller.ScrollTo(anchoredPosition);
			scroller.CheckBounds();
			scroller.rectTransform.anchoredPosition = scroller.targetPos;
		}

		if (horizontal)
		{
			vx = Delta.Lerp(vx, 0f, inertia, scroller.DeltaTime);
		}

		if (vertical)
		{
			vy = Delta.Lerp(vy, 0f, inertia, scroller.DeltaTime);
		}
	}
}
