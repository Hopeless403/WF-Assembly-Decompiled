#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardControllerDeck : CardController
{
	[SerializeField]
	public DeckDisplaySequence deckDisplaySequence;

	public CardContainer deckContainer;

	public CardContainer reserveContainer;

	[SerializeField]
	public DeckSelectSequence selectSequence;

	[SerializeField]
	public ScrollRect scrollRect;

	[SerializeField]
	public ContentSizeFitter contentSizeFitter;

	public override bool AllowDynamicSelectRelease => false;

	public override void Press()
	{
		if ((bool)pressEntity)
		{
			StartCoroutine(OpenMenu(pressEntity));
		}
	}

	public IEnumerator OpenMenu(Entity entity)
	{
		Disable();
		selectSequence.SetEntity(entity);
		if (!References.Battle)
		{
			if (owner.data.inventory.deck.Contains(entity.data))
			{
				if (entity.data.cardType.canReserve)
				{
					selectSequence.AddMoveDown(delegate
					{
						MoveToReserve(entity);
					});
				}
			}
			else if (owner.data.inventory.reserve.Contains(entity.data) && owner.GetCompanionCount() < owner.data.companionLimit)
			{
				selectSequence.AddMoveUp(delegate
				{
					MoveToDeck(entity);
				});
			}
		}

		yield return selectSequence.Run();
		Enable();
	}

	public void MoveToDeck(Entity entity)
	{
		if (!entity.InContainerGroup(deckContainer))
		{
			entity.RemoveFromContainers();
			deckContainer.Add(entity);
		}

		if (!owner.data.inventory.deck.Contains(entity.data))
		{
			owner.data.inventory.reserve.Remove(entity.data);
			owner.data.inventory.deck.Add(entity.data);
		}

		reserveContainer.TweenChildPositions();
		deckContainer.TweenChildPositions();
		StartCoroutine(FixLayoutsRoutine());
	}

	public void MoveToReserve(Entity entity)
	{
		if (!entity.InContainerGroup(reserveContainer))
		{
			entity.RemoveFromContainers();
			reserveContainer.Add(entity);
		}

		if (!owner.data.inventory.reserve.Contains(entity.data))
		{
			owner.data.inventory.deck.Remove(entity.data);
			owner.data.inventory.reserve.Add(entity.data);
		}

		reserveContainer.TweenChildPositions();
		deckContainer.TweenChildPositions();
		StartCoroutine(FixLayoutsRoutine());
	}

	public IEnumerator FixLayoutsRoutine()
	{
		Vector2 scrollPosition = scrollRect.normalizedPosition;
		contentSizeFitter.enabled = false;
		yield return null;
		contentSizeFitter.enabled = true;
		scrollRect.normalizedPosition = scrollPosition;
	}
}
