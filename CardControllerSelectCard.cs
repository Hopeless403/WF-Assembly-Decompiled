#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class CardControllerSelectCard : CardController
{
	[Header("Press Tween")]
	public float cardPressScaleFrom = 0.8f;

	public float cardPressScaleTo = 1f;

	public LeanTweenType cardPressEase = LeanTweenType.easeOutElastic;

	public float cardPressEaseDur = 1f;

	public float cardPressWobble = 1f;

	public UnityEventEntity pressEvent;

	public UnityEventEntity hoverEvent;

	public UnityEventEntity unHoverEvent;

	public override bool AllowDynamicSelectRelease => false;

	public new void OnEnable()
	{
		Events.OnEntityHover += CardHover;
		Events.OnEntityUnHover += CardUnHover;
	}

	public new void OnDisable()
	{
		Events.OnEntityHover -= CardHover;
		Events.OnEntityUnHover -= CardUnHover;
	}

	public override void Press()
	{
		if (canPress && (bool)pressEntity && !pressEntity.inPlay)
		{
			Debug.Log("Pressing [" + pressEntity.name + "]");
			TweenHover(pressEntity);
			if (cardPressEaseDur > 0f)
			{
				LeanTween.scale(pressEntity.offset.gameObject, Vector3.one * cardPressScaleTo, cardPressEaseDur).setFrom(Vector3.one * cardPressScaleFrom).setEase(cardPressEase);
			}

			if (cardPressWobble != 0f)
			{
				pressEntity.wobbler?.WobbleRandom(cardPressWobble);
			}
		}
	}

	public override void Release()
	{
		if ((bool)pressEntity && hoverEntity == pressEntity && !pressEntity.inPlay)
		{
			Debug.Log($"[{this}] PRESSING [{pressEntity}]! :D");
			Entity arg = pressEntity;
			pressEntity = null;
			pressEvent.Invoke(arg);
		}
	}

	public void CardHover(Entity entity)
	{
		hoverEvent.Invoke(entity);
	}

	public void CardUnHover(Entity entity)
	{
		unHoverEvent.Invoke(entity);
	}
}
