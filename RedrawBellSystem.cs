#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;

public class RedrawBellSystem : GameSystem
{
	public RectTransform _rectTransform;

	[SerializeField]
	public GameObject bell;

	[SerializeField]
	public GameObject bellActive;

	[SerializeField]
	public ParticleSystem chargeParticleSystem;

	[SerializeField]
	public ParticleSystem hitParticleSystem;

	[SerializeField]
	public UINavigationItem navigationItem;

	[Header("Counter")]
	[SerializeField]
	public int counterChange = -1;

	[SerializeField]
	public StatusIcon counterIcon;

	[Header("Keyword Popup")]
	[SerializeField]
	public KeywordData popUpKeyword;

	[SerializeField]
	public Vector2 popUpOffset = new Vector2(-1f, -1f);

	[SerializeField]
	public UnityEngine.Localization.LocalizedString textNotCharged;

	[SerializeField]
	public UnityEngine.Localization.LocalizedString textCharged;

	public UnityEngine.Animator _animator;

	public Character owner;

	public CardController controller;

	public Stat counter;

	public bool reset;

	[HideInInspector]
	public bool interactable;

	public bool poppedUp;

	public static UINavigationItem nav;

	public RectTransform rectTransform => _rectTransform ?? (_rectTransform = (RectTransform)base.transform);

	public UnityEngine.Animator animator => _animator ?? (_animator = GetComponent<UnityEngine.Animator>());

	public bool IsCharged => counter.current <= 0;

	public void OnEnable()
	{
		nav = navigationItem;
		Events.OnSceneChanged += SceneChanged;
		Events.OnBattlePhaseStart += BattlePhaseStart;
		Events.OnBattleTurnEnd += CounterIncrement;
	}

	public void OnDisable()
	{
		Events.OnSceneChanged -= SceneChanged;
		Events.OnBattlePhaseStart -= BattlePhaseStart;
		Events.OnBattleTurnEnd -= CounterIncrement;
	}

	public void BecomeInteractable()
	{
		if (!interactable)
		{
			interactable = true;
			Assign(Battle.instance.player, Battle.instance.playerCardController);
			Show();
		}
	}

	public void Show()
	{
		bell.SetActive(value: true);
		interactable = true;
		reset = false;
		counter.max = owner.data.redrawBell;
		SetCounter(counter.max);
		Events.InvokeRedrawBellRevealed(this);
		AnimatorTrigger("Enter");
		SfxSystem.OneShot("event:/sfx/inventory/redraw_bell_showup");
	}

	public void Hide()
	{
		bell.SetActive(value: false);
		interactable = false;
		if (poppedUp)
		{
			UnPop();
		}

		bellActive.SetActive(value: false);
	}

	public void SceneChanged(Scene scene)
	{
		if (interactable && scene.name != "Battle")
		{
			Hide();
		}
	}

	public void BattlePhaseStart(Battle.Phase phase)
	{
		switch (phase)
		{
			case Battle.Phase.Play:
				BecomeInteractable();
				break;
			case Battle.Phase.End:
				Hide();
				break;
		}
	}

	public void CounterIncrement(int turnNumber)
	{
		if (Battle.instance.phase != Battle.Phase.End)
		{
			Counter();
		}
	}

	public void Assign(Character owner, CardController controller)
	{
		this.owner = owner;
		this.controller = controller;
		GetComponentInChildren<ToggleBasedOnCardController>(includeInactive: true)?.AssignCardController(controller);
	}

	public void Activate()
	{
		if (!interactable)
		{
			return;
		}

		int handSize = Events.GetHandSize(References.PlayerData.handSize);
		ActionRedraw actionRedraw = new ActionRedraw(owner, handSize);
		if (Events.CheckAction(actionRedraw))
		{
			UnplayableCrownCardSystem unplayableCrownCardSystem = Object.FindObjectOfType<UnplayableCrownCardSystem>();
			if ((object)unplayableCrownCardSystem != null && unplayableCrownCardSystem.active)
			{
				actionRedraw.DiscardAll();
			}
			else
			{
				ActionQueue.Add(actionRedraw);
			}

			ActionQueue.Add(new ActionEndTurn(owner));
			controller.Disable();
			if (IsCharged)
			{
				owner.freeAction = true;
				reset = false;
			}
			else
			{
				reset = true;
			}

			Events.InvokeRedrawBellHit(this);
			SfxSystem.OneShot("event:/sfx/inventory/redraw_bell_use");
			SetCounter(counter.max);
			AnimatorTrigger("Ring");
			Events.InvokeScreenShake(1f, 0f);
			Events.InvokeUINavigationReset();
			hitParticleSystem.Play();
		}
	}

	public void Counter()
	{
		if (reset)
		{
			reset = false;
			return;
		}

		int num = Mathf.Clamp(counter.current + counterChange, 0, counter.max);
		if (num != counter.current)
		{
			SetCounter(num);
			AnimatorTrigger("Shake");
			SfxSystem.OneShot("event:/sfx/inventory/redraw_bell_countdown");
		}
	}

	public void SetCounter(int value)
	{
		bool isCharged = IsCharged;
		counter.current = value;
		if ((bool)counterIcon)
		{
			counterIcon.SetValue(counter);
		}

		animator.SetBool("Charged", IsCharged);
		if (!isCharged && IsCharged)
		{
			SfxSystem.OneShot("event:/sfx/inventory/redraw_bell_charged");
		}
	}

	public void Pop()
	{
		if (!poppedUp && (bool)popUpKeyword)
		{
			int handSize = Events.GetHandSize(References.PlayerData.handSize);
			string text = popUpKeyword.body.Format(handSize);
			text += (IsCharged ? ("\n\n" + textCharged.GetLocalizedString()) : ("\n\n" + textNotCharged.GetLocalizedString()));
			CardPopUp.AssignTo(rectTransform, popUpOffset.x, popUpOffset.y);
			CardPopUp.AddPanel(popUpKeyword, text);
			poppedUp = true;
		}
	}

	public void UnPop()
	{
		if (poppedUp)
		{
			CardPopUp.RemovePanel(popUpKeyword.name);
			poppedUp = false;
		}
	}

	public void AnimatorTrigger(string name)
	{
		animator.SetTrigger(name);
	}

	public void AnimatorSetHover(bool value)
	{
		animator.SetBool("Hover", value);
	}

	public void AnimatorSetPress(bool value)
	{
		animator.SetBool("Press", value);
	}

	public void PlayChargeParticleSystem()
	{
		chargeParticleSystem.Play();
	}
}
