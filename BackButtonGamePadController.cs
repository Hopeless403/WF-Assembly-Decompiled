#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BackButtonGamePadController : MonoBehaviour
{
	public static bool isGoingBack;

	public Button backButton;

	public EventTrigger eventTrigger;

	public UINavigationLayer uINavigationLayer;

	public UINavigationItem navigationItem;

	public UnityEvent OnBackButtonOverride;

	public const float backClearTime = 0.1f;

	public Coroutine GoingBackClear;

	public bool navItemEnabled;

	public bool press;

	public void Update()
	{
		if (navItemEnabled)
		{
			if (!CheckNavigationItem())
			{
				navItemEnabled = false;
			}
			else if (isGoingBack)
			{
				if (GoingBackClear == null)
				{
					GoingBackClear = StartCoroutine(RunGoBackClear());
				}

				if (press)
				{
					press = false;
					Release();
				}
			}

			else if (MonoBehaviourSingleton<UINavigationSystem>.instance.lastActiveNavigationLayer == uINavigationLayer && !MonoBehaviourSingleton<Cursor3d>.instance.usingMouse)
			{
				if (press)
				{
					if (!InputSystem.Enabled || InputSystem.reset)
					{
						press = false;
						Release();
					}

					if (!InputSystem.IsButtonHeld("Back"))
					{
						Release();
					}
				}
				else if (InputSystem.IsButtonPressed("Back"))
				{
					Press();
				}
			}

			else if (press)
			{
				press = false;
				Release();
			}
		}
		else if (CheckNavigationItem())
		{
			navItemEnabled = true;
		}
	}

	public void Press()
	{
		press = true;
		if ((bool)eventTrigger && eventTrigger.enabled)
		{
			eventTrigger.OnPointerDown(null);
		}
	}

	public void Release()
	{
		if (press)
		{
			Invoke();
		}

		if ((bool)eventTrigger)
		{
			if (press && eventTrigger.enabled)
			{
				eventTrigger.OnPointerClick(null);
			}

			eventTrigger.OnPointerUp(null);
		}

		press = false;
	}

	public void Invoke()
	{
		isGoingBack = true;
		if (OnBackButtonOverride != null && OnBackButtonOverride.GetPersistentEventCount() > 0)
		{
			OnBackButtonOverride.Invoke();
		}
		else
		{
			backButton.onClick.Invoke();
		}
	}

	public IEnumerator RunGoBackClear()
	{
		yield return new WaitForSecondsRealtime(0.1f);
		isGoingBack = false;
		GoingBackClear = null;
	}

	public void Start()
	{
		CheckForNavigationLayer(base.transform);
	}

	public void CheckForNavigationLayer(Transform checkTransform)
	{
		UINavigationLayer component = checkTransform.GetComponent<UINavigationLayer>();
		if ((object)component != null && component.isOverrideLayer)
		{
			uINavigationLayer = component;
		}

		if (uINavigationLayer == null && checkTransform.transform.parent != null)
		{
			CheckForNavigationLayer(checkTransform.transform.parent);
		}
	}

	public bool CheckNavigationItem()
	{
		if ((bool)navigationItem)
		{
			return MonoBehaviourSingleton<UINavigationSystem>.instance.AvailableNavigationItems.Contains(navigationItem);
		}

		return true;
	}
}
