#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public class CardIconStack : CardContainer
{
	public Dictionary<Entity, RectTransform> activeIcons = new Dictionary<Entity, RectTransform>();

	public Queue<RectTransform> iconPool = new Queue<RectTransform>();

	public int iconCount;

	[SerializeField]
	public Vector3 randomAngleAmount = new Vector3(0f, 0f, 2f);

	public bool insertAtBottom;

	[SerializeField]
	public RectTransform iconPrefab;

	public override void SetSize(int size, float cardScale)
	{
		base.SetSize(size, cardScale);
		holder.sizeDelta = GameManager.CARD_SIZE * cardScale;
	}

	public override Vector3 GetChildPosition(Entity child)
	{
		IndexOf(child);
		float num = 0f;
		float num2 = 0f;
		float y = num;
		float z = num2;
		return new Vector3(0f, y, z);
	}

	public override void Add(Entity entity)
	{
		if (insertAtBottom)
		{
			entity.transform.SetParent(holder);
			entity.AddTo(this);
			entities.Insert(0, entity);
			entity.transform.SetSiblingIndex(0);
			Count++;
			CardAdded(entity);
			for (int i = 1; i < Count; i++)
			{
				TweenChildPosition(entities[i]);
			}
		}
		else
		{
			base.Add(entity);
		}
	}

	public override Vector3 GetChildRotation(Entity child)
	{
		return Vector3.zero;
	}

	public override void CardAdded(Entity entity)
	{
		base.CardAdded(entity);
		entity.enabled = false;
		if (entity.flipper != null && !entity.flipper.flipped)
		{
			entity.flipper.FlipDown();
		}

		AddIcon(entity);
	}

	public override void CardRemoved(Entity entity)
	{
		RemoveIcon(entity);
	}

	public void AddIcon(Entity entity)
	{
		activeIcons[entity] = GetIcon();
		UpdateIconPositions();
	}

	public void RemoveIcon(Entity entity)
	{
		RectTransform rectTransform = activeIcons[entity];
		if (rectTransform != null)
		{
			PoolIcon(rectTransform);
			activeIcons.Remove(entity);
		}
	}

	public void UpdateIconPositions()
	{
		int count = Count;
		for (int i = 0; i < count; i++)
		{
			Entity entity = this[i];
			RectTransform obj = activeIcons[entity];
			obj.SetSiblingIndex(i);
			obj.localPosition = gap * i;
			obj.localEulerAngles = Vector3.Scale(entity.random3, randomAngleAmount);
		}
	}

	public void PoolIcon(RectTransform icon)
	{
		icon.gameObject.SetActive(value: false);
		iconPool.Enqueue(icon);
	}

	public RectTransform GetIcon()
	{
		RectTransform rectTransform = null;
		if (iconPool.Count > 0)
		{
			rectTransform = iconPool.Dequeue();
		}
		else
		{
			rectTransform = Object.Instantiate(iconPrefab, base.transform);
			rectTransform.name = $"Icon {iconCount++}";
		}

		rectTransform.gameObject.SetActive(value: true);
		return rectTransform;
	}
}
