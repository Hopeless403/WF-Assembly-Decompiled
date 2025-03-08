#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class WorldSpaceCanvasFitScreen : WorldSpaceCanvasUpdater
{
	[ReadOnly]
	public float width;

	[ReadOnly]
	public float height;

	[SerializeField]
	public float scale = 1f;

	[SerializeField]
	public AnimationCurve largeScaleCurve;

	[SerializeField]
	public Vector2 padding;

	public UnityEvent<RectTransform> onUpdate;

	public override void UpdateSize()
	{
		if (!WorldSpaceCanvasFitScreenSystem.exists)
		{
			return;
		}

		if (WorldSpaceCanvasFitScreenSystem.instance.cam.orthographic)
		{
			height = WorldSpaceCanvasFitScreenSystem.instance.cam.orthographicSize * 2f;
			width = height * WorldSpaceCanvasFitScreenSystem.instance.aspectRatio;
		}
		else
		{
			height = 11.547f;
			width = height * WorldSpaceCanvasFitScreenSystem.instance.aspectRatio;
		}

		float num;
		if (width < 17.32051f)
		{
			num = width / 17.32051f * scale;
			width = 17.32051f;
			height = width / 1.5f;
		}
		else
		{
			if (width > 26.943f)
			{
				width = 26.943f;
			}

			AnimationCurve animationCurve = largeScaleCurve;
			if (animationCurve != null)
			{
				_ = animationCurve.length;
				_ = 0;
			}

			num = scale;
		}

		base.rectTransform.sizeDelta = new Vector2(width - padding.x, height - padding.y);
		base.rectTransform.localScale = new Vector3(num, num, 1f);
		onUpdate?.Invoke(base.rectTransform);
	}
}
