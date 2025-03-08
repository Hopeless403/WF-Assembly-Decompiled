#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class PositionBasedOnAspect : MonoBehaviourRect
{
	[SerializeField]
	public AnimationCurve curve;

	[SerializeField]
	public Vector3 maxPosition;

	[SerializeField]
	public Vector3 minPosition;

	public void UpdatePosition(RectTransform canvas)
	{
		Vector2 sizeDelta = canvas.sizeDelta;
		float time = sizeDelta.x / sizeDelta.y;
		float num = curve.Evaluate(time);
		base.rectTransform.anchoredPosition3D = minPosition + (maxPosition - minPosition) * num;
	}
}
