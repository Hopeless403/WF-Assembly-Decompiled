#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class JournalTab : MonoBehaviour
{
	[SerializeField]
	public ButtonType type = ButtonType.Sub;

	[SerializeField]
	public GameObject tweenTarget;

	[SerializeField]
	public Image image;

	[SerializeField]
	public UINavigationItem nav;

	[SerializeField]
	public bool selected;

	[SerializeField]
	public GameObject selectedGroup;

	[SerializeField]
	public GameObject unselectedGroup;

	[SerializeField]
	public UnityEvent onSelect;

	[SerializeField]
	public Transform[] unselectGroups;

	[Header("Colours")]
	[SerializeField]
	public Color baseColour;

	[SerializeField]
	public Color highlightColour;

	[SerializeField]
	public Color pressColour;

	[SerializeField]
	public Color disabledColour;

	public bool _interactable = true;

	public bool hover;

	public bool press;

	public bool interactable
	{
		get
		{
			return _interactable;
		}
		set
		{
			if (value)
			{
				SetUnHighlighted();
			}
			else
			{
				SetDisabled();
			}

			_interactable = value;
			if (nav != null)
			{
				nav.enabled = value;
			}
		}
	}

	public void OnEnable()
	{
		SetSelected();
	}

	public void Hover()
	{
		if (!hover && interactable)
		{
			hover = true;
			if (!press)
			{
				HoverTween();
				SetHighlighted();
				Events.InvokeButtonHover(type);
			}
		}
	}

	public void UnHover()
	{
		if (hover)
		{
			hover = false;
			if (!press)
			{
				UnHoverTween();
				SetUnHighlighted();
			}
		}
	}

	public void Press()
	{
		if (!press && interactable)
		{
			press = true;
			PressTween();
			SetPressed();
			Events.InvokeButtonPress(type);
		}
	}

	public void Release()
	{
		if (press)
		{
			press = false;
			if (hover)
			{
				SfxSystem.OneShot("event:/sfx/ui/journal_click");
				Select();
				SetHighlighted();
				HoverTween();
			}
			else
			{
				SetUnHighlighted();
				ReleaseTween();
			}
		}
	}

	public void Select()
	{
		Transform[] array = unselectGroups;
		for (int i = 0; i < array.Length; i++)
		{
			foreach (Transform item in array[i])
			{
				JournalTab component = item.GetComponent<JournalTab>();
				if ((object)component != null && component.selected)
				{
					component.Deselect();
				}
			}
		}

		selected = true;
		SetSelected();
		onSelect?.Invoke();
	}

	public void Deselect()
	{
		selected = false;
		SetSelected();
	}

	public void HoverTween()
	{
		LeanTween.cancel(tweenTarget);
		LeanTween.scale(tweenTarget, new Vector3(1.05f, 1.05f, 1f), 0.2f).setIgnoreTimeScale(useUnScaledTime: true).setEaseOutBack();
	}

	public void UnHoverTween()
	{
		LeanTween.cancel(tweenTarget);
		LeanTween.scale(tweenTarget, Vector3.one, 0.05f).setIgnoreTimeScale(useUnScaledTime: true);
	}

	public void PressTween()
	{
		LeanTween.cancel(tweenTarget);
		LeanTween.scale(tweenTarget, new Vector3(0.95f, 0.95f, 1f), 0.05f).setIgnoreTimeScale(useUnScaledTime: true);
	}

	public void ReleaseTween()
	{
		LeanTween.cancel(tweenTarget);
		LeanTween.scale(tweenTarget, Vector3.one, 0.05f).setIgnoreTimeScale(useUnScaledTime: true);
	}

	public void SetHighlighted()
	{
		if (image != null)
		{
			image.color = highlightColour;
		}
	}

	public void SetUnHighlighted()
	{
		if (image != null)
		{
			image.color = baseColour;
		}
	}

	public void SetPressed()
	{
		if (image != null)
		{
			image.color = pressColour;
		}
	}

	public void SetDisabled()
	{
		if (image != null)
		{
			image.color = disabledColour;
		}
	}

	public void SetSelected()
	{
		selectedGroup.SetActive(selected);
		unselectedGroup.SetActive(!selected);
	}
}
