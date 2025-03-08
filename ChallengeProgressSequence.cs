#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChallengeProgressSequence : MonoBehaviour
{
	[Serializable]
	public struct Profile
	{
		public float scale;

		public Color color;
	}

	[SerializeField]
	public ChallengeProgressDisplay progressPrefab;

	[SerializeField]
	public Profile[] profiles;

	[SerializeField]
	public Transform progressGroup;

	public readonly List<ChallengeProgressDisplay> panels = new List<ChallengeProgressDisplay>();

	public bool running { get; set; }

	public IEnumerator Start()
	{
		running = true;
		ChallengeProgressSystem challengeProgressSystem = UnityEngine.Object.FindObjectOfType<ChallengeProgressSystem>();
		if ((object)challengeProgressSystem != null)
		{
			List<ChallengeProgress> progress = challengeProgressSystem.progress;
			if (progress != null)
			{
				foreach (ChallengeProgress item in progress.Where((ChallengeProgress a) => a.currentValue > a.originalValue))
				{
					yield return AddDisplay(item);
					yield return new WaitForSeconds(0.5f);
				}
			}
		}

		running = false;
	}

	public IEnumerator AddDisplay(ChallengeProgress progress)
	{
		ChallengeProgressDisplay panel = UnityEngine.Object.Instantiate(progressPrefab, progressGroup);
		panels.Insert(0, panel);
		UpdatePanelPositions();
		ChallengeData challengeData = AddressableLoader.Get<ChallengeData>("ChallengeData", progress.challengeName);
		panel.Assign(challengeData);
		panel.SetFill(progress.originalValue, challengeData.goal);
		yield return new WaitForSeconds(0.5f);
		int fillTo = Mathf.Min(challengeData.goal, progress.currentValue);
		float a2 = Mathf.Clamp((float)fillTo / (float)challengeData.goal, 0f, 1f) * 2f;
		a2 = Mathf.Lerp(a2, 1f, 0.5f);
		Events.InvokeProgressStart((float)progress.originalValue / (float)challengeData.goal);
		panel.animator.SetBool("Increasing", value: true);
		LeanTween.value(panel.gameObject, progress.originalValue, fillTo, a2).setEaseOutQuad().setOnUpdate(delegate(float a)
		{
			panel.SetFill(a, challengeData.goal);
			panel.SetRemainingText(challengeData, a);
		});
		yield return new WaitForSeconds(a2);
		panel.animator.SetBool("Increasing", value: false);
		Events.InvokeProgressStop();
		if (fillTo >= challengeData.goal)
		{
			Events.InvokeProgressDing();
			panel.animator.SetTrigger("Ping");
			panel.SetRewardText(challengeData);
			yield return new WaitForSeconds(1.5f);
		}
	}

	public void UpdatePanelPositions()
	{
		float num = 0f;
		int num2 = 0;
		foreach (ChallengeProgressDisplay panel in panels)
		{
			Profile profile = profiles[Mathf.Min(num2, profiles.Length - 1)];
			panel.SetColor(profile.color);
			if (num2 > 0)
			{
				LeanTween.cancel(panel.gameObject);
				LeanTween.moveLocal(panel.gameObject, new Vector3(0f, num, 0f), 0.2f).setEase(LeanTweenType.easeOutQuint);
				LeanTween.scale(panel.gameObject, Vector3.one * profile.scale, 0.2f).setEase(LeanTweenType.easeOutQuint);
				panel.icon.gameObject.SetActive(value: false);
				panel.back.gameObject.SetActive(value: false);
				panel.progressText.gameObject.SetActive(value: false);
			}

			num -= 1.3f * profile.scale;
			num2++;
		}
	}
}
