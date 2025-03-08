#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using FMODUnity;
using Steamworks.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class ScoreSequence : MonoBehaviour
{
	[SerializeField]
	public TMP_Text timeText;

	[SerializeField]
	public TMP_Text timeScoreText;

	[SerializeField]
	public TMP_Text battlesText;

	[SerializeField]
	public TMP_Text battlesScoreText;

	[SerializeField]
	public TMP_Text goldText;

	[SerializeField]
	public TMP_Text goldScoreText;

	[SerializeField]
	public TMP_Text totalScoreText;

	[SerializeField]
	public TMP_Text globalRankText;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString totalStringRef;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString submittingStringRef;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString failedToSubmitStringRef;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString globalRankStringRef;

	[Header("SFX")]
	[SerializeField]
	public EventReference countSfx;

	[SerializeField]
	public EventReference countFinishSfx;

	[SerializeField]
	public EventReference countFinishFirstSfx;

	public bool running { get; set; }

	public IEnumerator Start()
	{
		yield return Sequence();
	}

	public void OnDisable()
	{
		StopAllCoroutines();
	}

	public IEnumerator Sequence()
	{
		running = true;
		totalScoreText.gameObject.SetActive(value: false);
		globalRankText.gameObject.SetActive(value: false);
		int submittedTime = ScoreSubmitSystem.SubmittedTime;
		int gold = ScoreSubmitSystem.SubmittedGold;
		int battlesWon = ScoreSubmitSystem.SubmittedBattlesWon;
		timeText.text = TimeSpan.FromSeconds(submittedTime).ToString();
		battlesText.text = battlesWon.ToString();
		goldText.text = gold.ToString();
		timeScoreText.text = "";
		battlesScoreText.text = "";
		goldScoreText.text = "";
		int scoreFromTime = ScoreSubmitSystem.GetScoreFromTime(References.Campaign.result == Campaign.Result.Win, submittedTime);
		Debug.Log($"Time Taken: {timeText.text} = {scoreFromTime} points");
		yield return CountScoreUp(timeScoreText, "+{0}", "<#f40>{0}", scoreFromTime);
		int scoreFromBattlesWon = ScoreSubmitSystem.GetScoreFromBattlesWon(battlesWon);
		Debug.Log($"Battles Won: {battlesText.text} = {scoreFromBattlesWon} points");
		yield return CountScoreUp(battlesScoreText, "+{0}", "<#f40>{0}", scoreFromBattlesWon);
		int scoreFromGold = ScoreSubmitSystem.GetScoreFromGold(gold);
		Debug.Log($"Blings: {goldText.text} = {scoreFromGold} points");
		yield return CountScoreUp(goldScoreText, "+{0}", "<#f40>{0}", scoreFromGold);
		totalScoreText.gameObject.SetActive(value: true);
		int submittedScore = ScoreSubmitSystem.SubmittedScore;
		Debug.Log($"Final Score: {submittedScore}");
		string localizedString = totalStringRef.GetLocalizedString();
		string positiveFormat = string.Format(localizedString, "<#ff0>{0}");
		string negativeFormat = string.Format(localizedString, "<#f40>{0}");
		yield return CountScoreUp(totalScoreText, positiveFormat, negativeFormat, submittedScore);
		if (!countFinishSfx.IsNull)
		{
			SfxSystem.OneShot(countFinishSfx);
		}

		LeaderboardUpdate? result = ScoreSubmitSystem.result;
		if (result.HasValue && result.GetValueOrDefault().Changed)
		{
			int newGlobalRank = ScoreSubmitSystem.result.Value.NewGlobalRank;
			if (newGlobalRank > 0)
			{
				Debug.Log($"Global Rank: {newGlobalRank}");
				globalRankText.gameObject.SetActive(value: true);
				globalRankText.text = string.Format(globalRankStringRef.GetLocalizedString(), newGlobalRank);
				if (newGlobalRank == 1 && !countFinishFirstSfx.IsNull)
				{
					SfxSystem.OneShot(countFinishFirstSfx);
				}
			}
		}

		running = false;
	}

	public IEnumerator SetSubmitScoreText()
	{
		globalRankText.gameObject.SetActive(value: true);
		if (ScoreSubmitSystem.status == ScoreSubmitSystem.Status.Submitting)
		{
			string text = submittingStringRef.GetLocalizedString();
			globalRankText.text = text;
			float t = 1f;
			string dots = "";
			yield return null;
			while (ScoreSubmitSystem.status == ScoreSubmitSystem.Status.Submitting)
			{
				t -= Time.deltaTime;
				if (t <= 0f)
				{
					t += 1f;
					dots = ((dots.Length < 3) ? (dots + ".") : "");
					globalRankText.text = text + dots;
				}

				yield return null;
			}
		}

		switch (ScoreSubmitSystem.status)
		{
			case ScoreSubmitSystem.Status.Failed:
				globalRankText.text = failedToSubmitStringRef.GetLocalizedString();
				break;
			case ScoreSubmitSystem.Status.Success:
			if (ScoreSubmitSystem.playerRank.HasValue)
			{
				int value = ScoreSubmitSystem.playerRank.Value;
				Debug.Log($"Global Rank: {value}");
				globalRankText.text = string.Format(globalRankStringRef.GetLocalizedString(), value);
				if (value == 1 && !countFinishFirstSfx.IsNull)
				{
					SfxSystem.OneShot(countFinishFirstSfx);
				}
				}
			else
			{
				globalRankText.gameObject.SetActive(value: false);
				}
	
				break;
		}
	}

	public IEnumerator CountScoreUp(TMP_Text element, string positiveFormat, string negativeFormat, int toScore, int fromScore = 0)
	{
		int v = fromScore;
		element.text = Format(positiveFormat, negativeFormat, fromScore);
		float time = Mathf.Clamp((float)toScore / 100f, 1f, 3f);
		float t = 0f;
		yield return null;
		while (t < time)
		{
			t += Time.deltaTime;
			int num = fromScore + Mathf.RoundToInt(Mathf.Min(1f, t / time) * (float)(toScore - fromScore));
			element.text = Format(positiveFormat, negativeFormat, num);
			if (v != num && !countSfx.IsNull)
			{
				SfxSystem.OneShot(countSfx);
			}

			v = num;
			yield return null;
		}
	}

	public static string Format(string positive, string negative, int value)
	{
		if (value < 0)
		{
			return string.Format(negative, value);
		}

		return string.Format(positive, value);
	}
}
