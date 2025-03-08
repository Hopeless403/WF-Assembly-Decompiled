#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Set Damage", menuName = "Card Scripts/Set Damage")]
public class CardScriptSetDamage : CardScript
{
	[SerializeField]
	public Vector2Int damageRange;

	public override void Run(CardData target)
	{
		if (target.hasAttack)
		{
			target.damage = damageRange.Random();
			target.damage = Mathf.Max(0, target.damage);
		}
	}
}
