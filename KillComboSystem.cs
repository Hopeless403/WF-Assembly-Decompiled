#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class KillComboSystem : GameSystem
{
	public string[] colors = new string[1] { "ffffff" };

	public string format = "<line-height=0.35><size=0.7><#{1}>x{0}\n<size=0.35>Combo\n\n<#FFD150>+{2}<sprite name=bling>";

	public int min = 2;

	public int baseGold;

	public int goldPerCombo = 5;

	public int count = 1;

	public void OnEnable()
	{
		Events.OnBattlePhaseStart += BattlePhaseStart;
		Events.OnBattleTurnEnd += BattleTurnEnd;
		Events.OnEntityKilled += EntityKilled;
	}

	public void OnDisable()
	{
		Events.OnBattlePhaseStart -= BattlePhaseStart;
		Events.OnBattleTurnEnd -= BattleTurnEnd;
		Events.OnEntityKilled -= EntityKilled;
	}

	public void BattlePhaseStart(Battle.Phase phase)
	{
		if (phase == Battle.Phase.Init)
		{
			count = 1;
		}
	}

	public void BattleTurnEnd(int turnNumber)
	{
		count = 1;
	}

	public void EntityKilled(Entity entity, DeathType deathType)
	{
		if ((bool)References.Player && entity.owner != References.Player)
		{
			if (count >= min)
			{
				Vector3 position = entity.transform.position;
				int goldAmount = GetGoldAmount(count);
				int num = Mathf.Clamp(count - min, 0, colors.Length - 1);
				string arg = colors[num];
				string text = string.Format(format, count, arg, goldAmount);
				FloatingText.Create(position + Vector3.down * 0.5f, text).Animate("Spring").Fade("Smooth", 0.5f, 0.5f);
				Events.InvokeDropGold(goldAmount, "Combo", References.Player, position);
				SfxSystem.OneShot("event:/sfx/attack/combo_marker");
				Events.InvokeKillCombo(count);
			}

			count++;
		}
	}

	public int GetGoldAmount(int killCount)
	{
		return Mathf.RoundToInt((float)(baseGold + (1 + killCount - min) * goldPerCombo) * References.PlayerData.comboGoldFactor);
	}
}
