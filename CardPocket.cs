#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public class CardPocket : CardContainer
{
	public class PositionHandler
	{
		public readonly CardPocket pocket;

		public readonly Entity entity;

		public readonly Vector3 startPosition;

		public readonly Vector3 positionChange;

		public readonly AnimationCurve curve;

		public readonly float duration;

		public float delay;

		public float t;

		public bool IsFinished => t >= duration;

		public PositionHandler(CardPocket pocket, Entity entity, Vector3 fromPosition, Vector3 toPosition, AnimationCurve slideCurve, float slideDuration, float slideDelay)
		{
			this.pocket = pocket;
			this.entity = entity;
			startPosition = fromPosition;
			positionChange = toPosition - fromPosition;
			curve = slideCurve;
			duration = slideDuration;
			delay = slideDelay;
			t = 0f;
		}

		public Vector3 GetPosition()
		{
			float num = curve.Evaluate(t / duration);
			return startPosition + positionChange * num;
		}

		public void Skip()
		{
			t = duration;
			Update(0f);
		}

		public void Update(float delta)
		{
			if (delay > 0f)
			{
				delay -= delta;
				if (delay <= 0f)
				{
					Events.InvokeEntityEnterPocket(entity, pocket);
				}
			}
			else
			{
				t += delta;
				entity.transform.localPosition = GetPosition();
			}
		}
	}

	[SerializeField]
	public Transform slideInPosition;

	[SerializeField]
	public AnimationCurve slideCurve;

	[SerializeField]
	public float slideDuration = 0.5f;

	[SerializeField]
	public float slideDelay = 0.5f;

	[SerializeField]
	public Vector3 randomAngleAmount = new Vector3(0f, 0f, 1f);

	[SerializeField]
	public CardPocketInteraction interaction;

	[SerializeField]
	public CardContainer[] skipAnimationFromContainers;

	public readonly Dictionary<Entity, PositionHandler> positions = new Dictionary<Entity, PositionHandler>();

	public override void AssignController(CardController controller)
	{
		base.AssignController(controller);
		GetComponentInChildren<ToggleBasedOnCardController>(includeInactive: true)?.AssignCardController(controller);
	}

	public void Update()
	{
		foreach (KeyValuePair<Entity, PositionHandler> position in positions)
		{
			if (!position.Value.IsFinished)
			{
				position.Value.Update(Time.deltaTime);
			}
		}
	}

	public override void SetSize(int size, float cardScale)
	{
		base.SetSize(size, cardScale);
		holder.sizeDelta = GameManager.CARD_SIZE * cardScale;
	}

	public override Vector3 GetChildPosition(Entity child)
	{
		return positions[child].GetPosition();
	}

	public override Vector3 GetChildRotation(Entity child)
	{
		return Vector3.Scale(child.random3, randomAngleAmount);
	}

	public Vector3 GetFinalChildPosition(Entity child)
	{
		int num = IndexOf(child);
		float num2 = 0f;
		float num3 = 0f;
		float x = 0f + gap.x * (float)num;
		float y = num2 + gap.y * (float)num;
		float z = num3 + gap.z * (float)num;
		return new Vector3(x, y, z);
	}

	public override void Hover()
	{
		base.Hover();
		if ((bool)interaction)
		{
			interaction.Hover();
		}
	}

	public override void UnHover()
	{
		base.UnHover();
		if ((bool)interaction)
		{
			interaction.UnHover();
		}
	}

	public override void CardAdded(Entity entity)
	{
		base.CardAdded(entity);
		entity.enabled = false;
		if ((bool)entity.uINavigationItem)
		{
			entity.uINavigationItem.isSelectable = false;
			entity.uINavigationItem.enabled = false;
		}

		if ((bool)entity.flipper)
		{
			entity.flipper.FlipDown();
		}

		PositionHandler positionHandler = new PositionHandler(this, entity, slideInPosition.localPosition, GetFinalChildPosition(entity), slideCurve, slideDuration, slideDelay);
		positions.Add(entity, positionHandler);
		if (entity.preContainers == null || entity.preContainers.Length == 0)
		{
			positionHandler.Skip();
			return;
		}

		CardContainer[] preContainers = entity.preContainers;
		foreach (CardContainer item in preContainers)
		{
			if (skipAnimationFromContainers.Contains(item))
			{
				positionHandler.Skip();
				break;
			}
		}
	}

	public override void CardRemoved(Entity entity)
	{
		base.CardRemoved(entity);
		if ((bool)entity.uINavigationItem)
		{
			entity.uINavigationItem.isSelectable = true;
			entity.uINavigationItem.enabled = true;
		}

		positions.Remove(entity);
	}
}
