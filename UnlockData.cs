#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Town/Unlock", fileName = "Unlock")]
public class UnlockData : DataFile
{
	public enum Type
	{
		None,
		BuildingStarted,
		BuildingFinished,
		Item,
		Pet,
		Tribe,
		Companion,
		Event,
		JournalPage,
		Charm
	}

	[SerializeField]
	public bool active = true;

	[ShowIf("active")]
	public bool activeInDemo = true;

	[ShowIf("active")]
	public bool activeInPressDemo = true;

	public Type type;

	public BuildingType relatedBuilding;

	public float lowPriority;

	public UnlockData[] requires;

	public UnityEngine.Localization.LocalizedString unlockTitle;

	public UnityEngine.Localization.LocalizedString unlockDesc;

	public bool IsActive => active;
}
