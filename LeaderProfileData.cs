#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Leader Profile", menuName = "Leader Profile")]
public class LeaderProfileData : ScriptableObject
{
	[Serializable]
	public class Collection
	{
		public enum Type
		{
			Prefab,
			Sprite,
			ColorSet
		}

		public string collectionName;

		public Type type;

		public CollectionModifier modifier;

		public void AddTo(CharacterType characterType, float multiply = 1f)
		{
			switch (type)
			{
				case Type.Prefab:
				{
					PrefabCollection collection2 = characterType.prefabs.First((CharacterType.PrefabGroup a) => a.name == collectionName).collection;
					CollectionModifier.Modify[] list = modifier.list;
				for (int i = 0; i < list.Length; i++)
				{
					CollectionModifier.Modify modify2 = list[i];
					collection2.AddWeight(modify2.index, modify2.addWeight * multiply);
					}
	
					break;
				}
				case Type.Sprite:
				{
					SpriteCollection collection3 = characterType.sprites.First((CharacterType.SpriteGroup a) => a.name == collectionName).collection;
					CollectionModifier.Modify[] list = modifier.list;
				for (int i = 0; i < list.Length; i++)
				{
					CollectionModifier.Modify modify3 = list[i];
					collection3.AddWeight(modify3.index, modify3.addWeight * multiply);
					}
	
					break;
				}
				case Type.ColorSet:
				{
					ColorSetCollection collection = characterType.colorSets.First((CharacterType.ColorSetGroup a) => a.name == collectionName).collection;
					CollectionModifier.Modify[] list = modifier.list;
				for (int i = 0; i < list.Length; i++)
				{
					CollectionModifier.Modify modify = list[i];
					collection.AddWeight(modify.index, modify.addWeight * multiply);
					}
	
					break;
				}
			}
		}
	}

	[SerializeField]
	public SpriteCollection backgroundPool;

	[SerializeField]
	public Collection[] collectionsToAdd;

	public Sprite GetRandomBackground()
	{
		return backgroundPool.RandomItem();
	}

	public void Apply(CharacterType type)
	{
		Collection[] array = collectionsToAdd;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].AddTo(type);
		}
	}

	public void UnApply(CharacterType type)
	{
		Collection[] array = collectionsToAdd;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].AddTo(type, -1f);
		}
	}
}
