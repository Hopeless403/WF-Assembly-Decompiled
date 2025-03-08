#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class HandOverlay : MonoBehaviour
{
	[SerializeField]
	[ReadOnly]
	public Character owner;

	[SerializeField]
	public CardContainer drawContainer;

	[SerializeField]
	public CardContainer handContainer;

	[SerializeField]
	public CardContainer discardContainer;

	[SerializeField]
	public TweenUI showTween;

	[SerializeField]
	public TweenUI hideTween;

	[SerializeField]
	public SpriteSetter[] spriteSetters;

	[Header("Shadow For Battle Phase")]
	[SerializeField]
	public CanvasGroup shadow;

	[SerializeField]
	public LeanTweenType shadowFadeEase = LeanTweenType.easeInOutQuart;

	[SerializeField]
	public float shadowFadeDur = 1f;

	public bool shadowActive;

	public void Start()
	{
		Events.OnBattlePhaseStart += BattlePhaseStart;
		Events.OnSettingChanged += SettingChanged;
		UpdateVisibility(Settings.Load("HideHandOverlay", defaultValue: false));
	}

	public void OnDestroy()
	{
		Events.OnBattlePhaseStart -= BattlePhaseStart;
		Events.OnSettingChanged -= SettingChanged;
	}

	public void BattlePhaseStart(Battle.Phase phase)
	{
		if (phase == Battle.Phase.Battle)
		{
			if (!shadowActive)
			{
				LeanTween.cancel(shadow.gameObject);
				shadow.LeanAlpha(1f, shadowFadeDur).setEase(shadowFadeEase);
				shadowActive = true;
			}
		}
		else if (shadowActive)
		{
			LeanTween.cancel(shadow.gameObject);
			shadow.LeanAlpha(0f, shadowFadeDur).setEase(shadowFadeEase);
			shadowActive = false;
		}
	}

	public void SettingChanged(string key, object value)
	{
		if (key == "HideHandOverlay" && value is bool)
		{
			bool hidden = (bool)value;
			UpdateVisibility(hidden);
		}
	}

	public void UpdateVisibility(bool hidden)
	{
		base.gameObject.SetActive(!hidden);
	}

	public void SetOwner(Character character)
	{
		owner = character;
		drawContainer.owner = owner;
		handContainer.owner = owner;
		discardContainer.owner = owner;
		owner.drawContainer = drawContainer;
		owner.handContainer = handContainer;
		owner.discardContainer = discardContainer;
		handContainer.SetSize(owner.data.handSize, handContainer.CardScale);
		SpriteSetter[] array = spriteSetters;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Set(character.data.classData.name);
		}
	}

	public IEnumerator Show()
	{
		SfxSystem.OneShot("event:/sfx/inventory/showup");
		showTween.Fire();
		yield return Sequences.Wait(showTween.GetDuration());
	}

	public IEnumerator Hide()
	{
		hideTween.Fire();
		yield return Sequences.Wait(hideTween.GetDuration());
	}
}
