#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Injuries/Injury", fileName = "Injury")]
public class StatusEffectInjury : StatusEffectData
{
	[SerializeField]
	public float healthFactor = 0.5f;

	[SerializeField]
	public float damageFactor = 0.5f;

	[SerializeField]
	public int counterIncrease;

	public bool hasRun;

	public override void Init()
	{
		Events.OnBattleStart += BattleStart;
	}

	public void OnDestroy()
	{
		Events.OnBattleStart -= BattleStart;
	}

	public void BattleStart()
	{
		Run();
	}

	public override bool RunBeginEvent()
	{
		if (!target.inPlay)
		{
			Run();
		}

		return false;
	}

	public void Run()
	{
		if (!hasRun)
		{
			hasRun = true;
			if (counterIncrease > 0 && target.counter.max > 0)
			{
				target.counter.max += counterIncrease;
				target.counter.current += counterIncrease;
			}

			target.damage.current = Mathf.CeilToInt((float)target.damage.current * damageFactor);
			target.hp.current = Mathf.CeilToInt((float)target.hp.current * healthFactor);
			target.PromptUpdate();
		}
	}
}
