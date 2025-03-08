#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class DisableWhenDeckpackOpened : MonoBehaviour
{
	[SerializeField]
	public Behaviour[] components;

	[SerializeField]
	public GameObject[] gameObjects;

	public void OnEnable()
	{
		Events.OnDeckpackOpen += Open;
		Events.OnDeckpackClose += Close;
	}

	public void OnDisable()
	{
		Events.OnDeckpackOpen -= Open;
		Events.OnDeckpackClose -= Close;
	}

	public void Open()
	{
		Behaviour[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}

		GameObject[] array2 = gameObjects;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].SetActive(value: false);
		}
	}

	public void Close()
	{
		Behaviour[] array = components;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = true;
		}

		GameObject[] array2 = gameObjects;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].SetActive(value: true);
		}
	}
}
