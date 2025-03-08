#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Specific/Halt X", fileName = "Halt X")]
public class StatusEffectHaltX : StatusEffectData
{
	[SerializeField]
	public StatusEffectData effectToHalt;

	[SerializeField]
	public bool ignoreSilence = true;

	public override void Init()
	{
		Debug.Log($"→ Halting Count Down of [{effectToHalt.name}] for [{target}]");
		Events.OnStatusEffectCountDown += StatusCountDown;
	}

	public void OnDestroy()
	{
		Debug.Log($"→ Resuming Count Down of [{effectToHalt.name}] for [{target}]");
		Events.OnStatusEffectCountDown -= StatusCountDown;
	}

	public void StatusCountDown(StatusEffectData status, ref int amount)
	{
		if (status.type == effectToHalt.type && status.target == target && !Silenced())
		{
			amount = 0;
		}
	}

	public bool Silenced()
	{
		if (target.silenced)
		{
			return !ignoreSilence;
		}

		return false;
	}
}
