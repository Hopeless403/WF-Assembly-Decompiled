#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class EventManager : MonoBehaviour
{
	public static EventManager instance;

	public Transform eventRoutineHolder;

	[SerializeField]
	public TweenUI enterTween;

	public static Transform EventRoutineHolder => instance.eventRoutineHolder;

	public void Awake()
	{
		instance = this;
	}

	public void OnEnable()
	{
		if (GameManager.Ready)
		{
			CinemaBarSystem.InInstant();
			enterTween.Fire();
		}
	}

	public void OnDisable()
	{
		CinemaBarSystem.OutInstant();
	}
}
