#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AssignCharmSequence : UISequence
{
	[SerializeField]
	public Image background;

	[SerializeField]
	public float backgroundAlpha = 0.67f;

	[SerializeField]
	public Transform cardHolder;

	[SerializeField]
	public float cardScale = 1f;

	[SerializeField]
	public ParticleSystem glows;

	[SerializeField]
	public ParticleSystem ding;

	[SerializeField]
	public UnityEngine.Animator animator;

	public Entity target;

	public CardUpgradeData upgradeData;

	public float fade;

	public static float fadeInDur = 0.5f;

	public static float fadeOutDur = 0.25f;

	public Transform previousParent;

	public int previousChildIndex;

	public Vector3 previousPosition;

	public void Assign(Entity target, CardUpgradeData upgradeData)
	{
		this.target = target;
		this.upgradeData = upgradeData;
	}

	public void Focus()
	{
		target.ForceUnHover();
		previousParent = target.transform.parent;
		previousChildIndex = target.transform.GetSiblingIndex();
		previousPosition = target.transform.localPosition;
		target.transform.SetParent(cardHolder, worldPositionStays: true);
		LeanTween.moveLocal(target.gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeOutQuart);
		target.wobbler?.WobbleRandom();
		LeanTween.scale(target.gameObject, Vector3.one * cardScale, 0.67f).setEase(LeanTweenType.easeOutBack);
		LeanTween.rotateLocal(target.gameObject, Vector3.zero, 1f).setEase(LeanTweenType.easeOutBack);
	}

	public void Unfocus()
	{
		if ((bool)target && target.StillExists())
		{
			target.transform.parent = previousParent;
			target.transform.SetSiblingIndex(previousChildIndex);
			target.TweenToContainer();
			target.wobbler?.WobbleRandom();
		}
	}

	public override IEnumerator Run()
	{
		base.gameObject.SetActive(value: true);
		yield return Sequences.Wait(startDelay);
		BackgroundFade(backgroundAlpha, fadeInDur);
		Focus();
		animator.SetTrigger("Assign");
		SfxSystem.OneShot("event:/sfx/inventory/charm_assign");
		yield return new WaitForSeconds(3f);
		BackgroundFade(0f, fadeOutDur);
		yield return new WaitForSeconds(fadeOutDur * 0.5f);
		Unfocus();
		yield return new WaitForSeconds(fadeOutDur * 0.5f);
		base.gameObject.SetActive(value: false);
	}

	public void BackgroundFade(float alpha, float dur)
	{
		LeanTween.cancel(background.gameObject);
		LeanTween.value(background.gameObject, fade, alpha, dur).setEase(LeanTweenType.easeOutQuad).setOnUpdate(delegate(float a)
		{
			fade = a;
			background.color = background.color.WithAlpha(a);
		});
	}

	public void Rumble()
	{
		Events.InvokeScreenRumble(0f, 1f, 0f, 1f, 0.5f, 0.25f);
	}

	public void AssignCharm()
	{
		new Routine(upgradeData.Assign(target));
	}

	public void StartGlow()
	{
		glows.Play();
	}

	public void Ding()
	{
		ding.Play();
	}
}
