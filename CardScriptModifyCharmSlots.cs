#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Modify Upgrade Slots", menuName = "Card Scripts/Modify Upgrade Slots")]
public class CardScriptModifyCharmSlots : CardScript
{
	[SerializeField]
	public int addCharmSlots;

	public override void Run(CardData target)
	{
		if (addCharmSlots != 0)
		{
			if (target.customData != null && target.customData.TryGetValue("extraCharmSlots", out var value) && value is int num)
			{
				target.customData["extraCharmSlots"] = num + addCharmSlots;
			}
			else
			{
				target.SetCustomData("extraCharmSlots", addCharmSlots);
			}
		}
	}
}
