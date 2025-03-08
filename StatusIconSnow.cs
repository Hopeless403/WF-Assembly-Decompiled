#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using TMPro;
using UnityEngine;

public class StatusIconSnow : StatusIcon
{
	public void CustomSetText()
	{
		TMP_Text tMP_Text = textElement;
		Stat stat = GetValue();
		int num = stat.current + base.target.counter.current;
		if (tMP_Text != null)
		{
			tMP_Text.text = num.ToString();
			if (alterTextColours)
			{
				tMP_Text.color = ((num > stat.max) ? textColourAboveMax : textColour);
			}
		}
	}

	public void CustomDestroy()
	{
		Transform parent = base.transform.parent;
		if (!(parent != null))
		{
			return;
		}

		foreach (Transform item in parent)
		{
			StatusIcon component = item.GetComponent<StatusIcon>();
			if (component.type == "counter")
			{
				component.Ping();
				break;
			}
		}
	}
}
