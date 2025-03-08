#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class GridLayoutBasedOnAspect : MonoBehaviour
{
	public GridLayoutGroup _layoutGroup;

	[SerializeField]
	public AnimationCurve curve;

	[SerializeField]
	public bool setSpacing;

	[SerializeField]
	[ShowIf("setSpacing")]
	public Vector2 minSpacing;

	[SerializeField]
	[ShowIf("setSpacing")]
	public Vector2 maxSpacing;

	[SerializeField]
	public bool autoLinkToParent;

	public GridLayoutGroup layoutGroup => _layoutGroup ?? (_layoutGroup = GetComponent<GridLayoutGroup>());

	public void Awake()
	{
		if (autoLinkToParent)
		{
			WorldSpaceCanvasFitScreen componentInParent = base.transform.GetComponentInParent<WorldSpaceCanvasFitScreen>();
			if ((bool)componentInParent)
			{
				componentInParent.onUpdate.AddListener(UpdateLayout);
				UpdateLayout(componentInParent.rectTransform);
			}
		}
	}

	public void UpdateLayout(RectTransform canvas)
	{
		Vector2 sizeDelta = canvas.sizeDelta;
		float time = sizeDelta.x / sizeDelta.y;
		float num = curve.Evaluate(time);
		if (setSpacing)
		{
			layoutGroup.spacing = minSpacing + (maxSpacing - minSpacing) * num;
		}
	}
}
