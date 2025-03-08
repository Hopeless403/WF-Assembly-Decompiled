#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dead;
using FMODUnity;
using UnityEngine;
using UnityEngine.Localization;

public class GainBlessingSequence2 : UISequence
{
	[Serializable]
	public struct SelectPrefab
	{
		public BossRewardData.Type bossRewardType;

		public BossRewardSelect prefab;
	}

	[SerializeField]
	public Transform rewardGroup;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString titleKey;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString chooseKey;

	[SerializeField]
	public EventReference selectSfxEvent;

	[SerializeField]
	public UnityEngine.Animator animator;

	[SerializeField]
	public ParticleSystem buildUpPS;

	[SerializeField]
	public ParticleSystem explodePS;

	[SerializeField]
	public HandleSpinner handleSpinner;

	[SerializeField]
	public GameObject openTrigger;

	[SerializeField]
	public GameObject closedEye;

	[SerializeField]
	public GameObject openEye;

	[Header("Prefabs")]
	[SerializeField]
	public SelectPrefab[] selectPrefabs;

	[Header("Positioning")]
	[SerializeField]
	public float spacing = 2.5f;

	[SerializeField]
	public float randX = 0.1f;

	[SerializeField]
	public float randY = 1f;

	[Header("SFX")]
	[SerializeField]
	public EventReference appearSfxEvent;

	[SerializeField]
	public EventReference openSfxEvent;

	public CampaignNodeTypeBoss.RewardData rewardData;

	public bool canOpen;

	public bool open;

	public int taken;

	public override IEnumerator Run()
	{
		CampaignNode campaignNode = Campaign.FindCharacterNode(References.Player);
		rewardData = campaignNode.data.Get<CampaignNodeTypeBoss.RewardData>("rewards");
		List<BossRewardData.Data> rewardsToCreate = rewardData.rewards;
		if (rewardsToCreate != null && rewardsToCreate.Count > 0)
		{
			SfxSystem.OneShot(appearSfxEvent);
			CinemaBarSystem.In();
			UpdateCinemaBarPrompt();
			canOpen = true;
			while (!open)
			{
				yield return null;
			}

			UpdateCinemaBarPrompt();
			CreateRewards(rewardsToCreate);
			while (!promptEnd)
			{
				yield return null;
			}

			CinemaBarSystem.Out();
		}
	}

	public void StartOpen()
	{
		if (!open && canOpen)
		{
			canOpen = false;
			openTrigger.SetActive(value: false);
			StartCoroutine(OpenRoutine());
		}
	}

	public IEnumerator OpenRoutine()
	{
		CinemaBarSystem.Top.RemovePrompt();
		SfxSystem.OneShot(openSfxEvent);
		closedEye.SetActive(value: false);
		openEye.SetActive(value: true);
		animator.SetBool("Open", value: true);
		handleSpinner.Spin();
		Events.InvokeScreenRumble(0f, 0.5f, 0f, 0.05f, 0.25f, 0.05f);
		yield return new WaitForSeconds(0.75f);
		handleSpinner.Stop();
		yield return new WaitForSeconds(0.25f);
		Events.InvokeScreenRumble(0f, 1f, 0f, 0.5f, 0.5f, 0.1f);
		buildUpPS.Play();
		yield return new WaitForSeconds(1f);
		Events.InvokeScreenShake(1.5f, 0f);
		explodePS.Play();
		open = true;
		animator.gameObject.Destroy();
	}

	public void UpdateCinemaBarPrompt()
	{
		string text = (open ? chooseKey.GetLocalizedString().Format(rewardData.canTake - taken) : titleKey.GetLocalizedString());
		CinemaBarSystem.Top.SetPrompt(text, "Select");
	}

	public void CreateRewards(List<BossRewardData.Data> rewardsToCreate)
	{
		UnityEngine.Random.InitState(Campaign.FindCharacterNode(References.Player).seed);
		float num = (0f - spacing) * (float)(rewardsToCreate.Count - 1) * 0.5f;
		int num2 = PettyRandom.Choose<int>(1, -1);
		foreach (BossRewardData.Data item in rewardsToCreate)
		{
			CreateReward(item, num, 0f, num2);
			num += spacing;
			if (PettyRandom.Range(0f, 1f) > 0.2f)
			{
				num2 *= -1;
			}
		}
	}

	public void CreateReward(BossRewardData.Data reward, float startX, float startY, float ySign)
	{
		BossRewardSelect bossRewardSelect = UnityEngine.Object.Instantiate(selectPrefabs.FirstOrDefault((SelectPrefab a) => a.bossRewardType == reward.type).prefab, rewardGroup);
		float x = startX + PettyRandom.Range(0f - randX, randX);
		float y = startY + PettyRandom.Range(0f, randY) * ySign;
		bossRewardSelect.transform.localPosition = PettyRandom.Vector3();
		LeanTween.moveLocal(bossRewardSelect.gameObject, new Vector3(x, y), PettyRandom.Range(1f, 2f)).setEaseOutElastic();
		bossRewardSelect.SetUp(reward, this);
	}

	public void Select(BossRewardData.Data reward)
	{
		if (!promptEnd)
		{
			reward.Select();
			if (++taken >= rewardData.canTake)
			{
				promptEnd = true;
			}
			else
			{
				UpdateCinemaBarPrompt();
			}

			SfxSystem.OneShot(selectSfxEvent);
		}
	}
}
