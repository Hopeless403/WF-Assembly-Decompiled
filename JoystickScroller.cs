#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class JoystickScroller : MonoBehaviour
{
	[SerializeField]
	public string scrollAction = "Scroll Vertical";

	public ScrollRect _scrollRect;

	public readonly Vector2 add = new Vector2(0f, -20f);

	public ScrollRect scrollRect => _scrollRect ?? (_scrollRect = GetComponent<ScrollRect>());

	public void LateUpdate()
	{
		if (!scrollRect.enabled)
		{
			return;
		}

		float axis = InputSystem.GetAxis(scrollAction);
		if (Mathf.Abs(axis) > float.Epsilon && (bool)scrollRect.content)
		{
			float verticalNormalizedPosition = scrollRect.verticalNormalizedPosition;
			if ((axis > 0f && verticalNormalizedPosition < 1f) || (axis < 0f && verticalNormalizedPosition > 0f))
			{
				scrollRect.content.anchoredPosition += add * (axis * Mathf.Abs(axis) * Time.unscaledDeltaTime);
			}
		}
	}
}
