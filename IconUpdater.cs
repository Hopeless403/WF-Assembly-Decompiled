#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(StatusIcon))]
public class IconUpdater : MonoBehaviour
{
	public Sprite iconIfValueBelowMax;

	public Sprite iconIfValueAboveMax;

	public Image image;

	public Sprite defaultIcon;

	public void Awake()
	{
		image = GetComponent<Image>();
		defaultIcon = image.sprite;
	}

	public void CheckUpdate(Stat previousValue, Stat newValue)
	{
		StatusIcon component = GetComponent<StatusIcon>();
		if (component != null && component.target != null)
		{
			if (newValue.current < newValue.max && previousValue.current >= previousValue.max)
			{
				image.sprite = ((iconIfValueBelowMax != null) ? iconIfValueBelowMax : defaultIcon);
			}
			else if (iconIfValueAboveMax != null && newValue.current > newValue.max && previousValue.current <= previousValue.max)
			{
				image.sprite = ((iconIfValueAboveMax != null) ? iconIfValueAboveMax : defaultIcon);
			}

			else if (newValue.current == newValue.max && previousValue.current != previousValue.max)
			{
				image.sprite = defaultIcon;
			}
		}
	}
}
