#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class CardInspector : MonoBehaviour
{
	[SerializeField]
	public bool animatePopUps = true;

	[SerializeField]
	public bool ignoreTimeScale;

	[SerializeField]
	public bool showHiddenKeywords = true;

	[SerializeField]
	[ShowIf("showHiddenKeywords")]
	public KeywordData injuredKeyword;

	[Header("Pop up panels")]
	[SerializeField]
	public RectTransform leftPopGroup;

	[SerializeField]
	public RectTransform leftOverflowPopGroup;

	[SerializeField]
	public RectTransform rightPopGroup;

	[SerializeField]
	public RectTransform rightOverflowPopGroup;

	[SerializeField]
	public RectTransform bottomPopGroup;

	[SerializeField]
	public RectTransform[] overflowOrder;

	[SerializeField]
	public CardPopUpPanel popUpPrefab;

	[SerializeField]
	public CardTooltip cardTooltipPrefab;

	[SerializeField]
	public LayoutGroup[] layoutsToFix;

	public readonly List<Tooltip> popups = new List<Tooltip>();

	public readonly List<KeywordData> currentPoppedKeywords = new List<KeywordData>();

	public void CreatePopups(Entity inspect)
	{
		Events.InvokeEntityHover(inspect);
		CreateIconPopups(inspect.display.healthLayoutGroup, leftPopGroup);
		CreateIconPopups(inspect.display.damageLayoutGroup, rightPopGroup);
		CreateIconPopups(inspect.display.counterLayoutGroup, bottomPopGroup);
		if (inspect.display is Card card)
		{
			foreach (CardData mentionedCard in card.mentionedCards)
			{
				Popup(mentionedCard, rightPopGroup);
			}

			foreach (KeywordData keyword in card.keywords)
			{
				Popup(keyword, rightPopGroup);
			}
		}

		if (showHiddenKeywords)
		{
			foreach (KeywordData hiddenKeyword in inspect.GetHiddenKeywords())
			{
				Popup(hiddenKeyword, rightPopGroup);
			}

			List<CardData.StatusEffectStacks> injuries = inspect.data.injuries;
			if (injuries != null && injuries.Count > 0)
			{
				Popup(injuredKeyword, rightPopGroup);
			}
		}

		CoroutineManager.Start(FixLayoutsAfterFrame());
	}

	public void CreateIconPopups(RectTransform iconLayoutGroup, Transform popGroup)
	{
		CardPopUpTarget[] componentsInChildren = iconLayoutGroup.GetComponentsInChildren<CardPopUpTarget>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			KeywordData[] keywords = componentsInChildren[i].keywords;
			foreach (KeywordData keyword in keywords)
			{
				Popup(keyword, popGroup);
			}
		}
	}

	public void ClearPopups()
	{
		foreach (Tooltip popup in popups)
		{
			popup.gameObject.Destroy();
		}

		popups.Clear();
		currentPoppedKeywords.Clear();
	}

	public IEnumerator FixLayoutsAfterFrame()
	{
		yield return null;
		yield return FixLayouts();
	}

	public IEnumerator FixLayouts()
	{
		yield return null;
		LayoutGroup[] array = layoutsToFix;
		foreach (LayoutGroup layoutGroup in array)
		{
			if (!(layoutGroup is VerticalLayoutGroup layout))
			{
				if (layoutGroup is HorizontalLayoutGroup layout2)
				{
					layout2.FitToChildren();
				}
			}
			else
			{
				layout.FitToChildren();
			}
		}

		if (CheckOverflow(bottomPopGroup))
		{
			yield return FixLayouts();
		}
	}

	public bool CheckOverflow(params RectTransform[] checkCollide)
	{
		for (int i = 0; i < overflowOrder.Length - 1; i++)
		{
			RectTransform rectTransform = overflowOrder[i];
			if (rectTransform.childCount > 0 && CheckCollide(rectTransform, checkCollide))
			{
				Transform child = rectTransform.GetChild(rectTransform.childCount - 1);
				RectTransform parent = overflowOrder[i + 1];
				child.SetParent(parent);
				child.SetSiblingIndex(0);
				return true;
			}
		}

		return false;
	}

	public static bool CheckCollide(RectTransform target, IEnumerable<RectTransform> checks)
	{
		foreach (RectTransform check in checks)
		{
			if (RectOverlap(target, check))
			{
				Debug.Log($"[{target.rect}] Overlaps [{check.rect}]");
				return true;
			}
		}

		return false;
	}

	public static bool RectOverlap(RectTransform a, RectTransform b)
	{
		Vector3 position = a.position;
		Vector2 size = a.rect.size;
		Vector2 pivot = a.pivot;
		float x = position.x - pivot.x * size.x;
		float y = position.y - pivot.y * size.y;
		Rect rect = new Rect(x, y, size.x, size.y);
		Vector3 position2 = b.position;
		Vector2 size2 = b.rect.size;
		Vector2 pivot2 = b.pivot;
		float x2 = position2.x - pivot2.x * size2.x;
		float y2 = position2.y - pivot2.y * size2.y;
		Rect other = new Rect(x2, y2, size2.x, size2.y);
		return rect.Overlaps(other);
	}

	public CardPopUpPanel Popup(KeywordData keyword, Transform group)
	{
		if (!currentPoppedKeywords.Contains(keyword))
		{
			CardPopUpPanel cardPopUpPanel = Object.Instantiate(popUpPrefab, group);
			cardPopUpPanel.gameObject.name = keyword.name;
			cardPopUpPanel.animate = animatePopUps;
			cardPopUpPanel.ignoreTimeScale = ignoreTimeScale;
			cardPopUpPanel.Set(keyword);
			Events.InvokePopupPanelCreated(keyword, cardPopUpPanel);
			currentPoppedKeywords.Add(keyword);
			popups.Add(cardPopUpPanel);
			{
				foreach (KeywordData keyword2 in Text.GetKeywords(keyword.body))
				{
					CardPopUpPanel value = Popup(keyword2, group);
					cardPopUpPanel.children.AddIfNotNull(value);
				}

				return cardPopUpPanel;
			}
		}

		return null;
	}

	public CardTooltip Popup(CardData cardData, Transform group)
	{
		CardTooltip cardTooltip = Object.Instantiate(cardTooltipPrefab, group);
		cardTooltip.gameObject.name = cardData.name;
		cardTooltip.animate = animatePopUps;
		cardTooltip.ignoreTimeScale = ignoreTimeScale;
		cardTooltip.Set(cardData);
		popups.Add(cardTooltip);
		foreach (KeywordData keyword in cardTooltip.keywords)
		{
			CardPopUpPanel value = Popup(keyword, group);
			cardTooltip.children.AddIfNotNull(value);
		}

		return cardTooltip;
	}
}
