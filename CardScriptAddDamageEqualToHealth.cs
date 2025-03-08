#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Add Damage Equal To Health", menuName = "Card Scripts/Add Damage Equal To Health")]
public class CardScriptAddDamageEqualToHealth : CardScript
{
	public override void Run(CardData target)
	{
		target.damage += target.hp;
	}
}
