#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using Dead;
using UnityEngine;

public class FlyOffScreen : MonoBehaviourCacheTransform
{
	[Header("Movement")]
	public Vector3 velocity;

	[SerializeField]
	public Vector3 grav = new Vector3(0f, -80f, 0f);

	[SerializeField]
	public Vector3 rotation = new Vector3(0f, 100f, 320f);

	[SerializeField]
	public Vector3 frictMult = new Vector3(0.95f, 1f, 0.92f);

	[Header("Fade")]
	public CanvasGroup canvasGroup;

	[SerializeField]
	public float alpha = 2f;

	[SerializeField]
	public float fade = 2f;

	[Header("Rotation Amount")]
	[SerializeField]
	public Vector2 rotateRangeX = new Vector2(1f, 2f);

	[SerializeField]
	public Vector2 rotateRangeY = new Vector2(0f, 1f);

	[SerializeField]
	public Vector2 rotateRangeZ = new Vector2(1f, 2f);

	public void Awake()
	{
		rotation.Scale(new Vector3(rotateRangeX.PettyRandom(), rotateRangeY.PettyRandom(), rotateRangeZ.PettyRandom()));
		Begin();
	}

	public void Update()
	{
		velocity += grav * Time.deltaTime;
		velocity = Delta.Multiply(velocity, frictMult, Time.deltaTime);
		base.transform.position = base.transform.position + velocity * Time.deltaTime;
		Vector3 localEulerAngles = base.transform.localEulerAngles;
		localEulerAngles += rotation * (Mathf.Sign(velocity.x) * Time.deltaTime);
		base.transform.localEulerAngles = localEulerAngles;
		alpha -= fade * Time.deltaTime;
		if (alpha <= 0f)
		{
			End();
		}
		else
		{
			canvasGroup.alpha = Mathf.Min(1f, alpha);
		}
	}

	public void Knockback(Hit lastHit)
	{
		Vector3 normalized = ((lastHit != null && (bool)lastHit.attacker) ? (base.transform.position - lastHit.attacker.transform.position).WithZ(0f) : Vector3.up.WithX(PettyRandom.Range(-1f, 1f))).normalized;
		Debug.Log($"knockback dir: {normalized}");
		float x = Mathf.Clamp(normalized.x * PettyRandom.Range(5f, 10f), -1f, 1f);
		float y = PettyRandom.Range(15f, 25f);
		float z = 0f - PettyRandom.Range(10f, 30f);
		Knockback(new Vector3(x, y, z));
	}

	public void Knockback(Vector3 dir)
	{
		velocity = dir;
	}

	public virtual void Begin()
	{
	}

	public virtual void End()
	{
		base.gameObject.Destroy();
	}
}
