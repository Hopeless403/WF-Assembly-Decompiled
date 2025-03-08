#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class Discarder : MonoBehaviour
{
	public void Discard(Entity entity)
	{
		CoroutineManager.Start(ClearStatusEffects(entity));
		entity.counter.current = entity.counter.max;
		entity.uses.current = entity.uses.max;
		entity.PromptUpdate();
	}

	public static IEnumerator ClearStatusEffects(Entity entity)
	{
		Debug.Log($"DISCARDER → Clearing Status Effects [{entity}]");
		int count = entity.statusEffects.Count;
		for (int i = count - 1; i >= 0; i--)
		{
			StatusEffectData statusEffectData = entity.statusEffects[i];
			if ((bool)statusEffectData && statusEffectData.removeOnDiscard)
			{
				yield return statusEffectData.Remove();
			}
		}
	}
}
