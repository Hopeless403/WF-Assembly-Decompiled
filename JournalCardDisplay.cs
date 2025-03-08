#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class JournalCardDisplay : MonoBehaviour
{
	[Serializable]
	public struct PositionProfile
	{
		public RectTransform[] transforms;

		public Vector3[] anchoredPositions;

		public void Set()
		{
			for (int i = 0; i < transforms.Length; i++)
			{
				transforms[i].anchoredPosition = anchoredPositions[i];
			}
		}
	}

	[SerializeField]
	public CardInspector inspector;

	[SerializeField]
	public PositionProfile normalPositionProfile;

	[SerializeField]
	public PositionProfile bigPositionProfile;

	[Header("Card Info Elements")]
	[SerializeField]
	public TMP_Text nameText;

	[SerializeField]
	public ImageSprite typeIcon;

	[SerializeField]
	public TMP_Text typeText;

	public CanvasGroup _canvasGroup;

	public Card current;

	public readonly Vector3 from = new Vector3(0.75f, 0.75f, 0.75f);

	public CanvasGroup canvasGroup => _canvasGroup ?? (_canvasGroup = base.gameObject.GetOrAdd<CanvasGroup>());

	public void OnDisable()
	{
		if ((bool)current)
		{
			nameText.text = "";
			typeIcon.enabled = false;
			typeText.text = "";
			inspector.ClearPopups();
			CardManager.ReturnToPoolNextFrame(current);
			current = null;
		}
	}

	public void Display(CardData cardData)
	{
		if ((bool)current)
		{
			if ((bool)current.entity.data && (bool)cardData && current.entity.data.name == cardData.name)
			{
				return;
			}

			CardManager.ReturnToPool(current);
		}

		current = CardManager.Get(cardData, null, null, inPlay: false, isPlayerCard: true);
		current.transform.SetParent(base.transform);
		if (current.entity.height == 2)
		{
			bigPositionProfile.Set();
		}
		else
		{
			normalPositionProfile.Set();
		}

		StopAllCoroutines();
		StartCoroutine(UpdateCard(current));
	}

	public IEnumerator UpdateCard(Card card)
	{
		yield return card.UpdateData();
		nameText.text = card.entity.data.title;
		typeIcon.enabled = true;
		typeIcon.SetSprite(card.entity.data.cardType.icon);
		typeText.text = card.entity.data.cardType.title;
		card.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
		card.transform.localScale = Vector3.one;
		inspector.ClearPopups();
		inspector.CreatePopups(card.entity);
		LeanTween.cancel(base.gameObject);
		base.transform.localScale = from;
		LeanTween.scale(base.gameObject, Vector3.one, 0.2f).setEaseOutBack().setIgnoreTimeScale(useUnScaledTime: true);
		canvasGroup.alpha = 0f;
		LeanTween.alphaCanvas(canvasGroup, 1f, 0.2f).setEaseOutBack().setIgnoreTimeScale(useUnScaledTime: true);
	}
}
