#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using Dead;
using NaughtyAttributes;
using UnityEngine;

public class AngleWobbler : MonoBehaviour
{
	public Transform holder;

	public Transform target;

	public bool globalSpace = true;

	[SerializeField]
	public Vector3 movementInfluence = new Vector3(-1f, 0.5f, 0f);

	[SerializeField]
	public float rotationMax = 45f;

	[SerializeField]
	public Vector2 wobbleFactorRange = new Vector2(4.5f, 5.5f);

	[SerializeField]
	public Vector2 wobbleDampingRange = new Vector2(0.9f, 0.95f);

	[SerializeField]
	public Vector2 wobbleAccRange = new Vector2(0.65f, 0.75f);

	[SerializeField]
	[ReadOnly]
	public float wobbleFactor;

	[SerializeField]
	[ReadOnly]
	public float wobbleDamping;

	[SerializeField]
	[ReadOnly]
	public float wobbleAcc;

	public Vector3 prePosition;

	public float rotation;

	public float rotationVelocity;

	public float startAngle;

	public Vector3 HolderPosition => holder.position;

	public void Awake()
	{
		SetAngle(target.localEulerAngles.z);
		wobbleFactor = wobbleFactorRange.PettyRandom();
		wobbleDamping = wobbleDampingRange.PettyRandom();
		wobbleAcc = wobbleAccRange.PettyRandom();
	}

	public void OnEnable()
	{
		if (holder != null)
		{
			prePosition = HolderPosition;
		}
	}

	public void Update()
	{
		rotationVelocity -= rotation * wobbleAcc * Time.deltaTime;
		rotationVelocity = Delta.Multiply(rotationVelocity, wobbleDamping, Time.deltaTime);
		rotation += rotationVelocity * 200f * Time.deltaTime;
		if (globalSpace)
		{
			Vector3 eulerAngles = target.eulerAngles;
			target.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, startAngle + rotation);
		}
		else
		{
			Vector3 localEulerAngles = target.localEulerAngles;
			target.localEulerAngles = new Vector3(localEulerAngles.x, localEulerAngles.y, startAngle + rotation);
		}

		Vector3 holderPosition = HolderPosition;
		Vector3 movement = holderPosition - prePosition;
		Wobble(movement);
		prePosition = holderPosition;
	}

	public void Wobble(Vector3 movement)
	{
		float num = Vector3.Scale(movement, movementInfluence).magnitude * wobbleFactor;
		rotation = Mathf.Clamp(rotation + num, 0f - rotationMax, rotationMax);
	}

	public void WobbleRandom()
	{
		float num = (rotationMax * PettyRandom.Range(0.5f, 1f)).WithRandomSign();
		rotation = Mathf.Clamp(rotation + num, 0f - rotationMax, rotationMax);
	}

	public void SetAngle(float angle)
	{
		startAngle = angle;
	}
}
