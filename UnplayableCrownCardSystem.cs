#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;

public class UnplayableCrownCardSystem : GameSystem
{
	[SerializeField]
	public UnityEngine.Localization.LocalizedString promptKey;

	public bool active;

	public bool blocked;

	public bool promptShown;

	public int handCount;

	public const float timerMax = 0.5f;

	public float timer;

	public void OnEnable()
	{
		Events.OnBattleStart += BattleStart;
		Events.OnSceneChanged += SceneChanged;
		Events.OnBattleTurnEnd += BattleTurnEnd;
	}

	public void OnDisable()
	{
		Events.OnBattleStart -= BattleStart;
		Events.OnSceneChanged -= SceneChanged;
		Events.OnBattleTurnEnd -= BattleTurnEnd;
	}

	public void BattleStart()
	{
		if (!active)
		{
			Activate();
		}
	}

	public void SceneChanged(Scene scene)
	{
		if (active)
		{
			Deactivate();
		}
	}

	public void BattleTurnEnd(int turnCount)
	{
		if (active)
		{
			Deactivate();
		}
	}

	public void Activate()
	{
		active = true;
		handCount = References.Player.handContainer.Count;
		timer = 0.5f;
	}

	public void Update()
	{
		if (active && timer > 0f)
		{
			timer -= Time.deltaTime;
			if (timer <= 0f)
			{
				timer = 0.5f;
				Check();
			}
		}
	}

	public void Check()
	{
		int count = References.Player.handContainer.Count;
		if (count == handCount)
		{
			return;
		}

		handCount = count;
		if (handCount == 0)
		{
			Deactivate();
		}
		else
		{
			if (handCount <= 0)
			{
				return;
			}

			bool flag = blocked;
			blocked = CheckBlocked();
			if (blocked && !flag)
			{
				ShowPrompt();
				RedrawBellSystem redrawBellSystem = Object.FindObjectOfType<RedrawBellSystem>();
				if ((object)redrawBellSystem != null)
				{
					redrawBellSystem.Enable();
					redrawBellSystem.BecomeInteractable();
				}
			}
			else if (flag && !blocked)
			{
				Deactivate();
			}
		}
	}

	public void Deactivate()
	{
		active = false;
		blocked = false;
		if (promptShown)
		{
			PromptSystem.Hide();
			promptShown = false;
		}
	}

	public static bool CheckBlocked()
	{
		int num = 0;
		CardContainer[] containers = Object.FindObjectsOfType<CardContainer>().ToArray();
		foreach (Entity item in References.Player.handContainer)
		{
			if (CardIsBlocked(item, containers))
			{
				num++;
			}
		}

		return num == References.Player.handContainer.Count;
	}

	public static bool CardIsBlocked(Entity card, CardContainer[] containers)
	{
		foreach (StatusEffectData statusEffect in card.statusEffects)
		{
			if (statusEffect is StatusEffectRecycle statusEffectRecycle)
			{
				if (!statusEffectRecycle.IsEnoughJunkInHand())
				{
					return true;
				}

				break;
			}
		}

		if (!card.NeedsTarget)
		{
			return false;
		}

		foreach (CardContainer container in containers)
		{
			if (card.CanPlayOn(container))
			{
				return false;
			}
		}

		foreach (Entity card2 in References.Battle.cards)
		{
			if (card2.enabled && card.CanPlayOn(card2))
			{
				return false;
			}
		}

		return true;
	}

	public void ShowPrompt()
	{
		PromptSystem.Create(Prompt.Anchor.Left, 1.5f, 2f, 5f, Prompt.Emote.Type.Scared);
		PromptSystem.SetTextAction(() => promptKey.GetLocalizedString());
		promptShown = true;
	}
}
