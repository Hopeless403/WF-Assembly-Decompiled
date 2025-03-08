#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChallengeDisplayCreator : MonoBehaviour
{
	public ChallengeData challenge;

	[SerializeField]
	public ChallengeProgressDisplay displayPrefab;

	[SerializeField]
	public Transform displayParent;

	[SerializeField]
	public bool checkOnEnable = true;

	public void OnEnable()
	{
		if (checkOnEnable)
		{
			Check();
		}
	}

	public void Check()
	{
		if (!challenge || !challenge.reward.IsActive || !displayPrefab)
		{
			return;
		}

		List<string> list = SaveSystem.LoadProgressData<List<string>>("completedChallenges", null) ?? new List<string>();
		if (list.Contains(challenge.name))
		{
			return;
		}

		ChallengeData[] requires = challenge.requires;
		foreach (ChallengeData challengeData in requires)
		{
			if (!list.Contains(challengeData.name))
			{
				return;
			}
		}

		int num = (SaveSystem.LoadProgressData<List<ChallengeProgress>>("challengeProgress", null)?.FirstOrDefault((ChallengeProgress a) => a.challengeName == challenge.name))?.currentValue ?? 0;
		ChallengeProgressDisplay challengeProgressDisplay = Object.Instantiate(displayPrefab, displayParent ? displayParent : base.transform);
		challengeProgressDisplay.Assign(challenge);
		challengeProgressDisplay.SetFill(num, challenge.goal);
	}
}
