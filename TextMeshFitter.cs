#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TextMeshFitter : MonoBehaviour
{
	public TMP_Text _textElement;

	public RectTransform _rectTransform;

	[SerializeField]
	public SpriteRenderer parentSprite;

	[SerializeField]
	public bool autoUpdate = true;

	[SerializeField]
	public Vector2 minSize;

	public bool dirty = true;

	public string text
	{
		get
		{
			return textElement.text;
		}
		set
		{
			textElement.text = value;
			dirty = true;
		}
	}

	public TMP_Text textElement => _textElement ?? (_textElement = GetComponent<TMP_Text>());

	public RectTransform rectTransform => _rectTransform ?? (_rectTransform = base.transform as RectTransform);

	public void Update()
	{
		if (dirty || (autoUpdate && textElement.havePropertiesChanged))
		{
			Run();
			dirty = false;
		}
	}

	public void Run()
	{
		TMP_LineInfo[] lineInfo = textElement.textInfo.lineInfo;
		float num = 0f;
		float num2 = 0f;
		TMP_LineInfo[] array = lineInfo;
		for (int i = 0; i < array.Length; i++)
		{
			TMP_LineInfo tMP_LineInfo = array[i];
			if (tMP_LineInfo.characterCount > 0)
			{
				num = Mathf.Max(num, tMP_LineInfo.length);
				num2 += tMP_LineInfo.lineHeight;
			}
		}

		Vector4 margin = textElement.margin;
		num += margin.x + margin.z;
		num2 += margin.y + margin.w;
		num = Mathf.Max(minSize.x, num);
		num2 = Mathf.Max(minSize.y, num2);
		if (parentSprite != null)
		{
			Vector3 localScale = rectTransform.localScale;
			parentSprite.size = new Vector2(num * localScale.x, num2 * localScale.y);
		}

		rectTransform.sizeDelta = new Vector2(num, num2);
	}

	[Button("Force Update", EButtonEnableMode.Always)]
	public void ForceUpdate()
	{
		Run();
	}
}
