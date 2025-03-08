#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.Localization;

public class CardUnlockSequence : MonoBehaviour
{
	[SerializeField]
	public Transform cardHolder;

	[SerializeField]
	public float duration = 2f;

	[SerializeField]
	public string waitForPress = "Select";

	[SerializeField]
	public ParticleSystem burstParticleSystem;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString titleKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString promptKey;

	public IEnumerator Run(Entity card, CardContainer endContainer)
	{
		Events.InvokeEntityShowUnlocked(card);
		base.gameObject.SetActive(value: true);
		CinemaBarSystem.In();
		CinemaBarSystem.Top.SetPrompt(titleKey.GetLocalizedString(), "");
		CinemaBarSystem.Bottom.SetPrompt(promptKey.GetLocalizedString(), "Select");
		CinemaBarSystem.SetSortingLayer("UI2");
		Transform obj = card.transform;
		obj.SetParent(cardHolder);
		obj.localPosition = Vector3.zero;
		burstParticleSystem.Play();
		obj.localScale = new Vector3(2f, 0f, 1f);
		LeanTween.scale(obj.gameObject, Vector3.one, 1.5f).setEase(LeanTweenType.easeOutElastic);
		yield return new WaitForSeconds(duration);
		if (!waitForPress.IsNullOrWhitespace())
		{
			yield return new WaitUntil(() => InputSystem.IsButtonPressed(waitForPress));
		}

		CinemaBarSystem.Clear();
		CinemaBarSystem.Out();
		if (endContainer != null)
		{
			endContainer.Add(card);
			endContainer.TweenChildPositions();
		}

		base.gameObject.SetActive(value: false);
	}
}
