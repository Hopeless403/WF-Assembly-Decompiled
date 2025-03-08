#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TribeFlagDisplay : MonoBehaviour
{
	public Image flagImage;

	[SerializeField]
	public Sprite flagSprite;

	[SerializeField]
	public GameObject locked;

	[SerializeField]
	public ButtonAnimator button;

	[SerializeField]
	public InputAction inputAction;

	public void SetFlagSprite(Sprite sprite)
	{
		flagSprite = sprite;
		flagImage.sprite = sprite;
	}

	public void AddPressAction(UnityAction action)
	{
		inputAction.action.AddListener(action);
	}

	public void ClearPressActions()
	{
		inputAction.action.RemoveAllListeners();
	}

	public void SetInteractable(bool interactable)
	{
		button.interactable = interactable;
	}

	public void SetUnlocked()
	{
		flagImage.sprite = flagSprite;
		button.interactable = true;
	}

	public void SetAvailable()
	{
		locked.SetActive(value: false);
	}
}
