#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class CardPopUp : MonoBehaviourRectSingleton<CardPopUp>
{
	[SerializeField]
	public Canvas canvas;

	[SerializeField]
	public RectTransform bounds;

	[SerializeField]
	public RectTransform boundingBox;

	[SerializeField]
	public RectTransform layout;

	[SerializeField]
	public ContentSizeFitter sizeFitter;

	[SerializeField]
	public float gap = 0.05f;

	[SerializeField]
	[Range(-1f, 1f)]
	public float posX = 1f;

	[SerializeField]
	[Range(-1f, 1f)]
	public float posY;

	[Header("Tooltip Prefabs")]
	[SerializeField]
	public AssetReferenceGameObject keywordTooltipPrefab;

	[SerializeField]
	public int keywordTooltipInitialPool = 1;

	[SerializeField]
	public AssetReferenceGameObject cardTooltipPrefab;

	[SerializeField]
	public int cardTooltipInitialPool = 1;

	public readonly Dictionary<string, Tooltip> activePanels = new Dictionary<string, Tooltip>();

	public RectTransform follow;

	public readonly List<Tooltip> pool = new List<Tooltip>();

	public bool ignoreTimeScale;

	public void Awake()
	{
		for (int i = 0; i < keywordTooltipInitialPool; i++)
		{
			PoolPanel(keywordTooltipPrefab.InstantiateAsync(layout).WaitForCompletion().GetComponent<CardPopUpPanel>());
		}

		for (int j = 0; j < cardTooltipInitialPool; j++)
		{
			PoolPanel(cardTooltipPrefab.InstantiateAsync(layout).WaitForCompletion().GetComponent<CardTooltip>());
		}
	}

	public void Update()
	{
		if ((bool)follow)
		{
			Vector2 vector = follow.sizeDelta * follow.lossyScale * follow.pivot;
			Vector2 vector2 = layout.sizeDelta * layout.lossyScale * layout.pivot;
			float x = (vector.x + vector2.x + gap) * posX;
			float y = (vector.y + vector2.y + gap) * posY;
			Vector3 pos = follow.position + new Vector3(x, y, 0f);
			base.rectTransform.position = ApplyLimits(pos);
		}
		else
		{
			Clear();
		}
	}

	public static void SetCanvasLayer(string layerName, int orderInLayer)
	{
		MonoBehaviourRectSingleton<CardPopUp>.instance.canvas.sortingLayerName = layerName;
		MonoBehaviourRectSingleton<CardPopUp>.instance.canvas.sortingOrder = orderInLayer;
	}

	public static void SetIgnoreTimeScale(bool ignore)
	{
		MonoBehaviourRectSingleton<CardPopUp>.instance.ignoreTimeScale = ignore;
	}

	public static void Reset()
	{
		SetCanvasLayer("PopUp", 0);
		SetIgnoreTimeScale(ignore: false);
	}

	public static void AssignToCard(Card card)
	{
		MonoBehaviourRectSingleton<CardPopUp>.instance.follow = card.canvas.transform as RectTransform;
		MonoBehaviourRectSingleton<CardPopUp>.instance.posX = 1f;
		MonoBehaviourRectSingleton<CardPopUp>.instance.posY = 0f;
	}

	public static void AssignTo(RectTransform rect, float posX, float posY)
	{
		MonoBehaviourRectSingleton<CardPopUp>.instance.follow = rect;
		MonoBehaviourRectSingleton<CardPopUp>.instance.posX = posX;
		MonoBehaviourRectSingleton<CardPopUp>.instance.posY = posY;
	}

	public static CardPopUpPanel AddPanel(string name, string title, string body)
	{
		if (MonoBehaviourRectSingleton<CardPopUp>.instance.activePanels.ContainsKey(name))
		{
			return null;
		}

		CardPopUpPanel panel = MonoBehaviourRectSingleton<CardPopUp>.instance.GetPanel<CardPopUpPanel>();
		panel.gameObject.name = name;
		panel.ignoreTimeScale = MonoBehaviourRectSingleton<CardPopUp>.instance.ignoreTimeScale;
		panel.Set(title, body);
		MonoBehaviourRectSingleton<CardPopUp>.instance.activePanels.Add(name, panel);
		foreach (KeywordData keyword in Text.GetKeywords(body))
		{
			CardPopUpPanel value = AddPanel(keyword);
			panel.children.AddIfNotNull(value);
		}

		return panel;
	}

	public static CardPopUpPanel AddPanel(KeywordData keyword, string forceBody = null)
	{
		if (MonoBehaviourRectSingleton<CardPopUp>.instance.activePanels.ContainsKey(keyword.name))
		{
			return null;
		}

		CardPopUpPanel panel = MonoBehaviourRectSingleton<CardPopUp>.instance.GetPanel<CardPopUpPanel>();
		panel.gameObject.name = keyword.name;
		panel.ignoreTimeScale = MonoBehaviourRectSingleton<CardPopUp>.instance.ignoreTimeScale;
		panel.Set(keyword, forceBody);
		Events.InvokePopupPanelCreated(keyword, panel);
		MonoBehaviourRectSingleton<CardPopUp>.instance.activePanels.Add(keyword.name, panel);
		foreach (KeywordData keyword2 in Text.GetKeywords(keyword.body))
		{
			CardPopUpPanel value = AddPanel(keyword2);
			panel.children.AddIfNotNull(value);
		}

		return panel;
	}

	public static CardTooltip AddPanel(CardData cardData)
	{
		CardTooltip panel = MonoBehaviourRectSingleton<CardPopUp>.instance.GetPanel<CardTooltip>();
		panel.gameObject.name = cardData.name;
		panel.Set(cardData);
		MonoBehaviourRectSingleton<CardPopUp>.instance.activePanels.Add(cardData.name, panel);
		foreach (KeywordData keyword in panel.keywords)
		{
			CardPopUpPanel value = AddPanel(keyword);
			panel.children.AddIfNotNull(value);
		}

		return panel;
	}

	public static void RemovePanel(string name)
	{
		if (MonoBehaviourRectSingleton<CardPopUp>.instance.activePanels.TryGetValue(name, out var value))
		{
			foreach (Tooltip child in value.children)
			{
				RemovePanel(child.name);
			}

			MonoBehaviourRectSingleton<CardPopUp>.instance.PoolPanel(value);
			MonoBehaviourRectSingleton<CardPopUp>.instance.activePanels.Remove(name);
		}
		else
		{
			Debug.Log("Panel [" + name + "] does not exist (CardPopUp)");
		}

		MonoBehaviourRectSingleton<CardPopUp>.instance.StartFixLayouts();
	}

	public T GetPanel<T>() where T : Tooltip
	{
		Tooltip tooltip = pool.FirstOrDefault((Tooltip a) => a is T);
		if (!tooltip)
		{
			Type typeFromHandle = typeof(T);
			tooltip = ((typeFromHandle == typeof(CardTooltip)) ? ((Tooltip)cardTooltipPrefab.InstantiateAsync(layout).WaitForCompletion().GetComponent<CardTooltip>()) : ((Tooltip)keywordTooltipPrefab.InstantiateAsync(layout).WaitForCompletion().GetComponent<CardPopUpPanel>()));
		}
		else
		{
			pool.RemoveAt(pool.IndexOf(tooltip));
		}

		tooltip.gameObject.SetActive(value: true);
		tooltip.transform.SetAsLastSibling();
		return tooltip as T;
	}

	public void PoolPanel(Tooltip panel)
	{
		panel.children.Clear();
		panel.gameObject.SetActive(value: false);
		pool.Add(panel);
	}

	public static void Clear()
	{
		foreach (KeyValuePair<string, Tooltip> activePanel in MonoBehaviourRectSingleton<CardPopUp>.instance.activePanels)
		{
			activePanel.Value.gameObject.Destroy();
		}

		MonoBehaviourRectSingleton<CardPopUp>.instance.activePanels.Clear();
		MonoBehaviourRectSingleton<CardPopUp>.instance.follow = null;
	}

	public void StartFixLayouts()
	{
		if ((bool)sizeFitter)
		{
			StopAllCoroutines();
			StartCoroutine(FixLayouts());
		}
	}

	public IEnumerator FixLayouts()
	{
		yield return null;
		sizeFitter.enabled = false;
		yield return null;
		sizeFitter.enabled = true;
	}

	public Vector3 ApplyLimits(Vector3 pos)
	{
		Vector2 vector = Vector2.Scale(Vector2.Scale(bounds.sizeDelta, bounds.pivot) - Vector2.Scale(boundingBox.sizeDelta, boundingBox.pivot), base.rectTransform.lossyScale);
		return Vector2Ext.Clamp(pos, -vector, vector).WithZ(pos.z);
	}
}
