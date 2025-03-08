#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Deadpan.Enums.Engine.Components.Modding;
using FMODUnity;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class JournalCardManager : MonoBehaviour
{
	[SerializeField]
	public JournalCardManagerPopulator populator;

	[SerializeField]
	public string[] categoryNames = new string[3] { "Friendly", "Item", "Clunker" };

	[SerializeField]
	public JournalCard cardPrefab;

	[SerializeField]
	public Transform content;

	[SerializeField]
	public Scroller scroller;

	[SerializeField]
	public JournalCardDisplay cardDisplay;

	[SerializeField]
	public Button[] tabs;

	[SerializeField]
	public GameObject[] tabSelected;

	[SerializeField]
	public RewiredHotKeyController hotKeyTabLeft;

	[SerializeField]
	public RewiredHotKeyController hotKeyTabRight;

	[SerializeField]
	public EventReference selectCardSfxEvent;

	public List<string> discovered;

	public int currentCategory;

	public readonly List<AsyncOperationHandle<CardData>> handles = new List<AsyncOperationHandle<CardData>>();

	public readonly List<JournalCard> cardIcons = new List<JournalCard>();

	public Locale locale;

	public static readonly Dictionary<string, float> scaleOverride = new Dictionary<string, float> { { "FinalBoss2", 1.3f } };

	public bool requiresRebuild;

	public bool subbed;

	public void OnEnable()
	{
		if (!subbed)
		{
			Events.OnModLoaded += ModToggled;
			Events.OnModUnloaded += ModToggled;
			subbed = true;
		}

		if (requiresRebuild)
		{
			if (!populator.populated)
			{
				populator.Populate();
			}

			CreateCards(currentCategory);
			requiresRebuild = false;
			StartCoroutine(ScrollToTop());
			return;
		}

		discovered = SaveSystem.LoadProgressData<List<string>>("cardsDiscovered");
		if (discovered != null)
		{
			foreach (JournalCard cardIcon in cardIcons)
			{
				cardIcon.CheckDiscovered(discovered, this);
			}
		}
		else
		{
			discovered = new List<string>();
		}

		if (locale != null && locale != LocalizationSettings.SelectedLocale)
		{
			locale = LocalizationSettings.SelectedLocale;
			CreateCards(currentCategory);
		}
	}

	public void ModToggled(WildfrostMod mod)
	{
		requiresRebuild = true;
	}

	public void Start()
	{
		CreateCards(0);
		locale = LocalizationSettings.SelectedLocale;
	}

	public void CreateCards(string categoryName)
	{
		for (int i = 0; i < categoryNames.Length; i++)
		{
			if (categoryNames[i] == categoryName)
			{
				if (i != currentCategory)
				{
					CreateCards(i);
				}

				break;
			}
		}
	}

	public void CreateCards(int categoryIndex)
	{
		currentCategory = categoryIndex;
		content.DestroyAllChildren();
		cardIcons.Clear();
		foreach (KeyValuePair<string, CardData> item in LoadCardData(populator.GetCategory(categoryNames[categoryIndex])))
		{
			item.Deconstruct(out var key, out var value);
			string title = key;
			CardData cardData = value;
			JournalCard journalCard = Object.Instantiate(cardPrefab, content);
			cardIcons.Add(journalCard);
			float value2;
			float scale = (scaleOverride.TryGetValue(cardData.name, out value2) ? value2 : 1f);
			journalCard.SetCardArt(cardData, scale);
			if (discovered.Contains(cardData.name))
			{
				journalCard.SetDiscovered(title, this);
			}
		}

		StartCoroutine(ScrollToTop());
	}

	public static List<KeyValuePair<string, CardData>> LoadCardData(JournalCardManagerPopulator.Category category)
	{
		List<KeyValuePair<string, CardData>> list = new List<KeyValuePair<string, CardData>>();
		foreach (string cardName in category.cardNames)
		{
			CardData cardData = AddressableLoader.Get<CardData>("CardData", cardName);
			list.Add(new KeyValuePair<string, CardData>(cardData.title, cardData));
		}

		return list.OrderBy((KeyValuePair<string, CardData> a) => a.Key).ToList();
	}

	public IEnumerator ScrollToTop()
	{
		yield return new WaitForEndOfFrame();
		scroller.ScrollImmediate(100f);
	}

	public void Select(CardData cardData)
	{
		cardDisplay.Display(cardData);
		SfxSystem.OneShot(selectCardSfxEvent);
	}

	public void SelectTab(int index)
	{
		CreateCards(index);
		GameObject[] array = tabSelected;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}

		tabSelected[index].SetActive(value: true);
	}

	public void NextTab()
	{
		currentCategory++;
		if (currentCategory >= categoryNames.Length)
		{
			currentCategory -= categoryNames.Length;
		}

		SelectTab(currentCategory);
	}

	public void PreviousTab()
	{
		currentCategory--;
		if (currentCategory < 0)
		{
			currentCategory += categoryNames.Length;
		}

		SelectTab(currentCategory);
	}
}
