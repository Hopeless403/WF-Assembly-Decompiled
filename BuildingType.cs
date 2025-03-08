#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Building", menuName = "Town/Building")]
public class BuildingType : DataFile
{
	public UnityEngine.Localization.LocalizedString titleKey;

	public UnityEngine.Localization.LocalizedString helpKey;

	public Prompt.Emote.Type helpEmoteType = Prompt.Emote.Type.Explain;

	[Header("Progression")]
	public UnlockData started;

	public UnlockData finished;

	public UnlockData[] unlocks;

	public string unlockedCheckedKey;
}
