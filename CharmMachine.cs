#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class CharmMachine : MonoBehaviour
{
	[SerializeField]
	public UnityEngine.Animator animator;

	[SerializeField]
	public CardPopUpTarget pop;

	public bool running;

	public bool hover;

	public bool CanRun()
	{
		if (!running)
		{
			return base.enabled;
		}

		return false;
	}

	public IEnumerator Run()
	{
		running = true;
		UnHover();
		animator.SetBool("EyesOpen", value: true);
		Events.InvokeScreenShake(0.2f, 0f);
		animator.SetTrigger("Rumble");
		SfxSystem.OneShot("event:/sfx/location/shop/charm_rumble");
		yield return Sequences.Wait(0.1f);
		animator.SetBool("OpenMouth", value: true);
		yield return Sequences.Wait(0.35f);
		animator.SetTrigger("DropCharm");
		SfxSystem.OneShot("event:/sfx/location/shop/charm_drop");
		yield return Sequences.Wait(1f);
		animator.SetBool("OpenMouth", value: false);
		animator.SetBool("EyesOpen", value: false);
		running = false;
	}

	public void Hover()
	{
		if (!hover && base.enabled && !running)
		{
			hover = true;
			animator.SetBool("Hover", hover);
			animator.SetBool("EyesOpen", hover);
			pop.Pop();
		}
	}

	public void UnHover()
	{
		if (hover)
		{
			hover = false;
			animator.SetBool("Hover", hover);
			animator.SetBool("EyesOpen", hover);
			pop.UnPop();
		}
	}
}
