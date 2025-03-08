#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using Dead;
using NaughtyAttributes;
using UnityEngine;

public class BossSetUp : MonoBehaviour
{
	public CharacterAvatar avatarPrefab;

	public CardData cardData;

	[SerializeField]
	[Required(null)]
	public Entity entity;

	[SerializeField]
	[Required(null)]
	public Character character;

	public void Awake()
	{
		SetUp();
	}

	public void SetUp()
	{
		entity.random3 = PettyRandom.Vector3();
		entity.data = cardData.Clone();
		entity.hp.max = entity.data.hp;
		entity.hp.current = entity.hp.max;
		entity.data.hasAttack = false;
	}
}
