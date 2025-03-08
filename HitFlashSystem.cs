#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HitFlashSystem : GameSystem
{
	public class HitFlash
	{
		public Material material;

		public float time;

		public Dictionary<Image, Material> imageDict = new Dictionary<Image, Material>();

		public List<TMP_Text> textElements = new List<TMP_Text>();

		public Entity target { get; set; }

		public bool ended => time <= 0f;

		public HitFlash(Entity entity, Material material, float duration)
		{
			target = entity;
			this.material = material;
			time = duration;
			Start();
		}

		public void Start()
		{
			Image[] componentsInChildren = target.GetComponentsInChildren<Image>(includeInactive: true);
			TMP_Text[] componentsInChildren2 = target.GetComponentsInChildren<TMP_Text>(includeInactive: true);
			Image[] array = componentsInChildren;
			foreach (Image image in array)
			{
				if (image.enabled && image.sprite != null && image.gameObject.GetComponent<Mask>() == null)
				{
					imageDict.Add(image, image.material);
					image.material = material;
				}
			}

			TMP_Text[] array2 = componentsInChildren2;
			foreach (TMP_Text tMP_Text in array2)
			{
				if (tMP_Text.enabled)
				{
					textElements.Add(tMP_Text);
					tMP_Text.enabled = false;
				}
			}
		}

		public void Update(float delta)
		{
			time -= delta;
			if (time <= 0f)
			{
				End();
			}
		}

		public void End()
		{
			foreach (KeyValuePair<Image, Material> item in imageDict)
			{
				if (item.Key != null)
				{
					item.Key.material = item.Value;
				}
			}

			foreach (TMP_Text textElement in textElements)
			{
				if (textElement != null)
				{
					textElement.enabled = true;
				}
			}
		}
	}

	public static HitFlashSystem instance;

	[SerializeField]
	public float flashDuration = 0.1f;

	[SerializeField]
	public Material damageMaterial;

	public readonly List<HitFlash> list = new List<HitFlash>();

	public void Awake()
	{
		instance = this;
	}

	public void OnEnable()
	{
		Events.OnEntityHit += EntityHit;
	}

	public void Update()
	{
		for (int num = list.Count - 1; num >= 0; num--)
		{
			HitFlash hitFlash = list[num];
			hitFlash.Update(Time.deltaTime);
			if (hitFlash.ended)
			{
				list.RemoveAt(num);
			}
		}
	}

	public void OnDisable()
	{
		Events.OnEntityHit -= EntityHit;
		foreach (HitFlash item in list)
		{
			item.End();
		}

		list.Clear();
	}

	public void EntityHit(Hit hit)
	{
		if (hit.Offensive && !hit.nullified)
		{
			HitFlash hitFlash = list.Find((HitFlash a) => a.target == hit.target);
			if (hitFlash != null)
			{
				hitFlash.time = flashDuration;
			}
			else
			{
				list.Add(new HitFlash(hit.target, damageMaterial, flashDuration));
			}
		}
	}

	public void RemoveFromTarget(Entity entity)
	{
		HitFlash hitFlash = list.Find((HitFlash a) => a.target == entity);
		if (hitFlash != null)
		{
			hitFlash.End();
			list.Remove(hitFlash);
		}
	}

	public static void Remove(Entity entity)
	{
		if (instance != null)
		{
			instance.RemoveFromTarget(entity);
		}
	}
}
