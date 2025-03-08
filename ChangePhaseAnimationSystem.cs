#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePhaseAnimationSystem : GameSystem
{
	public class Target
	{
		public Entity entity;

		public Transform previousParent;

		public Target(Entity entity)
		{
			this.entity = entity;
			previousParent = entity.transform.parent;
		}
	}

	[SerializeField]
	public Transform container;

	[SerializeField]
	public float zoomAmount = 3f;

	[SerializeField]
	public Image fade;

	[SerializeField]
	public Color fadeColor;

	[SerializeField]
	public Color flashColor;

	[SerializeField]
	public Image flash;

	[SerializeField]
	public AnimationCurve flashCurve;

	public float slowmo = 0.1f;

	public float durationIn = 0.3f;

	public float durationOut = 0.9f;

	public readonly List<Target> targets = new List<Target>();

	public void Assign(Entity target)
	{
		targets.Add(new Target(target));
		target.transform.SetParent(container, worldPositionStays: true);
		target.ResetDrawOrder();
	}

	public void ClearFocus()
	{
		foreach (Target target in targets)
		{
			if ((bool)target.entity && (bool)target.entity.transform && (bool)target.previousParent)
			{
				target.entity.transform.SetParent(target.previousParent);
			}
		}

		targets.Clear();
	}

	public void RemoveTarget(Entity entity)
	{
		for (int num = targets.Count - 1; num >= 0; num--)
		{
			if (targets[num].entity == entity)
			{
				targets.RemoveAt(num);
			}
		}
	}

	public void Flash()
	{
		ScreenFlashSystem.SetDrawOrder("ParticlesFront", 0);
		ScreenFlashSystem.SetColour(flashColor);
		ScreenFlashSystem.Run(0.2f);
	}

	public IEnumerator Focus(Entity target)
	{
		Assign(target);
		HitFlashSystem.Remove(target);
		LeanTween.value(base.gameObject, Time.timeScale, slowmo, 0.05f).setEase(LeanTweenType.linear).setOnUpdate(Events.InvokeTimeScaleChange);
		LeanTween.value(base.gameObject, Time.timeScale, 1f, 0.25f).setDelay(durationIn).setEase(LeanTweenType.linear)
			.setOnUpdate(Events.InvokeTimeScaleChange);
		LeanTween.value(base.gameObject, fade.color.a, fadeColor.a, durationIn).setEase(LeanTweenType.easeInOutQuint).setOnUpdate(delegate(float a)
		{
			fade.color = fade.color.With(-1f, -1f, -1f, a);
		});
		Vector3 to = target.transform.position.WithZ(zoomAmount);
		LeanTween.cancel(References.MinibossCameraMover.gameObject);
		LeanTween.move(References.MinibossCameraMover.gameObject, to, Mathf.Min(durationIn, 0.4f)).setEase(LeanTweenType.easeOutBack);
		yield return Sequences.Wait(durationIn);
		LeanTween.value(base.gameObject, Time.timeScale, 1f, 0.25f).setEase(LeanTweenType.linear).setOnUpdate(Events.InvokeTimeScaleChange);
	}

	public IEnumerator UnFocus()
	{
		yield return Sequences.Wait(0.1f);
		LeanTween.moveLocal(References.MinibossCameraMover.gameObject, Vector3.zero, durationOut).setEase(LeanTweenType.easeOutBack);
		LeanTween.value(base.gameObject, fade.color.a, 0f, 0.25f).setEase(LeanTweenType.easeInOutQuart).setOnUpdate(delegate(float a)
		{
			fade.color = fade.color.With(-1f, -1f, -1f, a);
		});
		yield return Sequences.Wait(durationOut);
		ClearFocus();
		PauseMenu.Unblock();
		DeckpackBlocker.Unblock();
	}
}
