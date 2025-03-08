#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

public class SparkleSpawner : MonoBehaviourRect
{
	public int sparkleCount;

	public Sprite sparkleSprite;

	public Queue<Sparkle> pool = new Queue<Sparkle>();

	public List<Transform> activeSparkles = new List<Transform>();

	public Vector2 sizeRange = new Vector2(0.25f, 0.75f);

	public Color colour = Color.white;

	public Vector2 spawnTimeRange = new Vector2(0.75f, 1f);

	public float timer;

	public void Update()
	{
		float y = base.rectTransform.rect.position.y;
		for (int num = activeSparkles.Count - 1; num >= 0; num--)
		{
			Transform transform = activeSparkles[num];
			Vector3 localPosition = transform.localPosition;
			if (localPosition.y < y)
			{
				pool.Enqueue(transform.GetComponent<Sparkle>());
				transform.gameObject.SetActive(value: false);
				activeSparkles.Remove(transform);
			}
			else
			{
				transform.localPosition = localPosition;
			}
		}

		timer -= Time.deltaTime;
		if (timer <= 0f)
		{
			NewSparkle();
			timer = spawnTimeRange.PettyRandom();
		}
	}

	public void NewSparkle()
	{
		Sparkle sparkle;
		if (pool.Count <= 0)
		{
			sparkle = new GameObject($"Sparkle {sparkleCount++}").AddComponent<Sparkle>();
			sparkle.sprite = sparkleSprite;
			sparkle.transform.SetParent(base.transform);
		}
		else
		{
			sparkle = pool.Dequeue();
			sparkle.gameObject.SetActive(value: true);
		}

		float num = sizeRange.PettyRandom();
		float a = Mathf.Min(1f, num * 2f) * 0.5f;
		Rect rect = base.rectTransform.rect;
		Vector3 localPosition = (rect.position + new Vector2(Random.Range(0f, rect.width), rect.height)).WithZ(0f);
		RectTransform component = sparkle.GetComponent<RectTransform>();
		component.localPosition = localPosition;
		component.localEulerAngles = Vector3.zero;
		component.localScale = Vector3.one;
		sparkle.size = num;
		sparkle.color = new Color(colour.r, colour.g, colour.b, a);
		activeSparkles.Add(sparkle.transform);
	}
}
