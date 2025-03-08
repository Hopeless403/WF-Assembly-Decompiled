#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using FMODUnity;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapNode : MonoBehaviour
{
	public MapNew map;

	public CampaignNode campaignNode;

	public MapNode[] connections;

	public int connectedTo;

	public bool interactable;

	[SerializeField]
	public Animator animator;

	[SerializeField]
	public GameObject highlight;

	[SerializeField]
	public GameObject glow;

	[SerializeField]
	public UINavigationItem uINavigationItem;

	[SerializeField]
	public MapNodeSpriteSetter spriteSetter;

	[SerializeField]
	[HideIf("HasSpriteSetter")]
	public Sprite[] spriteOptions;

	[SerializeField]
	[HideIf("HasSpriteSetter")]
	public Sprite[] clearedSpriteOptions;

	public int spriteIndex;

	[SerializeField]
	public EventReference highlightSfx;

	public bool selectable;

	public bool _hoverable = true;

	public bool _pressable = true;

	public bool reachable = true;

	public bool hasSprite;

	public bool hover;

	public bool press;

	[SerializeField]
	public Transform scaler;

	[SerializeField]
	public SpriteRenderer spriteRenderer;

	[SerializeField]
	public MapNodeLabel label;

	public bool HasSpriteSetter => spriteSetter;

	public bool hoverable
	{
		get
		{
			return _hoverable;
		}
		set
		{
			if (_hoverable != value)
			{
				if (value)
				{
					if (hasSprite)
					{
						spriteRenderer.color = Color.white;
					}

					if ((bool)scaler)
					{
						LeanTween.cancel(scaler.gameObject);
						LeanTween.scale(scaler.gameObject, Vector3.one * 1f, 0.33f).setEase(LeanTweenType.easeOutBack);
					}
				}
				else
				{
					Color color = (reachable ? new Color(1f, 1f, 1f, 1f) : new Color(1f, 1f, 1f, 0.25f));
					if (hasSprite)
					{
						spriteRenderer.color = color;
					}

					if ((bool)scaler)
					{
						LeanTween.cancel(scaler.gameObject);
						LeanTween.scale(scaler.gameObject, Vector3.one * 0.9f, 0.33f).setEase(LeanTweenType.easeOutBack);
					}

					if (hover)
					{
						UnHover();
					}
				}
			}

			if ((bool)uINavigationItem)
			{
				uINavigationItem.enabled = value;
			}

			_hoverable = value;
		}
	}

	public bool pressable
	{
		get
		{
			return _pressable;
		}
		set
		{
			_pressable = value;
		}
	}

	public bool IsHovered => hover;

	public Color color
	{
		set
		{
			if (hasSprite)
			{
				spriteRenderer.color = value;
			}
		}
	}

	public void Assign(CampaignNode node)
	{
		hasSprite = spriteRenderer;
		Random.InitState(node.seed);
		if (!HasSpriteSetter && spriteOptions.Length != 0)
		{
			spriteIndex = Random.Range(0, spriteOptions.Length);
		}

		campaignNode = node;
		Refresh();
		interactable = node.type.interactable;
		if (hasSprite)
		{
			spriteRenderer.GetComponent<Collider2D>()?.Destroy();
			spriteRenderer.gameObject.AddComponent<BoxCollider2D>();
		}

		if ((bool)glow)
		{
			glow.SetActive(node.glow && !node.cleared);
		}
	}

	public void Refresh()
	{
		if (!hasSprite)
		{
			return;
		}

		if (HasSpriteSetter)
		{
			spriteSetter.Set(this);
			return;
		}

		Sprite sprite = spriteOptions[spriteIndex];
		Sprite sprite2 = sprite;
		if (clearedSpriteOptions.Length != 0)
		{
			sprite2 = clearedSpriteOptions[spriteIndex % clearedSpriteOptions.Length];
		}

		SetSprite((!campaignNode.cleared) ? sprite : (sprite2 ? sprite2 : sprite));
	}

	public void SetSprite(Sprite sprite)
	{
		spriteRenderer.sprite = sprite;
	}

	public void OnEnable()
	{
		CheckForFocus();
	}

	public void CheckForFocus()
	{
		if (!MonoBehaviourSingleton<Cursor3d>.instance.usingMouse && (bool)highlight && highlight.gameObject.activeSelf && (bool)uINavigationItem && uINavigationItem.isActiveAndEnabled)
		{
			MonoBehaviourSingleton<UINavigationSystem>.instance.SetCurrentNavigationItem(uINavigationItem);
		}
	}

	public void Reveal()
	{
		campaignNode.revealed = true;
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
			base.transform.localScale = Vector3.zero;
			LeanTween.scale(base.gameObject, Vector3.one, Random.Range(0.4f, 0.5f)).setEase(LeanTweenType.easeOutBack);
			base.transform.localEulerAngles = new Vector3(0f, 0f, Random.Range(45f, 90f));
			LeanTween.rotateLocal(base.gameObject, Vector3.zero, Random.Range(0.4f, 0.5f)).setEase(LeanTweenType.easeOutBack);
			Events.InvokeMapNodeReveal(this);
			if (!hover && MonoBehaviourSingleton<UINavigationSystem>.instance.currentNavigationItem == uINavigationItem)
			{
				Hover();
			}
		}
	}

	public void Hover()
	{
		if (interactable && hoverable && !hover)
		{
			hover = true;
			LeanTween.cancel(spriteRenderer.gameObject);
			LeanTween.scale(spriteRenderer.gameObject, Vector3.one * 1.1f, 0.23f).setEase(LeanTweenType.easeOutBack);
			if ((bool)label)
			{
				label.Show();
			}

			Events.InvokeMapNodeHover(this);
		}
	}

	public void UnHover()
	{
		if (hover)
		{
			hover = false;
			LeanTween.cancel(spriteRenderer.gameObject);
			LeanTween.scale(spriteRenderer.gameObject, Vector3.one * 1f, 0.13f).setEase(LeanTweenType.easeOutBack);
			if ((bool)label)
			{
				label.Hide();
			}

			Events.InvokeMapNodeUnHover(this);
		}
	}

	public void Press(BaseEventData eventData)
	{
		if ((!(eventData is PointerEventData pointerEventData) || pointerEventData.button == PointerEventData.InputButton.Left) && interactable && pressable && !press && hover)
		{
			press = true;
			color = new Color(0.85f, 0.85f, 0.85f);
		}
	}

	public void Release(BaseEventData eventData)
	{
		if ((!(eventData is PointerEventData pointerEventData) || pointerEventData.button == PointerEventData.InputButton.Left) && press)
		{
			press = false;
			color = Color.white;
			if (hover)
			{
				Select();
			}
		}
	}

	public void Select()
	{
		if (selectable && map.TryMoveTo(this))
		{
			Events.InvokeMapNodeSelect(this);
			animator.Play("Select");
			glow.SetActive(value: false);
		}
		else
		{
			Events.InvokeMapNodeSelect(null);
			animator.Play("Shake");
		}
	}

	public void SetSelectable(bool value)
	{
		if (!selectable && value)
		{
			if ((bool)animator)
			{
				animator.Play("Selectable");
			}

			if ((bool)highlight)
			{
				highlight.gameObject.SetActive(value: true);
				highlight.transform.localScale = Vector3.zero;
				LeanTween.scale(highlight, Vector3.one, 1.25f).setEase(LeanTweenType.easeOutElastic).setDelay(0.25f);
			}

			SfxSystem.OneShot(highlightSfx);
			CheckForFocus();
		}
		else if (selectable && !value)
		{
			if ((bool)animator)
			{
				animator.Stop();
			}

			if ((bool)highlight)
			{
				highlight.gameObject.SetActive(value: false);
			}
		}

		selectable = value;
	}

	public override string ToString()
	{
		return base.name;
	}
}
