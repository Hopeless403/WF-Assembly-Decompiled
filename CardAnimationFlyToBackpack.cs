#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "FlyToBackpack", menuName = "Card Animation/Fly To Backpack")]
public class CardAnimationFlyToBackpack : CardAnimation
{
	[SerializeField]
	public AnimationCurve xCurve;

	[SerializeField]
	public AnimationCurve yCurve;

	[SerializeField]
	public AnimationCurve zCurve;

	[SerializeField]
	public AnimationCurve spinCurve;

	[SerializeField]
	public AnimationCurve scaleCurve;

	[SerializeField]
	public bool destroyOnEnd = true;

	[Header("Duration")]
	[SerializeField]
	public bool fixedDuration;

	[SerializeField]
	[ShowIf("fixedDuration")]
	public float duration = 0.5f;

	[SerializeField]
	[HideIf("fixedDuration")]
	public AnimationCurve durationToDistance;

	[Header("Glows")]
	[SerializeField]
	public AnimationCurve glowMain;

	[SerializeField]
	public Vector2 glowMainSize = new Vector2(4f, 6f);

	[SerializeField]
	public AnimationCurve glowExtra;

	[SerializeField]
	public int glowExtraCount = 4;

	[SerializeField]
	public Rect glowExtraArea = new Rect(-0.7f, -1.3f, 1.4f, 2.6f);

	[SerializeField]
	public Vector2 glowExtraSizeRange = new Vector2(2f, 3f);

	[SerializeField]
	public Vector2 glowExtraDelay = new Vector2(0f, 0.5f);

	[SerializeField]
	public Glow glowPrefab;

	[Header("Jump")]
	[SerializeField]
	public AnimationCurve yUpCurve;

	[SerializeField]
	public float yUpAmount = 0.25f;

	public override IEnumerator Routine(object data, float startDelay = 0f)
	{
		if (!(data is Entity entity))
		{
			yield break;
		}

		Transform target = entity.transform;
		if ((object)target == null)
		{
			yield break;
		}

		yield return new WaitForSeconds(startDelay);
		Deckpack deckpack = MonoBehaviourSingleton<Deckpack>.instance;
		if ((object)deckpack == null)
		{
			yield break;
		}

		target.SetParent(deckpack.transform, worldPositionStays: true);
		AngleWobbler[] wobblers = target.GetComponentsInChildren<AngleWobbler>();
		AngleWobbler[] array = wobblers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].globalSpace = false;
		}

		Vector3 start = target.localPosition;
		Vector3 zero = Vector3.zero;
		Vector3 offset = zero - start;
		float t = 0f;
		float z = target.localEulerAngles.z;
		Vector3 scale = target.localScale;
		float dur = (fixedDuration ? duration : durationToDistance.Evaluate(offset.WithZ(0f).magnitude));
		Object.Instantiate(glowPrefab, target).SetColor(Color.white).SetPosition(Vector2.zero)
			.SetSize(glowMainSize)
			.Fade(glowMain, dur);
		for (int j = 0; j < glowExtraCount; j++)
		{
			float num = glowExtraDelay.PettyRandom();
			float num2 = dur - num;
			Object.Instantiate(glowPrefab, target).RandomColor().SetPosition(glowExtraArea.RandomPosition())
				.SetSize(Vector2.one * glowExtraSizeRange.PettyRandom())
				.Fade(glowExtra, num2, num);
		}

		yield return null;
		while (t < 1f && (bool)target)
		{
			t += Time.deltaTime / dur;
			target.localPosition = start + offset.Multiply(xCurve.Evaluate(t), yCurve.Evaluate(t), zCurve.Evaluate(t)) + Vector3.up * yUpCurve.Evaluate(t) * yUpAmount;
			target.localEulerAngles = target.localEulerAngles.WithZ(z + spinCurve.Evaluate(t));
			target.localScale = scale * scaleCurve.Evaluate(t);
			yield return null;
		}

		if (destroyOnEnd && (bool)target)
		{
			entity.RemoveFromContainers();
			CardManager.ReturnToPool(entity);
			array = wobblers;
			foreach (AngleWobbler angleWobbler in array)
			{
				if ((bool)angleWobbler)
				{
					angleWobbler.globalSpace = true;
				}
			}
		}

		if ((bool)deckpack)
		{
			deckpack.Ping();
		}
	}
}
