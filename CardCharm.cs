#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using NaughtyAttributes;
using UnityEngine;

public class CardCharm : UpgradeDisplay
{
	public Transform holder;

	public Vector3 movementInfluence = new Vector3(-1f, 0.5f, 0f);

	public float rotationMax = 90f;

	public Vector2 wobbleFactorRange = new Vector3(4.5f, 5.5f);

	public Vector2 wobbleDampingRange = new Vector2(0.9f, 0.95f);

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

	public float startingZAngle;

	public void Start()
	{
		wobbleFactor = wobbleFactorRange.PettyRandom();
		wobbleDamping = wobbleDampingRange.PettyRandom();
		wobbleAcc = wobbleAccRange.PettyRandom();
	}

	public void OnEnable()
	{
		if (holder != null)
		{
			prePosition = holder.position;
		}
	}

	public void Update()
	{
		rotationVelocity -= rotation * wobbleAcc * Time.deltaTime;
		rotationVelocity = Delta.Multiply(rotationVelocity, wobbleDamping, Time.deltaTime);
		rotation += rotationVelocity * 200f * Time.deltaTime;
		base.transform.eulerAngles = new Vector3(0f, 0f, startingZAngle + rotation);
		Vector3 position = holder.position;
		Vector3 movement = position - prePosition;
		Wobble(movement);
		prePosition = position;
	}

	public void Wobble(Vector3 movement)
	{
		float num = Vector3.Scale(movement, movementInfluence).magnitude * wobbleFactor;
		rotation = Mathf.Clamp(rotation + num, 0f - rotationMax, rotationMax);
	}

	public void SetAngle(float angle)
	{
		startingZAngle = angle;
	}

	public void StopWobble()
	{
		rotationVelocity = 0f;
		rotation = 0f;
		base.transform.eulerAngles = new Vector3(0f, 0f, startingZAngle);
	}
}
