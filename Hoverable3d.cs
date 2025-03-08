#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.Events;

public class Hoverable3d : GameSystem
{
	[SerializeField]
	public UnityEvent onHover;

	[SerializeField]
	public UnityEvent onUnHover;

	public void Hover()
	{
		onHover?.Invoke();
	}

	public void UnHover()
	{
		onUnHover?.Invoke();
	}
}
