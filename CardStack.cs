#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class CardStack : CardContainer
{
	[SerializeField]
	public Vector3 randomAngleAmount = new Vector3(0f, 0f, 2f);

	public bool insertAtBottom;

	public bool flipOnAdd = true;

	public override void SetSize(int size, float cardScale)
	{
		base.SetSize(size, cardScale);
		holder.sizeDelta = GameManager.CARD_SIZE * cardScale;
	}

	public override Vector3 GetChildPosition(Entity child)
	{
		int num = IndexOf(child);
		float num2 = 0f;
		float num3 = 0f;
		float x = 0f + gap.x * (float)num;
		float y = num2 + gap.y * (float)num;
		float z = num3 + gap.z * (float)num;
		return new Vector3(x, y, z);
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
		return Vector3.Scale(child.random3, randomAngleAmount);
	}

	public override void CardAdded(Entity entity)
	{
		base.CardAdded(entity);
		entity.enabled = false;
		if (flipOnAdd && (bool)entity.flipper)
		{
			entity.flipper.FlipDown();
		}
	}
}
