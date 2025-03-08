#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;

public class EventRoutineCharm : EventRoutine
{
	[SerializeField]
	public UnityEngine.Animator animator;

	[SerializeField]
	public OpenCharmBlockSequence openSequence;

	[SerializeField]
	public Transform charmBlock;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString takeKey;

	[SerializeField]
	public SfxLoop loop;

	public bool routineActive;

	public void Open(BaseEventData eventData)
	{
		if ((!(eventData is PointerEventData pointerEventData) || pointerEventData.button == PointerEventData.InputButton.Left) && !base.data.Get<bool>("open") && !routineActive)
		{
			StartCoroutine(OpenRoutine());
		}
	}

	public IEnumerator OpenRoutine()
	{
		node.cleared = true;
		DeckpackBlocker.Block();
		CinemaBarSystem.Clear();
		Events.InvokeScreenShake(0.25f, 0f);
		Events.InvokeScreenRumble(0f, 0.25f, 0f, 0.05f, 0.2f, 0.3f);
		SfxSystem.OneShot("event:/sfx/location/charm/rumble");
		base.data["open"] = true;
		routineActive = true;
		yield return Sequences.Wait(0.1f);
		animator.SetTrigger("OpenMouth");
		yield return Sequences.Wait(0.35f);
		animator.SetTrigger("DropCharm");
		SfxSystem.OneShot("event:/sfx/location/charm/drop");
		yield return Sequences.Wait(1f);
		animator.SetBool("Zoom", value: true);
		routineActive = false;
	}

	public override IEnumerator Populate()
	{
		animator.SetBool("OpenMouth", base.data.Get<bool>("open"));
		yield return null;
	}

	public override IEnumerator Run()
	{
		CinemaBarSystem.Top.SetPrompt(takeKey.GetLocalizedString(), "Select");
		loop.Play();
		yield return new WaitUntil(() => base.data.Get<bool>("open") && !routineActive);
		yield return Sequences.Wait(0.15f);
		string assetName = base.data.Get<string>("charm");
		CardUpgradeData charmData = AddressableLoader.Get<CardUpgradeData>("CardUpgradeData", assetName).Clone();
		openSequence.SetCharm(charmData, charmBlock);
		openSequence.SetCharacter(base.player);
		charmBlock.gameObject.SetActive(value: false);
		yield return openSequence.Run();
		DeckpackBlocker.Unblock();
		loop.Stop();
		CinemaBarSystem.Clear();
	}
}
