#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;

[Serializable]
public class ChallengeProgress
{
	public string challengeName;

	public int currentValue;

	public int originalValue;

	public ChallengeProgress()
	{
	}

	public ChallengeProgress(string challengeName, int value)
	{
		this.challengeName = challengeName;
		currentValue = value;
		originalValue = value;
	}
}
