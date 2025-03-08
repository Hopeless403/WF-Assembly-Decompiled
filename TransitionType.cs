#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public abstract class TransitionType : MonoBehaviour
{
	public bool IsRunning { get; set; }

	public virtual IEnumerator In()
	{
		return null;
	}

	public virtual IEnumerator Out()
	{
		return null;
	}

	public TransitionType()
	{
	}
}
