#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class CampaignNodeTypeEvent : CampaignNodeType
{
	[SerializeField]
	public AssetReferenceGameObject routinePrefabRef;

	public override IEnumerator Run(CampaignNode node)
	{
		yield return Transition.To("Event");
		AsyncOperationHandle<GameObject> task = routinePrefabRef.InstantiateAsync(EventManager.EventRoutineHolder);
		yield return new WaitUntil(() => task.IsDone);
		EventRoutine eventRoutine = task.Result.GetComponent<EventRoutine>();
		task.Result.AddComponent<AddressableReleaser>().Add(task);
		Events.InvokeEventStart(node, eventRoutine);
		yield return Populate(node);
		Events.InvokeEventPopulated(eventRoutine);
		Transition.End();
		yield return eventRoutine.Run();
		yield return Transition.To("MapNew");
		Transition.End();
		yield return MapNew.CheckCompanionLimit();
	}

	public virtual IEnumerator Populate(CampaignNode node)
	{
		return null;
	}

	public CampaignNodeTypeEvent()
	{
	}
}
