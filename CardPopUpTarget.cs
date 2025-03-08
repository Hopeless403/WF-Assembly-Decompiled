#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class CardPopUpTarget : MonoBehaviourRect
{
	[SerializeField]
	[ReadOnly]
	public bool popped;

	[SerializeField]
	public Card card;

	[HideIf("IsCard")]
	public KeywordData[] keywords;

	[SerializeField]
	[HideIf("IsCard")]
	[Range(-1f, 1f)]
	public float posX = 1f;

	[SerializeField]
	[HideIf("IsCard")]
	[Range(-1f, 1f)]
	public float posY;

	public readonly HashSet<string> current = new HashSet<string>();

	public bool IsCard => card != null;

	public void Pop()
	{
		if (IsCard)
		{
			CardPopUp.AssignToCard(card);
			if (card.mentionedCards != null)
			{
				foreach (CardData mentionedCard in card.mentionedCards)
				{
					if (current.Add(mentionedCard.name))
					{
						CardPopUp.AddPanel(mentionedCard);
					}
				}
			}

			if (card.keywords != null)
			{
				foreach (KeywordData keyword in card.keywords)
				{
					if (current.Add(keyword.name))
					{
						CardPopUp.AddPanel(keyword);
					}
				}
			}
		}
		else if (keywords.Length != 0)
		{
			CardPopUp.AssignTo(base.rectTransform, posX, posY);
			KeywordData[] array = keywords;
			foreach (KeywordData keywordData2 in array)
			{
				if (current.Add(keywordData2.name))
				{
					CardPopUp.AddPanel(keywordData2);
				}
			}
		}

		popped = true;
	}

	public void UnPop()
	{
		if (current.Count > 0)
		{
			foreach (string item in current)
			{
				CardPopUp.RemovePanel(item);
			}

			current.Clear();
		}

		popped = false;
	}

	public void OnDisable()
	{
		current.Clear();
	}
}
