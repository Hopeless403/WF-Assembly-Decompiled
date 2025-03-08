#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Diagnostics;
using Deadpan.Enums.Engine.Components.Modding;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModHolder : MonoBehaviour
{
	public WildfrostMod Mod;

	public TextMeshProUGUI Title;

	public TextMeshProUGUI Description;

	public Image Icon;

	public GameObject PublishButton;

	public ToggleSprite[] toggleSprites;

	public BellRinger bellRinger;

	public bool DrawingUpload;

	public void OnEnable()
	{
		Events.OnModLoaded += ModToggled;
		Events.OnModUnloaded += ModToggled;
	}

	public void OnDisable()
	{
		Events.OnModLoaded -= ModToggled;
		Events.OnModUnloaded -= ModToggled;
	}

	public void ModToggled(WildfrostMod mod)
	{
		if (!(mod.GUID != Mod.GUID))
		{
			UpdateSprites();
		}
	}

	public void ToggleMod()
	{
		Mod.ModToggle();
		if (Mod.HasLoaded)
		{
			bellRinger.Ring();
		}
	}

	public void OnGUI()
	{
		if (DrawingUpload)
		{
			if (GUILayout.Button("Confirm publish for " + Mod.GUID))
			{
				Mod.UpdateOrPublishWorkshop();
				DrawingUpload = false;
			}
			else if (GUILayout.Button("Cancel publish for " + Mod.GUID))
			{
				DrawingUpload = false;
			}
		}
	}

	public void PublishMod()
	{
		DrawingUpload = true;
	}

	public void OpenModDirectory()
	{
		Process.Start(Mod.ModDirectory);
	}

	public void UpdateInfo()
	{
		Title.text = Mod.Title;
		Description.text = Mod.Description;
		Icon.sprite = Mod.IconSprite;
		PublishButton.SetActive(Mod.ModDirectory.Contains(Application.streamingAssetsPath));
		UpdateSprites();
	}

	public void UpdateSprites()
	{
		ToggleSprite[] array = toggleSprites;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Set(Mod.HasLoaded);
		}
	}
}
