#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using UnityEngine;

public abstract class TargetConstraint : ScriptableObject
{
	[SerializeField]
	public bool not;

	public virtual bool Check(Entity target)
	{
		throw new NotImplementedException();
	}

	public virtual bool Check(CardData targetData)
	{
		throw new NotImplementedException();
	}

	public TargetConstraint()
	{
	}
}
