#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using UnityEngine;

public static class Sequences
{
	public static IEnumerator Wait(float seconds)
	{
		while (seconds > 0f)
		{
			seconds -= Time.deltaTime;
			if (!(seconds <= 0f))
			{
				yield return null;
				continue;
			}

			break;
		}
	}

	public static IEnumerator Null()
	{
		if (!GameManager.paused)
		{
			yield return new WaitForFixedUpdate();
		}
	}

	public static IEnumerator WaitForAnimationEnd(Entity entity)
	{
		yield return new WaitUntil(() => !entity.IsAliveAndExists() || !entity.curveAnimator || !entity.curveAnimator.active);
	}

	public static IEnumerator WaitForStatusEffectEvents()
	{
		while (StatusEffectSystem.EventsRunning)
		{
			yield return null;
		}
	}

	public static IEnumerator ShuffleTo(CardContainer fromContainer, CardContainer toContainer, float delayBetween = 0.05f)
	{
		if (!toContainer || !toContainer.Empty || !fromContainer)
		{
			yield break;
		}

		while (!fromContainer.Empty)
		{
			int index = Random.Range(0, fromContainer.Count);
			Entity entity = fromContainer[index];
			yield return CardMove(entity, new CardContainer[1] { toContainer });
			if (delayBetween > 0f)
			{
				yield return Wait(delayBetween);
			}
		}

		if (delayBetween <= 0f)
		{
			toContainer.SetChildPositions();
		}
	}

	public static IEnumerator CardDiscard(Entity entity)
	{
		Debug.Log($"Discarding [{entity}]");
		yield return CardMove(entity, new CardContainer[1] { entity.owner.discardContainer });
	}

	public static IEnumerator CardMove(Entity entity, CardContainer[] toContainers, int insertPos = -1, bool tweenAll = true)
	{
		bool enabled = entity.enabled;
		entity.RemoveFromContainers();
		if (insertPos >= 0)
		{
			CardContainer[] array = toContainers;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Insert(insertPos, entity);
			}
		}
		else
		{
			CardContainer[] array = toContainers;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Add(entity);
			}
		}

		entity.ResetDrawOrder();
		if (!entity.enabled && enabled)
		{
			yield return StatusEffectSystem.EntityDisableEvent(entity);
		}

		if (tweenAll)
		{
			CardContainer[] array = toContainers;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].TweenChildPositions();
			}

			if (toContainers.Length == 1 && toContainers[0] is CardSlotLane cardSlotLane)
			{
				foreach (Entity item in cardSlotLane)
				{
					if (item.height <= 1)
					{
						continue;
					}

					array = item.containers;
					foreach (CardContainer cardContainer in array)
					{
						if (cardContainer != cardSlotLane)
						{
							cardContainer.TweenChildPositions();
						}
					}
				}
			}
		}

		Events.InvokeEntityMove(entity);
		yield return StatusEffectSystem.CardMoveEvent(entity);
	}

	public static IEnumerator CardRecall(Entity entity)
	{
		if ((bool)entity.owner.discardContainer)
		{
			yield return Wait(0.3f);
			yield return CardMove(entity, new CardContainer[1] { entity.owner.discardContainer });
		}
		else if ((bool)entity.owner.reserveContainer)
		{
			yield return Wait(0.3f);
			int insertPos = RandomInclusive.Range(0, entity.owner.reserveContainer.Count - 1);
			yield return CardMove(entity, new CardContainer[1] { entity.owner.reserveContainer }, insertPos);
		}
	}

	public static IEnumerator EndCampaign(string newSceneKey)
	{
		yield return Transition.WaitUntilDone(Transition.Begin());
		if ((bool)References.Campaign)
		{
			References.Campaign.Final();
		}

		Routine.Clump clump = new Routine.Clump();
		clump.Add(SceneManager.Unload("Campaign"));
		clump.Add(SceneManager.Unload("UI"));
		yield return clump.WaitForEnd();
		yield return SceneChange(newSceneKey);
	}

	public static IEnumerator SceneChange(string newSceneKey)
	{
		string preActive = SceneManager.ActiveSceneKey;
		CardPopUp.Clear();
		yield return SceneManager.Load(newSceneKey, SceneType.Active);
		yield return SceneManager.Unload(preActive);
	}
}
