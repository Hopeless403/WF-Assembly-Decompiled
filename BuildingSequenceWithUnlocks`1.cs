#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using HarmonyLib;
using MonoMod.Cil;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class BuildingSequenceWithUnlocks<T> : BuildingSequence where T : BuildingSequenceWithUnlocks<T>
{
	[SerializeField]
	public GridLayoutGroup slotGrid;

	[SerializeField]
	public Transform challengeStonesParent;

	[SerializeField]
	public GameObject[] locks;

	[SerializeField]
	public CardContainer[] cardSlots;

	[SerializeField]
	public ChallengeStone[] challengeStones;

	[SerializeField]
	public UnityEvent onSetUpComplete;

	public static event RuntimeILReferenceBag.FastDelegateInvokers.Action<T> OnStart;

	public void AddChallengeStone(ChallengeData unlock)
	{
		if ((bool)unlock && !challengeStones.ToList().Find((ChallengeStone a) => a.challenge == unlock))
		{
			GameObject gameObject = Addressables.LoadAssetAsync<GameObject>("ChallengeStone").WaitForCompletion().InstantiateKeepName();
			gameObject.transform.parent = challengeStonesParent.transform;
			gameObject.transform.localPosition = gameObject.transform.localPosition.WithZ(0f);
			ChallengeStone componentInChildren = gameObject.GetComponentInChildren<ChallengeStone>();
			challengeStones = challengeStones.AddToArray(componentInChildren);
			componentInChildren.challenge = unlock;
			componentInChildren.OnEnable();
		}
	}

	public void AddSlot(ChallengeData unlock)
	{
		if ((bool)unlock && !cardSlots.ToList().Find((CardContainer a) => a.gameObject?.GetComponentInChildren<ChallengeDisplayCreator>()?.challenge == unlock))
		{
			GameObject obj = Addressables.LoadAssetAsync<GameObject>("ProgressableCardStack").WaitForCompletion().InstantiateKeepName();
			obj.transform.parent = slotGrid.transform;
			CardStack componentInChildren = obj.GetComponentInChildren<CardStack>();
			cardSlots = cardSlots.AddToArray(componentInChildren);
			locks = locks.AddToArray(componentInChildren.gameObject.transform.GetChild(0).gameObject);
			ChallengeDisplayCreator componentInChildren2 = obj.GetComponentInChildren<ChallengeDisplayCreator>();
			componentInChildren2.challenge = unlock;
			componentInChildren2.Check();
		}
	}

	public void _OnStart()
	{
		BuildingSequenceWithUnlocks<T>.OnStart?.Invoke(this as T);
	}

	public BuildingSequenceWithUnlocks()
	{
	}
}
