#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

public class AddCharmSlotModifierSystem : GameSystem
{
	public void OnEnable()
	{
		Events.OnEntityEnterBackpack += EntityEnterBackpack;
	}

	public void OnDisable()
	{
		Events.OnEntityEnterBackpack -= EntityEnterBackpack;
	}

	public static void EntityEnterBackpack(Entity entity)
	{
		AddCharmSlot(entity.data);
	}

	public static void AddCharmSlot(CardData target)
	{
		if (target.customData != null && target.customData.TryGetValue("extraCharmSlots", out var value) && value is int num)
		{
			target.customData["extraCharmSlots"] = num + 1;
		}
		else
		{
			target.SetCustomData("extraCharmSlots", 1);
		}
	}
}
