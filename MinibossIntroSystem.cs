#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MinibossIntroSystem : GameSystem
{
	[Serializable]
	public struct MinibossAnimation
	{
		public CardData cardData;

		public CardAnimation animation;
	}

	[SerializeField]
	public Transform container;

	[Header("Focus")]
	[SerializeField]
	public float focusAmount = 0.75f;

	[SerializeField]
	public float zoomAmount = 3f;

	[SerializeField]
	public CurveProfile focusTween;

	[SerializeField]
	public CurveProfile unfocusTween;

	[Header("Fade")]
	[SerializeField]
	public Image fade;

	[SerializeField]
	public Color fadeColor;

	[Header("Move miniboss to position")]
	[SerializeField]
	public Vector3 move = new Vector3(0f, 0f, -1f);

	[SerializeField]
	public CurveProfile moveTween;

	[SerializeField]
	public CurveProfile returnTween;

	[Header("Pauses before/after miniboss animation")]
	[SerializeField]
	public float pauseBefore = 0.33f;

	[SerializeField]
	public float pauseAfter = 1f;

	[Header("Delays")]
	[SerializeField]
	public float delayBefore = 0.2f;

	[SerializeField]
	public float delayAfter = 0.2f;

	[Header("Miniboss animations")]
	[SerializeField]
	public CardAnimation defaultAnimation;

	[SerializeField]
	public MinibossAnimation[] specificAnimations;

	[Header("SFX")]
	[SerializeField]
	public EventReference zoomSfx;

	[SerializeField]
	public EventReference shakeSfx;

	public readonly List<Entity> ignore = new List<Entity>();

	public Dictionary<string, CardAnimation> animationLookup;

	public Entity target;

	public Transform targetPreviousParent;

	public Routine routine;

	public bool hasRun;

	public void OnEnable()
	{
		Events.OnSceneChanged += SceneChanged;
		Events.OnEntityMove += EntityMove;
		animationLookup = new Dictionary<string, CardAnimation>();
		MinibossAnimation[] array = specificAnimations;
		for (int i = 0; i < array.Length; i++)
		{
			MinibossAnimation minibossAnimation = array[i];
			animationLookup[minibossAnimation.cardData.name] = minibossAnimation.animation;
		}
	}

	public void OnDisable()
	{
		Events.OnSceneChanged -= SceneChanged;
		Events.OnEntityMove -= EntityMove;
		StopAllCoroutines();
		routine?.Stop();
		if ((bool)References.MinibossCameraMover)
		{
			LeanTween.cancel(References.MinibossCameraMover.gameObject);
			References.MinibossCameraMover.localPosition = Vector3.zero;
		}
	}

	public void Ignore(Entity entity)
	{
		ignore.Add(entity);
	}

	public void SceneChanged(Scene scene)
	{
		ignore.Clear();
		hasRun = false;
	}

	public void EntityMove(Entity entity)
	{
		if (!hasRun && !ignore.Contains(entity))
		{
			CardType cardType = entity.data.cardType;
			if ((object)cardType != null && cardType.miniboss && entity.owner == Battle.instance.enemy && Battle.IsOnBoard(entity) && !Battle.IsOnBoard(entity.preContainers))
			{
				Activate(entity);
				hasRun = true;
			}
		}
	}

	public void Activate(Entity target)
	{
		ignore.Add(target);
		routine = new Routine(Routine(target));
		ActionQueue.Stack(new ActionSequence(WaitForRoutineToEnd()));
	}

	public IEnumerator WaitForRoutineToEnd()
	{
		while (routine.IsRunning)
		{
			yield return null;
		}
	}

	public void Assign(Entity target)
	{
		UnAssign();
		this.target = target;
		targetPreviousParent = target.transform.parent;
		target.transform.SetParent(container, worldPositionStays: true);
		target.ResetDrawOrder();
	}

	public void UnAssign()
	{
		if ((bool)target)
		{
			target.transform.SetParent(targetPreviousParent);
		}

		target = null;
	}

	public IEnumerator Routine(Entity target)
	{
		Events.InvokeSetWeatherIntensity(1f, 1f);
		Assign(target);
		LeanTween.value(base.gameObject, fade.color.a, fadeColor.a, focusTween.duration).setEase(LeanTweenType.easeInOutQuart).setOnUpdate(delegate(float a)
		{
			fade.color = fade.color.With(-1f, -1f, -1f, a);
		});
		target.flipper?.FlipUpInstant();
		CinemaBarSystem.SetSortingLayer("Inspect", 1);
		CinemaBarSystem.In();
		LeanTween.cancel(target.gameObject);
		yield return Sequences.Wait(delayBefore);
		Events.InvokeMinibossIntro(target);
		LeanTween.moveLocal(target.gameObject, Vector3.zero, moveTween.duration).setEase(moveTween.curve);
		yield return Sequences.Wait(pauseBefore);
		PauseMenu.Block();
		Vector3 originalCameraPos = Vector3.zero;
		Vector3 to = Vector3.Lerp(originalCameraPos, container.position, focusAmount).WithZ(zoomAmount);
		LeanTween.cancel(References.MinibossCameraMover.gameObject);
		LeanTween.move(References.MinibossCameraMover.gameObject, to, focusTween.duration).setEase(focusTween.curve);
		SfxSystem.OneShot(zoomSfx);
		CardAnimation valueOrDefault = animationLookup.GetValueOrDefault(target.name, defaultAnimation);
		SfxSystem.OneShot(shakeSfx);
		yield return valueOrDefault.Routine(target);
		yield return Sequences.Wait(pauseAfter);
		_ = target.actualContainers[0];
		Vector3 containerScale = target.GetContainerScale();
		Vector3 containerWorldRotation = target.GetContainerWorldRotation();
		Vector3 containerWorldPosition = target.GetContainerWorldPosition();
		LeanTween.cancel(target.gameObject);
		LeanTween.scale(target.gameObject, containerScale, returnTween.duration).setEase(returnTween.curve);
		LeanTween.rotate(target.gameObject, containerWorldRotation, returnTween.duration).setEase(returnTween.curve);
		LeanTween.move(target.gameObject, containerWorldPosition, returnTween.duration).setEase(returnTween.curve).setOnComplete((Action)delegate
		{
			target.wobbler?.WobbleRandom();
		});
		LeanTween.moveLocal(References.MinibossCameraMover.gameObject, originalCameraPos, unfocusTween.duration).setEase(LeanTweenType.easeInOutBack);
		LeanTween.value(base.gameObject, fade.color.a, 0f, unfocusTween.duration).setEase(unfocusTween.curve).setOnUpdate(delegate(float a)
		{
			fade.color = fade.color.With(-1f, -1f, -1f, a);
		});
		CinemaBarSystem.Out();
		Events.InvokeSetWeatherIntensity(0.25f, 3f);
		Events.InvokeMinibossIntroDone(target);
		yield return Sequences.Wait(unfocusTween.duration);
		UnAssign();
		PauseMenu.Unblock();
		yield return Sequences.Wait(delayAfter);
	}
}
