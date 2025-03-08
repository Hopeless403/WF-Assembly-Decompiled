#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class FrostEyeSystem : GameSystem
{
	[SerializeField]
	public AssetReference frostEyePrefab;

	public static AssetReference frostEyePrefabRef;

	public static readonly List<Entity> toProcess = new List<Entity>();

	public static readonly Dictionary<Card, List<GameObject>> toRemove = new Dictionary<Card, List<GameObject>>();

	public void OnEnable()
	{
		frostEyePrefabRef = frostEyePrefab;
		Events.OnEntityCreated += EntityCreated;
		Events.OnEntityDataUpdated += EntityDataUpdated;
		Events.OnCardPooled += CardPooled;
	}

	public void OnDisable()
	{
		Events.OnEntityCreated -= EntityCreated;
		Events.OnEntityDataUpdated -= EntityDataUpdated;
		Events.OnCardPooled -= CardPooled;
	}

	public static void EntityCreated(Entity entity)
	{
		if (entity.data.customData?.GetValueOrDefault("eyes", null) is string text && text == "frost")
		{
			toProcess.Add(entity);
		}
	}

	public static void EntityDataUpdated(Entity entity)
	{
		if (toProcess.Contains(entity))
		{
			toProcess.Remove(entity);
			Create(entity);
		}
	}

	public static void Create(Entity entity)
	{
		if (!(entity.display is Card card))
		{
			return;
		}

		Transform parent = card.mainImage.transform.parent;
		AvatarEyePositions componentInChildren = parent.GetComponentInChildren<AvatarEyePositions>();
		if ((object)componentInChildren != null)
		{
			Transform parent2 = componentInChildren.transform;
			AvatarEyePositions.Eye[] eyes = componentInChildren.eyes;
			foreach (AvatarEyePositions.Eye eye in eyes)
			{
				Transform obj = frostEyePrefabRef.InstantiateAsync(parent2).WaitForCompletion().transform;
				obj.localPosition = eye.pos;
				obj.localScale = eye.scale;
			}

			return;
		}

		EyeData eyeData = AddressableLoader.GetGroup<EyeData>("EyeData").FirstOrDefault((EyeData a) => a.cardData == entity.data.name);
		if (!eyeData)
		{
			return;
		}

		Transform transform = parent.Cast<Transform>().FirstOrDefault((Transform a) => a.gameObject.activeSelf);
		if ((bool)transform)
		{
			toRemove.Add(card, new List<GameObject>());
			EyeData.Eye[] eyes2 = eyeData.eyes;
			for (int i = 0; i < eyes2.Length; i++)
			{
				EyeData.Eye eye2 = eyes2[i];
				Transform transform2 = frostEyePrefabRef.InstantiateAsync(transform).WaitForCompletion().transform;
				transform2.SetLocalPositionAndRotation(eye2.position, Quaternion.Euler(0f, 0f, eye2.rotation));
				transform2.localScale = eye2.scale.WithZ(1f);
				toRemove[card].Add(transform2.gameObject);
			}
		}
	}

	public static void CardPooled(Card card)
	{
		if (!toRemove.ContainsKey(card))
		{
			return;
		}

		Debug.Log($"Destroying [{toRemove[card].Count}] Frosteye objects from [{card.name}]");
		foreach (GameObject item in toRemove[card])
		{
			Object.Destroy(item);
		}

		toRemove.Remove(card);
	}
}
