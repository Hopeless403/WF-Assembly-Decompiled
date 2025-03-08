#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class MapNodeLabel : MonoBehaviour
{
	[SerializeField]
	public SpriteRenderer spriteRenderer;

	[SerializeField]
	public TextMeshFitter textFitter;

	public Vector3 startPos;

	public Color startColor;

	public void Awake()
	{
		startPos = base.transform.localPosition;
		startColor = spriteRenderer.color;
	}

	public void Show()
	{
		base.gameObject.SetActive(value: true);
		LeanTween.cancel(base.gameObject);
		LeanTween.cancel(spriteRenderer.gameObject);
		base.transform.localPosition = startPos + new Vector3(0f, -0.15f, 0f);
		LeanTween.moveLocal(base.gameObject, startPos, 1f).setEase(LeanTweenType.easeOutElastic);
		spriteRenderer.color = startColor.With(-1f, -1f, -1f, 0f);
		LeanTween.color(spriteRenderer.gameObject, startColor, 0.25f).setEase(LeanTweenType.easeOutQuart);
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}
}
