#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Challenge Listener Highest", menuName = "Town/Challenge Listener Highest")]
public class ChallengeListenerHighest : ChallengeListener
{
	public override void Set(string challengeName, int oldValue, int newValue)
	{
		if (newValue >= target)
		{
			ChallengeListener.Add(challengeName, 1);
		}
	}
}
