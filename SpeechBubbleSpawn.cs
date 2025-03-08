#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleSpawn : MonoBehaviourRect
{
	public float spacing = 0.01f;

	public readonly List<SpeechBub> bubbles = new List<SpeechBub>();

	public void OnEnable()
	{
		Object.FindObjectOfType<SpeechBubbleSystem>()?.AddSpawnPoint(this);
	}

	public void OnDisable()
	{
		Object.FindObjectOfType<SpeechBubbleSystem>()?.RemoveSpawnPoint(this);
	}

	public void Update()
	{
		bool flag = false;
		for (int num = bubbles.Count - 1; num >= 0; num--)
		{
			SpeechBub speechBub = bubbles[num];
			if (!speechBub)
			{
				bubbles.RemoveAt(num);
			}
			else if (!flag)
			{
				flag = speechBub.sizeUpdated;
			}
		}

		if (flag)
		{
			UpdateSize();
			UpdatePositions();
		}
	}

	public SpeechBub Create(SpeechBub prefab, SpeechBubbleData data)
	{
		SpeechBub speechBub = Object.Instantiate(prefab, base.transform);
		Transform obj = speechBub.transform;
		obj.localPosition = Vector3.zero;
		if (obj is RectTransform rectTransform)
		{
			rectTransform.sizeDelta = rectTransform.sizeDelta.WithX(base.rectTransform.sizeDelta.x);
		}

		bubbles.Add(speechBub);
		speechBub.Set(data);
		UpdateSize();
		UpdatePositions();
		return speechBub;
	}

	public void UpdateSize()
	{
		Vector2 sizeDelta = new Vector2(0f, 0f);
		int count = bubbles.Count;
		for (int i = 0; i < count; i++)
		{
			SpeechBub speechBub = bubbles[i];
			if ((bool)speechBub)
			{
				Vector2 sizeDelta2 = speechBub.rectTransform.sizeDelta;
				sizeDelta.x = Mathf.Max(sizeDelta.x, sizeDelta2.x);
				sizeDelta.y += sizeDelta2.y + ((i > 0) ? spacing : 0f);
			}
		}

		if ((bool)base.rectTransform)
		{
			base.rectTransform.sizeDelta = sizeDelta;
		}
	}

	public void UpdatePositions()
	{
		if (!base.rectTransform)
		{
			return;
		}

		Vector2 sizeDelta = base.rectTransform.sizeDelta;
		Vector2 vector = new Vector2((0f - sizeDelta.x) * 0.5f, sizeDelta.y * 0.5f);
		int count = bubbles.Count;
		for (int i = 0; i < count; i++)
		{
			SpeechBub speechBub = bubbles[i];
			if ((bool)speechBub)
			{
				if (i < count - 1 && speechBub.hasTail)
				{
					speechBub.hasTail = false;
				}

				Vector2 sizeDelta2 = speechBub.rectTransform.sizeDelta;
				Vector2 vector2 = vector + new Vector2(sizeDelta2.x * 0.5f, (0f - sizeDelta2.y) * 0.5f);
				speechBub.SetPosition(vector2);
				vector.y -= sizeDelta2.y;
			}
		}
	}
}
