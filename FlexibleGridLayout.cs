#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
	public enum FitType
	{
		Uniform,
		Width,
		Height,
		FixedRows,
		FixedColumns
	}

	public FitType fitType;

	public int rows;

	public int columns;

	public Vector2 cellSize;

	public Vector2 spacing;

	public bool autoSizeX;

	public bool autoSizeY;

	public override void CalculateLayoutInputHorizontal()
	{
		base.CalculateLayoutInputHorizontal();
		autoSizeX = false;
		autoSizeY = false;
		if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
		{
			autoSizeX = true;
			autoSizeY = true;
			float f = Mathf.Sqrt(base.transform.childCount);
			rows = Mathf.CeilToInt(f);
			columns = Mathf.CeilToInt(f);
		}

		switch (fitType)
		{
			case FitType.Width:
			case FitType.FixedColumns:
				rows = Mathf.CeilToInt(base.transform.childCount / columns);
				break;
			case FitType.Height:
			case FitType.FixedRows:
				columns = Mathf.CeilToInt(base.transform.childCount / rows);
				break;
		}

		float width = base.rectTransform.rect.width;
		float height = base.rectTransform.rect.height;
		if (autoSizeX)
		{
			cellSize.x = width / (float)columns - spacing.x / (float)columns * (float)(columns - 1) - (float)(base.padding.left / columns) - (float)(base.padding.right / columns);
		}

		if (autoSizeY)
		{
			cellSize.y = height / (float)rows - spacing.y / (float)rows * (float)(columns - 1) - (float)(base.padding.top / rows) - (float)(base.padding.bottom / rows);
		}

		for (int i = 0; i < base.rectChildren.Count; i++)
		{
			int num = i / columns;
			int num2 = i % columns;
			RectTransform rect = base.rectChildren[i];
			float pos = cellSize.x * (float)num2 + spacing.x * (float)num2 + (float)base.padding.left;
			float pos2 = cellSize.y * (float)num + spacing.y * (float)num + (float)base.padding.top;
			SetChildAlongAxis(rect, 0, pos, cellSize.x);
			SetChildAlongAxis(rect, 1, pos2, cellSize.y);
		}
	}

	public override void CalculateLayoutInputVertical()
	{
	}

	public override void SetLayoutHorizontal()
	{
	}

	public override void SetLayoutVertical()
	{
	}
}
