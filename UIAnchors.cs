#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class UIAnchors : MonoBehaviour
{
	[Serializable]
	public class AnchorPoint
	{
		public UIAnchor target;

		public RectTransform anchor;

		public void Activate()
		{
			target.gameObject.SetActive(value: true);
			anchor.gameObject.SetActive(value: true);
		}

		public void Deactivate()
		{
			target.gameObject.SetActive(value: false);
			anchor.gameObject.SetActive(value: false);
		}

		public void SetUp()
		{
			target.SetUp();
		}

		public IEnumerator Move(float dur = 0.75f, LeanTweenType ease = LeanTweenType.easeOutBack)
		{
			LeanTween.move(target.gameObject, anchor.position, dur).setEase(ease);
			Vector3 movement = anchor.position - target.transform.position;
			target.GetComponentInChildren<Wobbler>()?.Wobble(movement);
			yield return Sequences.Wait(dur);
		}
	}

	public AnchorPoint[] list;

	public int Count => list.Length;

	public void Activate(int anchorIndex)
	{
		list[anchorIndex].Activate();
	}

	public void Deactivate(int anchorIndex)
	{
		list[anchorIndex].Deactivate();
	}

	public IEnumerator Reveal(int anchorIndex)
	{
		AnchorPoint obj = list[anchorIndex];
		UIAnchor target = obj.target;
		_ = obj.anchor;
		target.Reveal();
		yield return Sequences.Wait(target.revealDur);
	}

	public IEnumerator UnReveal(int anchorIndex, float delay = 0f)
	{
		UIAnchor target = list[anchorIndex].target;
		target.UnReveal(delay);
		yield return Sequences.Wait(target.hideDur + delay);
	}

	[Button(null, EButtonEnableMode.Always)]
	public void PromptUpdate()
	{
		StartCoroutine(UpdatePositions());
	}

	[Button(null, EButtonEnableMode.Always)]
	public void SetPositions()
	{
		AnchorPoint[] array = list;
		foreach (AnchorPoint anchorPoint in array)
		{
			anchorPoint.target.transform.position = anchorPoint.anchor.transform.position;
		}
	}

	public IEnumerator UpdatePositions()
	{
		List<Routine> routines = new List<Routine>();
		AnchorPoint[] array = list;
		foreach (AnchorPoint anchorPoint in array)
		{
			if (anchorPoint.target.gameObject.activeSelf)
			{
				routines.Add(new Routine(anchorPoint.Move()));
			}
		}

		bool routinesDone = false;
		while (!routinesDone)
		{
			routinesDone = true;
			foreach (Routine item in routines)
			{
				if (item != null && item.IsRunning)
				{
					routinesDone = false;
					break;
				}
			}

			yield return null;
		}
	}
}
