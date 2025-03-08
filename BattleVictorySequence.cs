#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class BattleVictorySequence : MonoBehaviour
{
	public CanvasGroup fade;

	public float fadeTime = 0.5f;

	public Transform rewardContainer;

	public CardContainer injuredContainer;

	public RectTransform injuredPanel;

	[SerializeField]
	public float startDelay;

	[SerializeField]
	public GameObject winLayout;

	[SerializeField]
	public GameObject injuriesLayout;

	[SerializeField]
	public GameObject[] disableGapsForInjuries;

	[SerializeField]
	public GameObject continueLayout;

	public bool active;

	public IEnumerator Run()
	{
		active = true;
		yield return new WaitForSeconds(startDelay);
		winLayout.SetActive(value: true);
		yield return new WaitForSeconds(0.75f);
		yield return RevealInjuries();
		continueLayout.SetActive(value: true);
		yield return new WaitUntil(() => !active);
	}

	public IEnumerator RevealInjuries()
	{
		CardData[] injuriesThisBattle = InjurySystem.GetInjuriesThisBattle();
		if (injuriesThisBattle != null && injuriesThisBattle.Length > 0)
		{
			SfxSystem.OneShot("event:/sfx/ui/injuries_showup");
			injuredContainer.SetSize(injuriesThisBattle.Length, 0.6f);
			CardData[] array = injuriesThisBattle;
			for (int i = 0; i < array.Length; i++)
			{
				Card card = CardManager.Get(array[i], null, null, inPlay: false, isPlayerCard: true);
				injuredContainer.Add(card.entity);
				yield return card.UpdateData();
			}

			injuredPanel.sizeDelta = injuredContainer.GetComponent<RectTransform>().sizeDelta + new Vector2(6f, 1.5f);
			injuredContainer.SetChildPositions();
			GameObject[] array2 = disableGapsForInjuries;
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j].SetActive(value: false);
			}

			injuriesLayout.SetActive(value: true);
			yield return new WaitForSeconds(0.75f);
		}
	}

	public bool CharacterDeckpackOpen(Character character)
	{
		bool result = false;
		if (character.entity?.display != null && character.entity.display is CharacterDisplay characterDisplay)
		{
			result = characterDisplay.IsDeckpackOpen;
		}

		return result;
	}

	public void End()
	{
		active = false;
	}
}
