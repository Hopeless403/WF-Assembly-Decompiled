#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;

public class Routine
{
	public delegate void FinishedHandler(bool manualStop);

	public class Clump
	{
		public readonly List<Routine> list = new List<Routine>();

		public int Count => list.Count;

		public static Clump Create(params IEnumerator[] routines)
		{
			Clump clump = new Clump();
			foreach (IEnumerator c in routines)
			{
				clump.Add(c);
			}

			return clump;
		}

		public void Add(Routine item)
		{
			list.Add(item);
		}

		public void Add(IEnumerator c)
		{
			Add(new Routine(c));
		}

		public IEnumerator WaitForEnd()
		{
			while (Count > 0)
			{
				bool flag = false;
				foreach (Routine item in list)
				{
					if (item != null && item.IsRunning)
					{
						flag = true;
						break;
					}
				}

				if (flag)
				{
					yield return null;
					continue;
				}

				break;
			}
		}

		public void Clear()
		{
			foreach (Routine item in list)
			{
				item.Stop();
			}

			list.Clear();
		}
	}

	public readonly CoroutineManager.State task;

	public bool IsRunning => task.IsRunning;

	public bool IsPaused => task.IsPaused;

	public event FinishedHandler Finished;

	public Routine(IEnumerator c, bool autoStart = true)
	{
		task = CoroutineManager.Create(c);
		task.Finished += TaskFinished;
		if (autoStart)
		{
			Start();
		}
	}

	public static Routine Create(IEnumerator c, bool autoStart = true)
	{
		return new Routine(c, autoStart);
	}

	public void Start()
	{
		task.Start();
	}

	public void Stop()
	{
		task.Stop();
	}

	public void Pause()
	{
		task.Pause();
	}

	public void Unpause()
	{
		task.Unpause();
	}

	public void TaskFinished(bool manual)
	{
		this.Finished?.Invoke(manual);
	}
}
