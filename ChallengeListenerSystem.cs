#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public abstract class ChallengeListenerSystem : GameSystem
{
	public ChallengeData challengeData;

	public ChallengeSystem challengeSystem;

	public void Assign(ChallengeData challengeData, ChallengeSystem challengeSystem)
	{
		this.challengeData = challengeData;
		this.challengeSystem = challengeSystem;
	}

	public void Complete()
	{
		ChallengeProgressSystem.AddProgress(challengeData.name, 1);
		challengeSystem.SetAsComplete(challengeData);
		Object.Destroy(this);
	}

	public ChallengeListenerSystem()
	{
	}
}
