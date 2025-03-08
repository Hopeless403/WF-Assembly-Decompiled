#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public class Transition : MonoBehaviourSingleton<Transition>
{
	public static TransitionType current;

	[SerializeField]
	public TransitionType[] transitions;

	public static bool Running;

	public TransitionType prefab;

	public void OnEnable()
	{
		Events.OnSettingChanged += SettingChanged;
	}

	public void OnDisable()
	{
		Events.OnSettingChanged -= SettingChanged;
	}

	public void SettingChanged(string key, object value)
	{
		if (!(key != "TransitionType") && value is int)
		{
			int transitionType = (int)value;
			SetTransitionType(transitionType);
		}
	}

	public void SetTransitionType(int index)
	{
		prefab = transitions[Mathf.Clamp(index, 0, transitions.Length - 1)];
	}

	public static TransitionType Begin()
	{
		if ((bool)current)
		{
			if (Running)
			{
				return current;
			}

			Object.Destroy(current.gameObject);
		}

		if (!MonoBehaviourSingleton<Transition>.instance.prefab)
		{
			MonoBehaviourSingleton<Transition>.instance.SetTransitionType(Settings.Load("TransitionType", 0));
		}

		current = Object.Instantiate(MonoBehaviourSingleton<Transition>.instance.prefab, MonoBehaviourSingleton<Transition>.instance.transform);
		MonoBehaviourSingleton<Transition>.instance.StopAllCoroutines();
		MonoBehaviourSingleton<Transition>.instance.StartCoroutine(current.In());
		Events.InvokeTransitionStart(current);
		Running = true;
		return current;
	}

	public static void End()
	{
		if ((bool)current)
		{
			MonoBehaviourSingleton<Transition>.instance.StopAllCoroutines();
			MonoBehaviourSingleton<Transition>.instance.StartCoroutine(current.Out());
			Events.InvokeTransitionEnd(current);
			Running = false;
		}
	}

	public static IEnumerator WaitUntilDone(TransitionType transition)
	{
		if ((object)transition == null)
		{
			transition = current;
		}

		yield return new WaitUntil(() => !transition || !transition.IsRunning);
	}

	public static IEnumerator To(string newSceneKey)
	{
		TransitionType transition = Begin();
		yield return WaitUntilDone(transition);
		yield return Sequences.SceneChange(newSceneKey);
	}
}
