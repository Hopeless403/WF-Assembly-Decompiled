#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using UnityEngine;

[Serializable]
public class BlessingData
{
	public GameModifierData modifierToAdd;

	public void Select()
	{
		ModifierSystem.AddModifier(Campaign.Data, modifierToAdd);
		Routine.Clump clump = new Routine.Clump();
		Script[] startScripts = modifierToAdd.startScripts;
		foreach (Script script in startScripts)
		{
			clump.Add(script.Run());
		}

		startScripts = modifierToAdd.setupScripts;
		foreach (Script script2 in startScripts)
		{
			clump.Add(script2.Run());
		}

		string[] systemsToAdd = modifierToAdd.systemsToAdd;
		foreach (string text in systemsToAdd)
		{
			Debug.Log($"[{modifierToAdd}] adding system: {text}");
			Campaign.instance.gameObject.AddComponentByName(text);
		}
	}
}
