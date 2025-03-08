#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class DrawExtraWhenRedrawNotChargedSystem : MonoBehaviour
{
	public const int extraDraw = 3;

	public RedrawBellSystem _redrawBellSystem;

	public RedrawBellSystem redrawBellSystem => _redrawBellSystem ?? (_redrawBellSystem = Object.FindObjectOfType<RedrawBellSystem>());

	public void OnEnable()
	{
		Events.OnGetHandSize += GetHandSize;
	}

	public void OnDisable()
	{
		Events.OnGetHandSize -= GetHandSize;
	}

	public void GetHandSize(ref int handSize)
	{
		if (!redrawBellSystem.IsCharged)
		{
			handSize += 3;
		}
	}
}
