#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.Serialization;

public class PlatformSpecific : MonoBehaviour
{
	[SerializeField]
	public bool editor = true;

	[SerializeField]
	public bool windows = true;

	[SerializeField]
	public bool @switch = true;

	[SerializeField]
	public bool android = true;

	[SerializeField]
	public bool iOs = true;

	[FormerlySerializedAs("mustBeRelease")]
	[SerializeField]
	public bool release;

	[SerializeField]
	public bool demo;

	[SerializeField]
	public bool notDemo;

	public void Awake()
	{
		bool flag = true;
		if (!windows)
		{
			flag = false;
		}

		if (demo)
		{
			flag = false;
		}

		if (!flag)
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
