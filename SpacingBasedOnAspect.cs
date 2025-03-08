#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalOrVerticalLayoutGroup))]
public class SpacingBasedOnAspect : MonoBehaviour
{
	[SerializeField]
	public AnimationCurve curve;

	[SerializeField]
	public float maxSpacing;

	[SerializeField]
	public float minSpacing;

	public HorizontalOrVerticalLayoutGroup _layout;

	public HorizontalOrVerticalLayoutGroup layout => _layout ?? (_layout = GetComponent<HorizontalOrVerticalLayoutGroup>());

	public void UpdatePosition(RectTransform canvas)
	{
		Vector2 sizeDelta = canvas.sizeDelta;
		float time = sizeDelta.x / sizeDelta.y;
		float num = curve.Evaluate(time);
		layout.spacing = minSpacing + (maxSpacing - minSpacing) * num;
	}
}
