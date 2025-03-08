#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;

public static class NewFinalBossChecker
{
	public static bool Check()
	{
		return SaveSystem.LoadProgressData("newFinalBoss", defaultValue: false);
	}

	public static IEnumerator Run()
	{
		SaveSystem.SaveProgressData("newFinalBoss", value: false);
		InputSystem.Disable();
		yield return SceneManager.Load("NewFrostGuardian", SceneType.Temporary);
		InputSystem.Enable();
		yield return SceneManager.WaitUntilUnloaded("NewFrostGuardian");
	}
}
