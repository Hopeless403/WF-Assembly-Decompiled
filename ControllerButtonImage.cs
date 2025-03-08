#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ControllerButtonImage : MonoBehaviour
{
	[SerializeField]
	public bool deactivateIfNull = true;

	[SerializeField]
	public bool disableIfNull;

	public Image image;

	public string actionName { get; set; }

	public void OnEnable()
	{
		image = GetComponent<Image>();
		Events.OnButtonStyleChanged += ButtonStyleChanged;
	}

	public void OnDisable()
	{
		Events.OnButtonStyleChanged -= ButtonStyleChanged;
	}

	public void Set(string actionName)
	{
		this.actionName = actionName;
		Sprite sprite = ControllerButtonSystem.Get(actionName);
		if (deactivateIfNull)
		{
			base.gameObject.SetActive(sprite);
		}

		if (disableIfNull)
		{
			image.enabled = sprite;
		}

		if ((bool)sprite)
		{
			image.sprite = sprite;
		}
	}

	public void ButtonStyleChanged()
	{
		if (!actionName.IsNullOrWhitespace())
		{
			Set(actionName);
		}
	}
}
