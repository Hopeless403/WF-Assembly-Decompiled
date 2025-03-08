#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueue : MonoBehaviourSingleton<ActionQueue>
{
	public readonly List<PlayAction> queue = new List<PlayAction>();

	[SerializeField]
	public int count;

	[SerializeField]
	public float delayBefore;

	[SerializeField]
	public float delayAfter;

	public static PlayAction current;

	public static readonly Routine.Clump parallelClump = new Routine.Clump();

	public static bool Empty => MonoBehaviourSingleton<ActionQueue>.instance.count <= 0;

	public static PlayAction Insert(int index, PlayAction action, bool fixedPosition = false)
	{
		if (fixedPosition)
		{
			action.fixedPosition = true;
		}

		int num = 0;
		for (num = index; num < MonoBehaviourSingleton<ActionQueue>.instance.count && MonoBehaviourSingleton<ActionQueue>.instance.queue[num].fixedPosition; num++)
		{
		}

		MonoBehaviourSingleton<ActionQueue>.instance.queue.Insert(num, action);
		MonoBehaviourSingleton<ActionQueue>.instance.count++;
		Events.InvokeActionQueued(action);
		return action;
	}

	public static PlayAction Add(PlayAction action, bool fixedPosition = false)
	{
		return Insert(MonoBehaviourSingleton<ActionQueue>.instance.count, action, fixedPosition);
	}

	public static PlayAction Stack(PlayAction action, bool fixedPosition = false)
	{
		return Insert(0, action, fixedPosition);
	}

	public static PlayAction[] GetActions()
	{
		return MonoBehaviourSingleton<ActionQueue>.instance.queue.ToArray();
	}

	public static int IndexOf(PlayAction action)
	{
		return MonoBehaviourSingleton<ActionQueue>.instance.queue.IndexOf(action);
	}

	public static bool Remove(PlayAction action)
	{
		if (IndexOf(action) >= 0 && action != current)
		{
			MonoBehaviourSingleton<ActionQueue>.instance.queue.Remove(action);
			MonoBehaviourSingleton<ActionQueue>.instance.count--;
			return true;
		}

		return false;
	}

	public static IEnumerator Wait(bool includeParallel = true)
	{
		while (MonoBehaviourSingleton<ActionQueue>.instance.count > 0)
		{
			yield return null;
		}

		if (includeParallel)
		{
			yield return parallelClump.WaitForEnd();
		}
	}

	public void Start()
	{
		StartCoroutine(Routine());
	}

	public static void Restart()
	{
		Debug.Log("~ ACTION QUEUE RESET ~");
		MonoBehaviourSingleton<ActionQueue>.instance.StopAllCoroutines();
		MonoBehaviourSingleton<ActionQueue>.instance.queue.Clear();
		MonoBehaviourSingleton<ActionQueue>.instance.count = 0;
		current = null;
		parallelClump.Clear();
		MonoBehaviourSingleton<ActionQueue>.instance.StartCoroutine(MonoBehaviourSingleton<ActionQueue>.instance.Routine());
	}

	public IEnumerator Routine()
	{
		while (true)
		{
			if (count > 0 && !GameManager.paused && !Deckpack.IsOpen && current == null)
			{
				yield return RunActionRoutine();
			}
			else
			{
				yield return null;
			}
		}
	}

	public IEnumerator RunActionRoutine()
	{
		int index = GetIndexOfHighestPriorityAction(queue);
		current = queue[index];
		current.fixedPosition = true;
		if (current.parallel)
		{
			RunParallel(current);
			queue.RemoveAt(index);
			count--;
		}
		else
		{
			yield return PerformAction(current);
			queue.RemoveAt(index);
			count--;
			yield return PostAction(current);
		}

		current = null;
	}

	public static int GetIndexOfHighestPriorityAction(IReadOnlyList<PlayAction> actions)
	{
		int num = int.MinValue;
		int result = -1;
		int num2 = actions.Count;
		for (int i = 0; i < num2; i++)
		{
			PlayAction playAction = actions[i];
			if (playAction.priority > num)
			{
				num = playAction.priority;
				result = i;
			}
		}

		return result;
	}

	public IEnumerator Run(PlayAction action)
	{
		yield return PerformAction(action);
		yield return PostAction(action);
	}

	public IEnumerator PerformAction(PlayAction action)
	{
		Events.InvokeActionPerform(action);
		if (action.pauseBefore + delayBefore > 0f)
		{
			yield return Sequences.Wait(action.pauseBefore + delayBefore);
		}

		if (action.IsRoutine)
		{
			yield return action.Run();
		}
		else
		{
			action.Process();
		}
	}

	public IEnumerator PostAction(PlayAction action)
	{
		Events.InvokeActionFinished(action);
		yield return StatusEffectSystem.ActionPerformedEvent(action);
		if (action.pauseAfter + delayAfter > 0f)
		{
			yield return Sequences.Wait(action.pauseAfter + delayAfter);
		}
	}

	public static void RunParallel(PlayAction action)
	{
		parallelClump.Add(MonoBehaviourSingleton<ActionQueue>.instance.Run(action));
	}
}
