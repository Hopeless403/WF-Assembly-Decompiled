#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class TribeDisplaySequence : MonoBehaviour
{
	[SerializeField]
	public string[] tribeNames;

	[SerializeField]
	public GameObject[] displays;

	public void Run(string className)
	{
		for (int i = 0; i < tribeNames.Length; i++)
		{
			if (tribeNames[i] == className)
			{
				Run(i);
			}
		}
	}

	public void Run(int classIndex)
	{
		for (int i = 0; i < displays.Length; i++)
		{
			displays[i].gameObject.SetActive(i == classIndex);
		}

		base.gameObject.SetActive(value: true);
	}
}
