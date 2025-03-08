#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Town/Challenge", fileName = "Challenge")]
public class ChallengeData : DataFile
{
	public bool hidden;

	public UnityEngine.Localization.LocalizedString titleKey;

	public UnityEngine.Localization.LocalizedString textKey;

	public UnityEngine.Localization.LocalizedString rewardKey;

	public int goal;

	public ChallengeListener listener;

	public Sprite icon;

	public ChallengeData[] requires;

	public UnlockData reward;

	public string text => textKey.GetLocalizedString();

	public string rewardText => rewardKey.GetLocalizedString();
}
