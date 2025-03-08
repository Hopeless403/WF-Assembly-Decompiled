#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class ScaleBasedOnAspect : MonoBehaviour
{
	[SerializeField]
	public AnimationCurve curve;

	[SerializeField]
	public Vector3 maxScale;

	[SerializeField]
	public Vector3 minScale;

	[SerializeField]
	public bool autoLinkToParent;

	public void Awake()
	{
		if (autoLinkToParent)
		{
			WorldSpaceCanvasFitScreen componentInParent = base.transform.GetComponentInParent<WorldSpaceCanvasFitScreen>();
			if ((bool)componentInParent)
			{
				componentInParent.onUpdate.AddListener(UpdateScale);
				UpdateScale(componentInParent.rectTransform);
			}
		}
	}

	public void UpdateScale(RectTransform canvas)
	{
		Vector2 sizeDelta = canvas.sizeDelta;
		float time = sizeDelta.x / sizeDelta.y;
		float num = curve.Evaluate(time);
		base.transform.localScale = minScale + (maxScale - minScale) * num;
	}
}
