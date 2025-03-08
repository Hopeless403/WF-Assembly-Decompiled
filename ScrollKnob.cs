#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class ScrollKnob : MonoBehaviourRect
{
	[SerializeField]
	public RectTransform bounds;

	[SerializeField]
	public bool horizontal;

	public Vector2 targetPosition;

	public void Update()
	{
		base.rectTransform.anchoredPosition = Delta.Lerp(base.rectTransform.anchoredPosition, targetPosition, 0.33f, Time.deltaTime);
	}

	public void SetPosition(Vector2 position)
	{
		SetPosition(GetRelevantAxis(position));
	}

	public void SetPosition(float normalizedPosition)
	{
		float relevantAxis = GetRelevantAxis(bounds.rect.size);
		float num = (0f - relevantAxis) * 0.5f + relevantAxis * Mathf.Clamp(normalizedPosition, 0f, 1f);
		if (horizontal)
		{
			targetPosition.x = num;
		}
		else
		{
			targetPosition.y = num;
		}
	}

	public float GetRelevantAxis(Vector2 vector2)
	{
		if (!horizontal)
		{
			return vector2.y;
		}

		return vector2.x;
	}
}
