#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Push", fileName = "Push")]
public class StatusEffectInstantPush : StatusEffectInstant
{
	public override IEnumerator Process()
	{
		int[] rowIndices = Battle.instance.GetRowIndices(target);
		if (rowIndices.Length == 1)
		{
			CardContainer row = Battle.instance.GetRow(target.owner, rowIndices[0]);
			int num = row.IndexOf(target);
			int num2 = Mathf.Min(num + GetAmount(), row.max - 1);
			if (num != num2)
			{
				row.RemoveAt(num);
				row.Insert(num2, target);
				foreach (Entity item in row)
				{
					row.TweenChildPosition(item);
				}
			}
		}

		yield return base.Process();
	}
}
