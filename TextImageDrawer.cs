#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextImageDrawer : TMP_Text
{
	[SerializeField]
	public TextImageData data;

	public Image[] images;

	public Transform _transform;

	public new Transform transform => _transform ?? (_transform = GetComponent<Transform>());

	public override string text
	{
		get
		{
			return m_text;
		}
		set
		{
			if (m_text != value)
			{
				m_text = value;
				DrawText();
			}
		}
	}

	public override Color color
	{
		get
		{
			return m_fontColor;
		}
		set
		{
			m_fontColor = value;
			SetColour();
		}
	}

	public override void Awake()
	{
		base.Awake();
		DrawText();
	}

	public void DrawText()
	{
		Clear();
		images = new Image[m_text.Length];
		for (int i = 0; i < m_text.Length; i++)
		{
			char value = m_text[i];
			if (data.TryGetSprite(value, out var result))
			{
				GameObject obj = new GameObject(value.ToString());
				Image image = obj.AddComponent<Image>();
				image.sprite = result;
				image.color = m_fontColor;
				image.material = material;
				image.raycastTarget = raycastTarget;
				image.maskable = base.maskable;
				image.preserveAspect = true;
				images[i] = image;
				Transform transform = obj.transform;
				if ((object)transform != null)
				{
					transform.SetParent(this.transform);
					transform.localScale = Vector3.one;
					transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
				}
			}
		}
	}

	public void SetColour()
	{
		Image[] array = images;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].color = m_fontColor;
		}
	}

	public void Clear()
	{
		Image[] array = images;
		if (array == null || array.Length <= 0)
		{
			return;
		}

		array = images;
		foreach (Image image in array)
		{
			if ((bool)image && (bool)image.gameObject)
			{
				Object.Destroy(image.gameObject);
			}
		}
	}
}
