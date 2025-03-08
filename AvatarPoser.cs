#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using Dead;
using UnityEngine;
using UnityEngine.UI;

public class AvatarPoser : MonoBehaviour
{
	[Serializable]
	public struct Pose
	{
		public string name;

		public bool setFace;

		public Sprite[] face;

		public bool setBody;

		public Sprite[] body;
	}

	[SerializeField]
	public Image faceImage;

	[SerializeField]
	public Image bodyImage;

	[SerializeField]
	[Range(0f, 2f)]
	public float pingStrength = 1f;

	[SerializeField]
	public Vector2 poseTime = new Vector2(1.5f, 2f);

	[SerializeField]
	public Pose[] poses;

	public readonly Dictionary<string, Pose> poseLookup = new Dictionary<string, Pose>();

	public Sprite baseFaceSprite;

	public Sprite baseBodySprite;

	public Vector3 scale;

	public float reset;

	public void Awake()
	{
		scale = base.transform.localScale;
		poseLookup.Clear();
		Pose[] array = poses;
		for (int i = 0; i < array.Length; i++)
		{
			Pose value = array[i];
			poseLookup[value.name] = value;
		}

		if ((bool)faceImage)
		{
			baseFaceSprite = faceImage.sprite;
		}

		if ((bool)bodyImage)
		{
			baseBodySprite = bodyImage.sprite;
		}
	}

	public void Update()
	{
		if (!(reset > 0f))
		{
			return;
		}

		reset -= Time.deltaTime;
		if (reset <= 0f)
		{
			if ((bool)faceImage)
			{
				faceImage.sprite = baseFaceSprite;
			}

			if ((bool)bodyImage)
			{
				bodyImage.sprite = baseBodySprite;
			}

			Ping();
		}
	}

	public void Set(string poseName)
	{
		if (poseLookup.TryGetValue(poseName, out var value))
		{
			if ((bool)faceImage && value.setFace)
			{
				faceImage.sprite = value.face[PettyRandom.Range(0, value.face.Length - 1)];
			}

			if ((bool)bodyImage && value.setBody)
			{
				bodyImage.sprite = value.body[PettyRandom.Range(0, value.body.Length - 1)];
			}

			reset = poseTime.PettyRandom();
			Ping();
		}
	}

	public void Ping()
	{
		if (!(pingStrength <= 0f))
		{
			LeanTween.cancel(base.gameObject);
			float num = 1f + 0.05f * pingStrength;
			base.transform.localScale = new Vector3(num * scale.x, 1f / num * scale.y, scale.z);
			LeanTween.scale(base.gameObject, scale, 1f).setEase(LeanTweenType.easeOutElastic);
		}
	}
}
