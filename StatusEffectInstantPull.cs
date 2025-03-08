#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Instant/Pull", fileName = "Pull")]
public class StatusEffectInstantPull : StatusEffectInstant
{
	public const int insertPos = 0;

	public override IEnumerator Process()
	{
		int[] rowIndices = Battle.instance.GetRowIndices(target);
		if (rowIndices.Length == 1)
		{
			CardContainer row = Battle.instance.GetRow(target.owner, rowIndices[0]);
			int num = row.IndexOf(target);
			if (num != 0)
			{
				row.RemoveAt(num);
				bool flag = false;
				if (row is CardSlotLane)
				{
					flag = row[0] == null;
					if (!flag)
					{
						flag = row.PushBackwards(0);
						if (!flag)
						{
							flag = row.PushForwards(0);
						}
					}
				}

				row.Insert((!flag) ? num : 0, target);
				if (flag)
				{
					foreach (CardContainer row2 in References.Battle.GetRows(target.owner))
					{
						row2.TweenChildPositions();
					}
				}
			}
		}

		yield return base.Process();
	}
}
