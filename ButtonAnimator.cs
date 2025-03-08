#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimator : MonoBehaviour
{
	[SerializeField]
	public ButtonType type;

	[InfoBox("This component should be on a separate object to the Button object, so as not to interfere with their tweens", EInfoBoxType.Normal)]
	[SerializeField]
	[Required(null)]
	public Selectable button;

	[SerializeField]
	public UINavigationItem nav;

	public bool hover;

	public bool press;

	public LTDescr tween;

	[Header("Text Colours")]
	public TMP_Text text;

	public Color textNormalColour = Color.white;

	[SerializeField]
	public bool textCopyBase = true;

	[HideIf("textCopyBase")]
	[SerializeField]
	public Color textHighlightColour = Color.white;

	[SerializeField]
	public bool strikeTextWhenDisabled;

	[ShowIf("strikeTextWhenDisabled")]
	[SerializeField]
	public Color textDisabledColour = Color.black;

	public Image image;

	public bool baseColourSet;

	public Color baseColour;

	public Color highlightColour = new Color(1f, 1f, 1f, 0.8f);

	public Color disabledColour = new Color(0.5f, 0.5f, 0.5f, 0.75f);

	[SerializeField]
	public bool setPressColour;

	[ShowIf("setPressColour")]
	[SerializeField]
	public Color pressColour;

	[Header("Tweens")]
	public TweenUI hoverTween;

	public TweenUI unHoverTween;

	public TweenUI pressTween;

	public TweenUI releaseTween;

	public bool interactable
	{
		get
		{
			return button.interactable;
		}
		set
		{
			if (value)
			{
				UnHighlight();
				if ((bool)text && strikeTextWhenDisabled)
				{
					text.fontStyle = FontStyles.Normal;
				}
			}
			else
			{
				Disable();
			}

			button.interactable = value;
			if ((bool)nav)
			{
				nav.enabled = value;
			}
		}
	}

	public bool IsHoveredOrPressed
	{
		get
		{
			if (!hover)
			{
				return press;
			}

			return true;
		}
	}

	public void OnEnable()
	{
		hover = false;
		press = false;
		if (interactable)
		{
			UnHighlight();
			if (text != null && strikeTextWhenDisabled)
			{
				text.fontStyle = FontStyles.Normal;
			}
		}
		else
		{
			Disable();
		}
	}

	public void OnDisable()
	{
		press = false;
		UnHoverInstant();
	}

	public virtual void Hover()
	{
		if (!hover && interactable)
		{
			hover = true;
			if (!press)
			{
				StopCurrentAnimation();
				hoverTween?.Fire();
				Events.InvokeButtonHover(type);
				Highlight();
			}
		}
	}

	public virtual void UnHover()
	{
		if (hover)
		{
			hover = false;
			if (!press)
			{
				StopCurrentAnimation();
				unHoverTween?.Fire();
				UnHighlight();
			}
		}
	}

	public void UnHoverInstant()
	{
		if (hover)
		{
			hover = false;
			if (!press)
			{
				StopCurrentAnimation();
				base.transform.localScale = Vector3.one;
				UnHighlight();
			}
		}
	}

	public virtual void Press()
	{
		if (press || !interactable)
		{
			return;
		}

		press = true;
		StopCurrentAnimation();
		pressTween?.Fire();
		Events.InvokeButtonPress(type);
		if (setPressColour)
		{
			if (image != null)
			{
				image.color = pressColour;
			}
		}
		else
		{
			UnHighlight();
		}
	}

	public virtual void Release()
	{
		if (press)
		{
			press = false;
			StopCurrentAnimation();
			if (!hover)
			{
				releaseTween?.Fire();
			}
			else if (interactable)
			{
				hoverTween?.Fire();
				Highlight();
			}
			else
			{
				hover = false;
			}

			if (setPressColour && !hover)
			{
				UnHighlight();
			}
		}
	}

	public void StopCurrentAnimation()
	{
		LeanTween.cancel(base.gameObject);
	}

	public void CheckSetBaseColour()
	{
		if (!baseColourSet)
		{
			baseColourSet = true;
			baseColour = image.color;
		}
	}

	public void Highlight()
	{
		if (image != null)
		{
			CheckSetBaseColour();
			image.color = highlightColour;
		}

		if (text != null)
		{
			text.color = (textCopyBase ? baseColour : textHighlightColour);
		}
	}

	public void UnHighlight()
	{
		if (image != null)
		{
			CheckSetBaseColour();
			image.color = baseColour;
		}

		if (text != null)
		{
			text.color = textNormalColour;
		}
	}

	public void Disable()
	{
		if (hover)
		{
			UnHover();
		}

		if (image != null)
		{
			CheckSetBaseColour();
			image.color = disabledColour;
		}

		if (text != null)
		{
			if (strikeTextWhenDisabled)
			{
				text.fontStyle = FontStyles.Strikethrough;
			}

			text.color = (strikeTextWhenDisabled ? textDisabledColour : disabledColour);
		}
	}
}
