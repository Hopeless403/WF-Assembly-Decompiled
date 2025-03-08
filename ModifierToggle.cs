#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class ModifierToggle : MonoBehaviour
{
	[SerializeField]
	public bool active = true;

	public bool canToggle = true;

	[SerializeField]
	[HideIf("canToggle")]
	public bool stillRing;

	public UnityEvent onToggle;

	[SerializeField]
	public UnityEngine.Animator animator;

	public bool IsActive => active;

	public void OnEnable()
	{
		UpdateAnimator();
	}

	public void UpdateAnimator()
	{
		if ((bool)animator && animator.isActiveAndEnabled)
		{
			animator.SetBool("Active", active);
			animator.SetBool("CanToggle", canToggle);
		}
	}

	public void Toggle()
	{
		if (canToggle)
		{
			SetActive(!active);
			onToggle?.Invoke();
		}
		else if (stillRing)
		{
			GetComponent<ModifierIcon>()?.Ding();
		}
	}

	public void SetActive(bool value)
	{
		active = value;
		UpdateArt();
	}

	public void UpdateArt()
	{
		if (active)
		{
			GetComponent<ModifierIcon>()?.Ding();
		}

		UpdateAnimator();
	}
}
