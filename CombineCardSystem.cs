#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombineCardSystem : GameSystem
{
	[Serializable]
	public struct Combo
	{
		public string[] cardNames;

		public string resultingCardName;

		public bool AllCardsInDeck(CardDataList deck)
		{
			bool result = true;
			string[] array = cardNames;
			foreach (string cardName in array)
			{
				if (!HasCard(cardName, deck))
				{
					result = false;
					break;
				}
			}

			return result;
		}

		public bool HasCard(string cardName, CardDataList deck)
		{
			foreach (CardData item in deck)
			{
				if (item.name == cardName)
				{
					return true;
				}
			}

			return false;
		}
	}

	[SerializeField]
	public string combineSceneName;

	[SerializeField]
	public Combo[] combos;

	public void OnEnable()
	{
		Events.OnEntityEnterBackpack += EntityEnterBackpack;
	}

	public void OnDisable()
	{
		Events.OnEntityEnterBackpack -= EntityEnterBackpack;
	}

	public void EntityEnterBackpack(Entity entity)
	{
		Combo[] array = combos;
		for (int i = 0; i < array.Length; i++)
		{
			Combo combo = array[i];
			if (combo.cardNames.Contains(entity.data.name) && combo.AllCardsInDeck(References.PlayerData.inventory.deck))
			{
				StopAllCoroutines();
				StartCoroutine(CombineSequence(combo));
				break;
			}
		}
	}

	public IEnumerator CombineSequence(Combo combo)
	{
		CombineCardSequence combineSequence = null;
		yield return SceneManager.Load(combineSceneName, SceneType.Temporary, delegate(Scene scene)
		{
			combineSequence = scene.FindObjectOfType<CombineCardSequence>();
		});
		if ((bool)combineSequence)
		{
			yield return combineSequence.Run(combo.cardNames, combo.resultingCardName);
		}

		yield return SceneManager.Unload(combineSceneName);
	}
}
