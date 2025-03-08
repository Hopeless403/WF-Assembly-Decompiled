#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AvatarPart : MonoBehaviour
{
	[Serializable]
	public struct Anchor
	{
		public string name;

		public Transform transform;
	}

	[Serializable]
	public struct Part
	{
		public string name;

		public Image image;

		public void Disable()
		{
			if (image != null)
			{
				image.enabled = false;
			}
		}

		public void Set(Sprite sprite, Vector2 scale)
		{
			if (image != null)
			{
				image.enabled = true;
				image.sprite = sprite;
				Transform transform = image.transform;
				transform.localScale *= scale;
			}
		}
	}

	[SerializeField]
	public Anchor[] anchors;

	[SerializeField]
	public Part[] parts;

	public Transform GetAnchor(string name)
	{
		return anchors.FirstOrDefault((Anchor a) => a.name == name).transform;
	}

	public Part Get(string name)
	{
		return parts.FirstOrDefault((Part a) => a.name == name);
	}
}
