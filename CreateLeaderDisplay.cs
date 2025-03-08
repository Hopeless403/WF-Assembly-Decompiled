#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class CreateLeaderDisplay : MonoBehaviour
{
	[SerializeField]
	public TweenUI startTween;

	[SerializeField]
	public float cardScale = 1f;

	public IEnumerator Start()
	{
		Character player = References.Player;
		Card card = CardManager.Get(References.LeaderData, null, player, inPlay: false, isPlayerCard: true);
		card.entity.returnToPool = false;
		Transform obj = card.transform;
		obj.localScale = Vector3.one * cardScale;
		card.entity.flipper.FlipUpInstant();
		card.hover.Disable();
		card.entity.uINavigationItem.enabled = false;
		obj.SetParent(base.transform);
		obj.localPosition = Vector3.zero;
		obj.localRotation = Quaternion.identity;
		card.entity.wobbler.WobbleRandom();
		yield return card.UpdateData();
		if ((bool)startTween)
		{
			startTween.Fire();
		}
	}
}
