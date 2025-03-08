#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TurnOrderButton : MonoBehaviour
{
	[SerializeField]
	public ButtonAnimator button;

	[SerializeField]
	public Image image;

	[SerializeField]
	public Sprite openSprite;

	[SerializeField]
	public Sprite closedSprite;

	[SerializeField]
	public ParticleSystem particleSystem;

	public void Awake()
	{
		Events.OnSceneChanged += SceneChanged;
		SetActive(value: false);
	}

	public void OnDestroy()
	{
		Events.OnSceneChanged -= SceneChanged;
	}

	public void OnEnable()
	{
		Events.OnCardControllerEnabled += CardControllerEnabled;
		Events.OnCardControllerDisabled += CardControllerDisabled;
	}

	public void OnDisable()
	{
		Events.OnCardControllerEnabled -= CardControllerEnabled;
		Events.OnCardControllerDisabled -= CardControllerDisabled;
	}

	public void CardControllerEnabled(CardController controller)
	{
		if ((bool)References.Battle && controller == References.Battle.playerCardController)
		{
			button.interactable = true;
		}
	}

	public void CardControllerDisabled(CardController controller)
	{
		if ((bool)References.Battle && controller == References.Battle.playerCardController)
		{
			button.interactable = false;
		}
	}

	public void SceneChanged(Scene scene)
	{
		SetActive(scene.name == "Battle");
	}

	public void SetActive(bool value)
	{
		base.gameObject.SetActive(value);
		button.interactable = value;
		CloseEye();
		button.interactable = false;
	}

	public void Select()
	{
		if (ActionQueue.Empty && (!References.Battle || !References.Battle.playerCardController.dragging))
		{
			Object.FindObjectOfType<TurnOrderDisplay>()?.Toggle();
		}
	}

	public void OpenEye()
	{
		image.sprite = openSprite;
		particleSystem.Play();
	}

	public void CloseEye()
	{
		image.sprite = closedSprite;
		particleSystem.Stop();
	}
}
