#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.Localization;

public class IntroSequence : MonoBehaviour
{
	[SerializeField]
	public string nextScene;

	[SerializeField]
	public UnityEngine.Animator animator;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString[] textKeys;

	public int textCurrent;

	public GameObject current;

	public void Start()
	{
		if (GameManager.Ready)
		{
			Transition.End();
			StartSequence();
		}
	}

	public void OnDisable()
	{
		CinemaBarSystem.Clear();
		CinemaBarSystem.OutInstant();
	}

	public void StartSequence()
	{
		CinemaBarSystem.InInstant();
		CinemaBarSystem.Clear();
		animator.SetTrigger("Play");
	}

	public void EndSequence()
	{
		new Routine(Transition.To(nextScene));
	}

	public void NextText()
	{
		CinemaBarSystem.SetScript(textKeys[textCurrent++].GetLocalizedString(), typewriterAnimation: true);
	}
}
