#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class CharacterData
{
	public string title;

	public string race;

	public string gender;

	[Header("Part Indices")]
	public int headIndex;

	public int bodyIndex;

	public int weapon1Index;

	public int weapon2Index;

	public int eyesIndex;

	public int mouthIndex;

	public int noseIndex;

	public int eyebrowIndex;

	public int earIndex;

	public int hairBackIndex;

	public int hairTopIndex;

	public int beardIndex;

	public int markingsIndex;

	public int clothingColorIndex;

	public int hairColorIndex;

	public int eyeColorIndex;

	public int skinColorIndex;

	public int markingsColorIndex;

	[Header("Scale")]
	public Vector2 bodyScale = Vector2.one;

	public Vector2 headScale = Vector2.one;

	public Vector2 eyeScale = Vector2.one;

	public Vector2 eyebrowScale = Vector2.one;

	public Vector2 noseScale = Vector2.one;

	public Vector2 mouthScale = Vector2.one;

	public Vector2 earScale = Vector2.one;

	public Vector2 hairScale = Vector2.one;

	public void Randomize(CharacterType type, bool lockTitle = false, bool lockBody = false, bool lockHead = false, bool lockEyes = false, bool lockEyebrows = false, bool lockMouth = false, bool lockNose = false, bool lockEars = false, bool lockHair = false, bool lockHairBack = false, bool lockBeard = false, bool lockHairColour = false, bool lockEyeColour = false, bool lockSkinColour = false, bool lockClothingColour = false, bool lockWeapon = false, bool lockMarkings = false, bool lockMarkingsColour = false)
	{
		if (!lockTitle)
		{
			title = Names.Pull(race, gender);
		}

		SetRandomPrefab(lockBody, type, "Body", ref bodyIndex);
		SetRandomPrefab(lockHead, type, "Head", ref headIndex);
		SetRandomPrefab(lockWeapon, type, "Weapon1", ref weapon1Index);
		SetRandomPrefab(lockWeapon, type, "Weapon2", ref weapon2Index);
		SetRandomPrefab(lockEyes, type, "Eyes", ref eyesIndex);
		SetRandomSprite(lockEyebrows, type, "Eyebrows", ref eyebrowIndex);
		SetRandomSprite(lockMouth, type, "Mouth", ref mouthIndex);
		SetRandomSprite(lockNose, type, "Nose", ref noseIndex);
		SetRandomSprite(lockEars, type, "Ears", ref earIndex);
		SetRandomSprite(lockHair, type, "HairTop", ref hairTopIndex);
		SetRandomSprite(lockHairBack, type, "HairBack", ref hairBackIndex);
		SetRandomSprite(lockBeard, type, "Beard", ref beardIndex);
		SetRandomSprite(lockMarkings, type, "Markings", ref markingsIndex);
		SetRandomColorSet(lockHairColour, type, "HairColour", ref hairColorIndex);
		SetRandomColorSet(lockEyeColour, type, "EyeColour", ref eyeColorIndex);
		SetRandomColorSet(lockSkinColour, type, "SkinColour", ref skinColorIndex);
		SetRandomColorSet(lockClothingColour, type, "ClothingColour", ref clothingColorIndex);
		SetRandomColorSet(lockMarkingsColour, type, "MarkingColour", ref markingsColorIndex);
		SetScale(locked: false, type, "Body", ref bodyScale);
		SetScale(locked: false, type, "Head", ref headScale);
		SetScale(locked: false, type, "Eyes", ref eyeScale);
		SetScale(locked: false, type, "Mouth", ref mouthScale);
		SetScale(locked: false, type, "Nose", ref noseScale);
		SetScale(locked: false, type, "Eyebrows", ref eyebrowScale);
		SetScale(locked: false, type, "Ears", ref earScale);
		SetScale(locked: false, type, "Hair", ref hairScale);
	}

	public void SetRandomPrefab(bool locked, CharacterType type, string name, ref int index)
	{
		if (!locked)
		{
			CharacterType.PrefabGroup prefabGroup = type.prefabs.FirstOrDefault((CharacterType.PrefabGroup a) => a.name == name);
			if (prefabGroup != null)
			{
				index = prefabGroup.collection.RandomIndex();
			}
		}
	}

	public void SetRandomSprite(bool locked, CharacterType type, string name, ref int index)
	{
		if (!locked)
		{
			CharacterType.SpriteGroup spriteGroup = type.sprites.FirstOrDefault((CharacterType.SpriteGroup a) => a.name == name);
			if (spriteGroup != null)
			{
				index = spriteGroup.collection.RandomIndex();
			}
		}
	}

	public void SetRandomColorSet(bool locked, CharacterType type, string name, ref int index)
	{
		if (!locked)
		{
			CharacterType.ColorSetGroup colorSetGroup = type.colorSets.FirstOrDefault((CharacterType.ColorSetGroup a) => a.name == name);
			if (colorSetGroup != null)
			{
				index = colorSetGroup.collection.RandomIndex();
			}
		}
	}

	public void SetScale(bool locked, CharacterType type, string name, ref Vector2 scale)
	{
		if (!locked)
		{
			CharacterType.ScaleRange scaleRange = type.scales.FirstOrDefault((CharacterType.ScaleRange a) => a.name == name);
			if (scaleRange != null)
			{
				scale = scaleRange.Convert();
			}
		}
	}
}
