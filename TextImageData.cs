#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Text Image Data", fileName = "Text Image Data")]
public class TextImageData : ScriptableObject
{
	[SerializeField]
	public List<char> chars;

	[SerializeField]
	public List<Sprite> sprites;

	public bool TryGetSprite(char value, out Sprite result)
	{
		int num = chars.IndexOf(value);
		if (num >= 0)
		{
			result = sprites[num];
			return true;
		}

		result = null;
		return false;
	}
}
