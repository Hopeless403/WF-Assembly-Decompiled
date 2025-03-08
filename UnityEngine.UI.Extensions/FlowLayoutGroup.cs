using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	[AddComponentMenu("Layout/Extensions/Flow Layout Group")]
	public class FlowLayoutGroup : LayoutGroup
	{
		public enum Axis
		{
			Horizontal,
			Vertical
		}

		public float SpacingX;

		public float SpacingY;

		public bool ExpandHorizontalSpacing;

		public bool ChildForceExpandWidth;

		public bool ChildForceExpandHeight;

		public bool invertOrder;

		private float _layoutHeight;

		private float _layoutWidth;

		public float maxWidth = 20f;

		public float maxHeight = 10f;

		[SerializeField]
		protected Axis m_StartAxis;

		private readonly IList<RectTransform> _itemList = new List<RectTransform>();

		public Axis startAxis
		{
			get
			{
				return m_StartAxis;
			}
			set
			{
				SetProperty(ref m_StartAxis, value);
			}
		}

		protected bool IsCenterAlign
		{
			get
			{
				if (base.childAlignment != TextAnchor.LowerCenter && base.childAlignment != TextAnchor.MiddleCenter)
				{
					return base.childAlignment == TextAnchor.UpperCenter;
				}
				return true;
			}
		}

		protected bool IsRightAlign
		{
			get
			{
				if (base.childAlignment != TextAnchor.LowerRight && base.childAlignment != TextAnchor.MiddleRight)
				{
					return base.childAlignment == TextAnchor.UpperRight;
				}
				return true;
			}
		}

		protected bool IsMiddleAlign
		{
			get
			{
				if (base.childAlignment != TextAnchor.MiddleLeft && base.childAlignment != TextAnchor.MiddleRight)
				{
					return base.childAlignment == TextAnchor.MiddleCenter;
				}
				return true;
			}
		}

		protected bool IsLowerAlign
		{
			get
			{
				if (base.childAlignment != TextAnchor.LowerLeft && base.childAlignment != TextAnchor.LowerRight)
				{
					return base.childAlignment == TextAnchor.LowerCenter;
				}
				return true;
			}
		}

		public override void CalculateLayoutInputHorizontal()
		{
			if (startAxis == Axis.Horizontal)
			{
				base.CalculateLayoutInputHorizontal();
				float totalMin = GetGreatestMinimumChildWidth() + (float)base.padding.left + (float)base.padding.right;
				SetLayoutInputForAxis(totalMin, -1f, -1f, 0);
			}
			else
			{
				_layoutWidth = SetLayout(0, true);
			}
		}

		public override void SetLayoutHorizontal()
		{
			SetLayout(0, false);
		}

		public override void SetLayoutVertical()
		{
			SetLayout(1, false);
		}

		public override void CalculateLayoutInputVertical()
		{
			if (startAxis == Axis.Horizontal)
			{
				_layoutHeight = SetLayout(1, true);
				return;
			}
			base.CalculateLayoutInputHorizontal();
			float totalMin = GetGreatestMinimumChildHeigth() + (float)base.padding.bottom + (float)base.padding.top;
			SetLayoutInputForAxis(totalMin, -1f, -1f, 1);
		}

		public float SetLayout(int axis, bool layoutInput)
		{
			float num = ((startAxis == Axis.Vertical) ? maxHeight : base.rectTransform.rect.height);
			float num5 = ((startAxis == Axis.Horizontal) ? maxWidth : base.rectTransform.rect.width);
			float a = 0f;
			float num6 = 0f;
			float num7 = 0f;
			float num8 = 0f;
			float num9 = 0f;
			float num10 = 0f;
			float num11 = 0f;
			if (startAxis == Axis.Horizontal)
			{
				num10 = num;
				num11 = num5 - (float)base.padding.left - (float)base.padding.right;
				if (IsLowerAlign)
				{
					num8 = base.padding.bottom;
					num9 = base.padding.top;
				}
				else
				{
					num8 = base.padding.top;
					num9 = base.padding.bottom;
				}
				num6 = SpacingY;
				num7 = SpacingX;
			}
			else if (startAxis == Axis.Vertical)
			{
				num10 = num5;
				num11 = num - (float)base.padding.top - (float)base.padding.bottom;
				if (IsRightAlign)
				{
					num8 = base.padding.right;
					num9 = base.padding.left;
				}
				else
				{
					num8 = base.padding.left;
					num9 = base.padding.right;
				}
				num6 = SpacingX;
				num7 = SpacingY;
			}
			float num12 = 0f;
			float num2 = 0f;
			for (int i = 0; i < base.rectChildren.Count; i++)
			{
				int index = i;
				RectTransform rectTransform = base.rectChildren[index];
				float num3 = 0f;
				float num4 = 0f;
				if (startAxis == Axis.Horizontal)
				{
					if (invertOrder)
					{
						index = (IsLowerAlign ? (base.rectChildren.Count - 1 - i) : i);
					}
					rectTransform = base.rectChildren[index];
					num3 = LayoutUtility.GetPreferredSize(rectTransform, 0);
					num3 = Mathf.Min(num3, num11);
					num4 = LayoutUtility.GetPreferredSize(rectTransform, 1);
					num4 = Mathf.Min(num4, num11);
				}
				else if (startAxis == Axis.Vertical)
				{
					if (invertOrder)
					{
						index = (IsRightAlign ? (base.rectChildren.Count - 1 - i) : i);
					}
					rectTransform = base.rectChildren[index];
					num3 = LayoutUtility.GetPreferredSize(rectTransform, 1);
					num3 = Mathf.Min(num3, num11);
					num4 = LayoutUtility.GetPreferredSize(rectTransform, 0);
					num4 = Mathf.Min(num4, num11);
				}
				if (num12 + num3 > num11)
				{
					num12 -= num7;
					if (!layoutInput)
					{
						if (startAxis == Axis.Horizontal)
						{
							float yOffset = CalculateRowVerticalOffset(num10, num8, num2);
							LayoutRow(_itemList, num12, num2, num11, base.padding.left, yOffset, axis);
						}
						else if (startAxis == Axis.Vertical)
						{
							float xOffset = CalculateColHorizontalOffset(num10, num8, num2);
							LayoutCol(_itemList, num2, num12, num11, xOffset, base.padding.top, axis);
						}
					}
					_itemList.Clear();
					a = Mathf.Max(a, num12);
					num8 += num2;
					num8 += num6;
					num2 = 0f;
					num12 = 0f;
				}
				num12 += num3;
				_itemList.Add(rectTransform);
				if (num4 > num2)
				{
					num2 = num4;
				}
				if (i < base.rectChildren.Count - 1)
				{
					num12 += num7;
				}
			}
			if (!layoutInput)
			{
				if (startAxis == Axis.Horizontal)
				{
					float yOffset2 = CalculateRowVerticalOffset(num, num8, num2);
					num12 -= num7;
					LayoutRow(_itemList, num12, num2, num11 - (ChildForceExpandWidth ? 0f : num7), base.padding.left, yOffset2, axis);
				}
				else if (startAxis == Axis.Vertical)
				{
					float xOffset2 = CalculateColHorizontalOffset(num5, num8, num2);
					num12 -= num7;
					LayoutCol(_itemList, num2, num12, num11 - (ChildForceExpandHeight ? 0f : num7), xOffset2, base.padding.top, axis);
				}
			}
			_itemList.Clear();
			a = Mathf.Max(a, num12);
			num8 += num2;
			num8 += num9;
			if (layoutInput)
			{
				SetLayoutInputForAxis(num8, num8, -1f, axis);
			}
			if (startAxis == Axis.Vertical)
			{
				base.rectTransform.sizeDelta = base.rectTransform.sizeDelta.WithY(a);
			}
			else
			{
				base.rectTransform.sizeDelta = base.rectTransform.sizeDelta.WithX(a);
			}
			return num8;
		}

		private float CalculateRowVerticalOffset(float groupHeight, float yOffset, float currentRowHeight)
		{
			if (IsLowerAlign)
			{
				return groupHeight - yOffset - currentRowHeight;
			}
			if (IsMiddleAlign)
			{
				return groupHeight * 0.5f - _layoutHeight * 0.5f + yOffset;
			}
			return yOffset;
		}

		private float CalculateColHorizontalOffset(float groupWidth, float xOffset, float currentColWidth)
		{
			if (IsRightAlign)
			{
				return groupWidth - xOffset - currentColWidth;
			}
			if (IsCenterAlign)
			{
				return groupWidth * 0.5f - _layoutWidth * 0.5f + xOffset;
			}
			return xOffset;
		}

		protected void LayoutRow(IList<RectTransform> contents, float rowWidth, float rowHeight, float maxWidth, float xOffset, float yOffset, int axis)
		{
			float num = xOffset;
			if (!ChildForceExpandWidth && IsCenterAlign)
			{
				num += (maxWidth - rowWidth) * 0.5f;
			}
			else if (!ChildForceExpandWidth && IsRightAlign)
			{
				num += maxWidth - rowWidth;
			}
			float num2 = 0f;
			float num3 = 0f;
			if (ChildForceExpandWidth)
			{
				num2 = (maxWidth - rowWidth) / (float)_itemList.Count;
			}
			else if (ExpandHorizontalSpacing)
			{
				num3 = (maxWidth - rowWidth) / (float)(_itemList.Count - 1);
				if (_itemList.Count > 1)
				{
					if (IsCenterAlign)
					{
						num -= num3 * 0.5f * (float)(_itemList.Count - 1);
					}
					else if (IsRightAlign)
					{
						num -= num3 * (float)(_itemList.Count - 1);
					}
				}
			}
			for (int i = 0; i < _itemList.Count; i++)
			{
				int index = (IsLowerAlign ? (_itemList.Count - 1 - i) : i);
				RectTransform rect = _itemList[index];
				float a = LayoutUtility.GetPreferredSize(rect, 0) + num2;
				float num4 = LayoutUtility.GetPreferredSize(rect, 1);
				if (ChildForceExpandHeight)
				{
					num4 = rowHeight;
				}
				a = Mathf.Min(a, maxWidth);
				float num5 = yOffset;
				if (IsMiddleAlign)
				{
					num5 += (rowHeight - num4) * 0.5f;
				}
				else if (IsLowerAlign)
				{
					num5 += rowHeight - num4;
				}
				if (ExpandHorizontalSpacing && i > 0)
				{
					num += num3;
				}
				if (axis == 0)
				{
					SetChildAlongAxis(rect, 0, num, a);
				}
				else
				{
					SetChildAlongAxis(rect, 1, num5, num4);
				}
				if (i < _itemList.Count - 1)
				{
					num += a + SpacingX;
				}
			}
		}

		protected void LayoutCol(IList<RectTransform> contents, float colWidth, float colHeight, float maxHeight, float xOffset, float yOffset, int axis)
		{
			float num = yOffset;
			if (!ChildForceExpandHeight && IsMiddleAlign)
			{
				num += (maxHeight - colHeight) * 0.5f;
			}
			else if (!ChildForceExpandHeight && IsLowerAlign)
			{
				num += maxHeight - colHeight;
			}
			float num2 = 0f;
			float num3 = 0f;
			if (ChildForceExpandHeight)
			{
				num2 = (maxHeight - colHeight) / (float)_itemList.Count;
			}
			else if (ExpandHorizontalSpacing)
			{
				num3 = (maxHeight - colHeight) / (float)(_itemList.Count - 1);
				if (_itemList.Count > 1)
				{
					if (IsMiddleAlign)
					{
						num -= num3 * 0.5f * (float)(_itemList.Count - 1);
					}
					else if (IsLowerAlign)
					{
						num -= num3 * (float)(_itemList.Count - 1);
					}
				}
			}
			for (int i = 0; i < _itemList.Count; i++)
			{
				int index = (IsRightAlign ? (_itemList.Count - 1 - i) : i);
				RectTransform rect = _itemList[index];
				float num4 = LayoutUtility.GetPreferredSize(rect, 0);
				float a = LayoutUtility.GetPreferredSize(rect, 1) + num2;
				if (ChildForceExpandWidth)
				{
					num4 = colWidth;
				}
				a = Mathf.Min(a, maxHeight);
				float num5 = xOffset;
				if (IsCenterAlign)
				{
					num5 += (colWidth - num4) * 0.5f;
				}
				else if (IsRightAlign)
				{
					num5 += colWidth - num4;
				}
				if (ExpandHorizontalSpacing && i > 0)
				{
					num += num3;
				}
				if (axis == 0)
				{
					SetChildAlongAxis(rect, 0, num5, num4);
				}
				else
				{
					SetChildAlongAxis(rect, 1, num, a);
				}
				if (i < _itemList.Count - 1)
				{
					num += a + SpacingY;
				}
			}
		}

		public float GetGreatestMinimumChildWidth()
		{
			float num = 0f;
			for (int i = 0; i < base.rectChildren.Count; i++)
			{
				num = Mathf.Max(LayoutUtility.GetMinWidth(base.rectChildren[i]), num);
			}
			return num;
		}

		public float GetGreatestMinimumChildHeigth()
		{
			float num = 0f;
			for (int i = 0; i < base.rectChildren.Count; i++)
			{
				num = Mathf.Max(LayoutUtility.GetMinHeight(base.rectChildren[i]), num);
			}
			return num;
		}
	}
}
