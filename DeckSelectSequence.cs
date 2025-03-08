#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DeckSelectSequence : UISequence
{
	[SerializeField]
	public UnityEvent onEnable;

	[SerializeField]
	public UnityEvent onDisable;

	[Header("Custom Values")]
	[SerializeField]
	public Transform cardHolder;

	[SerializeField]
	public CanvasGroup fade;

	[SerializeField]
	public float fadeInTime = 0.1f;

	[SerializeField]
	public float fadeOutTime = 0.1f;

	[SerializeField]
	public CardSelector cardSelector;

	[SerializeField]
	public RenameCompanionSequence renameSequence;

	[SerializeField]
	public CrownHolder crownHolder;

	[Header("Buttons")]
	[SerializeField]
	public GameObject buttonGroup;

	[SerializeField]
	public TweenUI buttonShowTween;

	[SerializeField]
	public TweenUI buttonHideTween;

	[SerializeField]
	public GameObject renameButton;

	[SerializeField]
	public GameObject takeCrownButton;

	[SerializeField]
	public GameObject moveDownButton;

	[SerializeField]
	public Button moveDownButtonButton;

	[SerializeField]
	public GameObject moveUpButton;

	[SerializeField]
	public Button moveUpButtonButton;

	[Header("Movement")]
	[SerializeField]
	public float entityScale = 0.75f;

	[SerializeField]
	public float moveWobble = 1.5f;

	[SerializeField]
	public AnimationCurve moveCurve;

	[SerializeField]
	public float moveDur = 0.5f;

	public Entity entity;

	public Transform entityPreParent;

	public bool promptRename;

	public void SetEntity(Entity entity, bool canRename = true)
	{
		this.entity = entity;
		renameButton.SetActive((bool)entity && canRename);
		takeCrownButton.SetActive((bool)entity && EntityHasRemovableCrown(entity.data) && entity.data.cardType.canTakeCrown && (!References.Battle || References.Battle.ended));
		moveDownButton.SetActive(value: false);
		moveUpButton.SetActive(value: false);
	}

	public static bool EntityHasRemovableCrown(CardData cardData)
	{
		CardUpgradeData crown = cardData.GetCrown();
		if ((bool)crown)
		{
			return crown.canBeRemoved;
		}

		return false;
	}

	public void AddMoveDown(UnityAction callback)
	{
		moveDownButton.SetActive(value: true);
		moveDownButtonButton.onClick.RemoveAllListeners();
		moveDownButtonButton.onClick.AddListener(callback);
		moveDownButtonButton.onClick.AddListener(delegate
		{
			End();
			entity = null;
		});
	}

	public void AddMoveUp(UnityAction callback)
	{
		moveUpButton.SetActive(value: true);
		moveUpButtonButton.onClick.RemoveAllListeners();
		moveUpButtonButton.onClick.AddListener(callback);
		moveUpButtonButton.onClick.AddListener(delegate
		{
			End();
			entity = null;
		});
	}

	public void Rename()
	{
		promptRename = true;
	}

	public void TakeCrown()
	{
		CardUpgradeData crown = entity.data.GetCrown();
		if ((object)crown != null)
		{
			entity.data.RemoveCrown();
			if (entity.display is Card card && card.crownHolder is CrownHolder crownHolder)
			{
				crownHolder.Remove(crown);
			}

			References.PlayerData.inventory.upgrades.Add(crown);
			this.crownHolder.Create(crown);
			this.crownHolder.SetPositions();
		}
	}

	public override void End()
	{
		promptEnd = true;
	}

	public override IEnumerator Run()
	{
		onEnable?.Invoke();
		buttonGroup.SetActive(value: false);
		yield return Sequences.Wait(startDelay);
		base.gameObject.SetActive(value: true);
		if (moveWobble != 0f)
		{
			entity.wobbler?.WobbleRandom(moveWobble);
		}

		entityPreParent = entity.transform.parent;
		entity.transform.SetParent(cardHolder, worldPositionStays: true);
		LeanTween.cancel(entity.gameObject);
		LeanTween.moveLocal(entity.gameObject, Vector3.zero, moveDur).setEase(moveCurve);
		LeanTween.rotateLocal(entity.gameObject, Vector3.zero, moveDur).setEase(moveCurve);
		LeanTween.scale(entity.gameObject, Vector3.one * entityScale, moveDur).setEase(moveCurve);
		Events.InvokeEntityFocus(entity);
		fade.gameObject.SetActive(value: true);
		fade.alpha = 0f;
		fade.LeanAlpha(1f, fadeInTime);
		fade.blocksRaycasts = true;
		buttonGroup.SetActive(value: true);
		buttonShowTween?.Fire();
		bool protectEnd = true;
		int protectEndCount = 5;
		while (!promptEnd || protectEnd)
		{
			if (promptRename)
			{
				renameSequence.SetUnit(entity);
				yield return renameSequence.Run();
				promptRename = false;
			}

			yield return null;
			if (protectEnd)
			{
				promptEnd = false;
				protectEndCount--;
				if (protectEndCount <= 0)
				{
					protectEnd = false;
				}
			}
		}

		promptEnd = false;
		if (entity != null)
		{
			entity.transform.SetParent(entityPreParent, worldPositionStays: true);
			CardContainer[] containers = entity.containers;
			for (int i = 0; i < containers.Length; i++)
			{
				containers[i].TweenChildPositions();
			}

			if (moveWobble != 0f)
			{
				entity.wobbler?.WobbleRandom(moveWobble);
			}
		}

		fade.LeanAlpha(0f, fadeOutTime);
		fade.blocksRaycasts = false;
		onDisable?.Invoke();
		base.gameObject.SetActive(value: false);
	}
}
