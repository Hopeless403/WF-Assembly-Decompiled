#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GainCrownSequence : UISequence
{
	[Header("Custom Values")]
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
	public Image crownImage;

	[SerializeField]
	public CardUpgradeData defaultCrownData;

	public CardUpgradeData crownData;

	public void SetData(CardUpgradeData crownData)
	{
		this.crownData = crownData;
	}

	public override IEnumerator Run()
	{
		if (!crownData)
		{
			crownData = defaultCrownData.Clone();
		}

		crownImage.sprite = crownData.image;
		References.PlayerData.inventory.upgrades.Add(crownData);
		Events.InvokeUpgradeGained(crownData);
		fade.gameObject.SetActive(value: false);
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

		while (!promptEnd)
		{
			yield return null;
		}

		promptEnd = false;
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
	}

	public void CharacterOpenInventory()
	{
		if (References.Player.entity.display is CharacterDisplay characterDisplay)
		{
			characterDisplay.OpenInventory();
		}
	}
}
