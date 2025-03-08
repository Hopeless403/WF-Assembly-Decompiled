#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Text))]
public class TextFitter : MonoBehaviourRect
{
	public TMP_Text _textElement;

	[SerializeField]
	public LayoutElement layoutElement;

	[SerializeField]
	public RectTransform[] transforms;

	[SerializeField]
	public bool fitWidth = true;

	[SerializeField]
	[ShowIf("fitWidth")]
	public float minWidth = 3f;

	[SerializeField]
	[ShowIf("fitWidth")]
	public float maxWidth = 5f;

	[SerializeField]
	public bool fitHeight = true;

	[SerializeField]
	[ShowIf("fitHeight")]
	public float minHeight = 1f;

	[SerializeField]
	[ShowIf("fitHeight")]
	public float maxHeight = 1f;

	public TMP_Text textElement => _textElement ?? (_textElement = GetComponent<TMP_Text>());

	public void SetText(string text)
	{
		textElement.text = text;
		Fit();
	}

	public void Fit()
	{
		StopAllCoroutines();
		if (fitWidth)
		{
			StartCoroutine(FitRoutine());
		}
	}

	public IEnumerator FitRoutine()
	{
		yield return new WaitForEndOfFrame();
		Vector4 margin = textElement.margin;
		Vector3 size = textElement.textBounds.size;
		Vector2 sizeDelta = base.rectTransform.sizeDelta;
		float num = (fitWidth ? Mathf.Clamp(size.x + margin.x + margin.z, minWidth, maxWidth) : sizeDelta.x);
		float num2 = (fitHeight ? Mathf.Clamp(size.y + margin.y + margin.w, minHeight, maxHeight) : sizeDelta.y);
		Vector2 sizeDelta2 = sizeDelta.With(num, num2);
		base.rectTransform.sizeDelta = sizeDelta2;
		if ((bool)layoutElement)
		{
			if (fitWidth)
			{
				layoutElement.preferredWidth = num;
			}

			if (fitHeight)
			{
				layoutElement.preferredHeight = num2;
			}
		}

		RectTransform[] array = transforms;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].sizeDelta = sizeDelta2;
		}
	}
}
