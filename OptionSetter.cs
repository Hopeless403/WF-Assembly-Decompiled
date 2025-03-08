#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using TMPro;
using UnityEngine;

public class OptionSetter : MonoBehaviour
{
	[SerializeField]
	public GameObject[] options;

	public bool init;

	public void Start()
	{
		if (!init)
		{
			TMP_Dropdown component = GetComponent<TMP_Dropdown>();
			if ((object)component != null)
			{
				Set(component.value);
			}
		}
	}

	public void Set(int index)
	{
		for (int i = 0; i < options.Length; i++)
		{
			options[i].SetActive(i == index);
		}

		if (!init)
		{
			init = true;
		}
	}
}
