#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using TMPro;
using UnityEngine;

public class StormBellPointLimit : MonoBehaviour
{
	[SerializeField]
	public bool setOnAwake = true;

	[SerializeField]
	public TMP_Text text;

	public int _pointLimit;

	public int pointLimit
	{
		get
		{
			return _pointLimit;
		}
		set
		{
			_pointLimit = value;
			text.text = value.ToString();
		}
	}

	public void Awake()
	{
		if (setOnAwake)
		{
			int a = SaveSystem.LoadProgressData("maxStormPoints", 5);
			pointLimit = Mathf.Min(a, 10);
		}
	}
}
