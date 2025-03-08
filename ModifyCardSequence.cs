#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.Localization;

public class ModifyCardSequence : MonoBehaviour
{
	[SerializeField]
	public CardContainer cardContainer;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString titleStringRef;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString continueStringRef;

	[SerializeField]
	public TweenUI endTween;

	public bool canEnd;

	public bool end;

	public IEnumerator Run(CardData cardToModify, CardScript modifyScript)
	{
		int hp = cardToModify.hp;
		modifyScript.Run(cardToModify);
		int healthLost = hp - cardToModify.hp;
		Events.InvokeScreenRumble(0f, 0.2f, 0f, 0.25f, 0.25f, 0.1f);
		Card card = CardManager.Get(cardToModify, null, References.Player, inPlay: false, isPlayerCard: true);
		card.entity.flipper.FlipDownInstant();
		cardContainer.Add(card.entity);
		yield return card.UpdateData();
		card.transform.localPosition = Vector3.down;
		card.entity.wobbler.WobbleRandom();
		cardContainer.TweenChildPositions();
		CinemaBarSystem.In();
		CinemaBarSystem.Top.SetScript(StringExt.Format(titleStringRef.GetLocalizedString(), cardToModify.title, healthLost));
		CinemaBarSystem.Bottom.SetPrompt(continueStringRef.GetLocalizedString(), "Select");
		yield return new WaitForSeconds(0.2f);
		card.entity.flipper.FlipUp();
		canEnd = true;
		yield return new WaitUntil(() => end);
		CinemaBarSystem.Clear();
		CinemaBarSystem.Out();
		if ((bool)endTween)
		{
			endTween.Fire();
			yield return new WaitForSeconds(endTween.GetDuration());
		}
	}

	public void Continue()
	{
		if (canEnd)
		{
			end = true;
		}
	}
}
