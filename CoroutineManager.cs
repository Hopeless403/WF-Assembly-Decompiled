#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
	public class State
	{
		public delegate void FinishedHandler(bool manualStop);

		public IEnumerator coroutine;

		public bool running;

		public bool paused;

		public bool stopped;

		public bool IsRunning => running;

		public bool IsPaused => paused;

		public event FinishedHandler Finished;

		public State(IEnumerator coroutine)
		{
			this.coroutine = coroutine;
		}

		public void Pause()
		{
			paused = true;
		}

		public void Unpause()
		{
			paused = false;
		}

		public void Start()
		{
			running = true;
			instance.StartCoroutine(CallWrapper());
		}

		public void Stop()
		{
			stopped = true;
			running = false;
		}

		public IEnumerator CallWrapper()
		{
			yield return null;
			IEnumerator e = coroutine;
			while (running)
			{
				if (paused)
				{
					yield return null;
				}
				else if (e != null && e.MoveNext())
				{
					yield return e.Current;
				}
				else
				{
					running = false;
				}
			}

			this.Finished?.Invoke(stopped);
		}
	}

	public static CoroutineManager instance;

	public static void InstanceCheck()
	{
		if (!instance)
		{
			instance = Object.FindObjectOfType<CoroutineManager>();
			if (!instance)
			{
				instance = new GameObject("CoroutineManager").AddComponent<CoroutineManager>();
			}
		}
	}

	public static State Create(IEnumerator coroutine)
	{
		InstanceCheck();
		return new State(coroutine);
	}

	public static void Start(IEnumerator coroutine)
	{
		InstanceCheck();
		instance.StartCoroutine(coroutine);
	}
}
