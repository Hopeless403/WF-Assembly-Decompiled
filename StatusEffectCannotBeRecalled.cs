#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Traits/Cannot Recall", fileName = "Cannot Recall")]
public class StatusEffectCannotBeRecalled : StatusEffectData
{
	public bool active;

	public override bool RunBeginEvent()
	{
		if (!active && (bool)target && GetAmount() > 0)
		{
			Activate();
		}

		return false;
	}

	public override bool RunEndEvent()
	{
		if (active && (bool)target)
		{
			Deactivate();
		}

		return false;
	}

	public override bool RunEffectBonusChangedEvent()
	{
		if ((bool)target)
		{
			int amount = GetAmount();
			if (amount > 0 && !active)
			{
				Activate();
			}
			else if (amount <= 0 && active)
			{
				Deactivate();
			}
		}

		return false;
	}

	public void Activate()
	{
		target.blockRecall++;
		active = true;
	}

	public void Deactivate()
	{
		target.blockRecall--;
		active = false;
	}
}
