#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using TMPro;
using UnityEngine;

public class GainCharmSequence : UISequence
{
	[Header("Custom Values")]
	[SerializeField]
	public CardUpgradeData charmData;

	[SerializeField]
	public CardCharm charmPrefab;

	[SerializeField]
	public float charmScale = 2f;

	[SerializeField]
	public Vector2 charmPopUpOffset = new Vector2(1f, 0f);

	[SerializeField]
	public Transform charmHolder;

	[SerializeField]
	public CanvasGroup fade;

	[SerializeField]
	public float fadeInTime = 0.2f;

	[SerializeField]
	public float fadeOutTime = 0.2f;

	[SerializeField]
	public TweenUI[] startTweens;

	[SerializeField]
	public TweenUI[] endTweens;

	[SerializeField]
	public Character character;

	[SerializeField]
	public TMP_Text title;

	public CardCharmInteraction charmInteraction;

	public void SetCharm(CardUpgradeData charmData)
	{
		this.charmData = charmData;
	}

	public void SetCharacter(Character character)
	{
		this.character = character;
	}

	public void SetTitle(string text)
	{
		title.text = "<size=0.5><#A9AAD4>" + text + "</size></color>\n" + charmData.title;
	}

	public override IEnumerator Run()
	{
		character.data.inventory.upgrades.Add(charmData);
		Events.InvokeUpgradeGained(charmData);
		Campaign.PromptSave();
		fade.gameObject.SetActive(value: false);
		charmHolder.DestroyAllChildren();
		yield return Sequences.Wait(startDelay);
		base.gameObject.SetActive(value: true);
		fade.gameObject.SetActive(value: true);
		fade.alpha = 0f;
		fade.LeanAlpha(1f, fadeInTime);
		fade.blocksRaycasts = true;
		float a = 0f;
		TweenUI[] array = startTweens;
		foreach (TweenUI tweenUI in array)
		{
			tweenUI.Fire();
			a = Mathf.Max(a, tweenUI.GetDuration());
		}

		CardCharm cardCharm = Object.Instantiate(charmPrefab, charmHolder);
		cardCharm.holder = charmHolder;
		cardCharm.transform.localScale = Vector3.one * charmScale;
		cardCharm.SetData(charmData);
		charmInteraction = cardCharm.GetComponent<CardCharmInteraction>();
		if ((bool)charmInteraction)
		{
			charmInteraction.popUpOffset = charmPopUpOffset;
			charmInteraction.PopUpDescription();
		}

		while (!promptEnd)
		{
			yield return null;
		}

		promptEnd = false;
		charmInteraction.HideDescription();
		fade.LeanAlpha(0f, fadeOutTime);
		fade.blocksRaycasts = false;
		SfxSystem.OneShot("event:/sfx/ui/message_closing");
		float num = fadeOutTime;
		array = endTweens;
		foreach (TweenUI tweenUI2 in array)
		{
			tweenUI2.Fire();
			num = Mathf.Max(num, tweenUI2.GetDuration());
		}

		yield return Sequences.Wait(num);
		base.gameObject.SetActive(value: false);
		yield return WaitForInventoryClose();
	}

	public void CharacterOpenInventory()
	{
		if (character.entity.display is CharacterDisplay characterDisplay)
		{
			characterDisplay.OpenInventory();
		}
	}

	public static IEnumerator WaitForInventoryClose()
	{
		while (Deckpack.IsOpen)
		{
			yield return null;
		}
	}
}
