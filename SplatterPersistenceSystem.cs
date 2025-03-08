#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class SplatterPersistenceSystem : GameSystem
{
	[Serializable]
	public class SplatterData
	{
		public Sprite sprite;

		public Color color;

		public Vector3 offset;

		public Vector3 scale;

		public float angle;

		public SplatterData()
		{
		}

		public SplatterData(Splatter splatter)
		{
			sprite = splatter.sprite;
			color = splatter.color;
			Transform transform = splatter.transform;
			offset = transform.localPosition;
			scale = transform.localScale;
			angle = transform.localEulerAngles.z;
		}
	}

	[SerializeField]
	public int[] teamsToSave;

	[SerializeField]
	public Splatter basicSplatterPrefab;

	[SerializeField]
	[ReadOnly]
	public int savedCount;

	[SerializeField]
	[ReadOnly]
	public bool saveRequired;

	public Dictionary<ulong, SplatterData[]> storedSplatters;

	public void OnEnable()
	{
		Events.OnEntityCreated += EntityCreated;
		Events.OnEntityDestroyed += EntityDestroyed;
		Events.OnCampaignSaved += CheckSave;
		Events.OnCampaignLoaded += Load;
		Events.OnCampaignFinal += Clear;
	}

	public void OnDisable()
	{
		Events.OnEntityCreated -= EntityCreated;
		Events.OnEntityDestroyed -= EntityDestroyed;
		Events.OnCampaignSaved -= CheckSave;
		Events.OnCampaignLoaded -= Load;
		Events.OnCampaignFinal -= Clear;
	}

	public void EntityCreated(Entity entity)
	{
		if (storedSplatters != null && (bool)entity.data && storedSplatters.ContainsKey(entity.data.id) && (bool)entity.splatterSurface)
		{
			SplatterData[] array = storedSplatters[entity.data.id];
			SplatterData[] array2 = array;
			foreach (SplatterData data in array2)
			{
				entity.splatterSurface.Load(data, basicSplatterPrefab);
			}

			Debug.Log($"Creating [{array.Length}] splatters on [{entity}]");
		}
	}

	public void EntityDestroyed(Entity entity)
	{
		if (!(entity.data != null) || !(entity.owner != null) || !teamsToSave.Contains(entity.owner.team) || (!entity.owner.data.inventory.deck.Contains(entity.data) && !entity.owner.data.inventory.reserve.Contains(entity.data)) || !(entity.splatterSurface != null))
		{
			return;
		}

		Splatter[] activeSplatters = entity.splatterSurface.GetActiveSplatters();
		if (activeSplatters != null && activeSplatters.Length > 0)
		{
			List<SplatterData> list = activeSplatters.Select((Splatter splatter) => new SplatterData(splatter)).ToList();
			if (storedSplatters == null)
			{
				storedSplatters = new Dictionary<ulong, SplatterData[]>();
			}

			if (storedSplatters.ContainsKey(entity.data.id))
			{
				storedSplatters[entity.data.id] = list.ToArray();
			}
			else
			{
				storedSplatters.Add(entity.data.id, list.ToArray());
				savedCount++;
			}

			saveRequired = true;
			Debug.Log($"Storing [{list.Count}] splatters on data for [{entity}]");
		}
	}

	public void CheckSave()
	{
		if (saveRequired && Campaign.Data.GameMode.doSave)
		{
			SaveSystem.SaveCampaignData(Campaign.Data.GameMode, "splatter", storedSplatters);
			saveRequired = false;
		}
	}

	public void Load()
	{
		if (SaveSystem.CampaignDataExists(Campaign.Data.GameMode, "splatter"))
		{
			storedSplatters = SaveSystem.LoadCampaignData<Dictionary<ulong, SplatterData[]>>(Campaign.Data.GameMode, "splatter");
			savedCount = storedSplatters?.Count ?? 0;
		}
		else
		{
			Clear();
		}
	}

	public void Clear()
	{
		storedSplatters = null;
		savedCount = 0;
	}
}
