#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.SceneManagement;

public class CardPoolReturner : MonoBehaviour
{
	[SerializeField]
	public string[] scenesToIgnore;

	public void OnEnable()
	{
		Events.OnSceneUnload += SceneUnload;
	}

	public void OnDisable()
	{
		Events.OnSceneUnload -= SceneUnload;
	}

	public void SceneUnload(Scene scene)
	{
		if (scenesToIgnore.Contains(scene.name))
		{
			return;
		}

		StopWatch.Start();
		int num = 0;
		GameObject[] rootGameObjects = scene.GetRootGameObjects();
		for (int i = 0; i < rootGameObjects.Length; i++)
		{
			Card[] componentsInChildren = rootGameObjects[i].GetComponentsInChildren<Card>();
			foreach (Card card in componentsInChildren)
			{
				if (card.entity.returnToPool && CardManager.ReturnToPool(card))
				{
					num++;
				}
			}
		}

		Debug.Log($"[{num}] Cards returned to pool from [{scene.name}] ({StopWatch.Stop()}ms)");
	}
}
