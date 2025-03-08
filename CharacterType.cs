#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterType", menuName = "Character/Type")]
public class CharacterType : ScriptableObject
{
	[Serializable]
	public class PrefabGroup
	{
		public string name;

		public PrefabCollection collection;
	}

	[Serializable]
	public class SpriteGroup
	{
		public string name;

		public SpriteCollection collection;
	}

	[Serializable]
	public class ColorSetGroup
	{
		public string name;

		public ColorSetCollection collection;
	}

	[Serializable]
	public class ScaleRange
	{
		public string name;

		public bool lockRatio;

		public Vector2 xRange;

		public Vector2 yRange;

		public Vector2 Convert()
		{
			if (lockRatio)
			{
				float num = xRange.Random();
				return new Vector2(num, num);
			}

			return new Vector2(xRange.Random(), yRange.Random());
		}
	}

	public string race;

	public string gender;

	public PrefabGroup[] prefabs;

	public SpriteGroup[] sprites;

	public ColorSetGroup[] colorSets;

	public ScaleRange[] scales;
}
