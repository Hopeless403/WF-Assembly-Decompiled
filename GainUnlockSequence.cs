#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class GainUnlockSequence : MonoBehaviour
{
	public TMP_Text titleElement;

	public TMP_Text descriptionElement;

	[Header("Displays")]
	[SerializeField]
	public GameObject constructionDisplay;

	[SerializeField]
	public GameObject petHutDisplay;

	[SerializeField]
	public GameObject inventorDisplay;

	[SerializeField]
	public GameObject icebreakerDisplay;

	[SerializeField]
	public GameObject tribeHallDisplay;

	[SerializeField]
	public GameObject hotSpringDisplay;

	[SerializeField]
	public GameObject frostoscopeDisplay;

	[SerializeField]
	public GameObject challengeShrineDisplay;

	[SerializeField]
	public GameObject newItemIcon;

	public GameObject display;

	public void SetUp(UnlockData unlockData)
	{
		SetTitle(unlockData);
		SetDisplay(unlockData);
		SetDescription(unlockData);
		base.gameObject.SetActive(value: true);
	}

	public void SetTitle(UnlockData unlockData)
	{
		string text = string.Empty;
		if (unlockData.relatedBuilding != null && unlockData.relatedBuilding.titleKey != null)
		{
			text = text + "<size=7.5>" + unlockData.relatedBuilding.titleKey.GetLocalizedString() + "</size>";
		}

		if (unlockData.unlockTitle != null)
		{
			if (!text.IsNullOrWhitespace())
			{
				text += "\n";
			}

			text = text + "<#C3967F>" + unlockData.unlockTitle.GetLocalizedString();
		}

		titleElement.text = text;
	}

	public void SetDisplay(UnlockData unlockData)
	{
		newItemIcon.SetActive(unlockData.type == UnlockData.Type.Item || unlockData.type == UnlockData.Type.Pet || unlockData.type == UnlockData.Type.Tribe);
		if (display != null)
		{
			display.SetActive(value: false);
		}

		if (unlockData.type == UnlockData.Type.BuildingStarted)
		{
			SetDisplay(constructionDisplay);
			return;
		}

		switch (unlockData.relatedBuilding.name)
		{
			case "PetHut":
				SetDisplay(petHutDisplay);
				break;
			case "TribeHut":
				SetDisplay(tribeHallDisplay);
				break;
			case "Icebreakers":
				SetDisplay(icebreakerDisplay);
				break;
			case "InventorHut":
				SetDisplay(inventorDisplay);
				break;
			case "HotSpring":
				SetDisplay(hotSpringDisplay);
				break;
			case "Frostoscope":
				SetDisplay(frostoscopeDisplay);
				break;
			case "ChallengeShrine":
				SetDisplay(challengeShrineDisplay);
				break;
		}
	}

	public void SetDisplay(GameObject type)
	{
		display = type;
		display.SetActive(value: true);
	}

	public void SetDescription(UnlockData unlockData)
	{
		UnityEngine.Localization.LocalizedString unlockDesc = unlockData.unlockDesc;
		if (unlockDesc != null && !unlockDesc.IsEmpty)
		{
			string localizedString = unlockData.unlockDesc.GetLocalizedString();
			descriptionElement.text = localizedString;
		}
		else
		{
			descriptionElement.text = "";
		}
	}

	public void Close()
	{
		base.gameObject.SetActive(value: false);
	}
}
