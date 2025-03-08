#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAvatar : MonoBehaviour
{
	public AvatarPart root;

	public Texture2D paletteTexture;

	public Material recolourMaterial;

	public Material recolourMaterialCopy;

	public void OnDestroy()
	{
		if ((bool)paletteTexture)
		{
			Object.Destroy(paletteTexture);
		}

		if ((bool)recolourMaterialCopy)
		{
			Object.Destroy(recolourMaterialCopy);
		}
	}

	public void UpdateDisplay(CharacterData data)
	{
		CharacterType characterType = AssetLoader.Lookup<CharacterType>("CharacterTypes", data.race + data.gender);
		if (!characterType)
		{
			return;
		}

		Transform anchor = root.GetAnchor("body");
		if ((object)anchor == null)
		{
			return;
		}

		anchor.DestroyAllChildren();
		Object.Destroy(paletteTexture);
		GameObject prefab = GetPrefab(characterType, "Body", data.bodyIndex);
		if (!prefab)
		{
			return;
		}

		GameObject gameObject = Object.Instantiate(prefab, anchor);
		Transform obj = gameObject.transform;
		obj.localScale *= data.bodyScale;
		AvatarPart component = gameObject.GetComponent<AvatarPart>();
		if ((object)component != null)
		{
			GameObject prefab2 = GetPrefab(characterType, "Head", data.headIndex);
			if ((bool)prefab2)
			{
				Transform anchor2 = component.GetAnchor("head");
				if ((object)anchor2 != null)
				{
					GameObject gameObject2 = Object.Instantiate(prefab2, anchor2);
					gameObject2.transform.localScale = gameObject2.transform.localScale * data.headScale / gameObject.transform.localScale;
					AvatarPart component2 = gameObject2.GetComponent<AvatarPart>();
					if ((object)component2 != null)
					{
						Sprite sprite = GetSprite(characterType, "Mouth", data.mouthIndex);
						Sprite sprite2 = GetSprite(characterType, "Nose", data.noseIndex);
						Sprite sprite3 = GetSprite(characterType, "Eyebrows", data.eyebrowIndex);
						Sprite sprite4 = GetSprite(characterType, "Ears", data.earIndex);
						Sprite sprite5 = GetSprite(characterType, "HairTop", data.hairTopIndex);
						Sprite sprite6 = GetSprite(characterType, "HairBack", data.hairBackIndex);
						Sprite sprite7 = GetSprite(characterType, "Beard", data.beardIndex);
						Sprite sprite8 = GetSprite(characterType, "Markings", data.markingsIndex);
						SetSprite(characterType, component2, "mouth", sprite, data.mouthScale);
						SetSprite(characterType, component2, "nose", sprite2, data.noseScale);
						SetSprite(characterType, component2, "eyebrows", sprite3, data.eyebrowScale);
						SetSprite(characterType, component2, "ears", sprite4, data.earScale);
						SetSprite(characterType, component2, "hairtop", sprite5, data.hairScale);
						SetSprite(characterType, component2, "hairback", sprite6, data.hairScale);
						SetSprite(characterType, component2, "beard", sprite7, Vector2.one);
						SetSprite(characterType, component2, "markings", sprite8, Vector2.one);
						GameObject prefab3 = GetPrefab(characterType, "Eyes", data.eyesIndex);
						if ((bool)prefab3)
						{
							Transform anchor3 = component2.GetAnchor("eyes");
							if ((object)anchor3 != null)
							{
								Object.Instantiate(prefab3, anchor3);
							}
						}
					}
				}
			}

			GameObject prefab4 = GetPrefab(characterType, "Weapon1", data.weapon1Index);
			if ((object)prefab4 != null)
			{
				Transform anchor4 = component.GetAnchor("weapon1");
				if ((object)anchor4 != null)
				{
					Object.Instantiate(prefab4, anchor4);
					goto IL_0318;
				}
			}

			GameObject prefab5 = GetPrefab(characterType, "Weapon2", data.weapon2Index);
			if ((object)prefab5 != null)
			{
				Transform anchor5 = component.GetAnchor("weapon2");
				if ((object)anchor5 != null)
				{
					Object.Instantiate(prefab5, anchor5);
				}
			}
		}

		goto IL_0318;
		IL_0318:
		paletteTexture = new Texture2D(25, 1, TextureFormat.RGBA32, mipChain: false)
		{
			filterMode = FilterMode.Point
		};
		NativeArray<Color32> rawTextureData = paletteTexture.GetRawTextureData<Color32>();
		ColorSet colorSet = GetColorSet(characterType, "ClothingColour", data.clothingColorIndex);
		ColorSet colorSet2 = GetColorSet(characterType, "HairColour", data.hairColorIndex);
		ColorSet colorSet3 = GetColorSet(characterType, "EyeColour", data.eyeColorIndex);
		ColorSet colorSet4 = GetColorSet(characterType, "SkinColour", data.skinColorIndex);
		ColorSet colorSet5 = GetColorSet(characterType, "MarkingColour", data.markingsColorIndex);
		SetTexturePixels(characterType, paletteTexture, rawTextureData, 0, colorSet);
		SetTexturePixels(characterType, paletteTexture, rawTextureData, 4, colorSet2);
		SetTexturePixels(characterType, paletteTexture, rawTextureData, 13, colorSet3);
		SetTexturePixels(characterType, paletteTexture, rawTextureData, 14, colorSet4);
		SetTexturePixels(characterType, paletteTexture, rawTextureData, 17, colorSet5);
		paletteTexture.Apply();
		if ((bool)recolourMaterialCopy)
		{
			Object.Destroy(recolourMaterialCopy);
		}

		recolourMaterialCopy = Object.Instantiate(recolourMaterial);
		recolourMaterialCopy.SetTexture("_PaletteTex", paletteTexture);
		Image[] componentsInChildren = GetComponentsInChildren<Image>();
		foreach (Image image in componentsInChildren)
		{
			if (!image.TryGetComponent<Mask>(out var _))
			{
				image.material = recolourMaterialCopy;
			}
		}
	}

	public static GameObject GetPrefab(CharacterType type, string name, int index)
	{
		if (index >= 0)
		{
			CharacterType.PrefabGroup prefabGroup = type.prefabs.FirstOrDefault((CharacterType.PrefabGroup a) => a.name == name);
			if (prefabGroup != null)
			{
				return prefabGroup.collection[index];
			}
		}

		return null;
	}

	public static Sprite GetSprite(CharacterType type, string name, int index)
	{
		if (index >= 0)
		{
			CharacterType.SpriteGroup spriteGroup = type.sprites.FirstOrDefault((CharacterType.SpriteGroup a) => a.name == name);
			if (spriteGroup != null)
			{
				return spriteGroup.collection[index];
			}
		}

		return null;
	}

	public static ColorSet GetColorSet(CharacterType type, string name, int index)
	{
		if (index >= 0)
		{
			CharacterType.ColorSetGroup colorSetGroup = type.colorSets.FirstOrDefault((CharacterType.ColorSetGroup a) => a.name == name);
			if (colorSetGroup != null)
			{
				return colorSetGroup.collection[index];
			}
		}

		return null;
	}

	public static void SetSprite(CharacterType type, AvatarPart part, string partName, Sprite sprite, Vector2 scale)
	{
		AvatarPart.Part part2 = part.Get(partName);
		if (!sprite)
		{
			part2.Disable();
		}
		else
		{
			part2.Set(sprite, scale);
		}
	}

	public static void SetSprite(CharacterType type, AvatarPart part, Sprite sprite, Vector2 scale, params string[] partNames)
	{
		foreach (string text in partNames)
		{
			AvatarPart.Part part2 = part.Get(text);
			if (!sprite)
			{
				part2.Disable();
			}
			else
			{
				part2.Set(sprite, scale);
			}
		}
	}

	public static void SetTexturePixels(CharacterType type, Texture2D texture, NativeArray<Color32> data, int startX, ColorSet colorSet)
	{
		Color[] set = colorSet.set;
		foreach (Color32 value in set)
		{
			data[startX++] = value;
		}
	}
}
