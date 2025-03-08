#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;

public class WispAnimator : MonoBehaviourCacheTransform
{
	public float knockback = 8f;

	public float gravitate = 5f;

	public float maxSpeed = 2f;

	public ParticleSystem smokeFx;

	public SpriteRenderer sprite;

	public SpriteRenderer faceSprite;

	public ParticleSystem pingPS;

	public Vector3 velocity = Vector3.zero;

	public Transform target;

	public void KnockBackFrom(Vector3 from)
	{
		Vector3 normalized = (base.transform.position - from).WithZ(0f).normalized;
		velocity = normalized * knockback;
	}

	public void SetTarget(Transform target)
	{
		this.target = target;
	}

	public bool TargetExists()
	{
		return target;
	}

	public void JumpToTarget()
	{
		pingPS.Play();
		LeanTween.move(base.gameObject, target.position, 1f).setEaseInBack();
	}

	public void FadeToColour(Color color, float delay, float duration)
	{
		Color color2 = sprite.color;
		Gradient gradient = new Gradient
		{
			alphaKeys = new GradientAlphaKey[1]
			{
				new GradientAlphaKey(1f, 0f)
			},
			colorKeys = new GradientColorKey[2]
			{
				new GradientColorKey(color2, 0f),
				new GradientColorKey(color, 1f)
			}
		};
		LeanTween.value(base.gameObject, 0f, 1f, duration).setDelay(delay).setOnUpdate(delegate(float a)
		{
			Color color3 = gradient.Evaluate(a);
			sprite.color = color3;
			ParticleSystem.MainModule main = smokeFx.main;
			main.startColor = color3;
		});
	}

	public void SetSortingLayer(string layerName, int orderInLayer)
	{
		smokeFx.GetComponent<Renderer>().sortingLayerName = layerName;
		smokeFx.GetComponent<Renderer>().sortingOrder = orderInLayer;
		sprite.sortingLayerName = layerName;
		sprite.sortingOrder = orderInLayer + 1;
		faceSprite.sortingLayerName = layerName;
		faceSprite.sortingOrder = orderInLayer + 2;
	}

	public void Update()
	{
		base.transform.position += velocity * Time.deltaTime;
		Vector3 vector = ((maxSpeed == 0f) ? Vector3.zero : (target ? ((target.position - base.transform.position).normalized * maxSpeed) : Vector3.zero));
		velocity = Vector3.MoveTowards(velocity, vector, gravitate * Time.deltaTime);
	}
}
