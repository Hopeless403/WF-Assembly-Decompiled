#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.Events;

public class CardPocketInteraction : MonoBehaviour
{
	[SerializeField]
	public UINavigationItem nav;

	[SerializeField]
	public GameObject root;

	[SerializeField]
	public UnityEvent onClick;

	[SerializeField]
	public UnityEvent onPress;

	[SerializeField]
	public UnityEvent onRelease;

	[SerializeField]
	public UnityEvent onHover;

	[SerializeField]
	public UnityEvent onUnHover;

	public bool interactable = true;

	[SerializeField]
	public bool defaultAnimations = true;

	public bool hover;

	public bool hovered;

	public bool press;

	public bool pressed;

	public bool IsInteractable
	{
		get
		{
			if (interactable)
			{
				if (!(nav == null))
				{
					return nav.enabled;
				}

				return true;
			}

			return false;
		}
	}

	public void SetInteractable(bool value)
	{
		interactable = value;
	}

	public void Update()
	{
		if (hover)
		{
			if (!hovered)
			{
				hovered = true;
				if (defaultAnimations)
				{
					LeanTween.cancel(root);
					LeanTween.scale(root, Vector3.one * 1.1f, 0.2f).setEase(LeanTweenType.easeOutBack);
				}

				onHover?.Invoke();
			}
		}
		else if (hovered)
		{
			hovered = false;
			if (defaultAnimations)
			{
				LeanTween.cancel(root);
				LeanTween.scale(root, Vector3.one, 0.1f).setEase(LeanTweenType.easeOutQuart);
			}

			onUnHover?.Invoke();
		}

		if (press)
		{
			if (!pressed)
			{
				pressed = true;
				if (defaultAnimations)
				{
					LeanTween.cancel(root);
					LeanTween.scale(root, Vector3.one, 0.1f).setEase(LeanTweenType.easeOutQuart);
				}

				onPress?.Invoke();
			}
		}
		else if (pressed)
		{
			pressed = false;
			onRelease?.Invoke();
			if (hover)
			{
				if (defaultAnimations)
				{
					LeanTween.cancel(root);
					LeanTween.scale(root, Vector3.one * 1.1f, 0.1f).setEase(LeanTweenType.easeOutQuart);
				}

				onClick?.Invoke();
			}
			else
			{
				if (defaultAnimations)
				{
					LeanTween.cancel(root);
					LeanTween.scale(root, Vector3.one, 0.1f).setEase(LeanTweenType.easeOutQuart);
				}

				if (MonoBehaviourSingleton<UINavigationSystem>.instance.currentNavigationItem == nav)
				{
					Hover();
				}
			}
		}

		if (!press)
		{
			if (hover && InputSystem.IsSelectPressed())
			{
				Press();
			}
		}
		else if (!InputSystem.IsSelectHeld())
		{
			Release();
		}
	}

	public void Hover()
	{
		hover = IsInteractable;
	}

	public void UnHover()
	{
		hover = false;
	}

	public void Press()
	{
		press = IsInteractable;
	}

	public void Release()
	{
		press = false;
	}
}
