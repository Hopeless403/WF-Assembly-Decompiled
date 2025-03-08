#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageDropShadow : MonoBehaviourRect
{
	[SerializeField]
	public Vector2 offset = new Vector2(0.1f, -0.1f);

	[SerializeField]
	public Material shadowMaterial;

	[SerializeField]
	public Color shadowColor;

	public Image caster;

	public Image shadow;

	public RectTransform casterTransform;

	public RectTransform shadowTransform;

	public IEnumerator Start()
	{
		caster = GetComponent<Image>();
		casterTransform = base.rectTransform;
		shadowTransform = new GameObject("ImageDropShadow", typeof(RectTransform)).GetComponent<RectTransform>();
		shadowTransform.SetParent(casterTransform.parent);
		shadowTransform.SetSiblingIndex(casterTransform.GetSiblingIndex());
		yield return null;
		shadowTransform.localScale = Vector3.one;
		shadowTransform.localEulerAngles = Vector3.zero;
		shadow = shadowTransform.gameObject.AddComponent<Image>();
		shadow.raycastTarget = false;
		shadow.material = shadowMaterial;
		shadow.sprite = caster.sprite;
		shadow.color = shadowColor;
	}

	public void LateUpdate()
	{
		shadowTransform.sizeDelta = casterTransform.sizeDelta;
		shadowTransform.anchoredPosition = casterTransform.anchoredPosition + offset;
		shadowTransform.rotation = casterTransform.rotation;
	}
}
