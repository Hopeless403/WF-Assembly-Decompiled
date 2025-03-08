#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class DiscardDisplaySystem : GameSystem
{
	[SerializeField]
	public Transform display;

	[SerializeField]
	public TweenUI showTween;

	[SerializeField]
	public TweenUI hideTween;

	public Entity entityDrag;

	public CardContainer hoverDiscardContainer;

	public bool draggingCanDiscard;

	public void OnEnable()
	{
		Events.OnEntityDrag += EntityDrag;
		Events.OnEntityRelease += EntityRelease;
		Events.OnContainerHover += ContainerHover;
		Events.OnContainerUnHover += ContainerUnHover;
	}

	public void OnDisable()
	{
		Events.OnEntityDrag -= EntityDrag;
		Events.OnEntityRelease -= EntityRelease;
		Events.OnContainerHover -= ContainerHover;
		Events.OnContainerUnHover -= ContainerUnHover;
	}

	public void EntityDrag(Entity entity)
	{
		entityDrag = entity;
		draggingCanDiscard = entity.CanRecall();
	}

	public void EntityRelease(Entity entity)
	{
		if (entityDrag == entity)
		{
			entityDrag = null;
			Hide();
		}
	}

	public void ContainerHover(CardContainer container)
	{
		if (entityDrag != null && draggingCanDiscard && container != null && entityDrag.owner != null && container == entityDrag.owner.discardContainer)
		{
			hoverDiscardContainer = container;
			display.position = container.transform.position;
			Show();
		}
	}

	public void ContainerUnHover(CardContainer container)
	{
		if (hoverDiscardContainer == container)
		{
			hoverDiscardContainer = null;
			Hide();
		}
	}

	public void Show()
	{
		display.gameObject.SetActive(value: true);
		showTween?.Fire();
	}

	public void Hide()
	{
		StartCoroutine(HideRoutine());
	}

	public IEnumerator HideRoutine()
	{
		if (hideTween != null)
		{
			hideTween.Fire();
			yield return Sequences.Wait(hideTween.GetDuration());
		}

		display.gameObject.SetActive(value: false);
	}
}
