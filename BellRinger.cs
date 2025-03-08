#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using Dead;
using FMODUnity;
using NaughtyAttributes;
using UnityEngine;

public class BellRinger : MonoBehaviour
{
	public TweenUI[] ringTweens;

	public bool playRingSfx = true;

	[SerializeField]
	[ShowIf("playRingSfx")]
	public EventReference ringSfxEvent;

	[SerializeField]
	[ShowIf("playRingSfx")]
	public Vector2 ringSfxPitch = new Vector2(1f, 1f);

	public void Ring()
	{
		TweenUI[] array = ringTweens;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Fire();
		}

		if (playRingSfx)
		{
			PlayRingSfx();
		}
	}

	public void PlayRingSfx()
	{
		if (!ringSfxEvent.IsNull)
		{
			SfxSystem.OneShot(ringSfxEvent).setPitch(ringSfxPitch.PettyRandom() * PettyRandom.Range(0.95f, 1.05f));
		}
	}
}
