#region Assembly Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// C:\Program Files (x86)\Steam\steamapps\common\Wildfrost\Modded\Wildfrost_Data\Managed\Assembly-CSharp.dll
// Decompiled with ICSharpCode.Decompiler 8.1.1.7464
#endregion

using UnityEngine;
using UnityEngine.UI;

public class SplatterParticle : MonoBehaviour
{
	public Splatter splatterPrefab;

	public bool canHitSource;

	public SplatterSurface source;

	[SerializeField]
	public Canvas canvas;

	[SerializeField]
	public Image image;

	[SerializeField]
	public Vector2 sizeRange = new Vector2(0.3f, 0.4f);

	public Vector3 velocity;

	public Vector3 gravity = new Vector3(0f, 0f, 1f);

	public Vector3 frictMult = new Vector3(0.99f, 0.99f, 0.99f);

	public bool isInBackground;

	[SerializeField]
	public float backgroundZThreshold = 1f;

	public Color color
	{
		get
		{
			return image.color;
		}
		set
		{
			image.color = value;
		}
	}

	public void Awake()
	{
		base.transform.localScale = Vector3.one * sizeRange.PettyRandom();
	}

	public void Update()
	{
		base.transform.position += velocity * Time.deltaTime;
		velocity += gravity * Time.deltaTime;
		velocity = Delta.Multiply(velocity, frictMult, Time.deltaTime);
		if (!isInBackground)
		{
			if (base.transform.position.z > backgroundZThreshold)
			{
				canvas.sortingLayerName = "Background";
				isInBackground = true;
				canHitSource = true;
			}
		}
		else if (base.transform.position.z < backgroundZThreshold)
		{
			canvas.sortingLayerName = "Default";
			isInBackground = true;
		}

		if (base.transform.position.z > 10f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void SetSource(SplatterSurface source)
	{
		this.source = source;
	}

	public void OnTriggerEnter(Collider other)
	{
		SplatterSurface component = other.gameObject.GetComponent<SplatterSurface>();
		if (component != null && (canHitSource || component != source))
		{
			component.Splat(this);
			base.gameObject.Destroy();
		}
	}
}
