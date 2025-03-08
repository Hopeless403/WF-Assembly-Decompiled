#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class RecallChargeRedrawBellModifierSystem : GameSystem
{
	public RedrawBellSystem _redrawBellSystem;

	public RedrawBellSystem redrawBellSystem => _redrawBellSystem ?? (_redrawBellSystem = Object.FindObjectOfType<RedrawBellSystem>());

	public void OnEnable()
	{
		Events.OnDiscard += EntityDiscard;
	}

	public void OnDisable()
	{
		Events.OnDiscard -= EntityDiscard;
	}

	public void EntityDiscard(Entity entity)
	{
		if (entity.data.cardType.tag == "Friendly")
		{
			int counter = Mathf.Max(0, redrawBellSystem.counter.current - 2);
			redrawBellSystem.SetCounter(counter);
		}
	}
}
