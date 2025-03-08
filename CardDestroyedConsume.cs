#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardDestroyedConsume : MonoBehaviour, ICardDestroyed, IRemoveWhenPooled
{
	public string sortingLayer = "Default";

	public int sortingOrder = 10;

	public IEnumerator Start()
	{
		Entity entity = GetComponent<Entity>();
		AsyncOperationHandle<GameObject> handle = AddressableLoader.InstantiateAsync("CardBreakFX", entity.offset.position, Quaternion.identity, base.transform);
		SfxSystem.OneShot("event:/sfx/card/consume");
		yield return handle;
		LeanTween.scale(base.gameObject, Vector3.one * 0.6f, 0.25f).setEase(LeanTweenType.easeInBack);
		yield return new WaitForSeconds(0.25f);
		LeanTween.scale(base.gameObject, Vector3.one * 1.2f, 0.25f).setEase(LeanTweenType.easeInBack);
		yield return new WaitForSeconds(0.15f);
		ParticleSystem ps = handle.Result.GetComponent<ParticleSystem>();
		if ((object)ps != null)
		{
			ps.Play();
			if (entity.display is Card card)
			{
				CanvasGroup canvasGroup = card.canvasGroup;
				if ((object)canvasGroup != null)
				{
					LeanTween.alphaCanvas(canvasGroup, 0f, 0.1f);
				}
			}

			ParticleSystemRenderer[] componentsInChildren = ps.GetComponentsInChildren<ParticleSystemRenderer>();
			foreach (ParticleSystemRenderer obj in componentsInChildren)
			{
				obj.sortingLayerName = sortingLayer;
				obj.sortingOrder = sortingOrder;
			}

			yield return new WaitUntil(() => !ps || !ps.isPlaying);
		}

		Final();
	}

	public void Final()
	{
		Object.Destroy(this);
		CardManager.ReturnToPool(base.gameObject.GetComponent<Card>());
	}
}
