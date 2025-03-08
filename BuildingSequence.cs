#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using Dead;
using FMODUnity;
using NaughtyAttributes;
using UnityEngine;

public class BuildingSequence : MonoBehaviour
{
	[ReadOnly]
	public Building building;

	[SerializeField]
	public Talker talker;

	[SerializeField]
	public EventReference enterSfxEvent;

	public virtual IEnumerator Sequence()
	{
		yield return null;
	}

	public void TalkerGreet()
	{
		TalkerSay("greet", PettyRandom.Range(0.25f, 0.5f));
	}

	public void TalkerFirstGreet()
	{
		TalkerSay("firstGreet", 0.5f);
	}

	public void TalkerNewCard(CardData cardData)
	{
		TalkerSay("new card", 0.5f, cardData.title);
	}

	public void TalkerSay(string speechType, float delay, params object[] inserts)
	{
		if ((bool)talker)
		{
			talker.Say(speechType, delay, inserts);
		}
	}

	public void Play(Building building)
	{
		if (!enterSfxEvent.IsNull)
		{
			SfxSystem.OneShot(enterSfxEvent);
		}

		this.building = building;
		StartCoroutine(Sequence());
	}

	public void Close()
	{
		StopAllCoroutines();
		Object.FindObjectOfType<BuildingDisplay>(includeInactive: true)?.End();
	}
}
