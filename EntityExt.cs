#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EntityExt
{
	public static Vector3 GetContainerWorldPosition(this Entity entity)
	{
		Vector3 zero = Vector3.zero;
		if (entity.actualContainers.Count > 0)
		{
			foreach (CardContainer actualContainer in entity.actualContainers)
			{
				zero += actualContainer.holder.position + actualContainer.GetChildPosition(entity);
			}

			zero /= (float)entity.actualContainers.Count;
		}

		return zero;
	}

	public static Vector3 GetContainerLocalPosition(this Entity entity)
	{
		Vector3 result = Vector3.zero;
		if (entity.actualContainers.Count > 0)
		{
			Vector3 zero = Vector3.zero;
			foreach (CardContainer actualContainer in entity.actualContainers)
			{
				zero += actualContainer.holder.position + actualContainer.GetChildPosition(entity);
			}

			zero /= (float)entity.actualContainers.Count;
			result = zero - entity.actualContainers[0].holder.position;
		}

		return result;
	}

	public static Vector3 GetContainerWorldRotation(this Entity entity)
	{
		Vector3 zero = Vector3.zero;
		if (entity.actualContainers.Count > 0)
		{
			foreach (CardContainer actualContainer in entity.actualContainers)
			{
				zero += actualContainer.GetChildRotation(entity);
			}

			zero /= (float)entity.actualContainers.Count;
			zero += entity.actualContainers[0].holder.eulerAngles;
		}

		return zero;
	}

	public static Vector3 GetContainerLocalRotation(this Entity entity)
	{
		Vector3 zero = Vector3.zero;
		if (entity.actualContainers.Count > 0)
		{
			foreach (CardContainer actualContainer in entity.actualContainers)
			{
				zero += actualContainer.GetChildRotation(entity);
			}

			zero /= (float)entity.actualContainers.Count;
		}

		return zero;
	}

	public static Vector3 GetContainerScale(this Entity entity)
	{
		Vector3 zero = Vector3.zero;
		if (entity.actualContainers.Count > 0)
		{
			foreach (CardContainer actualContainer in entity.actualContainers)
			{
				zero += actualContainer.GetChildScale(entity);
			}

			zero /= (float)entity.actualContainers.Count;
		}

		return zero;
	}

	public static void TweenToContainer(this Entity entity)
	{
		int count = entity.actualContainers.Count;
		if (count <= 0)
		{
			return;
		}

		CardContainer cardContainer = entity.actualContainers[0];
		if (count > 1)
		{
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			Vector3 zero3 = Vector3.zero;
			foreach (CardContainer actualContainer in entity.actualContainers)
			{
				zero += actualContainer.GetChildScale(entity);
				zero2 += actualContainer.GetChildRotation(entity);
				zero3 += actualContainer.holder.position + actualContainer.GetChildPosition(entity);
			}

			zero /= (float)count;
			zero2 /= (float)count;
			zero3 /= (float)count;
			Vector3 to = cardContainer.holder.InverseTransformPoint(zero3) * -1f;
			LeanTween.cancel(entity.gameObject);
			LeanTween.scale(entity.gameObject, zero, cardContainer.scaleDurRand.PettyRandom()).setEase(cardContainer.scaleEase);
			LeanTween.rotateLocal(entity.gameObject, zero2, cardContainer.movementDurRand.PettyRandom()).setEase(cardContainer.movementEase);
			LeanTween.moveLocal(entity.gameObject, to, cardContainer.movementDurRand.PettyRandom()).setEase(cardContainer.movementEase);
		}
		else
		{
			Vector3 childScale = cardContainer.GetChildScale(entity);
			Vector3 childRotation = cardContainer.GetChildRotation(entity);
			Vector3 childPosition = cardContainer.GetChildPosition(entity);
			LeanTween.cancel(entity.gameObject);
			LeanTween.scale(entity.gameObject, childScale, cardContainer.scaleDurRand.PettyRandom()).setEase(cardContainer.scaleEase);
			LeanTween.rotateLocal(entity.gameObject, childRotation, cardContainer.movementDurRand.PettyRandom()).setEase(cardContainer.movementEase);
			LeanTween.moveLocal(entity.gameObject, childPosition, cardContainer.movementDurRand.PettyRandom()).setEase(cardContainer.movementEase);
		}
	}

	public static void ForceUnHover(this Entity entity)
	{
		EntityDisplay display = entity.display;
		if (display is Card)
		{
			CardHover hover = display.hover;
			if ((object)hover != null)
			{
				hover.OnPointerExit(null);
				hover.ForceUnHover();
			}
		}
	}

	public static bool IsOffensive(this Entity entity)
	{
		bool flag = entity.HasAttackIcon() || entity.data.original.damage > 0;
		if (!flag && entity.attackEffects.Any((CardData.StatusEffectStacks s) => s.data.offensive))
		{
			flag = true;
		}

		if (!flag && entity.statusEffects.Any((StatusEffectData s) => s.makesOffensive))
		{
			flag = true;
		}

		return flag;
	}

	public static bool CanRecall(this Entity entity)
	{
		if (entity.data.cardType.canRecall && entity.blockRecall <= 0 && (bool)References.Battle)
		{
			return Battle.IsOnBoard(entity);
		}

		return false;
	}

	public static bool StillExists(this Entity entity)
	{
		if ((bool)entity)
		{
			return !entity.inCardPool;
		}

		return false;
	}

	public static bool IsAliveAndExists(this Entity entity)
	{
		if ((bool)entity && !entity.inCardPool)
		{
			return entity.alive;
		}

		return false;
	}

	public static bool HasAnyCharms(this Entity entity)
	{
		if (entity.data.upgrades != null)
		{
			return entity.data.upgrades.Any((CardUpgradeData a) => a.type == CardUpgradeData.Type.Charm);
		}

		return false;
	}

	public static void ResetWhenHealthLostEffects(this Entity target)
	{
		foreach (StatusEffectData statusEffect in target.statusEffects)
		{
			if (statusEffect is StatusEffectApplyXWhenHealthLost statusEffectApplyXWhenHealthLost)
			{
				statusEffectApplyXWhenHealthLost.currentHealth = target.hp.current;
			}
		}
	}

	public static HashSet<KeywordData> GetHiddenKeywords(this Entity entity)
	{
		HashSet<KeywordData> hashSet = new HashSet<KeywordData>();
		foreach (CardData.StatusEffectStacks attackEffect in entity.attackEffects)
		{
			KeywordData[] hiddenKeywords = attackEffect.data.hiddenKeywords;
			foreach (KeywordData item in hiddenKeywords)
			{
				hashSet.Add(item);
			}
		}

		foreach (StatusEffectData statusEffect in entity.statusEffects)
		{
			KeywordData[] hiddenKeywords = statusEffect.hiddenKeywords;
			foreach (KeywordData item2 in hiddenKeywords)
			{
				hashSet.Add(item2);
			}
		}

		return hashSet;
	}

	public static bool HasAttackIcon(this Entity entity)
	{
		if (entity.damage.max <= 0 && !(entity.damage.current + entity.tempDamage > 0))
		{
			if ((bool)entity.data)
			{
				return entity.data.hasAttack;
			}

			return false;
		}

		return true;
	}

	public static bool LastHitStillProcessing(this Entity entity)
	{
		return entity.lastHit?.processing ?? false;
	}

	public static IEnumerator WaitForLastHitToFinishProcessing(this Entity entity)
	{
		return new WaitUntil(() => !(entity.lastHit?.processing ?? false));
	}
}
