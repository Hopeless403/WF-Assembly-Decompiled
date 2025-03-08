#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.SceneManagement;

public class FrostoscopeMapSystem : GameSystem
{
	[SerializeField]
	public UnlockData requiresUnlock;

	[SerializeField]
	public string[] visibleDuringScenes = new string[2] { "MapNew", "Event" };

	[SerializeField]
	public GameObject button;

	public void OnEnable()
	{
		Events.OnSceneChanged += SceneChanged;
		CheckIfUnlocked();
	}

	public void OnDisable()
	{
		Events.OnSceneChanged -= SceneChanged;
	}

	public void SceneChanged(Scene scene)
	{
		bool active = visibleDuringScenes.Contains(scene.name);
		button.SetActive(active);
	}

	public void CheckIfUnlocked()
	{
		if (!MetaprogressionSystem.IsUnlocked(requiresUnlock))
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
