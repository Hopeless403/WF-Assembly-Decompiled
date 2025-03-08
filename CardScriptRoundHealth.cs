#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Round Health", menuName = "Card Scripts/Round Health")]
public class CardScriptRoundHealth : CardScript
{
	[SerializeField]
	[HideIf("floor")]
	public bool ceil = true;

	[SerializeField]
	[HideIf("ceil")]
	public bool floor;

	[SerializeField]
	public int round = 10;

	public override void Run(CardData target)
	{
		float f = (float)target.hp / (float)round;
		if (ceil)
		{
			target.hp = Mathf.CeilToInt(f) * round;
		}
		else if (floor)
		{
			target.hp = Mathf.FloorToInt(f) * round;
		}
		else
		{
			target.hp = Mathf.RoundToInt(f) * round;
		}

		target.hp = Mathf.Max(1, target.hp);
	}
}
