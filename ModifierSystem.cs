#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ModifierSystem : GameSystem
{
	public void OnEnable()
	{
		Events.OnCampaignInit += RunInitScripts;
		Events.OnCampaignStart += AddSystems;
		Events.OnCampaignLoaded += AddSystems;
		Events.OnCampaignGenerated += RunStartScripts;
	}

	public void OnDisable()
	{
		Events.OnCampaignInit -= RunInitScripts;
		Events.OnCampaignStart -= AddSystems;
		Events.OnCampaignLoaded -= AddSystems;
		Events.OnCampaignGenerated -= RunStartScripts;
	}

	public static void AddModifier(CampaignData data, GameModifierData modifier)
	{
		if (data.Modifiers == null)
		{
			List<GameModifierData> list2 = (data.Modifiers = new List<GameModifierData>());
		}

		data.Modifiers.Add(modifier);
	}

	public static void RemoveModifier(CampaignData data, GameModifierData modifier)
	{
		data.Modifiers?.Remove(modifier);
	}

	public static void AddSystems()
	{
		if (Campaign.Data.Modifiers == null)
		{
			return;
		}

		foreach (GameModifierData item in Campaign.Data.Modifiers.OrderByDescending((GameModifierData m) => m.scriptPriority))
		{
			string[] systemsToAdd = item.systemsToAdd;
			foreach (string text in systemsToAdd)
			{
				if (Campaign.instance.systems.GetComponent(text) is GameSystem gameSystem)
				{
					Debug.Log($"[{item}] enabling system: {gameSystem}");
					gameSystem.enabled = true;
				}
				else
				{
					Debug.Log($"[{item}] adding system: {text}");
					Campaign.instance.systems.AddComponentByName(text);
				}
			}
		}
	}

	public static IEnumerator RunInitScripts()
	{
		return RunInitScripts(Campaign.Data.Modifiers);
	}

	public async Task RunStartScripts()
	{
		Routine routine = new Routine(RunCampaignStartScripts(Campaign.Data.Modifiers));
		while (routine.IsRunning && (bool)this)
		{
			await Task.Delay(5);
		}
	}

	public static IEnumerator RunInitScripts(IReadOnlyCollection<GameModifierData> modifiers)
	{
		if (modifiers == null)
		{
			yield break;
		}

		Debug.Log($"Running [{modifiers.Count}] Modifier Set Up Scripts");
		foreach (GameModifierData item in modifiers.OrderByDescending((GameModifierData m) => m.scriptPriority))
		{
			Debug.Log($"Running [{item}] Setup Scripts");
			Script[] setupScripts = item.setupScripts;
			foreach (Script script in setupScripts)
			{
				yield return script.Run();
			}
		}
	}

	public static IEnumerator RunCampaignStartScripts(IReadOnlyCollection<GameModifierData> modifiers)
	{
		if (modifiers == null)
		{
			yield break;
		}

		Debug.Log($"Running [{modifiers.Count}] Modifier Campaign Start Scripts");
		foreach (GameModifierData item in modifiers.OrderByDescending((GameModifierData m) => m.scriptPriority))
		{
			Debug.Log($"Running [{item}] Campaign Start Scripts");
			Script[] startScripts = item.startScripts;
			foreach (Script script in startScripts)
			{
				yield return script.Run();
			}
		}
	}
}
