#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class NoTargetTextSystem : GameSystem
{
	public static NoTargetTextSystem instance;

	[SerializeField]
	public AnimationCurve shakeCurve;

	[SerializeField]
	public Vector3 shakeAmount = new Vector3(1f, 0f, 0f);

	[SerializeField]
	public Vector2 shakeDurationRange = new Vector2(0.3f, 0.4f);

	[SerializeField]
	public TMP_Text textElement;

	[SerializeField]
	public Vector3 textPopOffset = new Vector3(0f, 1.5f, -1f);

	[SerializeField]
	public EventReference sfxEvent;

	[Header("Text")]
	[SerializeField]
	public UnityEngine.Localization.LocalizedString noTargetToAttackText;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString noAlliesToHealText;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString noTargetForStatusText;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString noSpaceToSummonText;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString noCardsToDrawText;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString noAllyToBoostText;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString noEnemyToBoostText;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString requiresJunkText;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString cantSplitText;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString noSummonedAlliesText;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString playCrownCardsText;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString noAllyToAttackText;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString cantMoveText;

	public readonly Dictionary<NoTargetType, string> strings = new Dictionary<NoTargetType, string>();

	public void OnEnable()
	{
		instance = this;
		LocalizationSettings.SelectedLocaleChanged += LocaleChanged;
		PopulateStrings();
	}

	public void OnDisable()
	{
		LocalizationSettings.SelectedLocaleChanged -= LocaleChanged;
	}

	public void LocaleChanged(Locale locale)
	{
		PopulateStrings();
	}

	public void PopulateStrings()
	{
		strings.Clear();
		strings[NoTargetType.NoTargetToAttack] = noTargetToAttackText.GetLocalizedString();
		strings[NoTargetType.NoAllyToHeal] = noAlliesToHealText.GetLocalizedString();
		strings[NoTargetType.NoTargetForStatus] = noTargetForStatusText.GetLocalizedString();
		strings[NoTargetType.NoSpaceToSummon] = noSpaceToSummonText.GetLocalizedString();
		strings[NoTargetType.NoCardsToDraw] = noCardsToDrawText.GetLocalizedString();
		strings[NoTargetType.NoAllyToBoost] = noAllyToBoostText.GetLocalizedString();
		strings[NoTargetType.NoEnemyToBoost] = noEnemyToBoostText.GetLocalizedString();
		strings[NoTargetType.RequiresJunk] = requiresJunkText.GetLocalizedString();
		strings[NoTargetType.CantSplit] = cantSplitText.GetLocalizedString();
		strings[NoTargetType.NoSummonedAllies] = noSummonedAlliesText.GetLocalizedString();
		strings[NoTargetType.PlayCrownCardsFirst] = playCrownCardsText.GetLocalizedString();
		strings[NoTargetType.NoAllyToAttack] = noAllyToAttackText.GetLocalizedString();
		strings[NoTargetType.CantMove] = cantMoveText.GetLocalizedString();
	}

	public static bool Exists()
	{
		return instance;
	}

	public static IEnumerator Run(Entity entity, NoTargetType type, params object[] args)
	{
		return instance._Run(entity, type, args);
	}

	public IEnumerator _Run(Entity entity, NoTargetType type, params object[] args)
	{
		if (base.enabled)
		{
			yield return Sequences.WaitForAnimationEnd(entity);
			float num = shakeDurationRange.Random();
			entity.curveAnimator.Move(shakeAmount.WithX(shakeAmount.x.WithRandomSign()), shakeCurve, 1f, num);
			textElement.text = ((type == NoTargetType.None) ? "" : strings[type].Format(args));
			PopText(entity.transform.position);
			yield return new WaitForSeconds(num);
		}
	}

	public void PopText(Vector3 fromPos)
	{
		GameObject obj = textElement.gameObject;
		obj.SetActive(value: true);
		LeanTween.cancel(obj);
		obj.transform.position = fromPos;
		LeanTween.move(obj, fromPos + textPopOffset, 1.5f).setEaseOutElastic();
		textElement.color = textElement.color.WithAlpha(1f);
		LeanTween.value(obj, 1f, 0f, 0.2f).setDelay(1.3f).setOnUpdate(delegate(float a)
		{
			textElement.color = textElement.color.WithAlpha(a);
		})
			.setOnComplete((Action)delegate
			{
				obj.SetActive(value: false);
			});
		SfxSystem.OneShot(sfxEvent);
	}
}
