#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class PetHutFlagSetter : MonoBehaviour
{
	[SerializeField]
	public SpriteRenderer flag;

	[SerializeField]
	public Sprite[] flagSprites;

	public void SetupFlag()
	{
		int num = Mathf.Clamp(SaveSystem.LoadProgressData("selectedPet", 0), 0, flagSprites.Length - 1);
		flag.sprite = flagSprites[num];
	}
}
