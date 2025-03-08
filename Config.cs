#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using UnityEngine;

public class Config : MonoBehaviourSingleton<Config>
{
	[Serializable]
	public class Data
	{
		public string version;

		public string versionNotation;

		public string versionFormat;

		public bool beta;
	}

	[SerializeField]
	public TextAsset configFile;

	public static Data _data;

	public static Data data => _data ?? (_data = JsonUtility.FromJson<Data>(MonoBehaviourSingleton<Config>.instance.configFile.text));

	public override void Awake()
	{
		base.Awake();
		Debug.Log(string.Format(data.versionFormat, data.versionNotation) + " Build: " + data.version);
	}
}
