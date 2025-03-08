#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using TMPro;
using UnityEngine;

public class StatNumberDisplay : MonoBehaviour
{
	[SerializeField]
	public bool disableIfZero = true;

	[SerializeField]
	public string statName = "damageDealt";

	[SerializeField]
	public string statKey = "basic";

	[SerializeField]
	public TMP_Text text;

	public void OnEnable()
	{
		int num = OverallStatsSystem.Get().Get(statName, statKey, 0);
		if (disableIfZero && num <= 0)
		{
			base.gameObject.SetActive(value: false);
		}
		else
		{
			text.text = num.ToString();
		}
	}
}
