#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using Dead;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BlingParticle : MonoBehaviour
{
	public BlingParticleSystem system;

	public Character owner;

	public int value = 1;

	[SerializeField]
	public Vector2 sizeRange = new Vector2(0.7f, 0.9f);

	[SerializeField]
	public Vector2 angleRange = new Vector2(-20f, 20f);

	[SerializeField]
	public Vector3 groundRotation = new Vector3(20f, 0f, 0f);

	[SerializeField]
	public Vector2 bounceRange = new Vector2(0.35f, 0.45f);

	[SerializeField]
	public Vector2 bounceSlowdownRange = new Vector2(0.75f, 0.85f);

	[SerializeField]
	public Vector2 flyToBagRange = new Vector2(2f, 2.5f);

	[SerializeField]
	public Vector2 startSpeedRange = new Vector2(1f, 2f);

	[SerializeField]
	public Vector2 startUpSpeedRange = new Vector2(0f, 1f);

	[SerializeField]
	public Vector2 frictMultRange = new Vector2(0.94f, 0.96f);

	[SerializeField]
	public float grav = 10f;

	[SerializeField]
	public Vector2 groundOffsetRange = new Vector2(0.5f, 1f);

	[SerializeField]
	public bool startTimerWhenOnGround = true;

	[SerializeField]
	[Range(0f, 1f)]
	public float zInfluence = 0.5f;

	[SerializeField]
	public float flyToBagSpeed = 1f;

	[SerializeField]
	public float flyMaxSpeed = 10f;

	public float worldGroundY;

	public float bounce;

	public float bounceSlowdown;

	public float flyToBag;

	public Vector3 velocity;

	public float frictMult;

	public bool onGround;

	public GoldDisplay targetBag;

	public SpriteRenderer _spr;

	public SpriteRenderer spr => _spr ?? (_spr = GetComponent<SpriteRenderer>());

	public Sprite sprite
	{
		set
		{
			spr.sprite = value;
		}
	}

	public string sortingLayer
	{
		set
		{
			spr.sortingLayerName = value;
		}
	}

	public void OnEnable()
	{
		sortingLayer = "ParticlesBehind";
		onGround = false;
		targetBag = null;
		float num = sizeRange.PettyRandom();
		bounce = bounceRange.PettyRandom();
		bounceSlowdown = bounceSlowdownRange.PettyRandom();
		flyToBag = flyToBagRange.PettyRandom();
		frictMult = frictMultRange.PettyRandom();
		base.transform.localScale = new Vector3(num, num, 1f);
		Vector3 localEulerAngles = groundRotation.WithZ(angleRange.PettyRandom());
		base.transform.localEulerAngles = localEulerAngles;
		worldGroundY = base.transform.position.y - groundOffsetRange.PettyRandom();
		spr.flipX = PettyRandom.Choose<bool>(true, false);
		Vector3 normalized = new Vector3(PettyRandom.Range(-1f, 1f), PettyRandom.Range(-1f, 1f), 0f).normalized;
		velocity = normalized * startSpeedRange.PettyRandom();
		velocity.y += startUpSpeedRange.PettyRandom();
	}

	public void Update()
	{
		Vector3 localPosition = base.transform.localPosition;
		localPosition += velocity * Time.deltaTime;
		if ((bool)targetBag)
		{
			Vector3 vector = targetBag.transform.position - base.transform.position;
			velocity += vector * (flyToBagSpeed * Time.deltaTime);
			if (vector.sqrMagnitude < 0.1f)
			{
				Collect();
			}
			else if (velocity.magnitude > flyMaxSpeed)
			{
				velocity = vector.normalized * flyMaxSpeed;
			}
		}
		else
		{
			if (!onGround && velocity.y <= 0f && localPosition.y <= worldGroundY)
			{
				velocity.y *= 0f - bounce;
				velocity.x *= bounceSlowdown;
				if (Mathf.Abs(velocity.y) < 0.05f)
				{
					velocity.y = 0f;
					onGround = true;
				}

				localPosition.y = worldGroundY;
			}

			if (zInfluence > 0f)
			{
				localPosition.z -= velocity.y * zInfluence * Time.deltaTime;
			}

			velocity.x = Delta.Multiply(velocity.x, frictMult, Time.deltaTime);
			if (!onGround)
			{
				velocity.y -= grav * Time.deltaTime;
			}
			else if (localPosition.y != worldGroundY)
			{
				onGround = false;
			}

			if (!startTimerWhenOnGround || onGround)
			{
				flyToBag -= Time.deltaTime;
				if (flyToBag <= 0f)
				{
					FlyToBag();
				}
			}
		}

		base.transform.localPosition = localPosition;
	}

	public void FlyToBag()
	{
		if (owner.entity.display is CharacterDisplay characterDisplay && characterDisplay.goldDisplay != null)
		{
			targetBag = characterDisplay.goldDisplay;
			sortingLayer = "ParticlesFront";
			Events.InvokeGoldFlyToBag(value, owner, base.transform.position);
		}
		else
		{
			Collect();
		}
	}

	public void Collect()
	{
		owner.GainGold(value);
		ReturnToPool();
		Events.InvokeCollectGold(value);
	}

	public void ReturnToPool()
	{
		system.Pool(this);
	}
}
