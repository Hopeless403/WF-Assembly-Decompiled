#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatsPanelSplatters : MonoBehaviour
{
	public struct Profile
	{
		public readonly Vector2 anchorMin;

		public readonly Vector2 anchorMax;

		public readonly Vector2 moveRange;

		public Profile(float minX, float minY, float maxX, float maxY, float moveX, float moveY)
		{
			anchorMin = new Vector2(minX, minY);
			anchorMax = new Vector2(maxX, maxY);
			moveRange = new Vector2(moveX, moveY);
		}
	}

	[SerializeField]
	public Sprite[] splatterSprites;

	[SerializeField]
	public ImageSprite[] splatters;

	public static readonly Profile[] profiles = new Profile[8]
	{
		new Profile(1f, 1f, 1f, 1f, 0f, -2f),
		new Profile(1f, 0.5f, 1f, 0.5f, 0f, -2f),
		new Profile(1f, 0f, 1f, 0f, -2f, 0f),
		new Profile(0f, 1f, 0f, 1f, 2f, 0f),
		new Profile(0f, 0.5f, 0f, 0.5f, 0f, 2f),
		new Profile(0f, 0f, 0f, 0f, 0f, 2f),
		new Profile(0.5f, 1f, 0.5f, 1f, 2f, 0f),
		new Profile(0.5f, 0f, 0.5f, 0f, -2f, 0f)
	};

	public readonly List<Sprite> spritePool = new List<Sprite>();

	public readonly List<Profile> profilePool = new List<Profile>();

	public void OnEnable()
	{
		List<Color> list = new List<Color>();
		SplatterPersistenceSystem splatterPersistenceSystem = Object.FindObjectOfType<SplatterPersistenceSystem>();
		if (splatterPersistenceSystem.storedSplatters != null && splatterPersistenceSystem.storedSplatters.Count > 0)
		{
			foreach (SplatterPersistenceSystem.SplatterData[] value in splatterPersistenceSystem.storedSplatters.Values)
			{
				list.AddRange(value.Select((SplatterPersistenceSystem.SplatterData a) => a.color));
			}
		}

		int num = Mathf.Min(splatters.Length, list.Count);
		for (int i = 0; i < num; i++)
		{
			ImageSprite image = splatters[i];
			Profile profile = PullProfile();
			Sprite sprite = PullSprite();
			Set(image, profile, sprite, list.RandomItem());
		}
	}

	public Profile PullProfile()
	{
		if (profilePool.Count <= 0)
		{
			profilePool.AddRange(profiles);
		}

		return profilePool.TakeRandom();
	}

	public Sprite PullSprite()
	{
		if (spritePool.Count <= 0)
		{
			spritePool.AddRange(splatterSprites);
		}

		return spritePool.TakeRandom();
	}

	public static void Set(ImageSprite image, Profile profile, Sprite sprite, Color color)
	{
		image.gameObject.SetActive(value: true);
		image.SetSprite(sprite);
		image.color = color;
		if (image.transform is RectTransform rectTransform)
		{
			rectTransform.anchorMax = profile.anchorMax;
			rectTransform.anchorMin = profile.anchorMin;
			rectTransform.anchoredPosition = new Vector2(Random.Range(0f, profile.moveRange.x), Random.Range(0f, profile.moveRange.y));
			rectTransform.localEulerAngles = new Vector3(0f, 0f, Random.Range(0f, 360f));
		}
	}
}
