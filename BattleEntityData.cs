#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Linq;

[Serializable]
public class BattleEntityData
{
	public CardSaveData cardSaveData;

	public int height;

	public int damage;

	public int damageMax;

	public int hp;

	public int hpMax;

	public int counter;

	public int counterMax;

	public int uses;

	public int usesMax;

	public bool flipped;

	public StatusEffectSaveData[] attackEffects;

	public BattleEntityData()
	{
	}

	public BattleEntityData(Entity entity)
	{
		cardSaveData = entity.data.Save();
		height = entity.height;
		damage = entity.damage.current;
		damageMax = entity.damage.max;
		hp = entity.hp.current;
		hpMax = entity.hp.max;
		counter = entity.counter.current;
		counterMax = entity.counter.max;
		uses = entity.uses.current;
		usesMax = entity.uses.max;
		flipped = !entity.enabled;
		attackEffects = entity.attackEffects.Select((CardData.StatusEffectStacks a) => a.Save()).ToArray();
	}
}
