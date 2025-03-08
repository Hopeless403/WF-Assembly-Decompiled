#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Budge", fileName = "Budge")]
public class StatusEffectInstantBudge : StatusEffectInstant
{
	public override IEnumerator Process()
	{
		int[] rowIndices = Battle.instance.GetRowIndices(target);
		if (rowIndices.Length == 1)
		{
			int num = rowIndices[0];
			int num2 = (num + 1) % Battle.instance.rowCount;
			if (num2 != num)
			{
				CardContainer row = Battle.instance.GetRow(target.owner, num2);
				if (row.Count < row.max)
				{
					Battle.instance.GetRow(target.owner, num);
					yield return Sequences.CardMove(target, new CardContainer[1] { row });
				}
			}
		}

		yield return base.Process();
	}
}
