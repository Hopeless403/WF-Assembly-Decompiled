#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveDeployerDots : MonoBehaviour
{
	[SerializeField]
	public float fanRadius = 0.5f;

	[SerializeField]
	public float startAngle = -90f;

	[SerializeField]
	public float arcMax = 90f;

	[SerializeField]
	public float arcGap = 20f;

	public List<WaveDot> dots;

	[SerializeField]
	public Transform dotContainer;

	[SerializeField]
	public WaveDot dotPrefab;

	[SerializeField]
	public WaveDot bigDotPrefab;

	public void Init(BattleWaveManager waveManager, int currentWaveIndex)
	{
		foreach (WaveDot dot in dots)
		{
			dot.gameObject.Destroy();
		}

		dots = new List<WaveDot>();
		foreach (BattleWaveManager.Wave item in waveManager.list)
		{
			WaveDot waveDot = UnityEngine.Object.Instantiate(item.isBossWave ? bigDotPrefab : dotPrefab, dotContainer);
			waveDot.gameObject.SetActive(value: true);
			dots.Add(waveDot);
		}

		SetPositions();
		UpdateDots(waveManager, currentWaveIndex);
	}

	public void UpdateDots(BattleWaveManager waveManager, int currentWaveIndex)
	{
		for (int i = 0; i < dots.Count; i++)
		{
			if (waveManager.list[i].spawned && currentWaveIndex != i)
			{
				dots[i].TurnOff();
			}
		}
	}

	public void SetPositions()
	{
		for (int i = 0; i < dots.Count; i++)
		{
			dots[i].transform.localPosition = GetPosition(i);
		}
	}

	public Vector3 GetPosition(int index)
	{
		float radians = (GetAngle(index) + startAngle) * (MathF.PI / 180f);
		return Lengthdir.ToVector2(fanRadius, radians);
	}

	public float GetAngle(int index)
	{
		float angleAdd = GetAngleAdd();
		return (0f - angleAdd) * (float)(dots.Count - 1) * 0.5f + angleAdd * (float)index;
	}

	public float GetAngleAdd()
	{
		int num = dots.Count - 1;
		return Mathf.Min((float)num * arcGap, arcMax) / (float)num;
	}
}
