#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using Dead;
using UnityEngine;
using UnityEngine.UI;

public class MetaprogressSequence : MonoBehaviour
{
	[SerializeField]
	public UnityEngine.Animator animator;

	[SerializeField]
	public GameObject tweener;

	[SerializeField]
	public float startDelay = 1f;

	[SerializeField]
	public float endDelay = 0.5f;

	[SerializeField]
	public Image progressImage;

	[SerializeField]
	public Vector2 randomizeProgress = new Vector2(0.9f, 1.1f);

	[SerializeField]
	public GainUnlockSequence gainUnlockSequence;

	[SerializeField]
	public ParticleSystem particleSystem;

	public float targetFillAmount;

	public const float progressPerParticle = 0.02f;

	public bool running { get; set; }

	public IEnumerator Start()
	{
		yield return Sequence();
	}

	public IEnumerator Sequence(float? amount = null)
	{
		running = true;
		yield return AddressableLoader.LoadGroup("UnlockData");
		List<string> alreadyUnlocked = MetaprogressionSystem.GetUnlockedList();
		List<UnlockData> remainingUnlocks = MetaprogressionSystem.GetRemainingUnlocks(alreadyUnlocked);
		Debug.Log(string.Format("{0} Remaining Unlocks: {1}", remainingUnlocks.Count, string.Join(", ", remainingUnlocks)));
		RemoveIneligibleUnlocks(remainingUnlocks, alreadyUnlocked);
		Debug.Log(string.Format("{0} Eligible Unlocks: {1}", remainingUnlocks.Count, string.Join(", ", remainingUnlocks)));
		Progression townProgress = SaveSystem.LoadProgressData("townProgress", new Progression(0f, 1f, 0f));
		progressImage.fillAmount = ((remainingUnlocks.Count > 0) ? townProgress.ProgressToNextUnlock() : 1f);
		if (remainingUnlocks.Count > 0)
		{
			yield return new WaitForSeconds(startDelay);
			float num = StatsSystem.Get().Count("battlesWon");
			float num2 = 7f;
			float num3;
			if (!amount.HasValue)
			{
				num3 = ((num > 0f) ? (Mathf.Round((num + 1f) / num2 * 3f * 3f) / 3f * 2f) : 0f);
			}
			else
			{
				float valueOrDefault = amount.GetValueOrDefault();
				num3 = valueOrDefault;
			}

			float num4 = num3 * randomizeProgress.Random();
			List<float> list = new List<float>();
			List<UnlockData> getUnlocks = new List<UnlockData>();
			float num5 = num4;
			while (remainingUnlocks.Count > 0)
			{
				float b = townProgress.required - townProgress.current;
				float num6 = Mathf.Min(num5, b);
				num5 -= num6;
				townProgress.current += num6;
				list.Add(townProgress.ProgressToNextUnlock());
				if (!townProgress.RequirementMet())
				{
					break;
				}

				townProgress.SetNextRequirement();
				UnlockData unlock = GetUnlock(remainingUnlocks);
				if ((object)unlock != null)
				{
					getUnlocks.Add(unlock);
					alreadyUnlocked.Add(unlock.name);
					MetaprogressionSystem.SetUnlocksReady(unlock.name);
					Debug.Log($"Unlocking {unlock}");
				}
			}

			SaveSystem.SaveProgressData("townProgress", townProgress);
			SaveSystem.SaveProgressData("unlocked", alreadyUnlocked);
			Debug.Log("Progressing: " + string.Join(", ", list));
			foreach (float item in list)
			{
				float progressToAdd = (targetFillAmount = item) - progressImage.fillAmount;
				if (progressToAdd > 0f)
				{
					Events.InvokeProgressStart(progressImage.fillAmount);
				}

				int particles = Mathf.CeilToInt(progressToAdd / 0.02f);
				if (particles > 0)
				{
					animator.SetBool("Increasing", value: true);
					while (particles > 0)
					{
						particleSystem.Emit(1);
						yield return new WaitForSeconds(Dead.Random.Range(0.02f, 0.03f));
						particles--;
					}
				}

				yield return new WaitUntil(() => !particleSystem || particleSystem.particleCount <= 0);
				Fill(targetFillAmount);
				animator.SetBool("Increasing", value: false);
				if (progressToAdd > 0f)
				{
					Events.InvokeProgressStop();
				}

				if (getUnlocks.Count > 0)
				{
					Events.InvokeProgressDing();
					Ping();
					getUnlocks.RemoveAt(0);
					yield return new WaitForSeconds(1.5f);
					if (getUnlocks.Count > 0 || remainingUnlocks.Count > 0)
					{
						progressImage.fillAmount = 0f;
					}
				}
			}
		}

		yield return new WaitForSeconds(endDelay);
		running = false;
	}

	public static UnlockData GetUnlock(IList<UnlockData> orderedList)
	{
		if (orderedList == null || orderedList.Count <= 0)
		{
			return null;
		}

		List<int> list = new List<int> { 0 };
		float lowPriority = orderedList[0].lowPriority;
		int count = orderedList.Count;
		for (int i = 1; i < count; i++)
		{
			if (orderedList[i].lowPriority <= lowPriority)
			{
				list.Add(i);
			}
		}

		int index = list.RandomItem();
		UnlockData result = orderedList[index];
		orderedList.RemoveAt(index);
		return result;
	}

	public static void RemoveIneligibleUnlocks(IList<UnlockData> remainingUnlocks, ICollection<string> alreadyUnlocked)
	{
		for (int num = remainingUnlocks.Count - 1; num >= 0; num--)
		{
			if (!MetaprogressionSystem.CheckUnlockRequirements(remainingUnlocks[num], alreadyUnlocked))
			{
				remainingUnlocks.RemoveAt(num);
			}
		}
	}

	public void OnParticleCollision(GameObject other)
	{
		if (!(other != particleSystem.gameObject))
		{
			float amount = ((Math.Abs(progressImage.fillAmount - targetFillAmount) < 0.02f) ? targetFillAmount : (progressImage.fillAmount + 0.02f));
			Fill(amount);
			Blip();
			Events.InvokeProgressBlip();
		}
	}

	public void Fill(float amount)
	{
		progressImage.fillAmount = amount;
		Events.InvokeProgressUpdate(amount);
	}

	public void Blip()
	{
		animator.SetTrigger("Blip");
	}

	public void Ping()
	{
		animator.SetTrigger("Ping");
	}
}
