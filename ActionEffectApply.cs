#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEffectApply : PlayAction
{
	public class Sequence
	{
		public readonly List<Entity> targets;

		public int amount;

		public Sequence(List<Entity> targets, int amount)
		{
			this.targets = targets;
			this.amount = amount;
		}

		public override string ToString()
		{
			return string.Format("Sequence: [{0}] → {1}", string.Join(", ", targets), amount);
		}
	}

	public readonly StatusEffectApplyX effect;

	public readonly List<Entity> targets = new List<Entity>();

	public readonly List<int> amounts = new List<int>();

	public readonly List<Sequence> sequences = new List<Sequence>();

	public bool running;

	public ActionEffectApply(StatusEffectApplyX effect, List<Entity> targets, int amount)
	{
		this.effect = effect;
		sequences.Add(new Sequence(targets, amount));
	}

	public void Stack(List<Entity> newTargets, int amount)
	{
		bool flag = false;
		foreach (Sequence sequence in sequences)
		{
			if (sequence.targets.Count == newTargets.Count && sequence.targets.ContainsAll(newTargets))
			{
				sequence.amount += amount;
				Debug.Log($"Stacking [{effect.name}] {sequence.amount - amount} → {sequence.amount}");
				flag = true;
				break;
			}

			if (amount == sequence.amount && !sequence.targets.ContainsAny(newTargets))
			{
				sequence.targets.AddRange(newTargets);
				Debug.Log("Stacking [" + effect.name + "] adding " + string.Join(", ", newTargets));
				flag = true;
				break;
			}
		}

		if (!flag)
		{
			sequences.Add(new Sequence(newTargets, amount));
			Debug.Log("Stacking [" + effect.name + "] ↓\n" + string.Join("\n", sequences));
		}
	}

	public override IEnumerator Run()
	{
		running = true;
		foreach (Sequence sequence in sequences)
		{
			if (!effect)
			{
				break;
			}

			yield return effect.Sequence(sequence.targets, sequence.amount);
		}
	}

	public void TryRemoveEntity(Entity entity)
	{
		foreach (Sequence sequence in sequences)
		{
			if (sequence.targets.Contains(entity))
			{
				sequence.targets.Remove(entity);
			}
		}
	}
}
