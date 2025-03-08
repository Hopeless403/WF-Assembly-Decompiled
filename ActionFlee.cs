#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using Dead;
using UnityEngine;

public class ActionFlee : PlayAction
{
	public readonly Entity entity;

	public ActionFlee(Entity entity)
	{
		this.entity = entity;
	}

	public override IEnumerator Run()
	{
		if (entity.IsAliveAndExists())
		{
			Debug.Log("[" + entity.name + "] Fleeing!");
			CardContainer[] toContainers = new CardContainer[1] { entity.owner.reserveContainer };
			yield return Sequences.CardMove(entity, toContainers, -1, tweenAll: false);
			CardContainer cardContainer = entity.actualContainers[0];
			Vector3 localPosition = entity.transform.localPosition;
			Vector3 childPosition = cardContainer.GetChildPosition(entity);
			float time = 0.8f;
			LeanTween.moveLocalX(entity.gameObject, childPosition.x, time).setEase(LeanTweenType.linear);
			LeanTween.moveLocalY(entity.gameObject, localPosition.y + 0.5f, 0.2f).setEase(Curves.Get("Jump")).setLoopPingPong(4);
			Events.InvokeEntityFlee(entity);
			yield return new WaitForSeconds(PettyRandom.Range(0.15f, 0.25f));
		}
	}
}
