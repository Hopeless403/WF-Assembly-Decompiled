#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;

public class ActionRefreshWhileActiveEffect : PlayAction
{
	public readonly StatusEffectWhileActiveX effect;

	public static ulong idCurrent;

	public readonly ulong id;

	public ActionRefreshWhileActiveEffect(StatusEffectWhileActiveX effect)
	{
		this.effect = effect;
		id = idCurrent++;
	}

	public override IEnumerator Run()
	{
		yield return effect.Deactivate();
		if (effect.CanActivate())
		{
			yield return effect.Activate();
		}

		PlayAction[] actions = ActionQueue.GetActions();
		for (int num = actions.Length - 1; num >= 0; num--)
		{
			PlayAction playAction = actions[num];
			if (playAction is ActionRefreshWhileActiveEffect actionRefreshWhileActiveEffect && actionRefreshWhileActiveEffect.id != id && actionRefreshWhileActiveEffect.effect.id == effect.id)
			{
				ActionQueue.Remove(playAction);
			}
		}
	}
}
