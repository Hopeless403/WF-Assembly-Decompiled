#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class ItemHolderPetCreator : MonoBehaviour
{
	[SerializeField]
	public Entity owner;

	public ItemHolderPet currentPet;

	public void Create(ItemHolderPet prefab)
	{
		DestroyCurrent();
		currentPet = Object.Instantiate(prefab, base.transform);
		currentPet.owner = owner;
		currentPet.Show();
	}

	public void DestroyCurrent()
	{
		if ((bool)currentPet)
		{
			currentPet.gameObject.Destroy();
		}
	}

	public void Used()
	{
		if ((bool)currentPet)
		{
			currentPet.Used();
		}
	}
}
