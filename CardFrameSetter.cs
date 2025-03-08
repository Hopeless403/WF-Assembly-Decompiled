#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class CardFrameSetter : MonoBehaviour
{
	[SerializeField]
	public AddressableTieredSpriteLoader[] spriteSetters;

	public bool loaded;

	public void Load(int frameLevel)
	{
		if (!loaded)
		{
			AddressableTieredSpriteLoader[] array = spriteSetters;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Load(frameLevel);
			}

			loaded = true;
		}
	}
}
