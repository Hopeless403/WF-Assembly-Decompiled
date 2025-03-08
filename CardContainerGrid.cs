#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

public class CardContainerGrid : CardContainer
{
	[SerializeField]
	public bool fixedWidth;

	[SerializeField]
	public float minHeight;

	[SerializeField]
	public Vector2 cellSize = new Vector2(2.25f, 3.375f);

	[SerializeField]
	public Vector2 spacing = new Vector2(0.5f, 0.5f);

	[SerializeField]
	public int columnCount = 5;

	[SerializeField]
	public TextAlignment align = TextAlignment.Center;

	[Header("Sort Cards by Type")]
	[SerializeField]
	public bool sort;

	[Header("A lil randomness to card position/rotation")]
	[SerializeField]
	public Vector3 randomOffset;

	[SerializeField]
	public Vector3 randomRotation;

	public override float CardScale => cellSize.x / 3f;

	public new void DestroyAll()
	{
		foreach (Entity entity in entities)
		{
			CardManager.ReturnToPool(entity);
		}

		entities.Clear();
		Count = 0;
	}

	public override void CardAdded(Entity entity)
	{
		base.CardAdded(entity);
		SetSize();
		Sort();
	}

	public override void CardRemoved(Entity entity)
	{
		base.CardRemoved(entity);
		SetSize();
		Sort();
	}

	public override Vector3 GetChildPosition(Entity child)
	{
		int num = IndexOf(child);
		int num2 = num % columnCount;
		int num3 = Mathf.FloorToInt(num / columnCount);
		int num4 = RowCount(num3);
		float num5 = (float)num4 * cellSize.x + (float)(num4 - 1) * spacing.x;
		Vector2 sizeDelta = base.rectTransform.sizeDelta;
		Vector2 vector = new Vector2(0f - sizeDelta.x, sizeDelta.y) * 0.5f;
		switch (align)
		{
			case TextAlignment.Center:
				vector.x = (0f - num5) * 0.5f;
				break;
			case TextAlignment.Right:
				vector.x = sizeDelta.x * 0.5f - num5;
				break;
		}

		vector.x += cellSize.x * 0.5f + spacing.x;
		vector.y -= cellSize.y * 0.5f + spacing.y;
		Vector2 vector2 = vector;
		vector2.x += (float)num2 * cellSize.x + (float)(num2 - 1) * spacing.x;
		vector2.y -= (float)num3 * cellSize.y + (float)(num3 - 1) * spacing.y;
		return (Vector3)vector2 + Vector3.Scale(child.random3, randomOffset);
	}

	public int RowCount(int rowIndex)
	{
		return Mathf.Clamp(Count - rowIndex * columnCount, 0, columnCount);
	}

	public override Vector3 GetChildRotation(Entity child)
	{
		return Vector3.Scale(child.random3, randomRotation);
	}

	public void SetSize()
	{
		int num = GetColumnCount();
		int rowCount = GetRowCount();
		float num2 = (fixedWidth ? base.rectTransform.sizeDelta.x : ((float)num * cellSize.x + (float)(num - 1) * spacing.x));
		float num3 = Mathf.Max(minHeight, (float)rowCount * cellSize.y + (float)(rowCount - 1) * spacing.y);
		LayoutElement component = GetComponent<LayoutElement>();
		if ((object)component != null)
		{
			component.preferredWidth = num2;
			component.preferredHeight = num3;
		}
		else
		{
			base.rectTransform.sizeDelta = new Vector2(num2, num3);
		}
	}

	public void Sort()
	{
		if (sort)
		{
			entities.Sort((Entity a, Entity b) => a.data.cardType.sortPriority.CompareTo(b.data.cardType.sortPriority));
		}
	}

	public int GetColumnCount()
	{
		return Mathf.Min(columnCount, Count);
	}

	public int GetRowCount()
	{
		return Mathf.CeilToInt((float)Count / (float)columnCount);
	}
}
