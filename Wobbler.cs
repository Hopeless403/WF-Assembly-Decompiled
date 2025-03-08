#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using Dead;
using UnityEngine;

public class Wobbler : MonoBehaviourCacheTransform, IPoolable
{
	public Vector3 rotationInfluence = new Vector3(10f, 10f, 2f);

	public Vector3 rotationMax = new Vector3(50f, 50f, 50f);

	public Vector3 rotationDamping = new Vector3(0.91f, 0.93f, 0.94f);

	public Vector3 rotationAcc = new Vector3(1f, 0.8f, 0.7f);

	public Vector3 rotation;

	public Vector3 rotationVelocity;

	public void Update()
	{
		rotationVelocity -= Vector3.Scale(rotation, rotationAcc) * Time.deltaTime;
		rotationVelocity = Delta.Multiply(rotationVelocity, rotationDamping, Time.deltaTime);
		rotation += rotationVelocity * (200f * Time.deltaTime);
		base.transform.localEulerAngles = rotation;
	}

	public void Wobble(Vector3 movement)
	{
		Vector3 vector = Vector3.Scale(new Vector3(movement.y, 0f - movement.x, 0f - movement.x), rotationInfluence);
		rotation = (rotation + vector).Clamp(-rotationMax, rotationMax);
	}

	public void WobbleRandom(float wobbleFactor = 1f)
	{
		Wobble(PettyRandom.Vector2() * wobbleFactor);
	}

	public void OnGetFromPool()
	{
	}

	public void OnReturnToPool()
	{
		rotation = Vector3.zero;
		rotationVelocity = Vector3.zero;
		base.transform.localScale = Vector3.one;
	}
}
