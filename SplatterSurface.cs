#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using Dead;
using NaughtyAttributes;
using UnityEngine;

public class SplatterSurface : MonoBehaviour, IPoolable
{
	public RectTransform splatterContainer;

	[SerializeField]
	public Vector2 splatterScaleRange = new Vector2(1f, 1f);

	[SerializeField]
	public Vector2 splatterAlphaRange = new Vector2(1f, 1f);

	[SerializeField]
	public bool limitSplatters;

	[SerializeField]
	[ShowIf("limitSplatters")]
	public int maxSplatters = 10;

	[SerializeField]
	public bool colorBlend;

	[SerializeField]
	[ShowIf("colorBlend")]
	public Vector2 colorBlendRange = new Vector2(0.2f, 0.4f);

	[SerializeField]
	[ShowIf("colorBlend")]
	public Color blend = Color.white;

	[SerializeField]
	public bool fadeSplatters;

	[SerializeField]
	[ShowIf("fadeSplatters")]
	public Color fadeToColour;

	[SerializeField]
	[ShowIf("fadeSplatters")]
	public float fadeToDelay;

	public readonly List<Splatter> splatters = new List<Splatter>();

	public void Splat(SplatterParticle particle)
	{
		Splatter splatter = Object.Instantiate(particle.splatterPrefab, splatterContainer);
		Transform transform = splatter.transform;
		splatters.Add(splatter);
		Color color = particle.color;
		if (colorBlend)
		{
			float t = colorBlendRange.PettyRandom();
			color = Color.Lerp(color, blend, t);
		}

		splatter.color = color.With(-1f, -1f, -1f, particle.color.a * splatterAlphaRange.PettyRandom());
		int num = (limitSplatters ? (splatters.Count - maxSplatters) : 0);
		if (fadeSplatters)
		{
			float time = Mathf.Max(1f, PettyRandom.Range(10f, 15f) - (float)num);
			if (SplatterSystem.CheckStartTween(time))
			{
				Color from = splatter.color;
				Color to = fadeToColour;
				LeanTween.value(splatter.gameObject, 0f, 1f, time).setEase(LeanTweenType.easeInOutQuint).setDelay(fadeToDelay)
					.setOnUpdate(delegate(float a)
					{
						splatter.color = Color.Lerp(from, to, a);
					});
			}
			else
			{
				splatter.color = fadeToColour;
			}
		}

		Vector3 vector = transform.localScale * splatterScaleRange.PettyRandom();
		float time2 = PettyRandom.Range(0.12f, 0.18f);
		if (SplatterSystem.CheckStartTween(time2))
		{
			transform.localScale = vector * 0.5f;
			LeanTween.scale(splatter.gameObject, vector, time2).setEase(LeanTweenType.easeOutBack);
		}
		else
		{
			transform.localScale = vector;
		}

		transform.position = particle.transform.position;
		transform.localPosition = transform.localPosition.WithZ(0f);
		if (limitSplatters)
		{
			for (int i = 0; i < num; i++)
			{
				splatters[0].FadeOut(num);
				splatters.RemoveAt(0);
			}
		}
	}

	public void Load(SplatterPersistenceSystem.SplatterData data, Splatter prefab)
	{
		Splatter splatter = Object.Instantiate(prefab, splatterContainer);
		splatters.Add(splatter);
		splatter.sprite = data.sprite;
		splatter.color = data.color;
		Transform obj = splatter.transform;
		obj.localPosition = data.offset;
		obj.localScale = data.scale;
		obj.localEulerAngles = new Vector3(0f, 0f, data.angle);
	}

	public Splatter[] GetActiveSplatters()
	{
		return splatters.Where((Splatter a) => !a.fading).ToArray();
	}

	public void OnGetFromPool()
	{
	}

	public void OnReturnToPool()
	{
		foreach (Splatter splatter in splatters)
		{
			Object.Destroy(splatter.gameObject);
		}

		splatters.Clear();
	}
}
