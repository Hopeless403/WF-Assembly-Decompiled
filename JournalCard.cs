#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JournalCard : MonoBehaviour
{
	[SerializeField]
	public TMP_Text nameText;

	[SerializeField]
	public Image background;

	[SerializeField]
	public Image image;

	[SerializeField]
	public Button button;

	[SerializeField]
	public Material discoveredMaterial;

	[SerializeField]
	public Image frame;

	[SerializeField]
	public Sprite[] frames;

	public CardData cardData;

	public bool discovered;

	public void SetCardArt(CardData cardData, float scale)
	{
		this.cardData = cardData;
		background.sprite = cardData.backgroundSprite;
		image.sprite = cardData.mainSprite;
		background.transform.localScale = Vector3.one * scale;
		image.transform.localScale = Vector3.one * scale;
		CreatedByLookup.TryGetCreatedByRoot(cardData.name, out var rootCardDataName);
		int frameLevel = CardFramesSystem.GetFrameLevel(rootCardDataName);
		frame.sprite = frames[frameLevel];
	}

	public void CheckDiscovered(List<string> discoveredCards, JournalCardManager manager)
	{
		if (!discovered && discoveredCards.Contains(cardData.name))
		{
			SetDiscovered(cardData.title, manager);
		}
	}

	public void SetDiscovered(string title, JournalCardManager manager)
	{
		discovered = true;
		nameText.text = title;
		background.enabled = true;
		image.material = discoveredMaterial;
		image.color = Color.white;
		button.onClick.AddListener(delegate
		{
			manager.Select(cardData);
		});
	}
}
