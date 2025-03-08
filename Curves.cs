#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Curves : MonoBehaviourSingleton<Curves>
{
	public List<Curve> list;

	public static AnimationCurve Get(string name)
	{
		return MonoBehaviourSingleton<Curves>.instance.list.First((Curve a) => a.name == name).curve;
	}
}
