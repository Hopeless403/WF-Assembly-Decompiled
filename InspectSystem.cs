#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InspectSystem : GameSystem
{
	[SerializeField]
	public GameObject container;

	[SerializeField]
	public Transform cardHolder;

	[SerializeField]
	public string openInput = "Inspect";

	[SerializeField]
	public string[] closeInputs = new string[3] { "Select", "Back", "Inspect" };

	[SerializeField]
	public string inspectCharmsInput = "Options";

	[SerializeField]
	public float cardScale = 1f;

	[SerializeField]
	public Vector2 cardRandomAngle = new Vector2(0f, 2f);

	[SerializeField]
	public List<Entity> hover = new List<Entity>();

	[SerializeField]
	public bool idleAnimation = true;

	[SerializeField]
	public float idleAnimationFactor = 1f;

	[SerializeField]
	public float canInspectDelay = 0.1f;

	[SerializeField]
	public float canEndDelay = 0.1f;

	[SerializeField]
	public KeywordData injuredKeyword;

	public float wait;

	[Header("Inspect Charms")]
	[SerializeField]
	public GameObject inspectCharmsLayout;

	[SerializeField]
	public InspectCharmsSystem inspectCharmsSystem;

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
	public RectTransform notePopGroup;

	[SerializeField]
	public RectTransform[] overflowOrder;

	[SerializeField]
	public CardPopUpPanel popUpPrefab;

	[SerializeField]
	public CardTooltip cardTooltipPrefab;

	[SerializeField]
	public LayoutGroup[] layoutsToFix;

	[Header("Fading")]
	[SerializeField]
	public CanvasGroup backgroundFade;

	[SerializeField]
	public CanvasGroup cardInfoFade;

	[SerializeField]
	public float fadeInDur = 0.2f;

	[SerializeField]
	public float fadeOutDur = 0.1f;

	public float fade;

	[Header("Card Info Elements")]
	[SerializeField]
	public TMP_Text nameText;

	[SerializeField]
	public ImageSprite typeIcon;

	[SerializeField]
	public TMP_Text typeText;

	[Header("Tribe Flag")]
	[SerializeField]
	public Image flagImage;

	public Entity drag;

	public Entity press;

	public Transform previousParent;

	public int previousChildIndex;

	public const float enableAnimationDelay = 0.1f;

	public float enableAnimationTimer;

	public float currentIdleAnimationFactor;

	public bool hasAnyCharms;

	public readonly List<Tooltip> popups = new List<Tooltip>();

	public readonly List<KeywordData> currentPoppedKeywords = new List<KeywordData>();

	public Entity inspect { get; set; }

	public static bool IsActive()
	{
		InspectSystem inspectSystem = Object.FindObjectOfType<InspectSystem>();
		if ((bool)inspectSystem)
		{
			return inspectSystem.inspect;
		}

		return false;
	}

	public void OnEnable()
	{
		Events.OnEntityHover += EntityHover;
		Events.OnEntityUnHover += EntityUnHover;
		Events.OnEntityDrag += EntityDrag;
		Events.OnEntityRelease += EntityRelease;
		Events.OnEntityDestroyed += EntityUnHover;
	}

	public void OnDisable()
	{
		Events.OnEntityHover -= EntityHover;
		Events.OnEntityUnHover -= EntityUnHover;
		Events.OnEntityDrag -= EntityDrag;
		Events.OnEntityRelease -= EntityRelease;
		Events.OnEntityDestroyed -= EntityUnHover;
	}

	public void Update()
	{
		if ((bool)inspect)
		{
			if (wait <= 0f && !MonoBehaviourSingleton<Cursor3d>.instance.usingMouse)
			{
				if (hasAnyCharms && !inspectCharmsSystem.gameObject.activeSelf && InputSystem.IsButtonPressed(inspectCharmsInput))
				{
					inspectCharmsSystem.Show();
				}

				if (!inspectCharmsSystem.gameObject.activeSelf && closeInputs.Any((string i) => InputSystem.IsButtonPressed(i)))
				{
					InspectEnd();
				}
			}
		}
		else if (!press)
		{
			if (!GameManager.paused && wait <= 0f && !drag && hover.Count == 1 && InputSystem.IsButtonPressed(openInput))
			{
				press = hover[0];
			}
		}

		else if (!GameManager.paused && !InputSystem.IsButtonHeld(openInput))
		{
			if (hover.Count == 1 && hover[0] == press)
			{
				ActionInspect actionInspect = new ActionInspect(press, this);
				if (Events.CheckAction(actionInspect))
				{
					actionInspect.Process();
				}
			}

			press = null;
		}

		if (enableAnimationTimer > 0f)
		{
			enableAnimationTimer -= Time.deltaTime;
			if (enableAnimationTimer <= 0f)
			{
				EnableIdleAnimation();
			}
		}

		if (wait > 0f)
		{
			wait -= Time.deltaTime;
		}
	}

	public void Inspect(Entity entity)
	{
		inspect = entity;
		SetFlag();
		hasAnyCharms = entity.HasAnyCharms();
		inspectCharmsLayout.SetActive(hasAnyCharms);
		container.SetActive(value: true);
		StopAllCoroutines();
		StartCoroutine(FadeIn());
		nameText.text = entity.data.title;
		typeIcon.SetSprite(entity.data.cardType.icon);
		typeText.text = entity.data.cardType.title;
		CreatePopups();
		entity.ForceUnHover();
		previousParent = entity.transform.parent;
		previousChildIndex = entity.transform.GetSiblingIndex();
		entity.transform.SetParent(cardHolder, worldPositionStays: true);
		LeanTween.moveLocal(entity.gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeOutQuart);
		entity.wobbler?.WobbleRandom();
		LeanTween.scale(entity.gameObject, Vector3.one * cardScale, 0.67f).setEase(LeanTweenType.easeOutBack);
		float z = cardRandomAngle.PettyRandom().WithRandomSign();
		LeanTween.rotateLocal(entity.gameObject, new Vector3(0f, 0f, z), 1f).setEase(LeanTweenType.easeOutBack);
		if (idleAnimation)
		{
			enableAnimationTimer = 0.1f;
		}

		Events.InvokeInspect(entity);
		Events.InvokeEntityFocus(entity);
		wait = canEndDelay;
	}

	public void SetFlag()
	{
		ClassData @class = GetClass(inspect.data);
		if ((bool)@class)
		{
			flagImage.gameObject.SetActive(value: true);
			flagImage.sprite = @class.flag;
		}
		else
		{
			flagImage.gameObject.SetActive(value: false);
		}
	}

	public static ClassData GetClass(CardData cardData)
	{
		if (cardData.cardType.tag == "Enemy")
		{
			return null;
		}

		if (cardData.cardType.name == "Leader")
		{
			ClassData[] classes = References.Classes;
			foreach (ClassData classData in classes)
			{
				CardData[] leaders = classData.leaders;
				for (int j = 0; j < leaders.Length; j++)
				{
					if (leaders[j].name == cardData.name)
					{
						return classData;
					}
				}
			}
		}
		else
		{
			ClassData[] classes = References.Classes;
			foreach (ClassData classData2 in classes)
			{
				foreach (CardData item in classData2.startingInventory.deck)
				{
					if (item.name == cardData.name)
					{
						return classData2;
					}
				}

				RewardPool[] rewardPools = classData2.rewardPools;
				foreach (RewardPool rewardPool in rewardPools)
				{
					if (rewardPool.isGeneralPool)
					{
						continue;
					}

					foreach (DataFile item2 in rewardPool.list)
					{
						if (item2.name == cardData.name)
						{
							return classData2;
						}
					}
				}
			}
		}

		return null;
	}

	public void TryInspectEnd()
	{
		if (wait <= 0f)
		{
			InspectEnd();
		}
	}

	public void InspectEnd()
	{
		inspect.transform.parent = previousParent;
		inspect.transform.SetSiblingIndex(previousChildIndex);
		inspect.TweenToContainer();
		inspect.wobbler?.WobbleRandom();
		DisableIdleAnimation();
		StopAllCoroutines();
		StartCoroutine(FadeOut());
		Events.InvokeInspectEnd(inspect);
		inspect = null;
		wait = canInspectDelay;
	}

	public IEnumerator FadeIn()
	{
		UpdateFade(0f);
		LeanTween.cancel(base.gameObject);
		LeanTween.value(base.gameObject, fade, 1f, fadeInDur).setEase(LeanTweenType.easeOutQuad).setOnUpdate(UpdateFade);
		yield return null;
	}

	public IEnumerator FadeOut()
	{
		LeanTween.cancel(base.gameObject);
		LeanTween.value(base.gameObject, fade, 0f, fadeOutDur).setEase(LeanTweenType.easeOutQuad).setOnUpdate(UpdateFade);
		yield return fadeOutDur;
		yield return null;
		ClearPopups();
		container.SetActive(value: false);
	}

	public void UpdateFade(float value)
	{
		fade = value;
		backgroundFade.alpha = fade;
		cardInfoFade.alpha = fade;
	}

	public void CreatePopups()
	{
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

		foreach (KeywordData hiddenKeyword in inspect.GetHiddenKeywords())
		{
			Popup(hiddenKeyword, rightPopGroup);
		}

		List<CardData.StatusEffectStacks> injuries = inspect.data.injuries;
		if (injuries != null && injuries.Count > 0)
		{
			Popup(injuredKeyword, rightPopGroup);
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
		cardTooltip.Set(cardData);
		popups.Add(cardTooltip);
		foreach (KeywordData keyword in cardTooltip.keywords)
		{
			CardPopUpPanel value = Popup(keyword, group);
			cardTooltip.children.AddIfNotNull(value);
		}

		return cardTooltip;
	}

	public void EnableIdleAnimation()
	{
		if ((bool)inspect?.data?.idleAnimationProfile && inspect.display is Card card && idleAnimationFactor != 0f)
		{
			if ((bool)card.imageIdleAnimator)
			{
				card.imageIdleAnimator.FadeIn();
				card.imageIdleAnimator.strength *= idleAnimationFactor;
			}

			if ((bool)card.backgroundIdleAnimator)
			{
				card.backgroundIdleAnimator.FadeIn();
				card.backgroundIdleAnimator.strength *= idleAnimationFactor;
			}

			currentIdleAnimationFactor = idleAnimationFactor;
		}
	}

	public void DisableIdleAnimation()
	{
		if (inspect?.display is Card card)
		{
			if ((bool)card.imageIdleAnimator)
			{
				card.imageIdleAnimator.FadeOut();
				card.imageIdleAnimator.strength /= currentIdleAnimationFactor;
			}

			if ((bool)card.backgroundIdleAnimator)
			{
				card.backgroundIdleAnimator.FadeOut();
				card.backgroundIdleAnimator.strength /= currentIdleAnimationFactor;
			}
		}
	}

	public void EntityHover(Entity entity)
	{
		if (!hover.Contains(entity))
		{
			hover.Add(entity);
		}
	}

	public void EntityUnHover(Entity entity)
	{
		hover.Remove(entity);
	}

	public void EntityDrag(Entity entity)
	{
		drag = entity;
	}

	public void EntityRelease(Entity entity)
	{
		if (entity == drag)
		{
			drag = null;
		}
	}
}
