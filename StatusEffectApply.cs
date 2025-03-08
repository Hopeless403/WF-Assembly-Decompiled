#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

public class StatusEffectApply
{
	public Entity applier;

	public Entity target;

	public StatusEffectData effectData;

	public int count;

	public StatusEffectApply(Entity applier, Entity target, StatusEffectData effectData, int count)
	{
		this.applier = applier;
		this.target = target;
		this.effectData = effectData;
		this.count = count;
	}
}
