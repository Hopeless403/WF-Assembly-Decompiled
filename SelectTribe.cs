#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using Dead;
using UnityEngine;

public class SelectTribe : MonoBehaviour
{
	[SerializeField]
	public TribeFlagDisplay tribeFlagPrefab;

	[SerializeField]
	public Transform tribeFlagGroup;

	[SerializeField]
	public SelectLeader leaderSelection;

	[SerializeField]
	public Transform selectedFlagAnchor;

	[SerializeField]
	public TitleSetter titleSetter;

	[SerializeField]
	public ParticleSystem selectParticleSystem;

	public readonly List<TribeFlagDisplay> flags = new List<TribeFlagDisplay>();

	public readonly List<ClassData> tribes = new List<ClassData>();

	public void SetAvailableTribes(List<ClassData> tribes)
	{
		this.tribes.Clear();
		this.tribes.AddRange(tribes);
	}

	public void Run()
	{
		titleSetter.Set();
		foreach (ClassData tribe in tribes)
		{
			TribeFlagDisplay flag = Object.Instantiate(tribeFlagPrefab, tribeFlagGroup);
			flag.SetAvailable();
			flag.SetUnlocked();
			flag.SetFlagSprite(tribe.flag);
			flag.AddPressAction(delegate
			{
				StartSelectRoutine(flag, tribe);
			});
			flags.Add(flag);
		}

		selectedFlagAnchor.DestroyAllChildren();
	}

	public void RevealAnimation()
	{
		foreach (TribeFlagDisplay flag in flags)
		{
			Transform transform = flag.flagImage.transform;
			if ((object)transform != null)
			{
				transform.localRotation = Quaternion.Euler(0f, 0f, PettyRandom.Range(-15f, -5f));
				LeanTween.cancel(transform.gameObject);
				LeanTween.rotateLocal(transform.gameObject, Vector3.zero, PettyRandom.Range(1f, 1.5f)).setEaseOutElastic();
				LeanTween.moveLocal(transform.gameObject, Vector3.zero, PettyRandom.Range(0.25f, 0.35f)).setFrom(Vector3.up * PettyRandom.Range(1f, 2f)).setEaseOutQuart();
			}
		}
	}

	public void StartSelectRoutine(TribeFlagDisplay flag, ClassData classData)
	{
		new Routine(SelectRoutine(flag, classData));
	}

	public IEnumerator SelectRoutine(TribeFlagDisplay flag, ClassData classData)
	{
		selectParticleSystem.transform.position = Cursor3d.PositionWithZ;
		selectParticleSystem.Play();
		if (!classData.selectSfxEvent.IsNull)
		{
			SfxSystem.OneShot(classData.selectSfxEvent);
		}

		flag.transform.SetParent(selectedFlagAnchor);
		tribeFlagGroup.DestroyAllChildren();
		flags.Clear();
		flag.ClearPressActions();
		flag.SetInteractable(interactable: false);
		tribes.Clear();
		tribes.Add(classData);
		flag.transform.localPosition = Vector3.up * 3f;
		LeanTween.cancel(flag.gameObject);
		LeanTween.moveLocal(flag.gameObject, Vector3.zero, PettyRandom.Range(0.2f, 0.3f)).setEaseOutQuart();
		Transform transform = flag.flagImage.transform;
		if ((object)transform != null)
		{
			transform.localRotation = Quaternion.Euler(0f, 0f, PettyRandom.Range(10f, 15f));
			LeanTween.cancel(transform.gameObject);
			LeanTween.rotateLocal(transform.gameObject, Vector3.zero, PettyRandom.Range(1f, 1.5f)).setEaseOutElastic();
		}

		leaderSelection.Run(tribes);
		yield return leaderSelection.GenerateLeaders(useSeed: true);
		leaderSelection.FlipUpLeaders();
	}
}
