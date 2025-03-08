#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JournalIconGroup : MonoBehaviour
{
	[SerializeField]
	public List<ButtonAnimator> buttons;

	[SerializeField]
	public Image[] icons;

	[SerializeField]
	public Color baseColour = Color.white;

	[SerializeField]
	public Color highlightColour = Color.white;

	public ButtonAnimator currentHover;

	public Image highlightedIcon;

	public void Awake()
	{
		Image[] array = icons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].color = baseColour;
		}
	}

	public void Update()
	{
		if (currentHover == null)
		{
			ButtonAnimator buttonAnimator = buttons.FirstOrDefault((ButtonAnimator a) => a.IsHoveredOrPressed);
			if (buttonAnimator != null)
			{
				currentHover = buttonAnimator;
				Changed();
			}
		}
		else if (!currentHover.IsHoveredOrPressed)
		{
			currentHover = null;
			Changed();
		}
	}

	public void Changed()
	{
		if (highlightedIcon != null)
		{
			highlightedIcon.color = baseColour;
		}

		if (currentHover != null)
		{
			int num = buttons.IndexOf(currentHover);
			highlightedIcon = icons[num];
			highlightedIcon.color = highlightColour;
		}
	}
}
