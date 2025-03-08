#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using Dead;
using UnityEngine;

[CreateAssetMenu(fileName = "ClunkerBossPhaseChange", menuName = "Card Animation/Clunker Boss Change")]
public class CardAnimationClunkerBossChange : CardAnimation
{
	[SerializeField]
	public Vector2Int explosionCountRange = new Vector2Int(3, 5);

	[SerializeField]
	public Vector2 explosionDelayRange = new Vector2(0.1f, 0.6f);

	[SerializeField]
	public ParticleSystem explosionStart;

	[SerializeField]
	public ParticleSystem explosion;

	[SerializeField]
	public ParticleSystem explosionEnd;

	[SerializeField]
	public float explosionPositionRandom = 1f;

	[SerializeField]
	public float duration = 1f;

	[SerializeField]
	public float rumbleAmount = 0.5f;

	[SerializeField]
	public float rumbleDurationIn = 0.5f;

	[SerializeField]
	public float rumbleDurationOut = 0.25f;

	public override IEnumerator Routine(object data, float startDelay = 0f)
	{
		if (data is Entity target)
		{
			SfxSystem.OneShot("event:/sfx/specific/boss_phase_change");
			float num = Mathf.Max(startDelay, duration);
			if (rumbleAmount > 0f)
			{
				Events.InvokeScreenRumble(0f, rumbleAmount, 0f, rumbleDurationIn, Mathf.Max(0f, duration - rumbleDurationIn), rumbleDurationOut);
			}

			if ((bool)explosionStart)
			{
				Explode(explosionStart, target.transform.position + RandomOffset());
			}

			int num2 = explosionCountRange.Random();
			Routine.Clump clump = new Routine.Clump();
			for (int i = 0; i < num2; i++)
			{
				Vector3 position = target.transform.position + RandomOffset();
				clump.Add(ExplodeAfterDelay(num * explosionDelayRange.PettyRandom(), position));
			}

			clump.Add(Sequences.Wait(num));
			yield return clump.WaitForEnd();
			Explode(explosionEnd, target.transform.position, 2f);
			target.curveAnimator?.Ping();
		}
	}

	public Vector3 RandomOffset()
	{
		return new Vector3(PettyRandom.Range(-1f, 1f), PettyRandom.Range(-1f, 1f), 0f).normalized * PettyRandom.Range(0f, explosionPositionRandom);
	}

	public IEnumerator ExplodeAfterDelay(float delay, Vector3 position)
	{
		yield return new WaitForSeconds(delay);
		Explode(explosion, position);
	}

	public void Explode(ParticleSystem prefab, Vector3 position, float screenShake = 1f)
	{
		Object.Instantiate(prefab, position, Quaternion.identity);
		Events.InvokeScreenShake(screenShake, 0f);
	}
}
