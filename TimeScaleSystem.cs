#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class TimeScaleSystem : GameSystem
{
	public void OnEnable()
	{
		Events.OnTimeScaleChange += TimeScaleSet;
	}

	public void OnDisable()
	{
		Events.OnTimeScaleChange -= TimeScaleSet;
	}

	public void TimeScaleSet(float value)
	{
		Time.timeScale = value;
	}
}
