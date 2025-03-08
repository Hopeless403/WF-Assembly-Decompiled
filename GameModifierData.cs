#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using Dead;
using FMODUnity;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Modifier", menuName = "Modifiers/Game Modifier")]
public class GameModifierData : DataFile
{
	public int value = 100;

	public bool visible = true;

	[ShowAssetPreview(64, 64)]
	public Sprite bellSprite;

	[ShowAssetPreview(64, 64)]
	public Sprite dingerSprite;

	public UnityEngine.Localization.LocalizedString titleKey;

	public UnityEngine.Localization.LocalizedString descriptionKey;

	public string[] systemsToAdd;

	public Script[] setupScripts;

	public Script[] startScripts;

	public int scriptPriority;

	public GameModifierData[] blockedBy;

	public HardModeModifierData linkedStormBell;

	[SerializeField]
	public EventReference ringSfxEvent;

	[SerializeField]
	public Vector2 ringSfxPitch = new Vector2(1f, 1f);

	public void PlayRingSfx()
	{
		if (!ringSfxEvent.IsNull)
		{
			SfxSystem.OneShot(ringSfxEvent).setPitch(ringSfxPitch.PettyRandom() * PettyRandom.Range(0.95f, 1.05f));
		}
	}
}
