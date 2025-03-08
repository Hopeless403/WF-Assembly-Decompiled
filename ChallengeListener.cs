#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Challenge Listener", menuName = "Town/Challenge Listener")]
public class ChallengeListener : DataFile
{
	public enum CheckType
	{
		MidRun,
		EndOfRun,
		CustomSystem
	}

	public CheckType checkType;

	[ShowIf("CheckTypeCustomSystem")]
	public string systemName;

	[HideIf("CheckTypeCustomSystem")]
	public string stat;

	[HideIf("CheckTypeCustomSystem")]
	public bool hasKey;

	[HideIf("CheckTypeCustomSystem")]
	[ShowIf("hasKey")]
	public string key;

	[HideIf("CheckTypeCustomSystem")]
	public int target;

	public bool CheckTypeCustomSystem => checkType == CheckType.CustomSystem;

	public virtual bool Check(string stat, string key)
	{
		if (stat == this.stat)
		{
			if (hasKey)
			{
				return key == this.key;
			}

			return true;
		}

		return false;
	}

	public virtual bool CheckComplete(CampaignStats stats)
	{
		return stats.Get(stat, hasKey ? key : "", 0) >= target;
	}

	public virtual void Set(string challengeName, int oldValue, int newValue)
	{
		Add(challengeName, newValue - oldValue);
	}

	public static void Add(string challengeName, int value)
	{
		ChallengeProgressSystem.AddProgress(challengeName, value);
	}

	public void AddCustomSystem(ChallengeData challengeData, ChallengeSystem challengeSystem)
	{
		if (checkType == CheckType.CustomSystem)
		{
			Component component = challengeSystem.gameObject.AddComponentByName(systemName);
			if (component is ChallengeListenerSystem challengeListenerSystem)
			{
				challengeListenerSystem.Assign(challengeData, challengeSystem);
			}
			else
			{
				component.Destroy();
			}
		}
	}
}
