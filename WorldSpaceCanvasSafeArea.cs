#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class WorldSpaceCanvasSafeArea : WorldSpaceCanvasUpdater
{
	[SerializeField]
	public RectTransform parent;

	[SerializeField]
	public bool considerX = true;

	[SerializeField]
	public bool considerY;

	public bool waitForParent;

	public void LateUpdate()
	{
		if (waitForParent && parent.gameObject.activeSelf && WorldSpaceCanvasFitScreenSystem.exists)
		{
			Vector2 sizeDelta = parent.sizeDelta;
			float num = (considerX ? WorldSpaceCanvasFitScreenSystem.instance.safeArea.x : 0f);
			float num2 = (considerY ? WorldSpaceCanvasFitScreenSystem.instance.safeArea.y : 0f);
			float num3 = (considerX ? WorldSpaceCanvasFitScreenSystem.instance.safeArea.width : 1f);
			float num4 = (considerY ? WorldSpaceCanvasFitScreenSystem.instance.safeArea.height : 1f);
			float num5 = (considerX ? (1f - WorldSpaceCanvasFitScreenSystem.instance.safeArea.width - WorldSpaceCanvasFitScreenSystem.instance.safeArea.x) : 0f);
			float num6 = (considerY ? (1f - WorldSpaceCanvasFitScreenSystem.instance.safeArea.height - WorldSpaceCanvasFitScreenSystem.instance.safeArea.y) : 0f);
			float num7 = num3 * sizeDelta.x;
			float num8 = num4 * sizeDelta.y;
			if (num7 / num8 < 1.5f)
			{
				float num9 = num8 * 1.5f / sizeDelta.x;
				float num10 = num9 - num3;
				num = Mathf.Max(0f, num - num10);
				num5 = Mathf.Max(0f, num5 - num10);
				num3 = num9;
			}

			if (base.rectTransform.anchorMin == Vector2.zero && base.rectTransform.anchorMax == Vector2.one)
			{
				base.rectTransform.offsetMin = new Vector2(sizeDelta.x * num, sizeDelta.y * num2);
				base.rectTransform.offsetMax = new Vector2(sizeDelta.x * (0f - num5), sizeDelta.y * (0f - num6));
			}
			else
			{
				base.rectTransform.sizeDelta = new Vector2(sizeDelta.x * num3, sizeDelta.y * num4);
				base.rectTransform.anchoredPosition = new Vector2(sizeDelta.x * num, sizeDelta.y * num2);
			}

			waitForParent = false;
		}
	}

	public override void UpdateSize()
	{
		waitForParent = true;
	}
}
