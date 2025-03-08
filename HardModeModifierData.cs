#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Modifiers/Hard Mode Modifier", fileName = "Hard Mode Modifier")]
public class HardModeModifierData : ScriptableObject
{
	public GameModifierData modifierData;

	public int stormPoints;

	public bool unlockedByDefault;

	[HideIf("unlockedByDefault")]
	public HardModeModifierData[] unlockRequires;

	[HideIf("unlockedByDefault")]
	public int unlockRequiresPoints;
}
