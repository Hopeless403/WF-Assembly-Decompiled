#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class StatDisplay : MonoBehaviour
{
	[SerializeField]
	public TMP_Text textElement;

	[SerializeField]
	public LocalizeStringEvent localizeStringEvent;

	public string statValue;

	public void Assign(GameStatData statData, string stringValue)
	{
		statValue = stringValue;
		localizeStringEvent.StringReference = statData.stringKey;
	}

	public void SetText(string text)
	{
		textElement.text = text.Replace("{0}", "<#fff>" + statValue);
	}
}
