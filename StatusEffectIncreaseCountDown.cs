#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Increase Count Down", fileName = "Increase Count Down")]
public class StatusEffectIncreaseCountDown : StatusEffectData
{
	public override void Init()
	{
		Events.OnEntityCountDown += EntityCountDown;
	}

	public void OnDestroy()
	{
		Events.OnEntityCountDown -= EntityCountDown;
	}

	public void EntityCountDown(Entity entity, ref int amount)
	{
		if (entity == target)
		{
			amount += GetAmount();
		}
	}
}
