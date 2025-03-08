#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleLogButton : MonoBehaviour
{
	[SerializeField]
	public JournalTab battleLogTab;

	public void Awake()
	{
		Events.OnSceneChanged += SceneChanged;
		SetActive(value: false);
	}

	public void OnDestroy()
	{
		Events.OnSceneChanged -= SceneChanged;
	}

	public void SceneChanged(Scene scene)
	{
		SetActive(scene.name == "Battle");
	}

	public void SetActive(bool value)
	{
		base.gameObject.SetActive(value);
		if ((bool)battleLogTab)
		{
			battleLogTab.gameObject.SetActive(value);
		}
	}
}
