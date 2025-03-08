#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(fileName = "Set Health", menuName = "Card Scripts/Set Health")]
public class CardScriptSetHealth : CardScript
{
	[SerializeField]
	public Vector2Int healthRange;

	public override void Run(CardData target)
	{
		if (target.hasHealth)
		{
			target.hp = healthRange.Random();
			target.hp = Mathf.Max(1, target.hp);
		}
	}
}
