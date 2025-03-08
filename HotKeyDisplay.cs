#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HotKeyDisplay : MonoBehaviour
{
	public Image _image;

	[SerializeField]
	public string actionName;

	public Image image => _image ?? (_image = GetComponent<Image>());

	public void OnEnable()
	{
		Events.OnButtonStyleChanged += Refresh;
		Refresh();
	}

	public void OnDisable()
	{
		Events.OnButtonStyleChanged -= Refresh;
	}

	public void Refresh()
	{
		if (MonoBehaviourSingleton<Cursor3d>.instance.usingMouse)
		{
			image.enabled = false;
			return;
		}

		JoystickButtonStyle.ElementButton element = ControllerButtonSystem.GetElement(RewiredControllerManager.GetPlayerController(0), actionName);
		image.enabled = element?.hasSprite ?? false;
		if (image.enabled)
		{
			image.sprite = element.buttonSprite;
		}
	}

	public void SetActionName(string value)
	{
		actionName = value;
		Refresh();
	}
}
