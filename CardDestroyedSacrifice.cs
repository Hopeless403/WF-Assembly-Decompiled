#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class CardDestroyedSacrifice : MonoBehaviour, ICardDestroyed, IRemoveWhenPooled
{
	public float dur = 0.5f;

	public const LeanTweenType ease = LeanTweenType.easeInBack;

	public Entity entity;

	public void Start()
	{
		entity = GetComponent<Entity>();
		entity.wobbler.WobbleRandom();
		Events.InvokeCameraAnimation("Droop");
		Events.InvokeScreenRumble(0f, 0.25f, 0f, 0.25f * dur, 0.5f * dur, 0.25f * dur);
		LeanTween.scale(base.gameObject, new Vector3(0.25f, 0.25f, 1f), dur).setEase(LeanTweenType.easeInBack).setOnComplete(Final);
		LeanTween.rotateY(base.gameObject, 90.WithRandomSign(), dur).setEase(LeanTweenType.easeInBack);
		if (entity.display is Card card)
		{
			CanvasGroup canvasGroup = card.canvasGroup;
			if ((object)canvasGroup != null)
			{
				LeanTween.alphaCanvas(canvasGroup, 0f, dur).setEase(LeanTweenType.easeInBack);
			}
		}
	}

	public void Final()
	{
		Object.Destroy(this);
		CardManager.ReturnToPool(entity);
	}
}
