#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "RewardData", menuName = "Reward Data")]
public class RewardData : ScriptableObject
{
	public string title;

	public string description;

	public Sprite icon;

	public GameObject buttonPrefab;

	public UISequence selectionScreenPrefab;

	public string acquireScript;

	public string[] acquireScriptArgs;

	public Sprite setCampaignNodeIcon;
}
