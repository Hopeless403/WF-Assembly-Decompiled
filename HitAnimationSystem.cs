#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class HitAnimationSystem : GameSystem
{
	[SerializeField]
	public AnimationCurve hitCurve;

	[SerializeField]
	public float strength = 0.65f;

	[SerializeField]
	public AnimationCurve strengthCurve;

	[SerializeField]
	public float duration = 0.667f;

	[SerializeField]
	public AnimationCurve durationCurve;

	[SerializeField]
	public float wobble = 2f;

	[SerializeField]
	public AnimationCurve wobbleCurve;

	public void OnEnable()
	{
		Events.OnEntityHit += EntityHit;
		Events.OnEntityDodge += EntityDodge;
	}

	public void OnDisable()
	{
		Events.OnEntityHit -= EntityHit;
		Events.OnEntityDodge -= EntityDodge;
	}

	public void EntityDodge(Hit hit)
	{
		if (hit.Offensive && hit.doAnimation)
		{
			Entity target = hit.target;
			if ((object)target != null && target.display is Card)
			{
				CardTakeHit(hit);
			}
		}
	}

	public void EntityHit(Hit hit)
	{
		if (hit.Offensive && hit.doAnimation && hit.countsAsHit)
		{
			Entity target = hit.target;
			if ((object)target != null && target.display is Card)
			{
				CardTakeHit(hit);
			}
		}
	}

	public void CardTakeHit(Hit hit)
	{
		CurveAnimator curveAnimator = hit.target.curveAnimator;
		if ((object)curveAnimator != null)
		{
			int offensiveness = hit.GetOffensiveness();
			float num = strength * strengthCurve.Evaluate(offensiveness);
			float num2 = duration * durationCurve.Evaluate(offensiveness);
			float num3 = wobble * wobbleCurve.Evaluate(offensiveness);
			Vector3 attackerPos = (hit.attacker ? hit.attacker.transform.position : Vector3.zero);
			Vector3 hitDirection = GetHitDirection(hit.target.transform.position, attackerPos);
			curveAnimator.Move(hitDirection * num, hitCurve, 0f, num2);
			if (num3 > 0f && (bool)hit.target.wobbler)
			{
				hit.target.wobbler.Wobble(hitDirection * num3);
			}
		}
	}

	public static Vector3 GetHitDirection(Vector3 targetPos, Vector3 attackerPos)
	{
		return (targetPos - attackerPos).normalized;
	}
}
