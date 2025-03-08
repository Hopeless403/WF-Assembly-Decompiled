#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class Shadow : MonoBehaviourCacheTransform
{
	[SerializeField]
	public Graphic graphic;

	[SerializeField]
	public Vector2 alphaRange;

	public Transform target;

	public CanvasGroup canvasGroup;

	public float preAlpha = -1f;

	public void Assign(Entity entity)
	{
		target = entity.offset.transform;
		if (entity.display is Card card)
		{
			CanvasGroup canvasGroup = card.canvasGroup;
			if ((object)canvasGroup != null)
			{
				this.canvasGroup = canvasGroup;
			}
		}
	}

	public void UpdateAlpha()
	{
		if (Mathf.Abs(canvasGroup.alpha - preAlpha) > 0.01f)
		{
			graphic.color = graphic.color.WithAlpha(Mathf.Lerp(alphaRange.x, alphaRange.y, canvasGroup.alpha));
		}

		preAlpha = canvasGroup.alpha;
	}
}
