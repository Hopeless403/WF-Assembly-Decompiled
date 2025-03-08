#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public abstract class StatusEffectOngoing : StatusEffectData
{
	[InfoBox("\"Reverse\" to reduce instead of increase", EInfoBoxType.Normal)]
	[SerializeField]
	public bool reverse;

	public override bool HasStackRoutine => true;

	public override bool HasEndRoutine => true;

	public virtual IEnumerator Add(int add)
	{
		return null;
	}

	public virtual IEnumerator Remove(int remove)
	{
		return null;
	}

	public override IEnumerator StackRoutine(int stacks)
	{
		yield return Add(reverse ? (-stacks) : stacks);
	}

	public override IEnumerator RemoveStacks(int amount, bool removeTemporary)
	{
		int num = Mathf.Min(count, amount);
		yield return Remove(reverse ? (-num) : num);
		yield return base.RemoveStacks(amount, removeTemporary);
	}

	public override IEnumerator EndRoutine()
	{
		return Remove(reverse ? (-count) : count);
	}

	public StatusEffectOngoing()
	{
	}
}
