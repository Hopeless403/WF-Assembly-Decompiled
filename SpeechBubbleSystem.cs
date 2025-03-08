#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleSystem : GameSystem
{
	public delegate void NotifyCreate(SpeechBubbleData data);

	public static SpeechBubbleSystem instance;

	public SpeechBub bubblePrefab;

	public AnimationCurve wordToDurationCurve;

	public float durationFactor = 1f;

	public List<SpeechBubbleSpawn> spawnPoints;

	public List<SpeechBubbleData> queue;

	public SpeechBub current;

	public static event NotifyCreate OnCreate;

	public static void Create(SpeechBubbleData data)
	{
		SpeechBubbleSystem.OnCreate?.Invoke(data);
	}

	public void OnEnable()
	{
		instance = this;
		spawnPoints = new List<SpeechBubbleSpawn>();
		queue = new List<SpeechBubbleData>();
		OnCreate += QueueBubble;
	}

	public void OnDisable()
	{
		OnCreate -= QueueBubble;
		StopAllCoroutines();
	}

	public void Update()
	{
		if (queue.Count > 0 && (!current || current.durationFactor < 0.5f))
		{
			CreateBubble(queue[0]);
			queue.RemoveAt(0);
		}
	}

	public void QueueBubble(SpeechBubbleData data)
	{
		queue.Add(data);
	}

	public void CreateBubble(SpeechBubbleData data)
	{
		if (data.delay > 0f)
		{
			StartCoroutine(CreateBubbleAfter(data, data.delay));
		}
		else if (spawnPoints.Count > 0)
		{
			SpeechBubbleSpawn speechBubbleSpawn = null;
			data.SetDuration(GetDuration(data.text));
			if (!data.target)
			{
				speechBubbleSpawn = spawnPoints.RandomItem();
			}
			else
			{
				Vector3 position = data.target.position;
				float num = float.MaxValue;
				foreach (SpeechBubbleSpawn spawnPoint in spawnPoints)
				{
					float num2 = spawnPoint.transform.position.DistanceTo(position);
					if (num2 < num)
					{
						num = num2;
						speechBubbleSpawn = spawnPoint;
					}
				}
			}

			if ((bool)speechBubbleSpawn)
			{
				current = speechBubbleSpawn.Create(bubblePrefab, data);
			}
		}
		else
		{
			Debug.LogWarning("Speech Bubble could not be created: No anchor points in scene!");
		}
	}

	public IEnumerator CreateBubbleAfter(SpeechBubbleData data, float delay)
	{
		yield return new WaitForSeconds(delay);
		data.delay = 0f;
		CreateBubble(data);
	}

	public static float GetDuration(string text)
	{
		string[] array = text.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
		return instance.wordToDurationCurve.Evaluate(array.Length) * instance.durationFactor;
	}

	public void AddSpawnPoint(SpeechBubbleSpawn spawnPoint)
	{
		spawnPoints.Add(spawnPoint);
	}

	public void RemoveSpawnPoint(SpeechBubbleSpawn spawnPoint)
	{
		spawnPoints.Remove(spawnPoint);
	}
}
