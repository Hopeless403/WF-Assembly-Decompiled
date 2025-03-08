#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using Deadpan.Enums.Engine.Components.Modding;
using UnityEngine;

public class DataFile : ScriptableObject
{
	[NonSerialized]
	public WildfrostMod ModAdded;

	public override bool Equals(object other)
	{
		return (other as UnityEngine.Object).GetInstanceID() == GetInstanceID();
	}

	public override int GetHashCode()
	{
		return base.name.GetHashCode();
	}
}
