#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using System.Collections;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(Card))]
public class CardRotator : MonoBehaviour
{
	[SerializeField]
	[Required(null)]
	[OnValueChanged("DataChanged")]
	public CardData data;

	[SerializeField]
	public float rotateSpeed;

	[SerializeField]
	public Vector3 rotateAmount;

	[SerializeField]
	public bool startFlippedDown = true;

	public Entity entity;

	public Card card;

	public float internalRotateSpeed = 1f;

	public float t;

	public IEnumerator Start()
	{
		entity = GetComponent<Entity>();
		entity.data = data;
		card = GetComponent<Card>();
		yield return card.UpdateData();
		if (startFlippedDown)
		{
			entity.flipper.FlipDownInstant();
			internalRotateSpeed = 0f;
			base.transform.localEulerAngles = Vector3.zero;
		}
	}

	public void Update()
	{
		t += Time.deltaTime * rotateSpeed * internalRotateSpeed;
		base.transform.localEulerAngles = rotateAmount * Mathf.Sin(t);
	}

	public void DataChanged()
	{
		if (!Application.isEditor)
		{
			StartCoroutine(UpdateData(data));
		}
	}

	public IEnumerator UpdateData(CardData data)
	{
		entity.data = data;
		yield return entity.ClearStatuses();
		card.ClearStatusIcons();
		yield return card.UpdateData();
	}

	[Button(null, EButtonEnableMode.Always)]
	public void Flip()
	{
		if (entity.flipper.flipped)
		{
			StartCoroutine(FlipUpRoutine());
		}
		else
		{
			StartCoroutine(FlipDownRoutine());
		}
	}

	public IEnumerator FlipUpRoutine()
	{
		entity.flipper.FlipUp();
		float dur = entity.flipper.flipUpDurationRand.y;
		yield return Sequences.Wait(dur / 2f);
		entity.wobbler.WobbleRandom();
		yield return Sequences.Wait(dur / 2f);
		LeanTween.value(0f, 1f, 1f).setOnUpdate(delegate(float a)
		{
			internalRotateSpeed = a;
		});
	}

	public IEnumerator FlipDownRoutine()
	{
		entity.flipper.FlipDown();
		float dur = entity.flipper.flipUpDurationRand.y;
		yield return Sequences.Wait(dur / 2f);
		entity.wobbler.WobbleRandom();
		yield return Sequences.Wait(dur / 2f);
		LeanTween.value(1f, 0f, 1f).setOnUpdate(delegate(float a)
		{
			internalRotateSpeed = a;
		});
	}
}
