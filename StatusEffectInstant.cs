#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;

public abstract class StatusEffectInstant : StatusEffectData
{
	public override bool Instant => true;

	public override void Init()
	{
		base.OnBegin += Process;
	}

	public override IEnumerator BeginRoutine()
	{
		if (count > 0 || !canBeBoosted || this is StatusEffectInstantMultiple)
		{
			yield return base.BeginRoutine();
		}
		else
		{
			yield return Remove();
		}
	}

	public virtual IEnumerator Process()
	{
		yield return Remove();
	}

	public override int GetAmount()
	{
		return count;
	}

	public StatusEffectInstant()
	{
	}
}
