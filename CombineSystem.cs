#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dead;
using UnityEngine;
using UnityEngine.UI;

public class CombineSystem : GameSystem
{
	public static CombineSystem instance;

	[SerializeField]
	public Fader fader;

	[SerializeField]
	public Graphic flash;

	[SerializeField]
	public AnimationCurve flashCurve;

	[SerializeField]
	public AnimationCurve bounceCurve;

	[SerializeField]
	public Transform group;

	[SerializeField]
	public Transform pointPrefab;

	[SerializeField]
	public ParticleSystem ps;

	public readonly List<Transform> points = new List<Transform>();

	public readonly Dictionary<Entity, Transform> originalParents = new Dictionary<Entity, Transform>();

	public void Awake()
	{
		instance = this;
	}

	public IEnumerator Combine(Entity[] entities, Entity finalEntity)
	{
		fader.In();
		Vector3 zero = Vector3.zero;
		Entity[] array = entities;
		foreach (Entity entity in array)
		{
			zero += entity.transform.position;
		}

		zero /= (float)entities.Length;
		group.position = zero;
		array = entities;
		foreach (Entity entity2 in array)
		{
			Transform transform = UnityEngine.Object.Instantiate(pointPrefab, entity2.transform.position, Quaternion.identity, group);
			transform.gameObject.SetActive(value: true);
			originalParents[entity2] = entity2.transform.parent;
			entity2.transform.SetParent(transform);
			points.Add(transform);
		}

		foreach (Transform point in points)
		{
			LeanTween.moveLocal(to: point.localPosition.normalized * 0.5f, gameObject: point.gameObject, time: 0.4f).setEaseInBack();
		}

		yield return new WaitForSeconds(0.4f);
		Flash(0.5f);
		Events.InvokeScreenShake(1f, 0f);
		array = entities;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].wobbler.WobbleRandom();
		}

		foreach (Transform point2 in points)
		{
			LeanTween.moveLocal(to: point2.localPosition.normalized * 3f, gameObject: point2.gameObject, time: 1f).setEase(bounceCurve);
		}

		LeanTween.moveLocal(group.gameObject, new Vector3(0f, 0f, -2f), 1f).setEaseInOutQuad();
		LeanTween.rotateZ(group.gameObject, PettyRandom.Range(160f, 180f), 1f).setOnUpdateVector3(delegate
		{
			foreach (Transform point3 in points)
			{
				point3.transform.eulerAngles = Vector3.zero;
			}

		}).setEaseInOutQuad();
		yield return new WaitForSeconds(1f);
		Flash();
		Events.InvokeScreenShake(1f, 0f);
		if (ps != null)
		{
			ps.Play();
		}

		finalEntity.data = finalEntity.data.Clone(finalEntity.data.random3, finalEntity.data.id, runCreateScripts: false);
		finalEntity.data.forceTitle = finalEntity.data.title;
		Routine.Clump clump = new Routine.Clump();
		array = entities.Without(finalEntity);
		foreach (Entity entity3 in array)
		{
			finalEntity.data.forceTitle += entity3.data.title;
			clump.Add(Combine(entity3, finalEntity));
		}

		yield return clump.WaitForEnd();
		foreach (Entity.TraitStacks item in finalEntity.traits.Where((Entity.TraitStacks a) => a.data.name == "Build"))
		{
			item.count = 0;
		}

		yield return StatusEffectSystem.BuildEvent(finalEntity);
		yield return finalEntity.UpdateTraits();
		finalEntity.curveAnimator.Ping();
		finalEntity.wobbler.WobbleRandom();
		finalEntity.counter.current = finalEntity.counter.max;
		finalEntity.alive = false;
		yield return finalEntity.display.UpdateData();
		yield return finalEntity.UpdateTraits();
		finalEntity.alive = true;
		yield return StatusEffectSystem.EntityEnableEvent(finalEntity);
		fader.Out();
		yield return new WaitForSeconds(1f);
		foreach (KeyValuePair<Entity, Transform> item2 in originalParents.Where((KeyValuePair<Entity, Transform> pair) => (bool)pair.Key && (bool)pair.Value))
		{
			item2.Key.transform.SetParent(item2.Value);
			item2.Key.TweenToContainer();
			item2.Key.wobbler.WobbleRandom();
		}

		originalParents.Clear();
		foreach (Transform item3 in points.Where((Transform p) => p))
		{
			UnityEngine.Object.Destroy(item3.gameObject);
		}

		points.Clear();
		group.transform.localEulerAngles = Vector3.zero;
	}

	public static IEnumerator Combine(Entity entity, Entity inTo)
	{
		if (!inTo.data.hasAttack)
		{
			inTo.data.hasAttack = entity.data.hasAttack;
		}

		if (!inTo.data.hasHealth)
		{
			inTo.data.hasHealth = entity.data.hasHealth;
		}

		inTo.data.damage += entity.data.damage;
		inTo.data.hp += entity.data.hp;
		inTo.data.counter = Mathf.Max(inTo.data.counter, entity.data.counter);
		inTo.attackEffects = CardData.StatusEffectStacks.Stack(inTo.attackEffects, entity.attackEffects).ToList();
		List<StatusEffectData> list = entity.statusEffects.Clone();
		foreach (Entity.TraitStacks trait in entity.traits)
		{
			foreach (StatusEffectData passiveEffect in trait.passiveEffects)
			{
				list.Remove(passiveEffect);
			}

			int num = trait.count - trait.tempCount;
			if (num > 0)
			{
				inTo.GainTrait(trait.data, num);
			}
		}

		Events.InvokeScreenShake(1f, 0f);
		Routine.Clump clump = new Routine.Clump();
		foreach (StatusEffectData item in list)
		{
			clump.Add(StatusEffectSystem.Apply(inTo, item.applier, item, item.count));
		}

		yield return clump.WaitForEnd();
		entity.RemoveFromContainers();
		CardManager.ReturnToPool(entity);
	}

	public void Flash(float intensity = 1f, float duration = 0.1f)
	{
		flash.gameObject.SetActive(value: true);
		LeanTween.cancel(flash.gameObject);
		LeanTween.value(flash.gameObject, 0f, intensity, duration).setEase(flashCurve).setOnUpdate(delegate(float a)
		{
			flash.color = flash.color.With(-1f, -1f, -1f, a);
		})
			.setOnComplete((Action)delegate
			{
				flash.gameObject.SetActive(value: false);
			});
	}
}
